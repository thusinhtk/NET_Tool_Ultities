using System;
using System.Windows.Forms;

using Ultities.GUI;

namespace Ultities
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void generateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateDBC frmGenerate = new GenerateDBC();
            frmGenerate.ShowDialog();
        }

        private void dBCToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Are you sure to exit application", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void compareWithCanMatrixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateFW frmGenerateFW = new GenerateFW();
            frmGenerateFW.ShowDialog();
        }
    }
}
