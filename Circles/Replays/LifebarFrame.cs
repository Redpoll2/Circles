// Copyright (c) 2021 02Redpoll. See the MIT license for full information

using System.Globalization;

namespace Circles.Replays
{
    public struct LifebarFrame
    {
        /// <summary>
        /// Time in milliseconds into the song.
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// Value from 0 - 1 that represents the amount of life you have at the given time.
        /// </summary>
        public float Value { get; set; }

        public override string ToString() => $"{Time}|{Value.ToString(CultureInfo.InvariantCulture)}";
    }
}
