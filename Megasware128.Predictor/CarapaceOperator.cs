using System.Text.Json;
using Megasware128.Predictor.Models;

namespace Megasware128.Predictor;

class CarapaceOperator
{
    private readonly CarapaceExecutor _executor = new();

    public IEnumerable<string> List()
    {
        var output = _executor.Execute("--list");

        return output.ReadToEnd().Split('\n');
    }

    public CarapaceExportResult Export(string tool)
    {
        var output = _executor.Execute($"{tool} export");

        return JsonSerializer.Deserialize<CarapaceExportResult>(output.BaseStream);
    }

    public CarapaceExportActionResult Export(string tool, string arguments)
    {
        var output = _executor.Execute($"{tool} export {arguments}");

        return JsonSerializer.Deserialize<CarapaceExportActionResult>(output.BaseStream);
    }
}