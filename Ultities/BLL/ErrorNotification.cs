using static Ultities.DTO.Ultities;
using static Ultities.DTO.Ultities.ErrorDefine;


namespace Ultities.BLL
{
    class ErrorNotification
    {
        public static string NotificationString(ErrorDefine errDefine)
        {
            string strOutput;
            switch (errDefine)
            {
                #region Check frame

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

                #endregion !Check frame

                #region Check signal

                case C_ERROR_SGN_NAME_NULL:
                    strOutput = " Signal name must be not null";
                    break;

                case C_ERROR_SGN_NAME_LENGTH_G_THAN32:
                    strOutput = " Signal name length must be less than 32";
                    break;

                case C_ERROR_SGN_BYTEORDER_ONLY_LSB_MSB:
                    strOutput = " Signal only support for Motorola_LSB and Motorola_MSB";
                    break;

                case C_ERROR_SGN_STARTBIT_NULL:
                    strOutput = " Signal start bit is missing";
                    break;

                case C_ERROR_SGN_STARTBIT_G_THAN63:
                    strOutput = " Signal start bit must be less than 63 (start bit <= 63)";
                    break;

                case C_ERROR_SGN_BITLENGTH_NULL:
                    strOutput = " Signal bit length is missing";
                    break;

                case C_ERROR_SGN_BITLENGTH_G_THAN64:
                    strOutput = " Signal bit length must be less than 64 (bit length <= 64)";
                    break;

                case C_ERROR_SGN_DATATYPE_ONLY_UNSIGNED_SIGNED:
                    strOutput = " Signal data type should be UNSIGNED or SIGNED";
                    break;

                case C_ERROR_SGN_RESOLUTION_INVALID:
                    strOutput = " Signal RESOLUTION is invalid";
                    break;

                case C_ERROR_SGN_OFFSET_INVALID:
                    strOutput = " Signal OFFSET is invalid";
                    break;

                case C_ERROR_SGN_PHYMIN_INVALID:
                    strOutput = " Signal physical min is invalid";
                    break;

                case C_ERROR_SGN_PHYMAX_INVALID:
                    strOutput = " Signal physical max is invalid";
                    break;

                case C_ERROR_SGN_HEXMIN_INVALID:
                    strOutput = " Signal hexa min is invalid";
                    break;

                case C_ERROR_SGN_HEXMAX_INVALID:
                    strOutput = " Signal hexa max is invalid";
                    break;

                case C_ERROR_SGN_INITVALUE_NULL:
                    strOutput = " Signal INITVALUE must be not null";
                    break;

                case C_ERROR_SGN_INITVALUE_INVALID:
                    strOutput = " Signal INITVALUE is invalid";
                    break;

                case C_ERROR_SGN_NOTSEND_RECEIVE:
                    strOutput = "  At least signal must be sent or received by one node!";
                    break;

                #endregion ! Check signal

                default:
                    strOutput = "";
                    break;
            }
            return strOutput;
        }
    }
}
