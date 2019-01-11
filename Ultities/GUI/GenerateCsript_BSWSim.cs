using System;
using System.IO;
using System.Windows.Forms;

using Ultities.BLL;

namespace Ultities.GUI
{
    public partial class GenerateCsript_BSWSim : Form
    {


        public GenerateCsript_BSWSim()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void loadExcelFile_Click(object sender, EventArgs e)
        {

        }

        private void GenerateCsript_BSWSim_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            richTextBox1.AppendText("\\Generate\\FW_Lists.xlsx");
        }

        private void btnCheckFile_Click(object sender, EventArgs e)
        {

        }

        private void btnGenerateScript_Click(object sender, EventArgs e)
        {

        }
    }
}
