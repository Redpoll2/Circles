using Circles.Replays;
using Circles.Rulesets;
using Circles.Tests.Resources;
using Xunit;

namespace Circles.Tests
{
    public class ReplayTest
    {
        [Fact]
        public void LoadHeader()
        {
            var replay = new Replay();

            replay.Load(TestResources.GetResourceStream("aetrna - 07th Expansion - rog-unlimitation [AngelHoney] (2020-07-24) Osu.osr"));

            Assert.Equal("aetrna", replay.Username);
            Assert.Equal(Mods.DoubleTime | Mods.Hidden, replay.Mods);
        }

        [Fact]
        public void LoadFull()
        {
            var replay = new Replay();

            replay.Load(TestResources.GetResourceStream("aetrna - 07th Expansion - rog-unlimitation [AngelHoney] (2020-07-24) Osu.osr"), true);

            // Assert.NotNull(replay.Lifebar);      If lifebar is null, it don't fall below his max value (1)
            Assert.NotNull(replay.Data);
        }
    }
}
