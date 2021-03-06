﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands.PostHandshake
{
    public class OldStylePacket : Command
    {
        public const ushort ID = 5586;

        public string Message { get; set; }

        public OldStylePacket(EndianBinaryReader reader)
        {
            Message = Encoding.Default.GetString(reader.ReadBytes(reader.ReadUInt16()));
        }

        public OldStylePacket(string message)
        {
            Message = message;
            Write();
        }

        public void Write()
        {
            packetWriter.Write((short)(8 + Message.Length));
            packetWriter.Write(ID);
            packetWriter.Write((ushort)Message.Length);
            packetWriter.Write(Encoding.UTF8.GetBytes(Message));
            packetWriter.Write((short)-593);
            packetWriter.Write((short)-16529);
        }
    }
}
