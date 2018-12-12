using System;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using static Ultities.BLL.Constants;
using static Ultities.Logger.Logger;
using Ultities.BLL;

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
            if (!ValidateData())
            {
                MessageBox.Show("Please see information box or log file for more detail", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Can matrix file is OK!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void GenerateDBC_FormClosed(object sender, FormClosedEventArgs e)
        {
            prcs.CloseConnect();
        }
    }
}
