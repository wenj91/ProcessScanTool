﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace 进程线程检测
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new 线程进程检测());
        }
    }
}