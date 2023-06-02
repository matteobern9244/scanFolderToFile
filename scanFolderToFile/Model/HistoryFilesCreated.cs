using System;

namespace ScanFolderToFile.Model
{
    public class HistoryFilesCreated
    {
        public class FileCreated
        {
            public string NameFile { get; set; }
            public string ExtensionFile { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }
}
