﻿using System;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using System.Diagnostics;

namespace SetPointPlus
{
	static class Program
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			// 言語の変更。とりあえず。
			if (args.Length > 0 && args[0].StartsWith("/lang:"))
			{
				var lang = args[0].Substring(6);
				Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
			}

			Debug.Listeners.Add(new TextWriterTraceListener("debug.log"));
			Debug.AutoFlush = true;

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
