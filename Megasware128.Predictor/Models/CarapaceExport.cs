namespace Megasware128.Predictor.Models;

record CarapaceExportResult(string Name, string Short, string Long, IReadOnlyList<CarapaceExportCommand> Commands, IReadOnlyList<CarapaceExportFlag> LocalFlags, IReadOnlyList<CarapaceExportFlag> PersistentFlags);
record CarapaceExportCommand(string Name, string Short, IReadOnlyList<CarapaceExportFlag> LocalFlags);
record CarapaceExportFlag(string Longhand, string Shorthand, string Usage, string Type);

record CarapaceExportActionResult(bool Nospace, IReadOnlyList<CarapaceExportAction> RawValues);
record CarapaceExportAction(string Value, string Display, string Description);