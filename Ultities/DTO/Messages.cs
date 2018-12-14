using System.Collections.Generic;

namespace Ultities.DTO
{
    class Messages
    {
        private string messageName;
        private string messageID;
        private string messageSendType;
        private string messageCycleTime;
        private string messageLength;
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

        public string MessageCycleTime
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

        public string MessageLength
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

        internal List<Signal> ListSignal
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

        internal List<Node> ListNode
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
