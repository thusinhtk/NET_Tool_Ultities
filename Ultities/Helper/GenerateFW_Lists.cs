using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

using Ultities.DTO;

using static Ultities.BLL.Constants;
using static Ultities.GUI.GenerateFW;
using static Ultities.BLL.BLL_Process;
using static Ultities.Logger.Logger;
using static Ultities.DTO.Ultities.SendReceiveType;

using ClosedXML.Excel;


namespace Ultities.Helper
{
    class GenerateFW_Lists
    {
        private DataTable _dt = new DataTable();
        private static IEnumerable<XElement> _record;
        private static int failedThreshold;
        public DataTable Dt
        {
            get
            {
                return _dt;
            }

            set
            {
                _dt = value;
            }
        }

        public int FailedThreshold
        {
            get
            {
                return failedThreshold;
            }

            set
            {
                failedThreshold = value;
            }
        }

        // Constructor
        public GenerateFW_Lists()
        {
        }

        internal void CreateXMLData(/*ref XElement root_dncifTestScript*/)
        {
            foreach (Messages msg in canMatrix)
            {
                XElement root_dncifTestScript = new XElement("DNCSIM_TestScript");
                // Check whether frame is receive and not event frame
                if (ESPIsReceiveMessage(msg) && !(msg.MessageSendType.ToLower() == "event"))
                {
                    // ScriptTitle
                    CreateScriptTitle(root_dncifTestScript, msg);

                    // ScriptContent
                    CreateScriptContent(root_dncifTestScript, msg);

                    // ScriptParameterRemap
                    CreateScriptParameterRemap(root_dncifTestScript, msg);

                    string fileName = "ComScl_Frame_Rx_" + msg.MessageName + ".xml";
                    root_dncifTestScript.Save(@fileName);
                    MessageBox.Show(fileName + "is created");

                    root_dncifTestScript.RemoveAll();
                }

            }
        }

        private void CreateScriptParameterRemap(XElement root_dncifTestScript, Messages msg)
        {
            XElement scriptParameterRemap = new XElement("ScriptParameterRemap");
            scriptParameterRemap.Add(new XElement("SourceParameter", "ControllerNumber"));
            scriptParameterRemap.Add(new XElement("TargetParameter", "CAN0"));
            root_dncifTestScript.Add(scriptParameterRemap);

            scriptParameterRemap = new XElement("ScriptParameterRemap");
            scriptParameterRemap.Add(new XElement("SourceParameter", "RxFrame"));
            scriptParameterRemap.Add(new XElement("TargetParameter", msg.MessageName + "_" + "CanIf2PduR"));
            root_dncifTestScript.Add(scriptParameterRemap);

            scriptParameterRemap = new XElement("ScriptParameterRemap");
            scriptParameterRemap.Add(new XElement("SourceParameter", "RxCycleTime"));
            scriptParameterRemap.Add(new XElement("TargetParameter", msg.MessageCycleTime));
            root_dncifTestScript.Add(scriptParameterRemap);

            string checksumSignal = "";
            if (IsChecksumFrame(msg))
            {
                checksumSignal = GetChecksumSignal(msg).SignalName;
            }
            scriptParameterRemap = new XElement("ScriptParameterRemap");
            scriptParameterRemap.Add(new XElement("SourceParameter", "Checksum"));
            scriptParameterRemap.Add(new XElement("TargetParameter", checksumSignal));
            root_dncifTestScript.Add(scriptParameterRemap);

            string alivecounterSignal = "";
            if (IsRntCounterFrame(msg))
            {
                alivecounterSignal = GetAliveCounterSignal(msg).SignalName;
            }
            scriptParameterRemap = new XElement("ScriptParameterRemap");
            scriptParameterRemap.Add(new XElement("SourceParameter", "Alivecounter"));
            scriptParameterRemap.Add(new XElement("TargetParameter", alivecounterSignal));
            root_dncifTestScript.Add(scriptParameterRemap);

            scriptParameterRemap = new XElement("ScriptParameterRemap");
            scriptParameterRemap.Add(new XElement("SourceParameter", "FW_Timeout"));
            scriptParameterRemap.Add(new XElement("TargetParameter", "ComScl_" + msg.MessageName + "_Timeout"));
            root_dncifTestScript.Add(scriptParameterRemap);

            scriptParameterRemap = new XElement("ScriptParameterRemap");
            scriptParameterRemap.Add(new XElement("SourceParameter", "FW_DataCorrupt"));
            scriptParameterRemap.Add(new XElement("TargetParameter", "ComScl_" + msg.MessageName + "_DataCorrupt"));
            root_dncifTestScript.Add(scriptParameterRemap);

            scriptParameterRemap = new XElement("ScriptParameterRemap");
            scriptParameterRemap.Add(new XElement("SourceParameter", "FW_Timeout_DebounceLevel"));
            scriptParameterRemap.Add(new XElement("TargetParameter", FailedThreshold));
            root_dncifTestScript.Add(scriptParameterRemap);

            scriptParameterRemap = new XElement("ScriptParameterRemap");
            scriptParameterRemap.Add(new XElement("SourceParameter", "FW_Timeout_DebounceLevel"));
            scriptParameterRemap.Add(new XElement("TargetParameter", FailedThreshold));
            root_dncifTestScript.Add(scriptParameterRemap);

            scriptParameterRemap = new XElement("ScriptParameterRemap");
            scriptParameterRemap.Add(new XElement("SourceParameter", "FW_DataCorrupt_DebounceLevel"));
            scriptParameterRemap.Add(new XElement("TargetParameter", FailedThreshold));
            root_dncifTestScript.Add(scriptParameterRemap);

            scriptParameterRemap = new XElement("ScriptParameterRemap");
            scriptParameterRemap.Add(new XElement("SourceParameter", "ValidDLC"));
            scriptParameterRemap.Add(new XElement("TargetParameter", msg.MessageLength));
            root_dncifTestScript.Add(scriptParameterRemap);

            scriptParameterRemap = new XElement("ScriptParameterRemap");
            scriptParameterRemap.Add(new XElement("SourceParameter", "InvalidDLC"));
            scriptParameterRemap.Add(new XElement("TargetParameter", Int32.Parse(msg.MessageLength) - 2));
            root_dncifTestScript.Add(scriptParameterRemap);

            scriptParameterRemap = new XElement("ScriptParameterRemap");
            scriptParameterRemap.Add(new XElement("SourceParameter", "FW_DebounceLevel_InitValue"));
            scriptParameterRemap.Add(new XElement("TargetParameter", FailedThreshold * (-1)));
            root_dncifTestScript.Add(scriptParameterRemap);
        }

