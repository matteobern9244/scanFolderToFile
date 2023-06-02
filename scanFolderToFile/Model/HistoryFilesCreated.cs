using Newtonsoft.Json;
using System;

namespace ScanFolderToFile.Model
{
    public class HistoryFilesCreated
    {
        [JsonProperty(PropertyName = "Nome File")]
        public string NameFile { get; set; }

        [JsonProperty(PropertyName = "Estensione File")]
        public string ExtensionFile { get; set; }

        [JsonProperty(PropertyName = "Data Creazione")]
        public DateTime CreatedAt { get; set; }
    }
}