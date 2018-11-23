using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultities.DTO;
using Ultities.DAO;
using static Ultities.DAO.Connection;
using static Ultities.BLL.Constants;
using Excel = Microsoft.Office.Interop.Excel;
using static Ultities.DTO.Ultities;
using static Ultities.DTO.Ultities.SendReceiveType;
using static Ultities.DTO.Ultities.ErrorDefine;
using static Ultities.DTO.Ultities.ErrorObject;



namespace Ultities.BLL
{
    class BLL_Process
    {
        public static List<Messages> canMatrix;
        private static Connection cn;

        private static int numberOfColumns;
        private static int numberOfRows;

        static ErrorObject errObject;
        static ErrorDefine errDefine = C_NO_ERROR;

        public void LoadData(string path)
        {
            // Open connnect
            cn = new Connection();
            cn.Connect(@path);

            // Set number of column and rows
            SetNumberColumnsAndRows();

            // Load data for list can matrix
            InitData();
        }
        void SetNumberColumnsAndRows()
        {
            #region Set number of column and rows

            numberOfColumns = range.Columns.Count;
            // Reset Column count
            for (int ccCnt = 22; ccCnt <= numberOfColumns; ++ccCnt)
            {
                string strTmp = Convert.ToString((range.Cells[1, ccCnt]).Value2);
                if (strTmp == null)
                {
                    numberOfColumns = --ccCnt;
                }
            }

            numberOfRows = range.Rows.Count;

            #endregion
        }

