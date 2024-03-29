﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;
using static ScanFolderToFile.Utils.Utils;

namespace ScanFolderToFile.Forms.OptionsForms
{
    public partial class FrmEditorFileTxt : Form
    {
        private readonly List<string> _colorList = new List<string>();
        private string _filename;    // file opened inside of RTB
        private const int Middle = 382;    // middle sum of RGB - max is 765
        private int _sumRgb;    // sum of the selected colors RGB
        private int _pos, _line, _column;    // for detecting line and column numbers

        public FrmEditorFileTxt()
        {
            InitializeComponent();
        }

        private void frmEditor_Load(object sender, EventArgs e)
        {
            richTextBox1.AllowDrop = true;     // to allow drag and drop to the RichTextBox
            richTextBox1.AcceptsTab = true;    // allow tab
            richTextBox1.WordWrap = false;    // disable word wrap on start
            richTextBox1.ShortcutsEnabled = true;    // allow shortcuts
            richTextBox1.DetectUrls = true;    // allow detect url
            fontDialog1.ShowColor = true;
            fontDialog1.ShowApply = true;
            fontDialog1.ShowHelp = true;
            colorDialog1.AllowFullOpen = true;
            colorDialog1.AnyColor = true;
            colorDialog1.SolidColorOnly = false;
            colorDialog1.ShowHelp = true;
            colorDialog1.AnyColor = true;
            leftAlignStripButton.Checked = true;
            centerAlignStripButton.Checked = false;
            rightAlignStripButton.Checked = false;
            boldStripButton3.Checked = false;
            italicStripButton.Checked = false;
            rightAlignStripButton.Checked = false;
            bulletListStripButton.Checked = false;
            wordWrapToolStripMenuItem.Image = null;
            MinimizeBox = false;
            MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // fill zoomDropDownButton item list
            zoomDropDownButton.DropDown.Items.Add("20%");
            zoomDropDownButton.DropDown.Items.Add("50%");
            zoomDropDownButton.DropDown.Items.Add("70%");
            zoomDropDownButton.DropDown.Items.Add("100%");
            zoomDropDownButton.DropDown.Items.Add("150%");
            zoomDropDownButton.DropDown.Items.Add("200%");
            zoomDropDownButton.DropDown.Items.Add("300%");
            zoomDropDownButton.DropDown.Items.Add("400%");
            zoomDropDownButton.DropDown.Items.Add("500%");

            // fill font sizes in combo box
            for (var i = 8; i < 80; i += 2)
            {
                fontSizeComboBox.Items.Add(i);
            }

            // fill colors in color drop down list
            foreach (var prop in typeof(Color).GetProperties())
            {
                if (prop.PropertyType.FullName == "System.Drawing.Color")
                {
                    _colorList.Add(prop.Name);
                }
            }

            // fill the drop down items list
            foreach (var color in _colorList)
            {
                colorStripDropDownButton.DropDownItems.Add(color);
            }

            // fill BackColor for each color in the DropDownItems list
            for (var i = 0; i < colorStripDropDownButton.DropDownItems.Count; i++)
            {
                // Create KnownColor object
                var selectedColor = (KnownColor)System.Enum.Parse(typeof(KnownColor), _colorList[i]); // parse to a KnownColor
                colorStripDropDownButton.DropDownItems[i].BackColor = Color.FromKnownColor(selectedColor);    // set the BackColor to its appropriate list item

                // Set the text color depending on if the barkground is darker or lighter
                // create Color object
                var col = Color.FromName(_colorList[i]);

                // 255,255,255 = White and 0,0,0 = Black
                // Max sum of RGB values is 765 -> (255 + 255 + 255)
                // Middle sum of RGB values is 382 -> (765/2)
                // Color is considered darker if its <= 382
                // Color is considered lighter if its > 382
                _sumRgb = ConvertToRgb(col);    // get the color objects sum of the RGB value
                if (_sumRgb <= Middle)    // Darker Background
                {
                    colorStripDropDownButton.DropDownItems[i].ForeColor = Color.White;    // set to White text
                }
                else if (_sumRgb > Middle)    // Lighter Background
                {
                    colorStripDropDownButton.DropDownItems[i].ForeColor = Color.Black;    // set to Black text
                }
            }

            // fill fonts in font combo box
            var fonts = new InstalledFontCollection();
            foreach (var family in fonts.Families)
            {
                fontStripComboBox.Items.Add(family.Name);
            }

            // determines the line and column numbers of mouse position on the richTextBox
            var pos = richTextBox1.SelectionStart;
            var line = richTextBox1.GetLineFromCharIndex(pos);
            var column = richTextBox1.SelectionStart - richTextBox1.GetFirstCharIndexFromLine(line);
            lineColumnStatusLabel.Text = @"Line " + (line + 1) + @", Column " + (column + 1);

            try
            {
                var pathFileTxt = GetPathFileTxt();
                if (!string.IsNullOrEmpty(pathFileTxt))
                {
                    richTextBox1.LoadFile(pathFileTxt, RichTextBoxStreamType.PlainText);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        //******************************************************************************************************************************
        // ConvertToRGB - Accepts a Color object as its parameter. Gets the RGB values of the object passed to it, calculates the sum. *
        //******************************************************************************************************************************
        private static int ConvertToRgb(Color c)
        {
            int r = c.R, // RED component value
                g = c.G, // GREEN component value
                b = c.B; // BLUE component value

            var sum =
                // calculate sum of RGB
                r + g + b;

            return sum;
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();     // select all text
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // clear
            richTextBox1.Clear();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();     // paste text
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();      // copy text
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();     // cut text
        }

        private void boldStripButton3_Click(object sender, EventArgs e)
        {
            switch (boldStripButton3.Checked)
            {
                case false:
                    boldStripButton3.Checked = true; // BOLD is true
                    break;
                case true:
                    boldStripButton3.Checked = false;    // BOLD is false
                    break;
            }

            if (richTextBox1.SelectionFont == null)
            {
                return;
            }

            // create fontStyle object
            var style = richTextBox1.SelectionFont.Style;

            // determines the font style
            if (richTextBox1.SelectionFont.Bold)
            {
                style &= ~FontStyle.Bold;
            }
            else
            {
                style |= FontStyle.Bold;

            }
            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, style);     // sets the font style
        }

        private void underlineStripButton_Click(object sender, EventArgs e)
        {
            switch (underlineStripButton.Checked)
            {
                case false:
                    underlineStripButton.Checked = true;     // UNDERLINE is active
                    break;
                case true:
                    underlineStripButton.Checked = false;    // UNDERLINE is not active
                    break;
            }

            if (richTextBox1.SelectionFont == null)
            {
                return;
            }

            // create fontStyle object
            var style = richTextBox1.SelectionFont.Style;

            // determines the font style
            if (richTextBox1.SelectionFont.Underline)
            {
                style &= ~FontStyle.Underline;
            }
            else
            {
                style |= FontStyle.Underline;
            }
            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, style);    // sets the font style
        }

        private void italicStripButton_Click(object sender, EventArgs e)
        {
            switch (italicStripButton.Checked)
            {
                case false:
                    italicStripButton.Checked = true;    // ITALICS is active
                    break;
                case true:
                    italicStripButton.Checked = false;    // ITALICS is not active
                    break;
            }

            if (richTextBox1.SelectionFont == null)
            {
                return;
            }

            // create fontStyle object
            var style = richTextBox1.SelectionFont.Style;

            // determines font style
            if (richTextBox1.SelectionFont.Italic)
            {
                style &= ~FontStyle.Italic;
            }
            else
            {
                style |= FontStyle.Italic;
            }
            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, style);    // sets the font style
        }

