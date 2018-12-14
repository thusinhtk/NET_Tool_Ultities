using System.Collections.Generic;

namespace Ultities.DTO
{
    class Signal
    {
        private string signalName;
        private string signalDescription;
        private string signalByteOrder;
        private string signalStartBit;
        private string signalBitLength;
        private string signalDataType;
        private string signalFactor;
        private string signalOffset;
        private string signalPhyMin;
        private string signalPhyMax;
        private string signalHexMin;
        private string signalHexMax;
        private string signalInitHex;
        private string signalInvalidHex;
        private string signalUnit;
        private string signalValueDescription;
        private List<Node> listNode;

        public string SignalName
        {
            get
            {
                return signalName;
            }

            set
            {
                signalName = value;
            }
        }

        public string SignalDescription
        {
            get
            {
                return signalDescription;
            }

            set
            {
                signalDescription = value;
            }
        }

        public string SignalByteOrder
        {
            get
            {
                return signalByteOrder;
            }

            set
            {
                signalByteOrder = value;
            }
        }

        public string SignalStartBit
        {
            get
            {
                return signalStartBit;
            }

            set
            {
                signalStartBit = value;
            }
        }

        public string SignalBitLength
        {
            get
            {
                return signalBitLength;
            }

            set
            {
                signalBitLength = value;
            }
        }

        public string SignalDataType
        {
            get
            {
                return signalDataType;
            }

            set
            {
                signalDataType = value;
            }
        }

        public string SignalFactor
        {
            get
            {
                return signalFactor;
            }

            set
            {
                signalFactor = value;
            }
        }

        public string SignalOffset
        {
            get
            {
                return signalOffset;
            }

            set
            {
                signalOffset = value;
            }
        }

        public string SignalPhyMin
        {
            get
            {
                return signalPhyMin;
            }

            set
            {
                signalPhyMin = value;
            }
        }

        public string SignalPhyMax
        {
            get
            {
                return signalPhyMax;
            }

            set
            {
                signalPhyMax = value;
            }
        }

        public string SignalHexMin
        {
            get
            {
                return signalHexMin;
            }

            set
            {
                signalHexMin = value;
            }
        }

        public string SignalHexMax
        {
            get
            {
                return signalHexMax;
            }

            set
            {
                signalHexMax = value;
            }
        }

        public string SignalInitHex
        {
            get
            {
                return signalInitHex;
            }

            set
            {
                signalInitHex = value;
            }
        }

        public string SignalInvalidHex
        {
            get
            {
                return signalInvalidHex;
            }

            set
            {
                signalInvalidHex = value;
            }
        }

        public string SignalUnit
        {
            get
            {
                return signalUnit;
            }

            set
            {
                signalUnit = value;
            }
        }

        public string SignalValueDescription
        {
            get
            {
                return signalValueDescription;
            }

            set
            {
                signalValueDescription = value;
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
