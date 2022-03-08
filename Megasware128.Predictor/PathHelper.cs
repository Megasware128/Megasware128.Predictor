using Windows.Win32;

namespace Megasware128.Predictor;

static class PathHelper
{
    public static unsafe bool ExistsOnPath(string executable)
    {
        fixed (char* executablePtr = $"{executable}.exe")
        {
            return PInvoke.PathFindOnPath(executablePtr);
        }
    }
}
