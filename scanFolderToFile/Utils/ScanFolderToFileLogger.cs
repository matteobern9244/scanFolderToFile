using NLog;
using System;
using System.Windows.Forms;

namespace ScanFolderToFile.Utils
{
    public static class ScanFolderToFileLogger
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public static void Info(string method, string message)
        {
            Logger.Info($"{method} - {message}");
        }

        public static void Error(Exception ex, string method, string message)
        {
            Logger.Error(ex, $"Error in {method} - {ex.Message}");
            MessageBox.Show($@" {message} - {ex.Message}");
        }
    }
}