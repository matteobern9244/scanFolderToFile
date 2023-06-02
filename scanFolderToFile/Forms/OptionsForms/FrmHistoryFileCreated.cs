﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ScanFolderToFile.Model;

namespace ScanFolderToFile.Forms.OptionsForms
{
    public partial class FrmHistoryFileCreated : Form
    {
        private readonly List<HistoryFilesCreated> _historyFilesCreated;

        public FrmHistoryFileCreated(List<HistoryFilesCreated> historyFilesCreated)
        {
            _historyFilesCreated = historyFilesCreated;
            InitializeComponent();
        }

        private void FrmHistoryFileCreated_Load(object sender, EventArgs e)
        {
            if (!_historyFilesCreated.Any()) return;
            var contentTxt = _historyFilesCreated.Select(filesCreated => $"{filesCreated.NameFile} {filesCreated.ExtensionFile} {filesCreated.CreatedAt}").ToList();
            textBoxContent.Text = string.Join(Environment.NewLine, contentTxt);
        }
    }
}
