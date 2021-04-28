using Circles.Replays;
using Circles.Tests.Resources;
using Xunit;

namespace Circles.Tests
{
    public class ReaderWriterTest
    {
        [Fact]
        public void Writer()
        {
            var replay = new Replay();

            replay.Load(TestResources.GetResourceStream("aetrna - 07th Expansion - rog-unlimitation [AngelHoney] (2020-07-24) Osu.osr"), true);
            replay.Username = null;
            replay.Save()
        }
    }
}
