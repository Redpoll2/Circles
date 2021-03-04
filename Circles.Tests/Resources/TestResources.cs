using System.IO;

namespace Circles.Tests.Resources
{
    public static class TestResources
    {
        public static Stream GetResourceStream(string filename)
        {
            var asm = typeof(TestResources).Assembly;

            return asm.GetManifestResourceStream(
                $"{asm.GetName().Name}.Resources.{filename}");
        }
    }
}
