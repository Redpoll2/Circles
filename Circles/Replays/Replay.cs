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
    public sealed class Replay
    {
        /// <summary>
        /// Game mode of the replay.
        /// </summary>
        public Ruleset Ruleset { get; set; }

        /// <summary>
        /// Version of the game when the replay was created (ex. 20131216)
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// osu! beatmap MD5 hash.
        /// </summary>
        public string BeatmapHash { get; set; }

        /// <summary>
        /// Player name.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// osu! replay MD5 hash (includes certain properties of the replay)
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// Number of 300s.
        /// </summary>
        public ushort Count300 { get; set; }

        /// <summary>
        /// Number of 100s in standard, 150s in Taiko, 100s in CTB, 100s in mania.
        /// </summary>
        public ushort Count100 { get; set; }

        /// <summary>
        /// Number of 50s in standard, small fruit in CTB, 50s in mania.
        /// </summary>
        public ushort Count50 { get; set; }

        /// <summary>
        /// Number of Gekis in standard, Max 300s in mania.
        /// </summary>
        public ushort Gekis { get; set; }

        /// <summary>
        /// Number of Katus in standard, 200s in mania.
        /// </summary>
        public ushort Katus { get; set; }

        /// <summary>
        /// Number of misses.
        /// </summary>
        public ushort Misses { get; set; }

        /// <summary>
        /// Total score displayed on the score report.
        /// </summary>
        public uint TotalScore { get; set; }

        /// <summary>
        /// Greatest combo displayed on the score report.
        /// </summary>
        public ushort MaxCombo { get; set; }

        /// <summary>
        /// Perfect/full combo (true = no misses and no slider breaks and no early finished sliders)
        /// </summary>
        public bool IsPerfect { get; set; }

        /// <summary>
        /// Mods used.
        /// </summary>
        public Mods Mods { get; set; }

        /// <summary>
        /// Life bar graph.
        /// </summary>
        public IEnumerable<LifebarFrame> Lifebar { get; set; }

        /// <summary>
        /// Date when score was submitted.
        /// </summary>
        public DateTime SubmitDate { get; set; }

        /// <summary>
        /// Replay data.
        /// </summary>
        public IEnumerable<DataFrame> Data { get; set; }

        /// <summary>
        /// Online Score ID.
        /// </summary>
        public long ScoreId { get; set; }

        /// <summary>
        /// Total accuracy of all hits. Divide this by the number of targets in the map to find the accuracy displayed in-game.
        /// </summary>
        /// <remarks>
        /// Only present if <see cref="Mods.Target"/> is enabled.
        /// </remarks>
        public double? TotalAccuracy { get; set; }

        /// <summary>
        /// On replays set on version 20130319 or later, the 32-bit integer RNG seed used for the score will be encoded into an additional replay frame at the end of the LZMA stream
        /// </summary>
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
