// Copyright (c) 2021 02Redpoll. See the MIT license for full information

using System.IO;
using System.Text;

namespace Circles
{
    public sealed class OsuBinaryReader : BinaryReader
    {
        public OsuBinaryReader(Stream input) : base(input, new UTF8Encoding(), false)
        {
        }

        public OsuBinaryReader(Stream input, Encoding encoding) : base(input, encoding, false)
        {
        }

        public OsuBinaryReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
        }

        public override string ReadString()
        {
            if (ReadByte() == 0x0B)
            {
                return base.ReadString();
            }

            return null;
        }
    }
}
