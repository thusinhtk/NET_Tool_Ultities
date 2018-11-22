using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultities.DTO
{
    class Signal
    {
        private string signalName;
        private string signalDescription;
        private string signalByteOrder;
        private UInt16 signalStartBit;
        private UInt16 signalBitLength;
        private string signalDataType;
        private double signalFactor;
        private double signalOffset;
        private double signalPhyMin;
        private double signalPhyMax;
        private UInt32 signalHexMin;
        private UInt64 signalHexMax;
        private UInt32 signalInitHex;
        private UInt64 signalInvalidHex;
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
        public double SignalOffset
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
        public double SignalPhyMin
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
        public double SignalPhyMax
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
        public ushort SignalStartBit
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
        public ushort SignalBitLength
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
        public double SignalFactor
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

        public uint SignalHexMin
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

        public ulong SignalHexMax
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

        public ulong SignalInvalidHex
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

        public uint SignalInitHex
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
    }
}
