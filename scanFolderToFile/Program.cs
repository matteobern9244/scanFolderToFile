﻿using System;
using System.Windows.Forms;
using ScanFolderToFile.Forms;

namespace ScanFolderToFile
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmPrincipal());
        }
    }
}