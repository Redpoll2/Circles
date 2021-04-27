// Copyright (c) 2021 02Redpoll. See the MIT license for full information

using Circles.Rulesets;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

using LzmaDecoder = SevenZip.Compression.LZMA.Decoder;

namespace Circles.Replays
{
    public sealed class Replay : IReplay
    {
        public Ruleset Ruleset { get; set; }
        public int Version { get; set; }
        public string BeatmapHash { get; set; }
        public string Username { get; set; }
        public string Hash { get; set; }
        public ushort Count300 { get; set; }
        public ushort Count100 { get; set; }
        public ushort Count50 { get; set; }
        public ushort Gekis { get; set; }
        public ushort Katus { get; set; }
        public ushort Misses { get; set; }
        public uint TotalScore { get; set; }
        public ushort MaxCombo { get; set; }
        public bool IsPerfect { get; set; }
        public Mods Mods { get; set; }
        public IEnumerable<LifebarFrame> Lifebar { get; set; }
        public DateTime SubmitDate { get; set; }
        public IEnumerable<DataFrame> Data { get; set; }
        public long ScoreId { get; set; }
        public double? TotalAccuracy { get; set; }
        public int? Seed { get; set; }

        /// <summary>
        /// Loads osu! replay from given stream.
        /// </summary>
        public void Load(string filename, bool fullLoad = false)
        {
            var reader = new BinaryReader(new FileStream(filename, FileMode.Open, FileAccess.Read));

            try
            {
                Load(reader, fullLoad);
            }
            finally
            {
                reader.Dispose();
            }
        }

        /// <summary>
        /// Loads osu! replay from given stream.
        /// </summary>
        public void Load(Stream inStream, bool fullLoad = false)
        {
            var reader = new BinaryReader(inStream);

            try
            {
                Load(reader, fullLoad);
            }
            finally
            {
                reader.Dispose();
            }
        }

        /// <summary>
        /// Loads osu! replay from specified <see cref="BinaryReader"/>.
        /// </summary>
        public void Load(BinaryReader reader, bool fullLoad = false)
        {
            Ruleset = (Ruleset)reader.ReadByte();
            Version = reader.ReadInt32();
            BeatmapHash = reader.ReadByte() == 0x0B ? reader.ReadString() : null;
            Username = reader.ReadByte() == 0x0B ? reader.ReadString() : null;
            Hash = reader.ReadByte() == 0x0B ? reader.ReadString() : null;
            Count300 = reader.ReadUInt16();
            Count100 = reader.ReadUInt16();
            Count50 = reader.ReadUInt16();
            Gekis = reader.ReadUInt16();
            Katus = reader.ReadUInt16();
            Misses = reader.ReadUInt16();
            TotalScore = reader.ReadUInt32();
            MaxCombo = reader.ReadUInt16();
            IsPerfect = reader.ReadBoolean();
            Mods = (Mods)reader.ReadInt32();

            if (fullLoad)
            {
                string lifedata = reader.ReadByte() == 0x0B ? reader.ReadString() : null;

                if (!string.IsNullOrEmpty(lifedata))
                {
                    var frames = new List<LifebarFrame>();

                    foreach (string block in lifedata.Split(','))
                    {
                        var pair = block.Split('|');

                        if (pair.Length == 2)
                        {
                            frames.Add(new LifebarFrame
                            {
                                Time = int.Parse(pair[0]),
                                Value = float.Parse(pair[1], CultureInfo.InvariantCulture)
                            });
                        }
                    }

                    Lifebar = frames.ToArray();
                }
            }

            SubmitDate = new DateTime(reader.ReadInt64(), DateTimeKind.Utc);

            if (fullLoad)
            {
                var frames = new List<DataFrame>();

                foreach (string frame in Encoding.UTF8.GetString(LzmaDecoder.Decompress(reader.ReadBytes(reader.ReadInt32()))).Split(','))
                {
                    if (string.IsNullOrEmpty(frame))
                    {
                        break;
                    }

                    var pair = frame.Split('|');

                    if (pair.Length == 4)
                    {
                        if (pair[0] == "-12345")
                        {
                            Seed = int.Parse(pair[3]);

                            break;
                        }

                        frames.Add(new DataFrame
                        {
                            Time = int.Parse(pair[0]),
                            X = float.Parse(pair[1], CultureInfo.InvariantCulture),
                            Y = float.Parse(pair[2], CultureInfo.InvariantCulture),
                            Actions = (Actions)byte.Parse(pair[3])
                        });
                    }
                }

                Data = frames.ToArray();
            }
            else
            {
                int length = reader.ReadInt32();

                reader.BaseStream.Position += length;
            }

            ScoreId = reader.ReadInt64();

            if (Mods.HasFlag(Mods.Target))
            {
                TotalAccuracy = reader.ReadDouble();
            }
        }
    }
}
