using System;
using System.Windows.Forms;
using static ScanFolderToFile.Utils.Utils;

namespace ScanFolderToFile.Forms.OptionsForms
{
    public partial class FrmListFilesDataDimension : Form
    {
        public string Result { get; private set; }

        public FrmListFilesDataDimension()
        {
            InitializeComponent();
        }

        private void checkedListBoxOptionsDates_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckOnlyOneElements(checkedListBoxOptionsDates, e);
        }

        private void FrmListFilesDataDimension_Load(object sender, System.EventArgs e)
        {
            monthCalendar.MaxDate = DateTime.Now;
        }

        private void cbDimension_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownFrom.Enabled = cbDimension.Checked;
            numericUpDownTo.Enabled = cbDimension.Checked;
            monthCalendar.Enabled = !cbDimension.Checked;
            checkedListBoxOptionsDates.Enabled = !cbDimension.Checked;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cbDimension.Checked)
            {
                //TODO: prendere dimensioni
            }
            else
            {
                //TODO: prendere DATE
            }





            //result = new MyData { v1 = "some string value", v2 = 123 }
            Result = ""; //TODO change in my object
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}