using System;
using System.Windows.Forms;

namespace SetPointPlus
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
#if DEBUG
			System.Threading.Thread.CurrentThread.CurrentUICulture =
				new System.Globalization.CultureInfo("en");
#endif
			InitializeComponent();
		}

		private bool Confirm(string message)
		{
			return MessageBox.Show(message, Properties.Resources.Confirm,
				MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
		}

		private void ShowInformation(string message)
		{
			MessageBox.Show(message, Properties.Resources.Information,
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			var devices = SetPointExtender.GetInstalledDevices();
			if (devices.Length == 0)
			{
				ShowInformation(Properties.Resources.DevicesAreNotInstalled);
#if !DEBUG
				this.Close();
#endif
				return;
			}

			foreach (var dev in devices)
			{
				this.deviceCheckedListBox.Items.Add(dev);
			}
		}

		private void ApplyCore()
		{
			if (!Confirm(Properties.Resources.BeforeApply)) return;

			SetPointExtender.ApplyToDefaultXml();

			foreach (var checkedDevice in deviceCheckedListBox.CheckedItems)
			{
				DeviceInfo devInfo = checkedDevice as DeviceInfo;
				SetPointExtender.ApplyToDeviceXml(devInfo);
			}

			if (deleteCheckBox.Checked)
				SetPointExtender.DeleteUserSettingFile(true);

			if (restartCheckBox.Checked)
				SetPointExtender.RestartSetPoint();

			ShowInformation(Properties.Resources.Processed);
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

		private void RestoreCore()
		{
			if (!Confirm(Properties.Resources.BeforeRestore)) return;

			SetPointExtender.RestoreDefaultXml();
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
	}
}
