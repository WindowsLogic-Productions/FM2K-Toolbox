using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FM2K_Toolbox
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        #region "File Menu"
        // Closes the application.
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
        #region "Help Menu"
        private void WhatDoesThisDoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("FM95/FM2K Toolbox is a little application for modifying the INI files of these old engines and in the future will be able to handle plug-ins for these engines. Pick a tab to display options pertaining to that engine or game executable.", "What does this do?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void WhatsNewToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion

        /* Plugins tab */

        // Browse files with .dll extension to add as plugin
        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a file";
            openFileDialog.Filter = "DLL files (*.dll)|*.dll";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {              
                foreach (var file in openFileDialog.FileNames)
                {
                    AddFileItem(file);

                    // TODO: add logic to save added files
                }
            }
        }

        // Add file to the panel
        private void AddFileItem(string filePath)
        {            
            Panel panel = new Panel();
            panel.Width = flowLayoutPanelFiles.ClientSize.Width - 25;
            panel.Height = 30;
            panel.Margin = new Padding(3);
      
            CheckBox checkBox = new CheckBox();
            checkBox.Width = 20;
            checkBox.Height = 20;
            checkBox.Location = new Point(5, 5);
           
            Label lbl = new Label();
            lbl.Text = System.IO.Path.GetFileName(filePath);
            lbl.AutoSize = false;
            lbl.Width = panel.Width - 30;
            lbl.Height = 25;
            lbl.Location = new Point(30, 5);
         
            panel.Controls.Add(checkBox);
            panel.Controls.Add(lbl);
         
            flowLayoutPanelFiles.Controls.Add(panel);
            UpdateRemoveButtonState();
        }

        // Remove selected files
        private void buttonRemove_Click(object sender, EventArgs e)
        {           
            var panelsToRemove = new List<Control>();

            foreach (Control ctrl in flowLayoutPanelFiles.Controls)
            {
                if (ctrl is Panel panel)
                {
                    foreach (Control child in panel.Controls)
                    {
                        if (child is CheckBox checkBox && checkBox.Checked)
                        {
                            panelsToRemove.Add(panel);
                            break;
                        }
                    }
                }
            }

            var result = ShowConfirmDialog(panelsToRemove.Count);

            if (result == DialogResult.Yes)
            {
                foreach (var panel in panelsToRemove)
                {
                    flowLayoutPanelFiles.Controls.Remove(panel);
                    panel.Dispose();
                }

                UpdateRemoveButtonState();
            }
        }

        // Update Remove Button State
        private void UpdateRemoveButtonState()
        {
            buttonRemove.Enabled = flowLayoutPanelFiles.Controls.Count > 0;
        }

        // show modal to confirm file removal
        private DialogResult ShowConfirmDialog(int panelsToRemoveCount)
        {
            if (panelsToRemoveCount == 0)
            {
                MessageBox.Show("Please select at least one file to remove.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return DialogResult.Cancel;
            }

            return MessageBox.Show(
                this,
                $"Are you sure you want to remove {panelsToRemoveCount} file(s)?",
                "Confirm Removal",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);           
        }
    }
}
