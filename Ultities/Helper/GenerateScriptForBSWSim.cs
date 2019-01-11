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

        const int C_COLUMN_FAILEDTHRESHOLD      = 09;
        const int C_COLUMN_PASSEDTHRESHOLD      = 10;
        const int C_COLUMN_FAILURELOGGING_TIME  = 11;
        const int C_COLUMN_FAILURERECOVERY_TIME = 12;

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
