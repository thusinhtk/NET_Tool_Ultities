﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ultities.DTO.Ultities;

namespace Ultities.DTO
{
    class Node
    {
        private string nodeName;
        private SendReceiveType sendType;
        public string NodeName
        {
            get
            {
                return nodeName;
            }

            set
            {
                nodeName = value;
            }
        }

        public SendReceiveType SendType
        {
            get
            {
                return sendType;
            }

            set
            {
                sendType = value;
            }
        }
    }
}
