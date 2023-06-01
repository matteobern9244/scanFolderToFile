using System;
using System.Security.Cryptography;
using System.Windows.Forms;
using static ScanFolderToFile.Utils.Utils;

namespace ScanFolderToFile.Forms.OptionsForms
{
    public partial class FrmEditFileTxt : Form
    {
        public FrmEditFileTxt()
        {
            InitializeComponent();
        }

        private void FrmEditFileTxt_Load(object sender, System.EventArgs e)
        {
            try
            {
                richTextBoxFileTxt.LoadFile(GetPathFileTxt(), RichTextBoxStreamType.PlainText);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
