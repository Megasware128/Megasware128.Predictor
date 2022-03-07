using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation.Language;
using System.Management.Automation.Subsystem.Prediction;
using System.Text;
using System.Text.Json;
using System.Threading;
using Windows.Win32;
using Windows.Win32.Foundation;

namespace Megasware128.Predictor;

class CarapacePredictor : ICommandPredictor
{
    private readonly ProcessStartInfo _startInfo = new("carapace", "--list")
    {
        StandardOutputEncoding = Encoding.UTF8,
        RedirectStandardOutput = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };
    private readonly string[] _completers;

    public CarapacePredictor()
    {
        var output = Process.Start(_startInfo).StandardOutput.ReadToEnd().Split('\n');

        unsafe bool ExistsOnPath(string executable)
        {
            fixed (char* executablePtr = executable)
            {
                return PInvoke.PathFindOnPath(executablePtr);
            }
        }

        _completers = output.Where(o => !string.IsNullOrWhiteSpace(o))
                            .Select(o => o[0..o.IndexOf(' ')])
                            // .Where(ExistsOnPath)
                            .ToArray();

        
    }

    public string Description => "Carapace command predictor";

    public Guid Id { get; } = new Guid("fceba9ea-9324-40ed-8038-cca5490dcbfa");

    public string Name => "Carapace";

    public bool CanAcceptFeedback(PredictionClient client, PredictorFeedbackKind feedback)
    {
        return false;
    }

    public SuggestionPackage GetSuggestion(PredictionClient client, PredictionContext context, CancellationToken cancellationToken)
    {
        var relatedAsts = context.RelatedAsts;

        var commandAst = default(CommandAst);
        var commandName = default(string);
        for (var i = relatedAsts.Count - 1; i >= 0; --i)
        {
            if (relatedAsts[i] is CommandAst c && c.GetCommandName() is string n && _completers.Contains(n))
            {
                commandAst = c;
                commandName = n;
                break;
            }
        }

        if (commandAst is null)
        {
            var list = new List<PredictiveSuggestion>();

            for (int i = 0; i < _completers.Length; i++)
            {
                var completer = _completers[i];
                if (completer.StartsWith(context.TokenAtCursor.Text))
                {
                    list.Add(new PredictiveSuggestion(completer));
                }
            }

            if (list.Count > 0)
            {
                return new SuggestionPackage(list);
            }
            else
            {
                return default;
            }
        }

        var arguments = new string[commandAst.CommandElements.Count];
        for (var i = 0; i < commandAst.CommandElements.Count; i++)
        {
            var element = commandAst.CommandElements[i];
            if (element is CommandParameterAst p)
            {
                arguments[i] = p.Extent.Text;
            }
            else if (element is CommandElementAst e)
            {
                arguments[i] = e.Extent.Text;
            }
        }

        _startInfo.Arguments = $"{commandName} powershell {string.Join(' ', arguments)}";
        var json = Process.Start(_startInfo).StandardOutput.BaseStream;

        var parsed = JsonDocument.Parse(json);

        var suggestions = parsed.RootElement.EnumerateArray().Select(e => new PredictiveSuggestion(context.InputAst.Extent.Text.Replace(context.TokenAtCursor.Text, e.GetProperty("CompletionText").GetString()), e.GetProperty("ToolTip").GetString()));

        if (suggestions.Any())
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
