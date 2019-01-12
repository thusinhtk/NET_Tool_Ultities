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



        public GenerateFW(int failedThreshold)
        {
            InitializeComponent();
            generateFWForm = this;
            g_FWLists.FailedThreshold = failedThreshold;
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
            bool result = g_FWLists.Export2Excel();
            if (result)
            {
                MessageBox.Show("Create FW list successful!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error when generating FW list file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            //row.DefaultCellStyle.BackColor = Color.Blue;
        }

        private void tbFailedThreshold_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9'))
                e.Handled = true;
        }

        private void tbFailedThreshold_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TextBox currenttb = (TextBox)sender;
            int temp = Int32.Parse(currenttb.Text);

            if (currenttb.Text == "")
            {
                MessageBox.Show(string.Format("Failed Threshold must be not null"));
                e.Cancel = true;
            }
            else if (temp <= 0 || temp > 50)
            {
                MessageBox.Show(string.Format("Failed Threshold range is must be greater than 0 and less than 50"));
                e.Cancel = true;
            }

            else
            {
                e.Cancel = false;
            }
        }

        private void tbFailedThreshold_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
