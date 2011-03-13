set PATH="C:\Program Files\WinRAR";%PATH%

if exist "SetPointPlus.zip" del "SetPointPlus.zip"
winrar a -m5 -afzip -esh "SetPointPlus.zip" docs\*.txt v4.bat v6.bat SetPointPlus.exe SetPointPlus.exe.config