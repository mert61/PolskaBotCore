﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    public class ActionRequest : Command
    {
        public const ushort ID = 31360;

        public int BarID { get; private set; } //var_2186
        public int Type { get; private set; } //var_2204
        public string Action { get; private set; } //var_2111

        public ActionRequest(string action, int type, int barID)
        {
            Action = action;
            Type = type;
            BarID = barID;
            Write();
        }

        public void Write()
        {
            packetWriter.Write((short)(10 + Action.Length));
            packetWriter.Write(ID);
            packetWriter.Write((short)Action.Length);
            packetWriter.Write(Encoding.UTF8.GetBytes(Action));
            packetWriter.Write((short)BarID);
            packetWriter.Write((short)Type);
            packetWriter.Write((short)19475);
        }
    }
}
