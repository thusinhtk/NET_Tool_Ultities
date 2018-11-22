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

        internal void CheckNodeOfMessage()
        {
            throw new NotImplementedException();
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
            bool isValidSendType = msg.MessageSendType.ToString().ToLower() != "cycle" ? true : false;
            isValidSendType &= msg.MessageSendType.ToString().ToLower() != "event" ? true : false;

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
                            MessageBox.Show("Message:" +message.MessageName + " - " + errObject.ErrorMessageOutput);
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

                        #endregion

                        #region Add signal for each message

                        List<Signal> listSignal = new List<Signal>();
                        int id = ++rCnt;
                        while (id <= numberOfRows)
                        {
                            strMessageName = Convert.ToString(range.Cells[id, COLUMN_MESSAGENAME].Value2);
                            string messageID = Convert.ToString(range.Cells[id, COLUMN_MESSAGEID].Value2);
                        if (strMessageName == null && messageID ==null) //Check whether message is exist to add signal
                            {
                                #region Add info for signal

                                Signal signal = new Signal();
                                signal.SignalName = Convert.ToString(range.Cells[id, COLUMN_SIGNALNAME].Value2);
                                signal.SignalDescription = Convert.ToString(range.Cells[id, COLUMN_SIGNALDESCRIPTION].Value2);
                                signal.SignalByteOrder = Convert.ToString(range.Cells[id, COLUMN_SIGNALBYTEFORMAT].Value2);
                                signal.SignalStartBit = (ushort)(range.Cells[id, COLUMN_SIGNALSTARTBIT].Value2);
                                signal.SignalBitLength = (ushort)(range.Cells[id, COLUMN_SIGNALBITLENGTH].Value2);
                                signal.SignalDataType = Convert.ToString(range.Cells[id, COLUMN_SIGNALDATATYPE].Value2);
                                signal.SignalFactor = Convert.ToDouble(range.Cells[id, COLUMN_SIGNALRESOLUTION].Value2);
                                signal.SignalOffset = Convert.ToDouble(range.Cells[id, COLUMN_SIGNALOFFSET].Value2);
                                signal.SignalPhyMin = Convert.ToDouble(range.Cells[id, COLUMN_SIGNALMINPHY].Value2);
                                signal.SignalPhyMax = Convert.ToDouble(range.Cells[id, COLUMN_SIGNALMAXPHY].Value2);
                                signal.SignalHexMin = Convert.ToUInt32(range.Cells[id, COLUMN_SIGNALMINHEX].Value2, 16);
                                signal.SignalHexMax = Convert.ToUInt64(range.Cells[id, COLUMN_SIGNALMAXHEX].Value2, 16);
                                signal.SignalInitHex = Convert.ToUInt32(range.Cells[id, COLUMN_SIGNALINITVALUE].Value2, 16);
                                signal.SignalInvalidHex = Convert.ToUInt64(range.Cells[id, COLUMN_SIGNALINVALIDVALUE].Value2, 16);
                                signal.SignalUnit = Convert.ToString(range.Cells[id, COLUMN_SIGNALUNIT].Value2);
                                signal.SignalValueDescription = Convert.ToString(range.Cells[id, COLUMN_SIGNALVALUEDESCRIPTION].Value2);

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
        }
    }
}
