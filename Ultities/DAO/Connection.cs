using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using static Ultities.BLL.Constants;
using System.Data;
using System.Windows.Forms;

namespace Ultities.DAO
{
    class Connection : IDisposable
    {
        public static Excel.Application xlApp;
        public static Excel.Workbook xlWorkBook;
        public static Excel.Worksheet xlWorkSheet;
        public static Excel.Range range;

        public bool Connect(string path)
        {
            bool result = false;

            try
            {
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(@path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(CURRENT_SHEET);
                range = xlWorkSheet.UsedRange;

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                throw new Exception(ex.Message);
            }

            return result;
        }

        public void CloseConnection()
        {
            Dispose(true);
        }

        public bool IsOpenConnection()
        {
            return xlApp != null ? true : false;
        }

        ~Connection()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                xlWorkBook.Close(true, null, null);
                xlApp.Quit();

                Marshal.ReleaseComObject(xlWorkSheet);
                Marshal.ReleaseComObject(xlWorkBook);
                Marshal.ReleaseComObject(xlApp);
            }          
        }
    }
}
