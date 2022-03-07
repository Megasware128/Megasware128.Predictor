using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Language;
using System.Management.Automation.Subsystem.Prediction;
using System.Threading;

namespace Megasware128.Predictor;

class CodePredictor : ICommandPredictor
{
    public string Description => "Command Predictor for Visual Studio Code";

    public Guid Id { get; } = new Guid("ad50c54b-c89b-4022-8399-d34aab3fa7ed");

    public string Name => "VSCode Predictor";

    public bool CanAcceptFeedback(PredictionClient client, PredictorFeedbackKind feedback)
    {
        switch (feedback)
        {
            case PredictorFeedbackKind.SuggestionDisplayed:
            case PredictorFeedbackKind.SuggestionAccepted:
            case PredictorFeedbackKind.CommandLineAccepted:
            case PredictorFeedbackKind.CommandLineExecuted:
                return true;
            default:
                return false;
        }
    }

    public SuggestionPackage GetSuggestion(PredictionClient client, PredictionContext context, CancellationToken cancellationToken)
    {
        var commandAst = context.RelatedAsts.OfType<CommandAst>().FirstOrDefault(c => c.GetCommandName() is "code" or "code-insiders");

        if (commandAst == null)
        {
            return new SuggestionPackage();
        }

        return GetCodeSuggestion(client, context, cancellationToken);
    }

    private SuggestionPackage GetCodeSuggestion(PredictionClient client, PredictionContext context, CancellationToken cancellationToken)
    {
        var suggestions = new List<PredictiveSuggestion>();
        
        var suggestion = new PredictiveSuggestion(".", "Open the woking directory in VS Code");

        return new SuggestionPackage(suggestions);
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
