﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil.IO;

namespace PolskaBot.Core.Darkorbit.Commands
{
    class ServerRequestCode : Command
    {
        public const ushort ID = 9058;

        public int codeLength { get; private set; }
        public byte[] code { get; private set; }
        public int fakeSize { get; private set; }

        public ServerRequestCode(EndianBinaryReader reader)
        {
            fakeSize = reader.ReadInt32();
            codeLength = reader.ReadInt32();
            code = reader.ReadBytes(codeLength);
        }
    }
}
