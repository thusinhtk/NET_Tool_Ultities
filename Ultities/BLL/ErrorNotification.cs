using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ultities.DTO.Ultities;
using static Ultities.DTO.Ultities.ErrorDefine;


namespace Ultities.BLL
{
    class ErrorNotification
    {
        public static string NotificationString(ErrorDefine errDefine)
        {
            string strOutput = "";
            switch (errDefine)
            {
                //Check frame
                case C_ERROR_MSG_NAME_NULL:
                    strOutput = " Frame name is missing";
                    break;

                case C_ERROR_MSG_ID_NULL:
                    strOutput = " Frame ID is missing";
                    break;

                case C_ERROR_MSG_SENDTYPE_NULL:
                    strOutput = " Frame send type is missing";
                    break;

                case C_ERROR_MSG_SENDTYPE_INVALID:
                    strOutput = " Frame send type is either Cycle or Event";
                    break;

                case C_ERROR_MSG_CYCLETIME_NULL:
                    strOutput = " Frame cycle time is missing";
                    break;

                case C_ERROR_MSG_CYCLETIME_L_THAN0:
                    strOutput = " Frame cycle time must greater than 0";
                    break;

                case C_ERROR_MSG_DLC_NULL:
                    strOutput = " Frame DLC is missing";
                    break;

                case C_ERROR_MSG_DLC_L_THAN0:
                    strOutput = " Frame DLC must greater than 0";
                    break;

                case C_ERROR_MSG_DLC_G_THAN8:
                    strOutput = " Frame DLC must less than 0";
                    break;

                case C_ERROR_MSG_NOTSEND_RECEIVE:
                    strOutput = " At least Frame must be sent or received by one node!";
                    break;

                //Check signal

                default:
                    strOutput = "";
                    break;

            }
            return strOutput;
        }
    }
}
