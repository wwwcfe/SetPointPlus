# coding: utf-8
from contextlib import closing
from xml.etree import ElementTree
from cStringIO import StringIO
import re
import os
import sys
import glob
import shutil
import platform
import _winreg
HKCU = _winreg.HKEY_CURRENT_USER
HKLM = _winreg.HKEY_LOCAL_MACHINE


def getregvalue(key, sub, valname):
    flag = _winreg.KEY_READ | _winreg.KEY_WOW64_64KEY if platform.machine() == "AMD64" else 0
    with _winreg.OpenKey(key, sub, 0, flag) as key:
        return _winreg.QueryValueEx(key, valname)[0]


def getinfo(ver):
    if ver == 6:  # SetPoint 6.x
        installdir = getregvalue(HKLM, r"SOFTWARE\Logitech\EvtMgr6", "InstallLocation")
        devicesdir = getregvalue(HKLM, r"SOFTWARE\Logitech\EvtMgr6", "DevicesFilePath")
        instdevice = getregvalue(HKCU, r"Software\Logitech\Info", "SP5Devices")
    elif ver == 4:
        installdir = getregvalue(
            HKLM, r"SOFTWARE\Logitech\SetPoint\CurrentVersion\Setup", "SetPoint Directory")
        devicesdir = getregvalue(
            HKLM, r"SOFTWARE\Logitech\SetPoint\CurrentVersion\Setup", "Devices Folder")
        instdevice = getregvalue(HKCU, r"Software\Logitech\Info", "SP5Devices")
    elif ver == -1:
        installdir = "testdata\\setpointplus"
        devicesdir = "testdata\\setpointplus"
        instdevice = "100009F,2000068"
    else: raise NotImplementedError("This script supports only SetPoint 4.x or 6.x")
    return installdir, devicesdir, instdevice.split(",")


def apply_to_default(f):
    def make_keystroke_handler(tree, name, count):
        el = ElementTree.SubElement(
            tree.find(".//HandlerSets"),
            "HandlerSet",
            {"Name": name, "HelpString": name}
        )
        for i in range(count):
            handler = ElementTree.SubElement(el, "Handler", {"Class": "KeystrokeAssignment"})
            ElementTree.SubElement(
                handler,
                "Param",
                {"VirtualKey": "0", "LParam": "0", "Modifier": "0", "DisplayName": "0"}
            )

    def ignorehandler(element, seen=set()):
        helpstring = element.get("HelpString")
        if not helpstring: return True
        if helpstring in seen: return True
        else: seen.add(helpstring)

        name = element.get("Name")
        if not name: return True
        elif name.startswith("AppOverride_"): return True
        elif name.startswith("ButtonUsage"): return True
        elif name.startswith("FirstUsageSet"): return True
        else: return False

    tree = ElementTree.parse(f)
    handlersets = [e for e in tree.findall(".//HandlerSet[@HelpString][@Name]") if not ignorehandler(e)]
    handlersetnames = [e.get("Name") for e in handlersets]
    print "HandlerSet:", len(handlersets)
    print handlersetnames

    if "DoubleKeystroke" not in handlersetnames:
        make_keystroke_handler(tree, "DoubleKeystroke", 2)
        handlersetnames.append("DoubleKeystroke")
    if "TripleKeystroke" not in handlersetnames:
        make_keystroke_handler(tree, "TripleKeystroke", 3)
        handlersetnames.append("TripleKeystroke")

    attrval = ",".join(handlersetnames)
    groups = tree.find(".//HandlerSetGroups")
    sppelement = groups.find("HandlerSetGroup[@Name='SetPointPlus']")
    if sppelement is not None:
        groups.remove(sppelement)

    ElementTree.SubElement(
        groups,
        "HandlerSetGroup", {"Name": "SetPointPlus", "HandlerSetNames": attrval}
    )
    return tree


