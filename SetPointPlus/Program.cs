using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SetPointPlus
{
	static class Program
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			//if (MessageBox.Show("管理者権限で実行しましたか?", "管理者権限が必要です", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
			//    return;
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
