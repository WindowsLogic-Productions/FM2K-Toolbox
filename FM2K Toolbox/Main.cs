using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
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
    }
}
