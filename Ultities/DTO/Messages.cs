using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultities.DTO
{
    class Messages
    {
        private string messageName;
        private string messageID;
        private string messageSendType;
        private UInt32 messageCycleTime;
        private UInt16 messageLength;
        private List<Signal> listSignal;
        private List<Node> listNode;

        public string MessageName
        {
            get
            {
                return messageName;
            }

            set
            {
                messageName = value;
            }
        }

        public string MessageID
        {
            get
            {
                return messageID;
            }

            set
            {
                messageID = value;
            }
        }

        public string MessageSendType
        {
            get
            {
                return messageSendType;
            }

            set
            {
                messageSendType = value;
            }
        }

        public uint MessageCycleTime
        {
            get
            {
                return messageCycleTime;
            }

            set
            {
                messageCycleTime = value;
            }
        }

        public ushort MessageLength
        {
            get
            {
                return messageLength;
            }

            set
            {
                messageLength = value;
            }
        }

        public List<Signal> ListSignal
        {
            get
            {
                return listSignal;
            }

            set
            {
                listSignal = value;
            }
        }

        public List<Node> ListNode
        {
            get
            {
                return listNode;
            }

            set
            {
                listNode = value;
            }
        }
    }
}