        bool InitData()
        {
            canMatrix = new List<Messages>();

            #region Init data for list can matrix

            bool result = false;
            //try
            //{
            int rCnt, cCnt;

            for (rCnt = START_OF_FIRST_ROW; rCnt <= numberOfRows; ++rCnt)
            {
                string strMessageName = Convert.ToString(range.Cells[rCnt, COLUMN_MESSAGENAME].Value2);
                if (strMessageName != null)
                {

                    #region Add info for message

                    Messages message = new Messages();

                    message.MessageName = strMessageName;
                    message.MessageID = Convert.ToString(range.Cells[rCnt, COLUMN_MESSAGEID].Value2);
                    message.MessageSendType = Convert.ToString(range.Cells[rCnt, COLUMN_MESSAGESENDTYPE].Value2);


                    int messageCycleTime;
                    string messageST = message.MessageSendType;
                    messageCycleTime = (messageST.ToLower()) == "event" ? 0 : Convert.ToInt16(range.Cells[rCnt, COLUMN_MESSAGECYCLE].Value2);
                    message.MessageCycleTime = Convert.ToUInt32(messageCycleTime);

                    message.MessageLength = Convert.ToUInt16(range.Cells[rCnt, COLUMN_MESSAGEDLC].Value2);

                    #endregion

                    #region check message info

                    errDefine = CheckMessageInfo(message);
                    if (errDefine != C_NO_ERROR)
                    {
                        // TODO - write log4net here

                        errObject = new ErrorObject(errDefine, message.MessageName);
                        MessageBox.Show("Message:" + message.MessageName + " - " + errObject.ErrorMessageOutput);
                        return false;
                    }

                    #endregion

                    #region ADD node for message

                    List<Node> listNode = new List<Node>();
                    for (cCnt = 22; cCnt <= numberOfColumns; ++cCnt)
                    {
                        Node node = new Node();
                        node.NodeName = Convert.ToString(range.Cells[1, cCnt].Value2);

                        string str = Convert.ToString(range.Cells[rCnt, cCnt].Value2);
                        SendReceiveType nodeSendType = str == "s" ? C_SEND : str == "r" ? C_RECEIVE : C_INVALID;
                        node.SendType = nodeSendType;

                        listNode.Add(node);
                    }

                    #region Check node info of message

                    errDefine = CheckListNodeInfo(listNode);
                    if (errDefine != C_NO_ERROR)
                    {
                        // TODO - write log4net here

                        errObject = new ErrorObject(errDefine, message.MessageName);
                        MessageBox.Show("Message:" + message.MessageName + " - " + errObject.ErrorMessageOutput);
                        return false;
                    }

                    #endregion

                    #endregion

                    #region Add signal for each message

                    List<Signal> listSignal = new List<Signal>();
                    int id = ++rCnt;
                    while (id <= numberOfRows)
                    {
                        strMessageName = Convert.ToString(range.Cells[id, COLUMN_MESSAGENAME].Value2);
                        string messageID = Convert.ToString(range.Cells[id, COLUMN_MESSAGEID].Value2);
                        if (strMessageName == null && messageID == null) //Check whether message is exist to add signal
                        {
                            #region Add info for signal

                            #region Load singal info

                            string signalName = Convert.ToString(range.Cells[id, COLUMN_SIGNALNAME].Value2);
                            string signalDescription = Convert.ToString(range.Cells[id, COLUMN_SIGNALDESCRIPTION].Value2);
                            string signalByteOrder = Convert.ToString(range.Cells[id, COLUMN_SIGNALBYTEFORMAT].Value2);

                            string signalStartBit = Convert.ToString(range.Cells[id, COLUMN_SIGNALSTARTBIT].Value2);
                            signalStartBit = signalStartBit == null ? "0" : signalStartBit;

                            string signalBitLength = Convert.ToString(range.Cells[id, COLUMN_SIGNALBITLENGTH].Value2);
                            signalBitLength = signalBitLength == null ? "0" : signalBitLength;

                            string signalDataType = Convert.ToString(range.Cells[id, COLUMN_SIGNALDATATYPE].Value2);
                            string signalFactor = Convert.ToString(range.Cells[id, COLUMN_SIGNALRESOLUTION].Value2);
                            string signalOffset = Convert.ToString(range.Cells[id, COLUMN_SIGNALOFFSET].Value2);
                            string signalPhyMin = Convert.ToString(range.Cells[id, COLUMN_SIGNALMINPHY].Value2);
                            string signalPhyMax = Convert.ToString(range.Cells[id, COLUMN_SIGNALMAXPHY].Value2);
                            string signalHexMin = Convert.ToString(range.Cells[id, COLUMN_SIGNALMINHEX].Value2);
                            string signalHexMax = Convert.ToString(range.Cells[id, COLUMN_SIGNALMAXHEX].Value2);
                            string signalInitHex = Convert.ToString(range.Cells[id, COLUMN_SIGNALINITVALUE].Value2);
                            string signalInvalidHex = Convert.ToString(range.Cells[id, COLUMN_SIGNALINVALIDVALUE].Value2);
                            string signalUnit = Convert.ToString(range.Cells[id, COLUMN_SIGNALUNIT].Value2);
                            string signalValueDescription = Convert.ToString(range.Cells[id, COLUMN_SIGNALVALUEDESCRIPTION].Value2);

                            #endregion

                            Signal signal = new Signal();
                            signal.SignalName = signalName;
                            signal.SignalDescription = signalDescription;
                            signal.SignalByteOrder = signalByteOrder;
                            signal.SignalStartBit = Convert.ToUInt16(signalStartBit);
                            signal.SignalBitLength = Convert.ToUInt16(signalBitLength);
                            signal.SignalDataType = signalDataType;
                            signal.SignalFactor = Convert.ToDouble(signalFactor);
                            signal.SignalOffset = Convert.ToDouble(signalOffset);
                            signal.SignalPhyMin = Convert.ToDouble(signalPhyMin);
                            signal.SignalPhyMax = Convert.ToDouble(signalPhyMax);
                            signal.SignalHexMin = Convert.ToUInt32(signalHexMin, 16);
                            signal.SignalHexMax = Convert.ToUInt64(signalHexMax, 16);
                            signal.SignalInitHex = Convert.ToUInt32(signalInitHex, 16);
                            signal.SignalInvalidHex = Convert.ToUInt64(signalInvalidHex, 16);
                            signal.SignalUnit = signalUnit;
                            signal.SignalValueDescription = signalValueDescription;

                            #region Check signal info

                            errDefine = CheckSignalnfo(signal);
                            if (errDefine != C_NO_ERROR)
                            {
                                // TODO - write log4net here

                                errObject = new ErrorObject(errDefine, message.MessageName);
                                MessageBox.Show("Message:" + message.MessageName + " - " + errObject.ErrorMessageOutput);
                                return false;
                            }

                            #endregion

                            #region ADD node for signal

                            List<Node> listNodeSignal = new List<Node>();
                            for (cCnt = 22; cCnt <= numberOfColumns; ++cCnt)
                            {
                                Node node = new Node();
                                node.NodeName = Convert.ToString(range.Cells[1, cCnt].Value2);

                                string str = Convert.ToString(range.Cells[rCnt, cCnt].Value2);
                                SendReceiveType nodeSendType = str == "s" ? C_SEND : str == "r" ? C_RECEIVE : C_INVALID;
                                node.SendType = nodeSendType;

                                listNodeSignal.Add(node);
                            }

                            #region Check node info of signal

                            errDefine = CheckListNodeInfo(listNode);
                            if (errDefine != C_NO_ERROR)
                            {
                                // TODO - write log4net here

                                errObject = new ErrorObject(errDefine, message.MessageName);
                                MessageBox.Show("Message:" + message.MessageName + " - " + errObject.ErrorMessageOutput);
                                return false;
                            }

                            #endregion

                            signal.ListNode = listNodeSignal; // Set list node for each signal

                            #endregion




                            listSignal.Add(signal); // Merge list of signal
                        }
                        else
                        {
                            rCnt = id - 1; //set index in last signal of previous message
                            break;
                        }
                        id++;
                    }

                    #endregion

                    // Add node and signal for message
                    message.ListNode = listNode;
                    message.ListSignal = listSignal;

                    canMatrix.Add(message);
                }
                else
                {
                    // TODO - write log4net here

                    errObject = new ErrorObject(C_ERROR_MSG_NAME_NULL, "");
                    MessageBox.Show(errObject.ErrorMessageOutput + "[Row,Column] : [" + rCnt + "," + COLUMN_MESSAGENAME + "]");
                    return false;
                }
            }
            result = true;
            //}
            //catch (Exception ex)
            //{
            //    // Write log4net here
            //    string str = ex.Message;
            //    result = false;
            //}
            //finally
            //{
            //}

            return result;

            #endregion
            #endregion
        }

