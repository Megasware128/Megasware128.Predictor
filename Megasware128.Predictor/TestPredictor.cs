using System;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Management.Automation.Subsystem;
using System.Management.Automation.Subsystem.Prediction;
using System.Threading;

namespace Megasware128.Predictor
{
    [Cmdlet(VerbsDiagnostic.Test, nameof(CarapacePredictor))]
    [OutputType(typeof(void))]
    public class TestPredictor : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string Input { get; set; }

        protected override void ProcessRecord()
        {
            try
            {
                WriteObject(SubsystemManager.GetSubsystemInfo(SubsystemKind.CommandPredictor).Implementations);

                var predictor = new CarapacePredictor();

                var client = new PredictionClient(nameof(TestPredictor), PredictionClientKind.Terminal);

                var ast = Parser.ParseInput(Input, out var tokens, out _);
                var context = new PredictionContext(ast, tokens);

                WriteObject(context);

                var suggestions = predictor.GetSuggestion(client, context, CancellationToken.None);

                WriteObject(suggestions);
                WriteObject(suggestions.SuggestionEntries);

                var result = CommandPrediction.PredictInputAsync(client, ast, tokens, 1000).Result;

                WriteObject(result);
                WriteObject(result.SelectMany(r => r.Suggestions));
            }
            catch (Exception e)
            {
                WriteObject(e);
            }
        }
    }
}
