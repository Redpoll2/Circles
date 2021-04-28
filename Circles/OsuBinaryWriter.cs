// Copyright (c) 2021 02Redpoll. See the MIT license for full information

using System.IO;
using System.Text;

namespace Circles
{
    public sealed class OsuBinaryWriter : BinaryWriter
    {
        public OsuBinaryWriter(Stream input) : base(input, new UTF8Encoding(), false)
        {
        }

        public OsuBinaryWriter(Stream input, Encoding encoding) : base(input, encoding, false)
        {
        }

        public OsuBinaryWriter(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
        }

        public override void Write(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Write(0x00);
            }
            else
            {
                Write(0x0B);
                base.Write(value);
            }
        }
    }
}
