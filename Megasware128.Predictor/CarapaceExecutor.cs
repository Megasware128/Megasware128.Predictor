using System.Diagnostics;
using System.Text;

namespace Megasware128.Predictor;

class CarapaceExecutor
{
    private readonly ProcessStartInfo _startInfo = new("carapace")
    {
        StandardOutputEncoding = Encoding.UTF8,
        RedirectStandardOutput = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };

    public StreamReader Execute(string arguments)
    {
        lock (_startInfo)
        {
            _startInfo.Arguments = arguments;
            return Process.Start(_startInfo).StandardOutput;
        }
    }
}