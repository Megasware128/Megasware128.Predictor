namespace Megasware128.Predictor;

class CarapaceService
{
    private readonly Dictionary<string, (string, string)[]> _completerSuggestions = new();

    public CarapaceService()
    {
        var carapaceOperator = new CarapaceOperator();

        var output = carapaceOperator.List();

        Completers = output.Where(o => !string.IsNullOrWhiteSpace(o))
                           .Select(o => o[0..o.IndexOf(' ')])
                           .Where(PathHelper.ExistsOnPath)
                           .ToArray();

        for (int i = 0; i < Completers.Length; i++)
        {
            var completer = Completers[i];

            var export = carapaceOperator.Export(completer);

            var commands = export.Commands;

            if(commands is null)
            {
                continue;
            }
            
            var suggestions = new (string, string)[commands.Count];

            for (int j = 0; j < commands.Count; j++)
            {
                var command = commands[j];

                suggestions[j] = (command.Name, command.Short);
            }

            _completerSuggestions.Add(completer, suggestions);
        }
    }

    public string[] Completers { get; }

    public (string Name, string Description)[] GetSuggestions(string completer)
    {
        if (_completerSuggestions.TryGetValue(completer, out var suggestions))
        {
            return suggestions;
        }

        return Array.Empty<(string, string)>();
    }
}