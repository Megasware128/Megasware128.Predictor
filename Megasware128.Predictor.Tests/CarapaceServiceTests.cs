using Xunit;

namespace Megasware128.Predictor.Tests;

public class CarapaceServiceTests
{
    [Fact]
    public void CompletersTest()
    {
        var service = new CarapaceService();

        Assert.NotEmpty(service.Completers);
    }

    [Fact]
    public void CarapaceCompleterTest()
    {
        var service = new CarapaceService();

        Assert.Contains("carapace", service.Completers);
    }
}