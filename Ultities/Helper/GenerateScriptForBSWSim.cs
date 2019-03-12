using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using static Ultities.Logger.Logger;

using Ultities.BLL;

namespace Ultities.Helper
{
    
    class GenerateScriptForBSWSim
    {
        static BLL_Process prcs = new BLL_Process();



        static bool isLoadingDataBefore = false;


        private bool LoadData(string path)
        {
            //Log4net
            _log.Info("Loading data...");

           // // Open connnect
           //prcs.BLL_Connect(@path);

           // // Set number of column and rows
           // prcs.SetNumberColumnsAndRows();

           // // Load data for list can matrix
           // TransferExcelToList();

           // isLoadingDataBefore = true;
            return true;

        }

    }
}
