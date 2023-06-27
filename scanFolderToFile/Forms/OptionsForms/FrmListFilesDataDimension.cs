using ScanFolderToFile.Model;
using System;
using System.Windows.Forms;

namespace ScanFolderToFile.Forms.OptionsForms
{
    public partial class FrmListFilesDataDimension : Form
    {
        public ListFilesFromDimensionOrDates Result { get; private set; }

        public FrmListFilesDataDimension()
        {
            InitializeComponent();
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
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var dimensionOrDates = new ListFilesFromDimensionOrDates();
            if (cbDimension.Checked)
            {
                dimensionOrDates.Dimensions = new Dimensions
                {
                    DimensionMin = numericUpDownFrom.Value,
                    DimensionMax = numericUpDownTo.Value
                };
            }
            else
            {
                dimensionOrDates.Dates = new Dates()
                {
                    StartDate = monthCalendar.SelectionStart.Date,
                    EndDate = monthCalendar.SelectionEnd.Date
                };
            }

            Result = dimensionOrDates;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}