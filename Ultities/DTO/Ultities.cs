﻿using Ultities.BLL;

namespace Ultities.DTO
{
    public class Ultities
    {
        public enum SendReceiveType
        {
            C_SEND = 0,
            C_RECEIVE,
            C_INVALID
        }

        public enum ErrorDefine
        {
            C_NO_ERROR = 0,

            #region MESSAGE - FRAME

            C_ERROR_MSG_NAME_NULL,

            C_ERROR_MSG_ID_NULL,

            C_ERROR_MSG_SENDTYPE_NULL,
            C_ERROR_MSG_SENDTYPE_INVALID,

            C_ERROR_MSG_CYCLETIME_NULL,
            C_ERROR_MSG_CYCLETIME_L_THAN0,

            C_ERROR_MSG_DLC_NULL,
            C_ERROR_MSG_DLC_L_THAN0,
            C_ERROR_MSG_DLC_G_THAN8,

            C_ERROR_MSG_NOTSEND_RECEIVE,

            #endregion

            #region SIGNAL

            C_ERROR_SGN_NAME_NULL,
            C_ERROR_SGN_NAME_LENGTH_G_THAN32,

            C_ERROR_SGN_BYTEORDER_ONLY_LSB_MSB,

            C_ERROR_SGN_STARTBIT_NULL,
            C_ERROR_SGN_STARTBIT_G_THAN63,

            C_ERROR_SGN_BITLENGTH_NULL,
            C_ERROR_SGN_BITLENGTH_G_THAN64,

            C_ERROR_SGN_DATATYPE_ONLY_UNSIGNED_SIGNED,

            C_ERROR_SGN_RESOLUTION_INVALID,

            C_ERROR_SGN_OFFSET_INVALID,

            C_ERROR_SGN_PHYMIN_INVALID,

            C_ERROR_SGN_PHYMAX_INVALID,

            C_ERROR_SGN_HEXMIN_INVALID,

            C_ERROR_SGN_HEXMAX_INVALID,

            C_ERROR_SGN_INITVALUE_NULL,
            C_ERROR_SGN_INITVALUE_INVALID,

            C_ERROR_SGN_NOTSEND_RECEIVE

            #endregion
        }

        public struct ErrorObject
        {
            ErrorDefine errorType;
            string errorMessageOutput;

            public ErrorDefine ErrorType
            {
                get
                {
                    return errorType;
                }

                set
                {
                    errorType = value;
                }
            }

            public string ErrorMessageOutput
            {
                get
                {
                    return errorMessageOutput;
                }

                set
                {
                    errorMessageOutput = value;
                }
            }

            public ErrorObject(ErrorDefine errorType)
            {
                this.errorType = errorType;
                this.errorMessageOutput = ErrorNotification.NotificationString(errorType);
            }

            public string GetNotification(ErrorDefine errorType)
            {
                return ErrorNotification.NotificationString(errorType);
            }
        }
    }
}
