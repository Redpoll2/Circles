// Copyright (c) 2021 02Redpoll. See the MIT license for full information

using System;
using System.Globalization;

namespace Circles.Replays
{
    public struct DataFrame
    {
        /// <summary>
        /// Time in milliseconds since the previous action.
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// x-coordinate of the cursor from 0 - 512
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// y-coordinate of the cursor from 0 - 384
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Bitwise combination of keys/mouse buttons pressed.
        /// </summary>
        public Actions Actions { get; set; }

        public override string ToString() => $"{Time}|{X.ToString(CultureInfo.InvariantCulture)}|{Y.ToString(CultureInfo.InvariantCulture)}|{(byte)Actions}";
    }

    [Flags]
    public enum Actions : byte
    {
        MouseLeft = 1,
        MouseRight = 1 << 1,
        KeyLeft = 1 << 2,
        KeyRight = 1 << 3,
        Smoke = 1 << 4
    }
}
