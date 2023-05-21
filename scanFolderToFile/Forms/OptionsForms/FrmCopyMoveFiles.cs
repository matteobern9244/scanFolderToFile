using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using static ScanFolderToFile.Utils.Utils;

namespace ScanFolderToFile.Forms.OptionsForms
{
    public partial class FrmCopyMoveFiles : Form
    {
        public FrmCopyMoveFiles()
        {
            InitializeComponent();
        }

        private void checkedListBoxOptions_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckOnlyOneElements(e);
        }

        private void CheckOnlyOneElements(ItemCheckEventArgs e)
        {
            for (var ix = 0; ix < checkedListBoxOptions.Items.Count; ++ix)
                if (ix != e.Index)
                    checkedListBoxOptions.SetItemChecked(ix, false);
        }

        private void btnExec_Click(object sender, System.EventArgs e)
        {
            if (checkedListBoxOptions.CheckedItems.Count <= 0) return;
            foreach (var itemChecked in checkedListBoxOptions.CheckedItems)
            {
                switch (itemChecked.ToString())
                {
                    case Constants.Copy:
                        CopyContentFolders();
                        break;
                    case Constants.Move:
                        MoveContentFolders();
                        break;
                }
            }
        }

        private void FrmCopyMoveFiles_Load(object sender, System.EventArgs e)
        {
            checkedListBoxOptions.DataSource = new List<string>
            {
                Constants.Copy,
                Constants.Move
            };
        }

        private void CopyContentFolders()
        {
            if (!string.IsNullOrEmpty(txtSource.Text) && !string.IsNullOrEmpty(txtSource.Text))
                CopyFilesRecursively(txtSource.Text, txtDestination.Text);
        }

        private void MoveContentFolders()
        {
            if (!string.IsNullOrEmpty(txtSource.Text) && !string.IsNullOrEmpty(txtDestination.Text))
                MoveAllContentFolders(txtSource.Text, txtDestination.Text);
        }

        private void btnSfogliaSrc_Click(object sender, System.EventArgs e)
        {
            if (folderBrowserDialogSource.ShowDialog() != DialogResult.OK) return;
            txtSource.Text = folderBrowserDialogSource.SelectedPath;
        }

        private void btnSfogliaDest_Click(object sender, System.EventArgs e)
        {
            if (folderBrowserDialogDestination.ShowDialog() != DialogResult.OK) return;
            txtDestination.Text = folderBrowserDialogDestination.SelectedPath;
        }

        private void btnOpenFolderDestionation_Click(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDestination.Text))
                OpenFolder(txtDestination.Text);
        }
    }
}