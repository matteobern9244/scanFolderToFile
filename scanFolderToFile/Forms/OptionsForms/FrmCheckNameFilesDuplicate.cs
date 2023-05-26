using ScanFolderToFile.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ScanFolderToFile.Forms.OptionsForms
{
    public partial class FrmCheckNameFilesDuplicate : Form
    {
        public List<IGrouping<string, KeyValuePair<string, string>>> NameFilesDuplicate { private get; set; }

        public FrmCheckNameFilesDuplicate(List<IGrouping<string, KeyValuePair<string, string>>> nameFilesDuplicate)
        {
            InitializeComponent();
            NameFilesDuplicate = nameFilesDuplicate;
        }

        private void FrmCheckNameFilesDuplicate_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (!NameFilesDuplicate.Any()) return;
                foreach (var keyValue in NameFilesDuplicate
                             .SelectMany(nameFileDuplicate => nameFileDuplicate.ToList()))
                    lvNameFilesDuplicate.Items.Add(new ListViewItem($"{keyValue.Key}{keyValue.Value}"));
            }
            catch (Exception ex)
            {
                ScanFolderToFileLogger.Error(ex, nameof(FrmCheckNameFilesDuplicate_Load), "Errore in caricamento dei nomi dei files duplicati.");
            }
        }
    }
}