        private void CreateScriptContent(XElement root_dncifTestScript, Messages msg)
        {
            XElement scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "FilterBus"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "HIDE,CAN0"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "FilterBus"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "HIDE,CAN1"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "FilterFrame"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "SHOW,CAN0," + msg.MessageName + "_CanIf2PduR"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "SetIgnition"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "ON, 12"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "SetVariable"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "NMSG_VarRBData_ST.RBVarCode_UB,1"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "SendFrame"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "ControllerNumber,RxFrame,RxCycleTime"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "RunProcesses"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "RxCycleTime"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "DSWSetAllCIDMID"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "DISABLED"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "CANFrameGetDLC"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "ControllerNumber,RxFrame"));
            scriptContent.Add(new XElement("TestResult", msg.MessageLength));
            scriptContent.Add(new XElement("ExpectResult", "ValidDLC"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult", "PASS"));
            scriptContent.Add(new XElement("VerificationMethod", "CheckValue"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "RunProcesses"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "RxCycleTime"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "GetVariable"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "NMSG_VarRBData_ST.RBVarCode_UB"));
            scriptContent.Add(new XElement("TestResult", "1"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "GetCycleTime"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "ControllerNumber,RxFrame"));
            scriptContent.Add(new XElement("TestResult", msg.MessageCycleTime));
            scriptContent.Add(new XElement("ExpectResult", msg.MessageCycleTime));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult", "PASS"));
            scriptContent.Add(new XElement("VerificationMethod", "CheckValue"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "Set Fault DLC to test DataCorrupt"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "CANFrameSetDLC"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "ControllerNumber,RxFrame,InvalidDLC"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "RunProcesses"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "RxCycleTime"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "CANFrameGetDLC"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "ControllerNumber,RxFrame"));
            scriptContent.Add(new XElement("TestResult",(int.Parse(msg.MessageLength)-2)));
            scriptContent.Add(new XElement("ExpectResult", "InvalidDLC"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult", "PASS"));
            scriptContent.Add(new XElement("VerificationMethod", "CheckValue"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "RunProcesses"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "500"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "DSWGetMIDStatus"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "FW_DataCorrupt"));
            scriptContent.Add(new XElement("TestResult", "10"));
            scriptContent.Add(new XElement("ExpectResult", "FW_DataCorrupt_DebounceLevel"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult", "PASS"));
            scriptContent.Add(new XElement("VerificationMethod", "CheckValue"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "CANFrameSetDLC"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "ControllerNumber,RxFrame,8"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "RunProcesses"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "500"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "DSWGetMIDStatus"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "FW_DataCorrupt"));
            scriptContent.Add(new XElement("TestResult", "-10"));
            scriptContent.Add(new XElement("ExpectResult", "FW_DebounceLevel_InitValue"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult", "PASS"));
            scriptContent.Add(new XElement("VerificationMethod", "CheckValue"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "Set Checksum Disabled (check how this is disabled)"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "CanSignalSetDisable"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "ControllerNumber,RxFrame,Checksum"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "RunProcesses"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "200"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "DSWGetMIDStatus"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "FW_DataCorrupt"));
            scriptContent.Add(new XElement("TestResult", "10"));
            scriptContent.Add(new XElement("ExpectResult", "FW_DataCorrupt_DebounceLevel"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult", "PASS"));
            scriptContent.Add(new XElement("VerificationMethod", "CheckValue"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "CanSignalSetEnable"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "ControllerNumber,RxFrame,Checksum"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "RunProcesses"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "200"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "DSWGetMIDStatus"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "FW_DataCorrupt"));
            scriptContent.Add(new XElement("TestResult", "-10"));
            scriptContent.Add(new XElement("ExpectResult", "FW_DebounceLevel_InitValue"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult", "PASS"));
            scriptContent.Add(new XElement("VerificationMethod", "CheckValue"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "Set Alivecounter Disabled"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "CanSignalSetDisable"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "ControllerNumber,RxFrame,Alivecounter"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "RunProcesses"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "200"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "DSWGetMIDStatus"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "FW_DataCorrupt"));
            scriptContent.Add(new XElement("TestResult", "10"));
            scriptContent.Add(new XElement("ExpectResult", "FW_DataCorrupt_DebounceLevel"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult", "PASS"));
            scriptContent.Add(new XElement("VerificationMethod", "CheckValue"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "CanSignalSetEnable"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "ControllerNumber,RxFrame,Alivecounter"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "RunProcesses"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "200"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "DSWGetMIDStatus"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "FW_DataCorrupt"));
            scriptContent.Add(new XElement("TestResult", "-10"));
            scriptContent.Add(new XElement("ExpectResult", "FW_DebounceLevel_InitValue"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult", "PASS"));
            scriptContent.Add(new XElement("VerificationMethod", "CheckValue"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "Stop Frame to Test Timeout"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "StopFrame"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "ControllerNumber,RxFrame"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "RunProcesses"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "500"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "DSWGetMIDStatus"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "FW_Timeout"));
            scriptContent.Add(new XElement("TestResult", "10"));
            scriptContent.Add(new XElement("ExpectResult", "FW_Timeout_DebounceLevel"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult", "PASS"));
            scriptContent.Add(new XElement("VerificationMethod", "CheckValue"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "SendFrame"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "ControllerNumber,RxFrame,RxCycleTime"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "RunProcesses"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "500"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command", "DSWGetMIDStatus"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters", "FW_Timeout"));
            scriptContent.Add(new XElement("TestResult", "-10"));
            scriptContent.Add(new XElement("ExpectResult", "FW_DebounceLevel_InitValue"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult", "PASS"));
            scriptContent.Add(new XElement("VerificationMethod", "CheckValue"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);

            scriptContent = new XElement("ScriptContent");
            scriptContent.Add(new XElement("Command"));
            scriptContent.Add(new XElement("Disabled"));
            scriptContent.Add(new XElement("Parameters"));
            scriptContent.Add(new XElement("TestResult"));
            scriptContent.Add(new XElement("ExpectResult"));
            scriptContent.Add(new XElement("Tolerance"));
            scriptContent.Add(new XElement("VerificationResult"));
            scriptContent.Add(new XElement("VerificationMethod"));
            scriptContent.Add(new XElement("Description"));
            root_dncifTestScript.Add(scriptContent);
        }

        private void CreateScriptTitle(XElement root_dncifTestScript, Messages msg)
        {
            XElement scriptTitle = new XElement("ScriptTitle");
            scriptTitle.Add(new XElement("ScriptName", "ComScl_Frame_Rx_" + msg.MessageName));
            scriptTitle.Add(new XElement("ScriptProgID", "BSWSIM.Application"));

            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            scriptTitle.Add(new XElement("ScriptCreator", userName.ToUpper()));

            scriptTitle.Add(new XElement("ScriptCreateDateTime", DateTime.Now));
            scriptTitle.Add(new XElement("ScriptDescription", "ESP receive message: " + msg.MessageName));
            root_dncifTestScript.Add(scriptTitle);
        }

        private void ReadXMLFWList(string path)
        {
            XElement root = XElement.Load(path);
            _record = from el in root.Elements("record") select el;
        }

        public void CreateDataTable()
        {
            // log4net
            _log.Info("Generate FW list: /t Creating data ...");

            ReadXMLFWList(XML_FWLIST_PATH);

            CreateHeader(ref _dt);
            CreateData(ref _dt);

            // log4net
            _log.Info("Generate FW list: /t Create data successfuly.");
        }

        private void CreateData(ref DataTable dt)
        {
            dt.Rows.Clear();

            foreach (Messages msg in canMatrix)
            {
                // Check whether frame is receive and not event frame
                if (ESPIsReceiveMessage(msg) && !(msg.MessageSendType.ToLower() == "event"))
                {
                    #region  For frame
                    string frame_name = msg.MessageName;
                    string frame_id = msg.MessageID;
                    string frame_sendtype = msg.MessageSendType;
                    int frame_cycletime = Int32.Parse(msg.MessageCycleTime);

                    string fwtype = "ComScl";
                    string fwname = fwtype + "_" + frame_name + "_Timeout" + Environment.NewLine;
                    if (IsGen93Checked())
                    {
                        if (IsChecksumFrame(msg))
                        {
                            fwname = fwname + fwtype + "_" + frame_name + "_Checksum" + Environment.NewLine;
                        }
                        if (IsRntCounterFrame(msg))
                        {
                            fwname = fwname + fwtype + "_" + frame_name + "_AliveCounter" + Environment.NewLine;
                        }


                        fwname = fwname + fwtype + "_" + frame_name + "_DataLengthCode";
                    }
                    else
                    {
                        fwname = fwname + fwtype + "_" + frame_name + "_DataCorrupt";
                    }

                    int l_FailedThreshold = FailedThreshold;
                    int l_PassedThreshold = -1 * FailedThreshold;
                    int l_FailureLogging = FailedThreshold * frame_cycletime;
                    int l_FailureRecovery = FailedThreshold * frame_cycletime;

                    dt.Rows.Add(frame_name, frame_id, frame_sendtype, frame_cycletime, "", "", fwname, fwtype, l_FailedThreshold, l_PassedThreshold, l_FailureLogging, l_FailureRecovery);

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

                            dt.Rows.Add("", "", "", "", signal_name, signal_interface, sgn_failure_name, sgn_failure_type, l_FailedThreshold, l_PassedThreshold, l_FailureLogging, l_FailureRecovery);
                        }
                    }

                    #endregion

                }
            }
        }

        private bool IsChecksumFrame(Messages msg)
        {
            foreach (Signal sig in msg.ListSignal)
            {
                string signalName_Lower = sig.SignalName.ToLower();
                if ((signalName_Lower.Contains("checksum")) ||
                    (signalName_Lower.Contains("chksm")) ||
                    (signalName_Lower.Contains("crccheck"))
                   )
                {
                    return true;
                }
            }
            return false;
        }
        private Signal GetChecksumSignal(Messages msg)
        {
            foreach (Signal sig in msg.ListSignal)
            {
                string signalName_Lower = sig.SignalName.ToLower();
                if ((signalName_Lower.Contains("checksum")) ||
                    (signalName_Lower.Contains("chksm")) ||
                    (signalName_Lower.Contains("crccheck"))
                   )
                {
                    return sig;
                }
            }
            return null;
        }

        private Signal GetAliveCounterSignal(Messages msg)
        {
            foreach (Signal sig in msg.ListSignal)
            {
                string signalName_Lower = sig.SignalName.ToLower();

                if ((signalName_Lower.Contains("rollingcounter")) ||
                    (signalName_Lower.Contains("messagecounter")) ||
                    (signalName_Lower.Contains("alive"))
                   )
                {
                    return sig;
                }
            }
            return null;
        }

        private bool IsRntCounterFrame(Messages msg)
        {
            foreach (Signal sig in msg.ListSignal)
            {
                string signalName_Lower = sig.SignalName.ToLower();

                if ((signalName_Lower.Contains("rollingcounter")) ||
                    (signalName_Lower.Contains("messagecounter")) ||
                    (signalName_Lower.Contains("alive"))
                   )
                {
                    return true;
                }
            }
            return false;
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

        public bool Export2Excel()
        {
            bool result = false;
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
                try
                {
                    wb.SaveAs(path + "FW_Lists.xlsx");

                    // log4net log error here
                    _log.Error("Generate FW_Lists.xlsx successful.");

                    MessageBox.Show("Generated succesful", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    result = true;
                }
                catch (Exception ex)
                {
                    // log4net log error here
                    _log.Error("FW_Lists.xlsx is opening" + ex);

                    MessageBox.Show(ex.ToString(), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    result = false;
                }
            }
            return result;
        }

    }
}
