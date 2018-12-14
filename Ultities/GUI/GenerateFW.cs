using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ultities.Helper;
using static Ultities.Helper.GenerateFW_Lists;

namespace Ultities.GUI
{
    public partial class GenerateFW : Form
    {
        private static GenerateFW generateFWForm = null;

        static GenerateFW_Lists g_FWLists = new GenerateFW_Lists();

        public GenerateFW()
        {
            InitializeComponent();
            generateFWForm = this;
        }

        public static bool IsGen93Checked()
        {
            return generateFWForm.chkbxIsGen93.Checked;
        }

        private void GenerateFW_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = GenerateFW_Lists._dt;
        }

        private void export2Excel_Click(object sender, EventArgs e)
        {
            g_FWLists.Export2Excel();
        }
    }
}
