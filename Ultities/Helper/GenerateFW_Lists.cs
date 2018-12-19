using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using Ultities.DTO;

using static Ultities.BLL.Constants;
using static Ultities.GUI.GenerateFW;
using static Ultities.BLL.BLL_Process;
using static Ultities.DTO.Ultities.SendReceiveType;

using ClosedXML.Excel;


namespace Ultities.Helper
{
    class GenerateFW_Lists
    {
        public static DataTable _dt = new DataTable();
        private static IEnumerable<XElement> _record;

        // Constructor
        public GenerateFW_Lists()
        {
        }

        private void ReadXMLFWList(string path)
        {
            XElement root = XElement.Load(path);
            _record = from el in root.Elements("record") select el;
        }

        private void CreateDataTable()
        {
            ReadXMLFWList(XML_FWLIST_PATH);

            CreateHeader(ref _dt);
            CreateData(ref _dt);
        }

        private void CreateData(ref DataTable dt)
        {
            dt.Rows.Clear();

            foreach (Messages msg in canMatrix)
            {
                if (ESPIsReceiveMessage(msg))
                {
                    #region  For frame
                    string frame_name = msg.MessageName;
                    string frame_id = msg.MessageID;
                    string frame_sendtype = msg.MessageSendType;
                    string frame_cycletime = msg.MessageCycleTime;

                    string fwtype = "ComScl";
                    string fwname = fwtype + "_" + frame_name + "_Timeout" + Environment.NewLine;
                    if (IsGen93Checked())
                    {
                        fwname = fwname + fwtype + "_" + frame_name + "_Checksum" + Environment.NewLine;
                        fwname = fwname + fwtype + "_" + frame_name + "_AliveCounter" + Environment.NewLine;
                        fwname = fwname + fwtype + "_" + frame_name + "_DataLengthCode";
                    }
                    else
                    {
                        fwname = fwname + fwtype + "_" + frame_name + "_DataCorrupt";
                    }

                    dt.Rows.Add(frame_name, frame_id, frame_sendtype, frame_cycletime, "", "", fwname, fwtype, "", "", "", "");

                    #endregion

                    #region For signal

                    foreach (Signal sig in msg.ListSignal)
                    {
                        if (IsSignalInvalid(sig))
                        {
                            foreach (XElement el in _record)
                            {
                                string temp = el.Element("dncif").Value.ToLower();
                                if (sig.SignalInterface.ToLower().Contains(temp))
                                {
                                    sig.SignalFailureWord = el.Element("failure_word_name").Value;
                                    break;
                                }
                            }

                            string signal_name = sig.SignalName;
                            string signal_interface = sig.SignalInterface;
                            string sgn_failure_name = sig.SignalFailureWord;
                            string sgn_failure_type = "Scl";

                            dt.Rows.Add("", "", "", "", signal_name, signal_interface, sgn_failure_name, sgn_failure_type, "", "", "", "");
                        }
                    }

                    #endregion

                }
            }
        }

        private void CreateHeader(ref DataTable dt)
        {
            dt.Columns.Clear();

            // Add column
            dt.Columns.Add("Frame Name", typeof(string));
            dt.Columns.Add("Frame ID", typeof(string));
            dt.Columns.Add("Frame Send Type", typeof(string));
            dt.Columns.Add("Frame Cycle Time (ms)", typeof(string));

            dt.Columns.Add("Signal Name", typeof(string));
            dt.Columns.Add("Interface of ASW", typeof(string));
            dt.Columns.Add("FW Name", typeof(string));
            dt.Columns.Add("FW Type", typeof(string));

            dt.Columns.Add("Failed Threshold", typeof(string));
            dt.Columns.Add("Passed threshold", typeof(string));
            dt.Columns.Add("Failure Logging (ms)", typeof(string));
            dt.Columns.Add("Failure Recovery (ms)", typeof(string));
        }


        // Applicable for Message and Signal
        private bool ESPIsReceiveMessage(Messages msg)
        {
            foreach (Node node in msg.ListNode)
            {
                if ((node.NodeName.ToLower() == NAMEOFESPNODE.ToLower()) && (node.SendType == C_RECEIVE))
                {
                    return true;
                }
            }

            #region for signal

            //else if (obj.GetType() == typeof(Signal))
            //{
            //    Signal sgn = (Signal)obj;
            //    foreach (Node node in sgn.ListNode)
            //    {
            //        if ((node.NodeName.ToLower() == NAMEOFESPNODE) && (node.SendType == C_RECEIVE))
            //        {
            //            return true;
            //        }
            //    }
            //}

            #endregion

            return false;
        }
        private bool IsSignalInvalid(Signal sig)
        {
            return sig.SignalInvalidHex != "" ? true : false;
        }

        public void Export2Excel()
        {
            CreateDataTable();

            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            path = path + "\\Generate\\";

            //Exporting to Excel
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(_dt, "FailureWord_List");
                wb.SaveAs(path + "FW_Lists.xlsx");
            }
        }


    }
}
