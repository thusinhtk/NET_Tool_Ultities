﻿using System;
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
using static Ultities.GenerateDBC;



namespace Ultities.BLL
{
    class BLL_Process
    {
        public static bool isLoadingDataBefore = false;

        private static Connection cn = new Connection();

        public static List<Messages> canMatrix =new List<Messages>();
        private static Messages message =new Messages();

        private static int numberOfColumns;
        private static int numberOfRows;

        private static ErrorObject errObject = new ErrorObject();
        private static ErrorDefine errDefine = C_NO_ERROR;

        public void LoadData(string path)
        {
            // Open connnect
            cn.Connect(@path);

            // Set number of column and rows
            SetNumberColumnsAndRows();

            // Load data for list can matrix
            TransferExcelToList();

            isLoadingDataBefore = true;
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

        internal bool ValidateData()
        {
            // Clear text append before checking
            GenerateDBC.ClearTextInfo();

            foreach (Messages msg in canMatrix)
            {
                //check frame info
                errDefine = CheckMessageInfo(msg);
                SetTextForGui(errDefine, msg.MessageName);

                //check node of frame
                errDefine = CheckListNodeInfo(msg.ListNode);
                SetTextForGui(errDefine, msg.MessageName);

                //Check signal
                foreach (Signal signal in msg.ListSignal)
                {
                    // signal info
                    errDefine = CheckSignalInfo(signal);
                    SetTextForGui(errDefine, signal.SignalName);

                    //check node of signal
                    errDefine = CheckListNodeInfo(signal.ListNode);
                    SetTextForGui(errDefine, signal.SignalName);
                }
            }
            return true;
        }
        bool SetTextForGui(ErrorDefine eDefine, string name)
        {
            if (eDefine != C_NO_ERROR)
            {
                // TODO - write log4net here
                GenerateDBC.SetTextInfo("Info:" + name + errObject.GetNotification(errDefine));
                return false;
            }
            return true;
        }

        bool IsFrameRow(string frameName, string frameID, string frameSendTye, string frameCycle, string frameLength)
        {
            bool isAllNull;

            isAllNull = (frameName == "") ? true : false;
            isAllNull &= (frameSendTye == "") ? true : false;
            isAllNull &= (frameCycle == "") ? true : false;
            isAllNull &= (frameLength == "") ? true : false;

            return !isAllNull;
        }

        bool TransferExcelToList()
        {
            bool result = false;       

            int rCnt, cCnt;
            bool isFirstFrame;

            List<Signal> listSignal = new List<Signal>();

            object[,] values = (object[,])range.Value2;

            SetProgressBarStatus(0);

            rCnt = START_OF_FIRST_ROW;

            while (rCnt <= values.GetLength(0))
            {
                isFirstFrame = (rCnt == START_OF_FIRST_ROW) ? true : false;

                // Add info for frame
                string msgName = Convert.ToString(values[rCnt, COLUMN_MESSAGENAME]);
                string msgID = Convert.ToString(values[rCnt, COLUMN_MESSAGEID]);
                string msgSendType = Convert.ToString(values[rCnt, COLUMN_MESSAGESENDTYPE]);
                string msgCycleTime = Convert.ToString(values[rCnt, COLUMN_MESSAGECYCLE]);
                string msgLength = Convert.ToString(values[rCnt, COLUMN_MESSAGEDLC]);

                if (IsFrameRow(msgName, msgID, msgSendType, msgCycleTime, msgLength))
                {
                    //Add list signal to frame for each next frame
                    if (!isFirstFrame) //For first frame
                    {
                        message.ListSignal = listSignal;
                        canMatrix.Add(message);

                        message = null; // Dispose object message
                        listSignal = new List<Signal>();
                    }

                    // Frame info
                    message = new Messages();
                    message.MessageName = msgName;
                    message.MessageID = msgID;
                    message.MessageSendType = msgSendType;
                    message.MessageCycleTime = msgCycleTime;
                    message.MessageLength = msgLength;

                    //Node of frame info
                    List<Node> listNode = new List<Node>();
                    for (cCnt = 22; cCnt <= numberOfColumns; ++cCnt)
                    {
                        Node node = new Node();
                        node.NodeName = Convert.ToString(values[1, cCnt]);

                        string str = Convert.ToString(values[rCnt, cCnt]);
                        SendReceiveType nodeSendType = str == "s" ? C_SEND : str == "r" ? C_RECEIVE : C_INVALID;
                        node.SendType = nodeSendType;

                        listNode.Add(node);
                    }
                    // Add list node of frame
                    message.ListNode = listNode;
                }
                else // Signal row
                {
                    string signalName = Convert.ToString(values[rCnt, COLUMN_SIGNALNAME]);
                    string signalDescription = Convert.ToString(values[rCnt, COLUMN_SIGNALDESCRIPTION]);
                    string signalByteOrder = Convert.ToString(values[rCnt, COLUMN_SIGNALBYTEFORMAT]);
                    string signalStartBit = Convert.ToString(values[rCnt, COLUMN_SIGNALSTARTBIT]);
                    string signalBitLength = Convert.ToString(values[rCnt, COLUMN_SIGNALBITLENGTH]);
                    string signalDataType = Convert.ToString(values[rCnt, COLUMN_SIGNALDATATYPE]);
                    string signalFactor = Convert.ToString(values[rCnt, COLUMN_SIGNALRESOLUTION]);
                    string signalOffset = Convert.ToString(values[rCnt, COLUMN_SIGNALOFFSET]);
                    string signalPhyMin = Convert.ToString(values[rCnt, COLUMN_SIGNALMINPHY]);
                    string signalPhyMax = Convert.ToString(values[rCnt, COLUMN_SIGNALMAXPHY]);
                    string signalHexMin = Convert.ToString(values[rCnt, COLUMN_SIGNALMINHEX]);
                    string signalHexMax = Convert.ToString(values[rCnt, COLUMN_SIGNALMAXHEX]);
                    string signalInitHex = Convert.ToString(values[rCnt, COLUMN_SIGNALINITVALUE]);
                    string signalInvalidHex = Convert.ToString(values[rCnt, COLUMN_SIGNALINVALIDVALUE]);
                    string signalUnit = Convert.ToString(values[rCnt, COLUMN_SIGNALUNIT]);
                    string signalValueDescription = Convert.ToString(values[rCnt, COLUMN_SIGNALVALUEDESCRIPTION]);

                    Signal signal = new Signal();
                    signal.SignalName = signalName;
                    signal.SignalDescription = signalDescription;
                    signal.SignalByteOrder = signalByteOrder;
                    signal.SignalStartBit = signalStartBit;
                    signal.SignalBitLength = signalBitLength;
                    signal.SignalDataType = signalDataType;
                    signal.SignalFactor = signalFactor;
                    signal.SignalOffset = signalOffset;
                    signal.SignalPhyMin = signalPhyMin;
                    signal.SignalPhyMax = signalPhyMax;
                    signal.SignalHexMin = signalHexMin;
                    signal.SignalHexMax = signalHexMax;
                    signal.SignalInitHex = signalInitHex;
                    signal.SignalInvalidHex = signalInvalidHex;
                    signal.SignalUnit = signalUnit;
                    signal.SignalValueDescription = signalValueDescription;

                    List<Node> listNodeSignal = new List<Node>();
                    for (cCnt = 22; cCnt <= numberOfColumns; ++cCnt)
                    {
                        Node node = new Node();
                        node.NodeName = Convert.ToString(values[1, cCnt]);

                        string str = Convert.ToString(values[rCnt, cCnt]);
                        SendReceiveType nodeSendType = str == "s" ? C_SEND : str == "r" ? C_RECEIVE : C_INVALID;
                        node.SendType = nodeSendType;

                        listNodeSignal.Add(node);
                    }
                    signal.ListNode = listNodeSignal;
                    listSignal.Add(signal);
                }

                if (rCnt == values.GetLength(0)) //For last frame
                {
                    message.ListSignal = listSignal;
                    canMatrix.Add(message);

                    result = true;
                }
                double percent = (double)rCnt / Convert.ToDouble(values.GetLength(0));

                SetProgressBarStatus(percent * 100);
                rCnt++;
            }
            
            #region Code comment for refer, not user FOREVER
            /* temporary comment
             for (rCnt = START_OF_FIRST_ROW; rCnt <= numberOfRows; ++rCnt)
             {
                 isFirstFrame = rCnt == START_OF_FIRST_ROW ? true:false;

                 // Add info for frame
                 string msgName = Convert.ToString(range.Cells[rCnt, COLUMN_MESSAGENAME].Value2); ;
                 string msgID = Convert.ToString(range.Cells[rCnt, COLUMN_MESSAGEID].Value2);
                 string msgSendType = Convert.ToString(range.Cells[rCnt, COLUMN_MESSAGESENDTYPE].Value2);
                 string msgCycleTime = Convert.ToString(range.Cells[rCnt, COLUMN_MESSAGECYCLE].Value2);
                 string msgLength = Convert.ToString(range.Cells[rCnt, COLUMN_MESSAGEDLC].Value2);

                 if (IsFrameRow(msgName, msgID, msgSendType, msgCycleTime, msgLength))
                 {
                     //Add list signal to frame for each next frame
                     if (!isFirstFrame) //For first frame
                     {
                         message.ListSignal = listSignal;
                         canMatrix.Add(message);

                         message = null; // Dispose object message
                         listSignal = new List<Signal>();
                     }


                     // Frame info
                     message = new Messages();
                     message.MessageName = msgName;
                     message.MessageID = msgID;
                     message.MessageSendType = msgSendType;
                     message.MessageCycleTime = msgCycleTime;
                     message.MessageLength = msgLength;

                     //Node of frame info
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
                     // Add list node of frame
                     message.ListNode = listNode;
                 }
                 else // Signal row
                 {      
                     string signalName = Convert.ToString(range.Cells[rCnt, COLUMN_SIGNALNAME].Value2);
                     string signalDescription = Convert.ToString(range.Cells[rCnt, COLUMN_SIGNALDESCRIPTION].Value2);
                     string signalByteOrder = Convert.ToString(range.Cells[rCnt, COLUMN_SIGNALBYTEFORMAT].Value2);
                     string signalStartBit = Convert.ToString(range.Cells[rCnt, COLUMN_SIGNALSTARTBIT].Value2);
                     string signalBitLength = Convert.ToString(range.Cells[rCnt, COLUMN_SIGNALBITLENGTH].Value2);
                     string signalDataType = Convert.ToString(range.Cells[rCnt, COLUMN_SIGNALDATATYPE].Value2);
                     string signalFactor = Convert.ToString(range.Cells[rCnt, COLUMN_SIGNALRESOLUTION].Value2);
                     string signalOffset = Convert.ToString(range.Cells[rCnt, COLUMN_SIGNALOFFSET].Value2);
                     string signalPhyMin = Convert.ToString(range.Cells[rCnt, COLUMN_SIGNALMINPHY].Value2);
                     string signalPhyMax = Convert.ToString(range.Cells[rCnt, COLUMN_SIGNALMAXPHY].Value2);
                     string signalHexMin = Convert.ToString(range.Cells[rCnt, COLUMN_SIGNALMINHEX].Value2);
                     string signalHexMax = Convert.ToString(range.Cells[rCnt, COLUMN_SIGNALMAXHEX].Value2);
                     string signalInitHex = Convert.ToString(range.Cells[rCnt, COLUMN_SIGNALINITVALUE].Value2);
                     string signalInvalidHex = Convert.ToString(range.Cells[rCnt, COLUMN_SIGNALINVALIDVALUE].Value2);
                     string signalUnit = Convert.ToString(range.Cells[rCnt, COLUMN_SIGNALUNIT].Value2);
                     string signalValueDescription = Convert.ToString(range.Cells[rCnt, COLUMN_SIGNALVALUEDESCRIPTION].Value2);

                     Signal signal = new Signal();
                     signal.SignalName = signalName;
                     signal.SignalDescription = signalDescription;
                     signal.SignalByteOrder = signalByteOrder;
                     signal.SignalStartBit = signalStartBit;
                     signal.SignalBitLength = signalBitLength;
                     signal.SignalDataType = signalDataType;
                     signal.SignalFactor = signalFactor;
                     signal.SignalOffset = signalOffset;
                     signal.SignalPhyMin = signalPhyMin;
                     signal.SignalPhyMax = signalPhyMax;
                     signal.SignalHexMin = signalHexMin;
                     signal.SignalHexMax = signalHexMax;
                     signal.SignalInitHex = signalInitHex;
                     signal.SignalInvalidHex = signalInvalidHex;
                     signal.SignalUnit = signalUnit;
                     signal.SignalValueDescription = signalValueDescription;

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
                     signal.ListNode = listNodeSignal;
                     listSignal.Add(signal);
                 }

                 if(rCnt == numberOfRows) //For last frame
                 {
                     message.ListSignal = listSignal;
                     canMatrix.Add(message);

                     result = true;
                 }
             }
             ! tempprary comment */
            #endregion

            return result;
        }
            
        
        bool LoadingData()
        {
            /*
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
            */
            return false;
        }

        private ErrorDefine CheckListNodeInfo(List<Node> listNode)
        {
            uint count = 0;
            foreach (Node node in listNode)
            {
                if (node.SendType != C_INVALID)
                {
                    count++;
                }
            }
            return count > 0 ? C_NO_ERROR : C_ERROR_MSG_NOTSEND_RECEIVE;
        }

        internal ErrorDefine CheckMessageInfo(Messages msg)
        {
            #region check message info
            // msg id
            if (msg.MessageID == null)
            {
                return C_ERROR_MSG_ID_NULL;
            }

            //msg send type
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

            //msg cycle time
            int tempNumber = 0;
            bool canConvert = int.TryParse(msg.MessageCycleTime, out tempNumber); // Null will return false
            if (canConvert)
            {
                if (tempNumber <= 0)
                {
                    return C_ERROR_MSG_CYCLETIME_L_THAN0;
                }
            }
            else
            {
                return C_ERROR_MSG_CYCLETIME_NULL;
            }

            // msg length
            tempNumber = 0;
            canConvert = int.TryParse(msg.MessageLength, out tempNumber); // Null will return false
            if (canConvert)
            {
                if (tempNumber <= 0)
                {
                    return C_ERROR_MSG_DLC_L_THAN0;
                }
                if (tempNumber >8)
                {
                    return C_ERROR_MSG_DLC_G_THAN8;
                }
            }
            else
            {
                return C_ERROR_MSG_CYCLETIME_NULL;
            }

            #endregion

            return C_NO_ERROR;           
        }

        internal ErrorDefine CheckSignalInfo(Signal signal)
        {
            uint tempNumber_Positive;
            int tempNumber_Negative;
            bool isValid;

            // signal name
            #region
            if (signal.SignalName == null)
            {
                return C_ERROR_SGN_NAME_NULL;
            }
            if(signal.SignalName.Length >32)
            {
                return C_ERROR_SGN_NAME_LENGTH_G_THAN32;
            }
            #endregion

            // byte order
            #region
            isValid = signal.SignalByteOrder.ToLower() == "motorola lsb" ? true : false;
            isValid &= signal.SignalByteOrder.ToLower() == "motorola msb" ? true : false;

            if (!isValid)
            {
                return C_ERROR_SGN_BYTEORDER_ONLY_LSB_MSB;
            }
            #endregion

            // start bit
            #region
            bool canConvert = uint.TryParse(signal.SignalStartBit, out tempNumber_Positive); // Null will return false
            if (canConvert)
            {
                if (tempNumber_Positive > 63)
                {
                    return C_ERROR_SGN_STARTBIT_G_THAN63;
                }
            }
            else
            {
                return C_ERROR_SGN_STARTBIT_NULL;
            }
            #endregion

            // bit length
            #region
            canConvert = uint.TryParse(signal.SignalBitLength, out tempNumber_Positive); // Null will return false
            if (canConvert)
            {
                if (tempNumber_Positive > 63)
                {
                    return C_ERROR_SGN_BITLENGTH_G_THAN64;
                }
            }
            else
            {
                return C_ERROR_SGN_BITLENGTH_NULL;
            }
            #endregion

            // data type
            #region
            isValid = signal.SignalDataType.ToLower() == "unsigned" ? true : false;
            isValid &= signal.SignalDataType.ToLower() == "signed" ? true : false;

            if (!isValid)
            {
                return C_ERROR_SGN_DATATYPE_ONLY_UNSIGNED_SIGNED;
            }
            #endregion

            // resolution (factor)
            #region
            tempNumber_Positive = 0;
            canConvert = uint.TryParse(signal.SignalFactor, out tempNumber_Positive); // Null will return false
            if (!canConvert)
            {
                return C_ERROR_SGN_RESOLUTION_INVALID;
            }
            #endregion

            // offset
            #region
            canConvert = int.TryParse(signal.SignalOffset, out tempNumber_Negative);
            if (!canConvert)
            {
                return C_ERROR_SGN_OFFSET_INVALID;
            }
            #endregion

            // phy min
            #region
            canConvert = int.TryParse(signal.SignalPhyMin, out tempNumber_Negative);
            if (!canConvert)
            {
                return C_ERROR_SGN_PHYMIN_INVALID;
            }
            #endregion

            // phy max
            #region
            canConvert = uint.TryParse(signal.SignalPhyMax, out tempNumber_Positive);
            if (!canConvert)
            {
                return C_ERROR_SGN_PHYMAX_INVALID;
            }
            #endregion

            // hex min
            #region
            canConvert = uint.TryParse(signal.SignalHexMin, out tempNumber_Positive);
            if (!canConvert)
            {
                return C_ERROR_SGN_HEXMIN_INVALID;
            }
            #endregion

            // hex max
            #region
            canConvert = uint.TryParse(signal.SignalHexMax, out tempNumber_Positive);
            if (!canConvert)
            {
                return C_ERROR_SGN_HEXMAX_INVALID;
            }
            #endregion

            // init hex - check whether hexa number if not null

            // invalid hex - check whether hexa number if not null

            return C_NO_ERROR;
        }

        bool CheckNumber(string numberString, int outNumber)
        {
            return int.TryParse(numberString, out outNumber);
        }

        bool CheckNullValue(string value)
        {
            return value == null ? true : false;
        }
    }
}