using System;
using System.Windows.Forms;

namespace SetPointPlus
{
	public partial class MainForm : Form
	{
		// 実装とインターフェースを分ける
		// デザインパターン Proxy?

		private readonly string Version6x = "SetPoint 6.x";
		private readonly string Version4x = "SetPoint 4.x";

		public MainForm()
		{
			InitializeComponent();

			comboBox.Items.Add(Version4x);
			comboBox.Items.Add(Version6x);

			switch (SetPointExtender.GetInstalledSetPointVersion())
			{
				case SetPointVersion.V6:
					comboBox.SelectedItem = Version6x;
					break;
				case SetPointVersion.V4:
					comboBox.SelectedItem = Version4x;
					break;
				default:
					// SetPoint 自体がないため、復元も適用もできない
					applyButton.Enabled = false;
					restoreButton.Enabled = false;
					deviceCheckedListBox.Enabled = false;
					ShowInformation(Properties.Resources.SetPointIsNotDetected);
					break;
			}
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
				// SetPoint がインストールされてないかもしれないので適用はできない
				// ただ、デバイスがインストールされていないだけかもしれないので復元だけはできるようにする
				applyButton.Enabled = false;
				restoreButton.Enabled = true;
				deviceCheckedListBox.Enabled = false;
				ShowInformation(Properties.Resources.DevicesAreNotInstalled);
				return;
			}

			// デバイスが見つかっ場合
			applyButton.Enabled = true;
			restoreButton.Enabled = true;
			deviceCheckedListBox.Enabled = true;

			foreach (var dev in devices)
			{
				this.deviceCheckedListBox.Items.Add(dev);
			}
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
