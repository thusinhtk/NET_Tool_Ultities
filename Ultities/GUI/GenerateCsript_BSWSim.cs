using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Ultities.BLL;
using Ultities.Helper;

namespace Ultities.GUI
{
    public partial class GenerateCsript_BSWSim : Form
    {
        static GenerateFW_Lists g_FWLists = new GenerateFW_Lists();
        private XElement root_dncifTestScript = new XElement("DNCSIM_TestScript");

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
            g_FWLists.CreateXMLData(/*ref root_dncifTestScript*/);
        }

        private void CreateXMLFile(string path)
        {

        }
    }
}
