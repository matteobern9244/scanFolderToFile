using System;
using System.IO;
using static ScanFolderToFile.Constants;

namespace ScanFolderToFile.Utils
{
    public static class Utils
    {
        public static void CheckFolder()
        {
            try
            {
                if (Directory.Exists(PathFolder)) return;
                Directory.CreateDirectory(PathFolder);
                ScanFolderToFileLogger.Info(nameof(CheckFolder),
                    $"Non esiste la cartella di destinazione, creata cartella {PathFolder}");
            }
            catch (Exception ex)
            {
                ScanFolderToFileLogger.Error(ex, nameof(CheckFolder), $"Errore in creazione cartella {PathFolder}");
            }
        }
    }
}