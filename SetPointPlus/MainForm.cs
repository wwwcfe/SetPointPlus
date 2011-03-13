using System;
using System.Windows.Forms;

namespace SetPointPlus
{
	public partial class MainForm : Form
	{
		private readonly string Version6x = "SetPoint 6.x";
		private readonly string Version4x = "SetPoint 4.x";

		public MainForm()
		{
			InitializeComponent();

			comboBox.Items.Add(Version4x);
			comboBox.Items.Add(Version6x);

			SetPointExtender.InitializeForV6();
		}

		private static bool Confirm(string message)
		{
			return MessageBox.Show(message, Properties.Resources.Confirm,
				MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
		}

		private static void ShowInformation(string message)
		{
			MessageBox.Show(message, Properties.Resources.Information,
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void ApplyCore()
		{
			if (!Confirm(Properties.Resources.ConfirmApply)) return;

			SetPointExtender.ApplyToDefaultXml();
			SetPointExtender.ApplyToHelpString();

			foreach (var checkedDevice in deviceCheckedListBox.CheckedItems)
			{
				var devInfo = checkedDevice as DeviceInfo;
				SetPointExtender.ApplyToDeviceXml(devInfo);
			}

			if (deleteCheckBox.Checked)
				SetPointExtender.DeleteUserSettingFile(true);

			if (restartCheckBox.Checked)
				SetPointExtender.RestartSetPoint();

			ShowInformation(Properties.Resources.Processed);
		}

		private void RestoreCore()
		{
			if (!Confirm(Properties.Resources.ConfirmRestore)) return;

			SetPointExtender.RestoreDefaultXml();
			SetPointExtender.RestoreHelpString();
			SetPointExtender.RestoreDeviceXml();
			// ユーザー設定ファイルを復元 (.bak ファイルを消す)
			SetPointExtender.RestoreUserSettingFile();

			//ユーザーが選択していたら元のファイルも消す
			if (deleteCheckBox.Checked)
				SetPointExtender.DeleteUserSettingFile(false);

			if (restartCheckBox.Checked)
				SetPointExtender.RestartSetPoint();

			ShowInformation(Properties.Resources.Processed);
		}

		private void LoadDevices()
		{
			// 消しておく
			deviceCheckedListBox.Items.Clear();

			var devices = SetPointExtender.GetInstalledDevices();
			if (devices.Length == 0)
			{
				ShowInformation(Properties.Resources.DevicesAreNotInstalled);
				return;
			}

			foreach (var dev in devices)
			{
				this.deviceCheckedListBox.Items.Add(dev);
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
//            var devices = SetPointExtender.GetInstalledDevices();
//            if (devices.Length == 0)
//            {
//                ShowInformation(Properties.Resources.DevicesAreNotInstalled);
//#if !DEBUG
//                this.Close();
//#endif
//                return;
//            }

//            foreach (var dev in devices)
//            {
//                this.deviceCheckedListBox.Items.Add(dev);
//            }
		}

		private void applyButton_Click(object sender, EventArgs e)
		{
			this.Enabled = false;
			try
			{
				ApplyCore();
			}
			finally
			{
				this.Enabled = true;
			}
		}

		private void restoreButton_Click(object sender, EventArgs e)
		{
			this.Enabled = false;
			try
			{
				RestoreCore();
			}
			finally
			{
				this.Enabled = true;
			}
		}

		private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			string selectedItem = comboBox.SelectedItem as string;
			if (selectedItem.Equals(Version6x))
				SetPointExtender.InitializeForV6();
			else
				SetPointExtender.InitializeForV4();

			LoadDevices();
		}
	}
}
