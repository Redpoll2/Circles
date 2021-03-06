// Copyright (c) 2021 02Redpoll. See the MIT license for full information

using Circles.Rulesets;
using System;
using System.Collections.Generic;

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
    }
}