        private ErrorDefine CheckSignalnfo(Signal signal)
        {
            #region Check signal name
            if (signal.SignalName == null)
            {
                return C_ERROR_SGN_NAME_NULL;
            }
            if (signal.SignalName.Length > 32)
            {
                return C_ERROR_SGN_NAME_LENGTH_INVALID;
            } 
            #endregion

            #region Check signal byte order
            if (signal.SignalByteOrder == null)
            {
                return C_ERROR_SGN_BITLENGTH_NULL;
            }

            bool isSignalByteOrderInvalid = signal.SignalByteOrder.ToLower() == "Motorola LSB" ? true : false;
            isSignalByteOrderInvalid &= signal.SignalByteOrder.ToLower() == "Motorola MSB" ? true : false;

            if (!isSignalByteOrderInvalid)
            {
                return C_ERROR_SGN_BITLENGTH_INVALID;
            } 
            #endregion

            #region Check signal start bit
            if (signal.SignalStartBit > 63)
            {
                return C_ERROR_SGN_STARTBIT_INVALID;
            } 
            #endregion

            #region Check signal bit length
            if (signal.SignalBitLength == 0)
            {
                return C_ERROR_SGN_BITLENGTH_NULL;
            }
            if (signal.SignalBitLength > 64)
            {
                return C_ERROR_SGN_BITLENGTH_INVALID;
            } 
            #endregion


            #region Check signal data type
            if (signal.SignalDataType == null)
            {
                return C_ERROR_SGN_DATATYPE_NULL;
            }

            bool isValidDataType = signal.SignalDataType.ToLower() == "unsigned" ? true : false;
            isValidDataType &= signal.SignalDataType.ToLower() == "signed" ? true : false;

            if (!isValidDataType)
            {
                return C_ERROR_SGN_DATATYPE_INVALID;
            }
            #endregion

            #region Check signal factor 

            #endregion

            #region Check signal offset 

            #endregion

            #region Check signal phy min 

            #endregion

            #region Check signal phy max 

            #endregion

            #region  Check signal hex min 

            #endregion

            #region Check signal hex max 

            #endregion

            #region Check signal init hex 

            #endregion

            #region Check signal invalid hex 

            #endregion

            return C_NO_ERROR;
        }

        private ErrorDefine CheckListNodeInfo(List<Node> listNode)
        {
            uint count = 0;
            foreach (Node node in listNode)
            {
                if (node.SendType == C_INVALID)
                {
                    count++;
                }
            }
            return count > 0 ? C_ERROR_MSG_NOTSEND_RECEIVE : C_NO_ERROR;
        }

        internal ErrorDefine CheckMessageInfo(Messages msg)
        {
            #region check message info
            //
            if (msg.MessageID.ToString() == null)
            {
                return C_ERROR_MSG_ID_NULL;
            }

            //
            bool isValidSendType = msg.MessageSendType.ToLower() != "cycle" ? true : false;
            isValidSendType &= msg.MessageSendType.ToLower() != "event" ? true : false;

            if (msg.MessageSendType.ToString() == null)
            {
                return C_ERROR_MSG_SENDTYPE_NULL;
            }
            else if (isValidSendType)
            {
                return C_ERROR_MSG_SENDTYPE_INVALID;
            }

            //
            if (msg.MessageCycleTime.ToString() == null)
            {
                return C_ERROR_MSG_CYCLETIME_NULL;
            }

            //
            if (msg.MessageLength.ToString() == null)
            {
                return C_ERROR_MSG_DLC_NULL;
            }
            else if (msg.MessageLength > 8)
            {
                return C_ERROR_MSG_DLC_INVALID;
            }

            return C_NO_ERROR;

            #endregion
        }
    }
}
