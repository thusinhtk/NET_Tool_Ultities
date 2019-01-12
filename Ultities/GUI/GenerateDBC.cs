using System;
using System.Windows.Forms;

using Ultities.BLL;
using Ultities.GUI;
using static Ultities.Logger.Logger;

namespace Ultities
{
    public partial class GenerateDBC : Form
    {
        private static GenerateDBC generateDBCForm = null;

        static string ExcelFilePath;

        static BLL_Process prcs = new BLL_Process();

        public static void SetTextInfo(string str)
        {
            if (generateDBCForm != null)
                generateDBCForm.richTextBox1.AppendText(str + '\n');
        }
        public static void ClearTextInfo()
        {
            if (generateDBCForm != null)
                generateDBCForm.richTextBox1.Text = "";
        }

        public static void SetMaxProgressBar(int value)
        {
            generateDBCForm.toolStripProgressBar1.Maximum = value;
        }

        public static void SetProgressBarStatus(double value)
        {
            generateDBCForm.toolStripProgressBar1.Value = Convert.ToInt32(value);

            if (generateDBCForm.toolStripProgressBar1.Value < 100)
            {
                generateDBCForm.toolStripStatusLabel1.Text = "Loading data...";

                generateDBCForm.toolStripStatusPercent.Text = Convert.ToInt32(value) + "";
            }
            else
            {
                generateDBCForm.toolStripStatusLabel1.Text = "Loading data successful";
                generateDBCForm.toolStripStatusPercent.Text = "Done";
            }
        }


        public GenerateDBC()
        {
            InitializeComponent();
            generateDBCForm = this;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "D:\\";
            openFileDialog1.Filter = "Excel files|*.xls;*.xlsm;*.xlsx";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                ExcelFilePath = excelPath.Text = file;
            }
        }

        private void checkDBC_Click(object sender, EventArgs e)
        {
            if (BLL_Process.isLoadingDataBefore)
            {

                if (!ValidateData())
                {
                    MessageBox.Show("Please see information box or log file for more detail", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Can matrix file is OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please perform step 1 above before!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        bool ValidateData()
        {
            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;

            return prcs.ValidateData();

            Cursor.Current = Cursors.Default;
        }

        public void CloseExcelFile(string path)
        {

        }
        private void excelPath_TextChanged(object sender, EventArgs e)
        {

        }

        private void genDBC_Click(object sender, EventArgs e)
        {

        }

        private void GenerateDBC_Load(object sender, EventArgs e)
        {
            _log.Info("--------------------------" + DateTime.Now + "--------------------------\n");
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            if (excelPath.Text != "")
            {

                var watch = System.Diagnostics.Stopwatch.StartNew();

                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor;

                prcs.LoadData(ExcelFilePath);

                Cursor.Current = Cursors.Default;

                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;

                //Log4net
                _log.Debug("Excute time: " + elapsedMs / 1000 + " s");
            }
            else
            {
                MessageBox.Show("Please provide link can matrix file", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                button1_Click(sender, e);
            }
        }

        private void GenerateDBC_FormClosed(object sender, FormClosedEventArgs e)
        {
            prcs.CloseConnect();
        }

        private void btnCreateFWList_Click(object sender, EventArgs e)
        {


            // Check loading data before
            if (BLL_Process.isLoadingDataBefore)
            {
                //Check failed threshold textbox
                if (tbFailedThreshold.Text == "")
                {
                    MessageBox.Show("Please fill failed threshold!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbFailedThreshold.Focus();
                    return;
                }

                // log4net
                _log.Info("-------------------------- Generate FW list--------------------------\n");

                int failedThreshold = Int32.Parse(tbFailedThreshold.Text);
                GenerateFW frmGenerateFW = new GenerateFW(failedThreshold);
                frmGenerateFW.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please load data before!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void tbFailedThreshold_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9'))
                e.Handled = true;
        }

        private void tbFailedThreshold_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TextBox currenttb = (TextBox)sender;

            if (currenttb.Text == "")
            {
                MessageBox.Show(string.Format("Failed Threshold must be not null"));
                e.Cancel = true;
            }
            else if (currenttb.Text != "")
            {
                int temp = Int32.Parse(currenttb.Text);
                if (temp <= 0 || temp > 50)
                {
                    MessageBox.Show(string.Format("Failed Threshold range is must be greater than 0 and less than 50"));
                    e.Cancel = true;
                }
            }

            else
            {
                e.Cancel = false;
            }
        }
    }
}
