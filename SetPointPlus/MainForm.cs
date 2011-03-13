using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

using System.IO;
using System.Diagnostics;
using Microsoft.Win32;

namespace SetPointPlus
{
	// Icon について
	// Clockmaker Icon Generator で 16x16 ~ 128x128 の PNG を作成
	// @icon 変換で上記のアイコンをまとめる
	// IconFX で「アイコンへイメージを一括変換」する。16x16 ~ 256x256 までのアイコンが作成される
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			var devices = SetPointExtender.GetInstalledDevices();
			if (devices.Length == 0)
			{
				MessageBox.Show("Logitech (Logicool) デバイスがインストールされていません。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
				this.Close();
				return;
			}

			foreach (var dev in devices)
			{
				this.deviceCheckedListBox.Items.Add(dev);
			}
		}

		private void ApplyCore()
		{
			if (MessageBox.Show("本当に適用してもよろしいですか?", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
				return;

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

			MessageBox.Show("処理が完了しました。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
			if (MessageBox.Show("本当に復元してもよろしいですか?", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
				return;
			SetPointExtender.RestoreDefaultXml();
			SetPointExtender.RestoreDeviceXml();
			SetPointExtender.RestoreUserSettingFile(); // ユーザー設定ファイルを復元 (.bak ファイルを消す)

			if (deleteCheckBox.Checked)
				SetPointExtender.DeleteUserSettingFile(false); //ユーザーが選択していたら下のファイルも消す

			if (restartCheckBox.Checked)
				SetPointExtender.RestartSetPoint();

			MessageBox.Show("処理が完了しました。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
