using NLog;
using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace ScanFolderToFile.Utils
{
    public static class ScanFolderToFileLogger
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public static void Info(string method, string message,
            bool showMessageBox = false,
            string caption = "",
            MessageBoxButtons messageBoxButtons = MessageBoxButtons.OK)
        {
            Logger.Info($"{method} - {message}");
            if (showMessageBox)
                MessageBox.Show($@"{message}", caption, messageBoxButtons);
        }

        public static void Error(Exception ex, string method, string message,
            string caption = Constants.Error, MessageBoxButtons messageBoxButtons = MessageBoxButtons.OK)
        {
            Logger.Error(ex, $"Error in {method} - {ex.Message}");
            MessageBox.Show($@"{message} - {ex.Message}", caption, messageBoxButtons);
        }

        public static void Fatal(Exception ex, string method, string message,
            string caption = Constants.Fatal, MessageBoxButtons messageBoxButtons = MessageBoxButtons.OK)
        {
            Logger.Fatal(ex, $"Fatal in {method} - {ex.Message}");
            MessageBox.Show($@"{message} - {ex.Message}", caption, messageBoxButtons);
        }

        public static void Warn(Exception ex, string method, string message,
            string caption = Constants.Warn, MessageBoxButtons messageBoxButtons = MessageBoxButtons.OK)
        {
            Logger.Warn(ex, $"Warn in {method} - {ex.Message}");
            MessageBox.Show($@"{message} - {ex.Message}", caption, messageBoxButtons);
        }
    }
}