        private void fontSizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (richTextBox1.SelectionFont == null)
            {
                return;
            }
            // sets the font size when changed
            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont.FontFamily, Convert.ToInt32(fontSizeComboBox.Text), richTextBox1.SelectionFont.Style);
        }

        private void fontStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (richTextBox1.SelectionFont == null)
            {
                // sets the Font Family style
                richTextBox1.SelectionFont = new Font(fontStripComboBox.Text, richTextBox1.Font.Size);
            }
            // sets the selected font famly style
            richTextBox1.SelectionFont = new Font(fontStripComboBox.Text, richTextBox1.SelectionFont.Size);
        }

        private void saveStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;
                var filename = saveFileDialog1.FileName;
                // save the contents of the rich text box
                richTextBox1.SaveFile(filename, RichTextBoxStreamType.PlainText);
                var file = Path.GetFileName(filename);
                MessageBox.Show(@"File " + file + @" è stato salvato con successo.", @"File salvato", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void openFileStripButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            _filename = openFileDialog1.FileName;
            // load the file into the richTextBox
            richTextBox1.LoadFile(_filename, RichTextBoxStreamType.PlainText);    // loads it in regular text format
        }

        private void colorStripDropDownButton_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // creates a KnownColor object
            var selectedColor = (KnownColor)Enum.Parse(typeof(KnownColor), e.ClickedItem.Text); // converts it to a Color Structure
            richTextBox1.SelectionColor = Color.FromKnownColor(selectedColor);    // sets the selected color
        }

        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            // highlight button border when buttons are true
            if (richTextBox1.SelectionFont == null) return;
            boldStripButton3.Checked = richTextBox1.SelectionFont.Bold;
            italicStripButton.Checked = richTextBox1.SelectionFont.Italic;
            underlineStripButton.Checked = richTextBox1.SelectionFont.Underline;
        }

        private void leftAlignStripButton_Click(object sender, EventArgs e)
        {
            // set properties
            centerAlignStripButton.Checked = false;
            rightAlignStripButton.Checked = false;
            switch (leftAlignStripButton.Checked)
            {
                case false:
                    leftAlignStripButton.Checked = true;    // LEFT ALIGN is active
                    break;
                case true:
                    leftAlignStripButton.Checked = false;    // LEFT ALIGN is not active
                    break;
            }
            richTextBox1.SelectionAlignment = HorizontalAlignment.Left;    // selects left alignment
        }

        private void centerAlignStripButton_Click(object sender, EventArgs e)
        {
            // set properties
            leftAlignStripButton.Checked = false;
            rightAlignStripButton.Checked = false;
            switch (centerAlignStripButton.Checked)
            {
                case false:
                    centerAlignStripButton.Checked = true;    // CENTER ALIGN is active
                    break;
                case true:
                    centerAlignStripButton.Checked = false;    // CENTER ALIGN is not active
                    break;
            }
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;     // selects center alignment
        }

        private void rightAlignStripButton_Click(object sender, EventArgs e)
        {
            // set properties
            leftAlignStripButton.Checked = false;
            centerAlignStripButton.Checked = false;

            switch (rightAlignStripButton.Checked)
            {
                case false:
                    rightAlignStripButton.Checked = true;    // RIGHT ALIGN is active
                    break;
                case true:
                    rightAlignStripButton.Checked = false;    // RIGHT ALIGN is not active
                    break;
            }
            richTextBox1.SelectionAlignment = HorizontalAlignment.Right;    // selects right alignment
        }

        private void bulletListStripButton_Click(object sender, EventArgs e)
        {
            switch (bulletListStripButton.Checked)
            {
                case false:
                    bulletListStripButton.Checked = true;
                    richTextBox1.SelectionBullet = true;    // BULLET LIST is active
                    break;
                case true:
                    bulletListStripButton.Checked = false;
                    richTextBox1.SelectionBullet = false;    // BULLET LIST is not active
                    break;
            }
        }

        private void increaseStripButton_Click(object sender, EventArgs e)
        {
            var fontSizeNum = fontSizeComboBox.Text;    // variable to hold selected size         
            try
            {
                var size = Convert.ToInt32(fontSizeNum) + 1;    // convert (fontSizeNum + 1)
                if (richTextBox1.SelectionFont == null)
                {
                    return;
                }
                // sets the updated font size
                richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont.FontFamily, size, richTextBox1.SelectionFont.Style);
                fontSizeComboBox.Text = size.ToString();    // update font size
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning); // show error message
            }
        }

        private void decreaseStripButton_Click(object sender, EventArgs e)
        {
            var fontSizeNum = fontSizeComboBox.Text;    // variable to hold selected size            
            try
            {
                var size = Convert.ToInt32(fontSizeNum) - 1;    // convert (fontSizeNum - 1)
                if (richTextBox1.SelectionFont == null)
                {
                    return;
                }
                // sets the updated font size
                richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont.FontFamily, size, richTextBox1.SelectionFont.Style);
                fontSizeComboBox.Text = size.ToString();    // update font size
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning); // show error message
            }
        }

        //*********************************************************************************************
        // richTextBox1_DragEnter - Custom Event. Copies text being dragged into the richTextBox      *
        //*********************************************************************************************
        private void richTextBox1_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.Text) ? DragDropEffects.Copy : // copies data to the RTB
                DragDropEffects.None; // doesn't accept data into RTB
        }
        //***************************************************************************************************
        // richTextBox1_DragEnter - Custom Event. Drops the copied text being dragged onto the richTextBox  *
        //***************************************************************************************************
        private void richTextBox1_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            // Get start position to drop the text.
            var i = richTextBox1.SelectionStart;
            var s = richTextBox1.Text.Substring(i);
            richTextBox1.Text = richTextBox1.Text.Substring(0, i);

            // Drop the text on to the RichTextBox.
            richTextBox1.Text += e.Data.GetData(DataFormats.Text).ToString();
            richTextBox1.Text += s;
        }

        private void undoStripButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();     // undo move
        }

        private void redoStripButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();    // redo move
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();     // close the form
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();     // undo move
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();     // redo move
        }

        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();     // cut text
        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            richTextBox1.Copy();     // copy text
        }

        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();    // paste text
        }

        private void selectAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();    // select all text
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // clear the rich text box
            richTextBox1.Clear();
            richTextBox1.Focus();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // delete selected text
            richTextBox1.SelectedText = "";
            richTextBox1.Focus();
        }

        private void OpenMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.LoadFile(openFileDialog1.FileName, RichTextBoxStreamType.PlainText);
            }
        }

        private void newMenuItem_Click(object sender, EventArgs e)
        {

            if (richTextBox1.Text != string.Empty)    // RTB has contents - prompt user to save changes
            {
                // save changes message
                var result = MessageBox.Show(@"Vuoi salvare le tue modifiche? L'editor non è vuoto.", @"Salva le modifiche?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                switch (result)
                {
                    case DialogResult.Yes:
                        {
                            // save the RTB contents if user selected yes
                            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                            {
                                var filename = saveFileDialog1.FileName;
                                // save the contents of the rich text box
                                richTextBox1.SaveFile(filename, RichTextBoxStreamType.PlainText);
                                var file = Path.GetFileName(filename);
                                MessageBox.Show(@"File " + file + @" è stato salvato con successo.", @"File salvato", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                            // finally - clear the contents of the RTB 
                            richTextBox1.ResetText();
                            richTextBox1.Focus();
                            break;
                        }
                    case DialogResult.No:
                        // clear the contents of the RTB 
                        richTextBox1.ResetText();
                        richTextBox1.Focus();
                        break;
                }
            }
            else // RTB has no contents
            {
                // clear the contents of the RTB 
                richTextBox1.ResetText();
                richTextBox1.Focus();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var filename = saveFileDialog1.FileName;
                // save the contents of the rich text box
                richTextBox1.SaveFile(filename, RichTextBoxStreamType.PlainText);
            }
            var file = Path.GetFileName(_filename); // get name of file
            MessageBox.Show(@"File " + file + @" è stato salvato con successo.", @"File salvato", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void zoomDropDownButton_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var zoomPercent = Convert.ToSingle(e.ClickedItem.Text.Trim('%')); // convert
            richTextBox1.ZoomFactor = zoomPercent / 100;    // set zoom factor

            if (e.ClickedItem.Image == null)
            {
                // sets all the image properties to null - incase one is already selected beforehand
                for (var i = 0; i < zoomDropDownButton.DropDownItems.Count; i++)
                {
                    zoomDropDownButton.DropDownItems[i].Image = null;
                }

                // draw bmp in image property of selected item, while its active
                var bmp = new Bitmap(5, 5);
                using (var gfx = Graphics.FromImage(bmp))
                {
                    gfx.FillEllipse(Brushes.Black, 1, 1, 3, 3);
                }
                e.ClickedItem.Image = bmp;    // draw ellipse in image property
            }
            else
            {
                e.ClickedItem.Image = null;
                richTextBox1.ZoomFactor = 1.0f;    // set back to NO ZOOM
            }
        }

        private void uppercaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectedText = richTextBox1.SelectedText.ToUpper();    // text to CAPS
        }

        private void lowercaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectedText = richTextBox1.SelectedText.ToLower();    // text to lowercase
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // draw bmp in image property of selected item, while its active
            var bmp = new Bitmap(5, 5);
            using (var gfx = Graphics.FromImage(bmp))
            {
                gfx.FillEllipse(Brushes.Black, 1, 1, 3, 3);
            }

            switch (richTextBox1.WordWrap)
            {
                case false:
                    richTextBox1.WordWrap = true;    // WordWrap is active
                    wordWrapToolStripMenuItem.Image = bmp;    // draw ellipse in image property
                    break;
                case true:
                    richTextBox1.WordWrap = false;    // WordWrap is not active
                    wordWrapToolStripMenuItem.Image = null;    // clear image property
                    break;
            }
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var oldFont = this.Font;    // gets current font

                if (fontDialog1.ShowDialog() == DialogResult.OK)
                {
                    fontDialog1_Apply(richTextBox1, EventArgs.Empty);
                }
                // set back to the recent font
                else if (fontDialog1.ShowDialog() == DialogResult.Cancel)
                {
                    // set current font back to the old font
                    this.Font = oldFont;

                    // sets the old font for the controls inside richTextBox1
                    foreach (Control containedControl in richTextBox1.Controls)
                    {
                        containedControl.Font = oldFont;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning); // error
            }
        }

        private void fontDialog1_HelpRequest(object sender, EventArgs e)
        {
            // display HelpRequest message
            MessageBox.Show(@"Scegliere un font e qualsiasi altro attributo, quindi premere Applica e OK.", @"Richiesta di aiuto della finestra di dialogo dei font", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void fontDialog1_Apply(object sender, EventArgs e)
        {
            fontDialog1.FontMustExist = true;    // error if font doesn't exist

            richTextBox1.Font = fontDialog1.Font;    // set selected font (Includes: FontFamily, Size, and, Effect. No need to set them separately)
            richTextBox1.ForeColor = fontDialog1.Color;    // set selected font color

            // sets the font for the controls inside richTextBox1
            foreach (Control containedControl in richTextBox1.Controls)
            {
                containedControl.Font = fontDialog1.Font;
            }
        }

        private void deleteStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectedText = ""; // delete selected text
        }

        private void clearFormattingStripButton_Click(object sender, EventArgs e)
        {
            fontStripComboBox.Text = "Font Family";
            fontSizeComboBox.Text = "Font Size";
            var pureText = richTextBox1.Text;    // get the current Plain Text     
            richTextBox1.Clear();    // clear RTB
            richTextBox1.ForeColor = Color.Black;    // ensure the text color is back to Black
            richTextBox1.Font = default(Font);    // set default font
            richTextBox1.Text = pureText;    // Set it back to its orginial text, added as plain text
            rightAlignStripButton.Checked = false;
            centerAlignStripButton.Checked = false;
            leftAlignStripButton.Checked = true;
        }

        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // draws the string onto the print document
            e.Graphics.DrawString(richTextBox1.Text, richTextBox1.Font, Brushes.Black, 100, 20);
            e.Graphics.PageUnit = GraphicsUnit.Inch;
        }

        private void printStripButton_Click(object sender, EventArgs e)
        {
            // printDialog associates with PrintDocument
            printDialog.Document = printDocument;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print(); // Print the document
            }
        }

        private void printPreviewStripButton_Click(object sender, EventArgs e)
        {
            printPreviewDialog.Document = printDocument;
            // Show PrintPreview Dialog 
            printPreviewDialog.ShowDialog();
        }

        private void printStripMenuItem_Click(object sender, EventArgs e)
        {
            // printDialog associates with PrintDocument
            printDialog.Document = printDocument;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
        }

        private void printPreviewStripMenuItem_Click(object sender, EventArgs e)
        {
            printPreviewDialog.Document = printDocument;
            // Show PrintPreview Dialog 
            printPreviewDialog.ShowDialog();
        }

        private void colorDialog1_HelpRequest(object sender, EventArgs e)
        {
            // display HelpRequest message
            MessageBox.Show(@"Selezionare un colore facendo clic su di esso. Questo cambierà il colore del testo.", @"Richiesta di aiuto per la finestra di dialogo dei colori", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void colorOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                // set the selected color to the RTB's forecolor
                richTextBox1.ForeColor = colorDialog1.Color;
            }
        }

        //****************************************************************************************************************************************
        // richTextBox1_KeyUp - Determines which key was released and gets the line and column numbers of the current cursor position in the RTB *
        //**************************************************************************************************************************************** 
        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            // determine key released
            switch (e.KeyCode)
            {
                case Keys.Down:
                    _pos = richTextBox1.SelectionStart;    // get starting point
                    _line = richTextBox1.GetLineFromCharIndex(_pos);    // get line number
                    _column = richTextBox1.SelectionStart - richTextBox1.GetFirstCharIndexFromLine(_line);    // get column number
                    lineColumnStatusLabel.Text = @"Line " + (_line + 1) + @", Column " + (_column + 1);
                    break;
                case Keys.Right:
                    _pos = richTextBox1.SelectionStart; // get starting point
                    _line = richTextBox1.GetLineFromCharIndex(_pos); // get line number
                    _column = richTextBox1.SelectionStart - richTextBox1.GetFirstCharIndexFromLine(_line);    // get column number
                    lineColumnStatusLabel.Text = @"Line " + (_line + 1) + @", Column " + (_column + 1);
                    break;
                case Keys.Up:
                    _pos = richTextBox1.SelectionStart; // get starting point
                    _line = richTextBox1.GetLineFromCharIndex(_pos); // get line number
                    _column = richTextBox1.SelectionStart - richTextBox1.GetFirstCharIndexFromLine(_line);    // get column number
                    lineColumnStatusLabel.Text = @"Line " + (_line + 1) + @", Column " + (_column + 1);
                    break;
                case Keys.Left:
                    _pos = richTextBox1.SelectionStart; // get starting point
                    _line = richTextBox1.GetLineFromCharIndex(_pos); // get line number
                    _column = richTextBox1.SelectionStart - richTextBox1.GetFirstCharIndexFromLine(_line);    // get column number
                    lineColumnStatusLabel.Text = @"Line " + (_line + 1) + @", Column " + (_column + 1);
                    break;
            }
        }

        //****************************************************************************************************************************
        // richTextBox1_MouseDown - Gets the line and column numbers of the cursor position in the RTB when the mouse clicks an area *
        //****************************************************************************************************************************
        private void richTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            var pos = richTextBox1.SelectionStart;    // get starting point
            var line = richTextBox1.GetLineFromCharIndex(pos);    // get line number
            var column = richTextBox1.SelectionStart - richTextBox1.GetFirstCharIndexFromLine(line);    // get column number
            lineColumnStatusLabel.Text = @"Line " + (line + 1) + @", Column " + (column + 1);
        }
    }
}