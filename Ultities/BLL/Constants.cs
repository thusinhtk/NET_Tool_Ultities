using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultities.BLL
{
    class Constants
    {
        #region Column index
        
        public static int COLUMN_MESSAGENAME = Convert.ToInt32(ConfigurationManager.AppSettings["Column_MessageName"]);
        public static int COLUMN_MESSAGEID = Convert.ToInt32(ConfigurationManager.AppSettings["Column_MessageID"]);
        public static int COLUMN_MESSAGESENDTYPE = Convert.ToInt32(ConfigurationManager.AppSettings["Column_MessageSendType"]);
        public static int COLUMN_MESSAGECYCLE = Convert.ToInt32(ConfigurationManager.AppSettings["Column_MessageCycle"]);
        public static int COLUMN_MESSAGEDLC = Convert.ToInt32(ConfigurationManager.AppSettings["Column_MessageDLC"]);
        public static int COLUMN_SIGNALNAME = Convert.ToInt32(ConfigurationManager.AppSettings["Column_SignalName"]);
        public static int COLUMN_SIGNALDESCRIPTION = Convert.ToInt32(ConfigurationManager.AppSettings["Column_SignalDescription"]);
        public static int COLUMN_SIGNALBYTEFORMAT = Convert.ToInt32(ConfigurationManager.AppSettings["Column_SignalByteFormat"]);
        public static int COLUMN_SIGNALSTARTBIT = Convert.ToInt32(ConfigurationManager.AppSettings["Column_SignalStartBit"]);
        public static int COLUMN_SIGNALBITLENGTH = Convert.ToInt32(ConfigurationManager.AppSettings["Column_SignalBitLength"]);
        public static int COLUMN_SIGNALDATATYPE = Convert.ToInt32(ConfigurationManager.AppSettings["Column_SignalDataType"]);
        public static int COLUMN_SIGNALRESOLUTION = Convert.ToInt32(ConfigurationManager.AppSettings["Column_SignalResolution"]);
        public static int COLUMN_SIGNALOFFSET = Convert.ToInt32(ConfigurationManager.AppSettings["Column_SignalOffset"]);
        public static int COLUMN_SIGNALMINPHY = Convert.ToInt32(ConfigurationManager.AppSettings["Column_SignalMinPhy"]);
        public static int COLUMN_SIGNALMAXPHY = Convert.ToInt32(ConfigurationManager.AppSettings["Column_SignalMaxPhy"]);
        public static int COLUMN_SIGNALMINHEX = Convert.ToInt32(ConfigurationManager.AppSettings["Column_SignalMinHex"]);
        public static int COLUMN_SIGNALMAXHEX = Convert.ToInt32(ConfigurationManager.AppSettings["Column_SignalMaxHex"]);
        public static int COLUMN_SIGNALINITVALUE = Convert.ToInt32(ConfigurationManager.AppSettings["Column_SignalInitValue"]);
        public static int COLUMN_SIGNALINVALIDVALUE = Convert.ToInt32(ConfigurationManager.AppSettings["Column_SignalInvalidValue"]);
        public static int COLUMN_SIGNALUNIT = Convert.ToInt32(ConfigurationManager.AppSettings["Column_SignalUnit"]);
        public static int COLUMN_SIGNALVALUEDESCRIPTION = Convert.ToInt32(ConfigurationManager.AppSettings["Column_SignalValueDescription"]);

        #endregion

        #region Other index

        public static int START_OF_FIRST_ROW  = Convert.ToInt32(ConfigurationManager.AppSettings["StartOfFirstRow"]);
        public static int NUMBER_LINE_OF_HEADER = Convert.ToInt32(ConfigurationManager.AppSettings["NumberLineOfHeader"]);
        public static string CURRENT_SHEET = ConfigurationManager.AppSettings["CurrentSheet"];

        #endregion
    }
}
