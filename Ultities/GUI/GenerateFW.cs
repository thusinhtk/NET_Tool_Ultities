using System;
using System.Drawing;
using System.Windows.Forms;

using Ultities.Helper;

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
            g_FWLists.CreateDataTable();

            dataGridView1.DataSource = g_FWLists.Dt;
            dataGridView1.Refresh();
        }

        private void export2Excel_Click(object sender, EventArgs e)
        {
            g_FWLists.Export2Excel();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            //row.DefaultCellStyle.BackColor = Color.Blue;
        }
    }
}
