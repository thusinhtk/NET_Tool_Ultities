using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
    }
}
