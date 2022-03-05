using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation.Language;
using System.Management.Automation.Subsystem.Prediction;
using System.Text.Json;
using System.Threading;

namespace Megasware128.Predictor;

class CarapacePredictor : ICommandPredictor
{
    public string Description => "Command Predictor for Carapace";

    public Guid Id { get; } = new Guid("fceba9ea-9324-40ed-8038-cca5490dcbfa");

    public string Name => "Carapace Predictor";

    public bool CanAcceptFeedback(PredictionClient client, PredictorFeedbackKind feedback)
    {
        return false;
    }

    public SuggestionPackage GetSuggestion(PredictionClient client, PredictionContext context, CancellationToken cancellationToken)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "carapace",
            Arguments = "--list",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        var output = Process.Start(processStartInfo).StandardOutput.ReadToEnd().Split('\n');

        var command = context.RelatedAsts.OfType<CommandAst>().FirstOrDefault(c => output.Any(o => o.StartsWith(c.GetCommandName())));

        if (command is null)
        {
            return default;
        }

        var arguments = context.RelatedAsts.Skip(1).Select(a => a.Extent.Text);

        processStartInfo.Arguments = $"{command.GetCommandName()} powershell {string.Join(' ', arguments)}";
        var json = Process.Start(processStartInfo).StandardOutput.ReadToEnd();

        var parsed = JsonDocument.Parse(json);

        var suggestions = parsed.RootElement.EnumerateArray().Select(e => new PredictiveSuggestion(e.GetProperty("CompletionText").GetString(), e.GetProperty("ToolTip").GetString()));

        if(suggestions.Any())
        {
            return new SuggestionPackage(suggestions.ToList());
        }

        return default;
    }

    public void OnCommandLineAccepted(PredictionClient client, IReadOnlyList<string> history)
    {
    }

    public void OnCommandLineExecuted(PredictionClient client, string commandLine, bool success)
    {
    }

    public void OnSuggestionAccepted(PredictionClient client, uint session, string acceptedSuggestion)
    {
    }

    public void OnSuggestionDisplayed(PredictionClient client, uint session, int countOrIndex)
    {
    }
}
