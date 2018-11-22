using System;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using static Ultities.BLL.Constants;
using Ultities.BLL;

namespace Ultities
{
    public partial class GenerateDBC : Form
    {
        public static Excel.Application xlApp;
        public static Excel.Workbook xlWorkBook;
        public static Excel.Worksheet xlWorkSheet;
        public static Excel.Range range;

        static string ExcelFilePath;

        static BLL_Process prcs = new BLL_Process();
        public GenerateDBC()
        {
            InitializeComponent();
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
        public bool CheckCanMatrix()
        {
            bool result = false;

            ////Check message info
            //prcs.CheckMessageInfo();
            //prcs.CheckNodeOfMessage();

            ////Check signal info
            //prcs.CheckSignalInfo();
            //prcs.CheckNodeOfSignal();


            return true;
        }

        private void checkDBC_Click(object sender, EventArgs e)
        {
            CheckCanMatrix();

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

        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            //var watch = System.Diagnostics.Stopwatch.StartNew();

            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;
            toolStripStatusLabel1.Text = "Loading data...";
                       
            prcs.LoadData(ExcelFilePath);

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
            toolStripStatusLabel1.Text = "Load data successful";

            //watch.Stop();
            //var elapsedMs = watch.ElapsedMilliseconds;
        }
    }
}
