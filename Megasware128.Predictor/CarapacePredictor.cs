using System.Management.Automation.Language;
using System.Management.Automation.Subsystem.Prediction;
using System.Text;

namespace Megasware128.Predictor;

class CarapacePredictor : ICommandPredictor
{

    private readonly CarapaceService _carapaceService = new();

    public string Description => "Carapace command predictor";

    public Guid Id { get; } = new Guid("fceba9ea-9324-40ed-8038-cca5490dcbfa");

    public string Name => "Carapace";

    public bool CanAcceptFeedback(PredictionClient client, PredictorFeedbackKind feedback)
    {
        return false;
    }

    public SuggestionPackage GetSuggestion(PredictionClient client, PredictionContext context, CancellationToken cancellationToken)
    {
        var completers = _carapaceService.Completers;

        var relatedAsts = context.RelatedAsts;

        var commandAst = default(CommandAst);
        var commandName = default(string);
        for (var i = relatedAsts.Count - 1; i >= 0; --i)
        {
            if (relatedAsts[i] is CommandAst c && c.GetCommandName() is string n && completers.Contains(n))
            {
                commandAst = c;
                commandName = n;
                break;
            }
        }

        if (commandAst is null)
        {
            var list = new List<PredictiveSuggestion>();

            for (int i = 0; i < completers.Length; i++)
            {
                var completer = completers[i];
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

        var suggestions = _carapaceService.GetSuggestions(commandName);

        if (suggestions.Any())
        {
            var list = new List<PredictiveSuggestion>();

            for (int i = 0; i < suggestions.Length; i++)
            {
                var suggestion = suggestions[i];
                if (suggestion.Name.StartsWith(context.TokenAtCursor.Text))
                {
                    list.Add(new PredictiveSuggestion(context.InputAst.Extent.Text.Replace(context.TokenAtCursor.Text, suggestion.Name), suggestion.Description));
                }
            }

            return new SuggestionPackage(list);
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
