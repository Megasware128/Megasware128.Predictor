using Xunit;

namespace Megasware128.Predictor.Tests;

public class CarapaceExecutoreTests
{
    [Fact]
    public void HelpTest()
    {
        var executor = new CarapaceExecutor();

        var output = executor.Execute("--help");

        Assert.Contains("Usage:", output.ReadToEnd());
    }
}