def apply_to_device(f):
    tree = ElementTree.parse(f)
    device = tree.find(".//Device")
    device.set("AppTier", "2")

    for button in tree.findall(".//Buttons/Button"):
        # ignore mouse left/right button
        # if you want to modify command of mouse left/right button,
        # you should not delete "Silent" attribute because these buttons become ineffective
        # some situation (UAC window, SetPoint main window close button)
        if button.get("Name") == "1" or button.get("Name") == "2":
            continue

        param = button.find("PARAM")
        if param is None:
            param = ElementTree.SubElement(button, "PARAM")
        param.set("AppSpecificSettingHidden", "0")

        # Trigger
        param = button.find("Trigger/PARAM")
        if param is not None and "Silent" in param.attrib:
            del param.attrib["Silent"]

        # TriggerState
        for state in button.findall("Trigger/TriggerState"):
            state.set("HandlerSetGroup", "SetPointPlus")
    return tree


def apply_to_strings(f):
    tree = ElementTree.parse(f)
    elements = [v for v in tree.findall(".//String") if v.get("ALIAS").startswith("HELP_")]
    countdict = {}
    for el in elements:
        if el.text in countdict:
            countdict[el.text] += 1
            el.text += " ({})".format(countdict[el.text])
        else:
            countdict[el.text] = 1
    return tree


def backup(filename):
    backupname = filename + ".bak"
    if not os.path.exists(backupname):
        shutil.copyfile(filename, backupname)


def restore(filename):
    backupname = filename + ".bak"
    if os.path.exists(backupname):
        shutil.copyfile(backupname, filename)
        os.remove(backupname)


def restore_all(info):
    restore(info[0] + "\\default.xml")
    restore(info[0] + "\\Strings.xml")
    for id in info[2]:
        restore(glob.glob("{0}\\*\\{1}\\{1}.xml".format(info[1], id))[0])


def apply_all(info):
    # default.xml
    defaultxml = info[0] + "\\default.xml"
    with open(defaultxml) as f:
        tree = apply_to_default(f)
    backup(defaultxml)
    tree.write(defaultxml, "utf-8")

    # Strings.xml
    fileenc = "ascii"  # use reading original file
    origenc = None  # from original file, use writing tree data
    stringxml = info[0] + "\\Strings.xml"
    with open(stringxml) as f:
        line = unicode(f.readline(), fileenc)
        m = re.search(ur'encoding=(["\'])(.+?)\1', line)
        if m:
            origenc = m.group(2)
            # use mbcs instead of sjis because UnicodeDecodeError
            fileenc = "mbcs" if origenc == "shift_jis" else origenc
        data = unicode(f.read(), fileenc)
    with closing(StringIO()) as f:
        # use utf-8 because ElementTree cant handle some sjis data
        f.write(u'<?xml version="1.0" encoding="utf-8"?>\n'.encode("utf-8"))
        f.write(data.encode("utf-8"))
        f.seek(0)
        tree = apply_to_strings(f)
    backup(stringxml)
    tree.write(stringxml, origenc)

    for id in info[2]:
        deviceidxml = glob.glob("{0}\\*\\{1}\\{1}.xml".format(info[1], id))[0]
        with open(deviceidxml) as f:
            tree = apply_to_device(f)
        backup(deviceidxml)
        tree.write(deviceidxml, "windows-1252", xml_declaration=1)

    # delete user.xml
    userxml = os.environ["APPDATA"] + "\\Logitech\\SetPoint\\user.xml"
    if os.path.exists(userxml):
        os.remove(userxml)


def usage():
    print "Note..."
    print "  * You should run this script as Administrator"
    print "  * Exit SetPoint before run this script"
    print "  * Your SetPoint setting will be reset"
    print "  * Before updating SetPoint, you should"
    print "    uninstall previous version and delete .bak files"
    print "Usage..."
    print "  SetPoint 6.x: setpointplus.py -v6"
    print "  SetPoint 4.x: setpointplus.py -v4"
    print "  Restore  6.x: setpointplus.py -v6 -r"
    print "  Restore  4.x: setpointplus.py -v4 -r"


def main():
    target = None
    r = False
    for a in sys.argv[1:]:
        if a == "-v6": target = 6
        elif a == "-v4": target = 4
        elif a == "-test": target = -1
        elif a == "-r": r = True
    if not target:
        usage()
        return
    info = getinfo(target)
    if r: restore_all(info)
    else: apply_all(info)


if __name__ == "__main__":
    main()
