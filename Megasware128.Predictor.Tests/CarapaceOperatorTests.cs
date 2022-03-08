using Xunit;

namespace Megasware128.Predictor.Tests;

public class CarapaceOperatorTests
{
    [Fact]
    public void ListTest()
    {
        var executor = new CarapaceOperator();

        var output = executor.List();

        Assert.NotEmpty(output);
    }

    [Fact]
    public void CarapaceCompleterTest()
    {
        var executor = new CarapaceOperator();

        var output = executor.List();

        Assert.True(output.Any(o => o.StartsWith("carapace")), "carapace is not in the list");
    }

    [Fact]
    public void ExportTest()
    {
        var executor = new CarapaceOperator();

        var output = executor.Export("carapace");

        Assert.NotNull(output);
    }

    [Fact]
    public void ExportActionTest()
    {
        var executor = new CarapaceOperator();

        var output = executor.Export("carapace", "export");

        Assert.NotNull(output);
    }
}