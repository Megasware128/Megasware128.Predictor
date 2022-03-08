using Xunit;

namespace Megasware128.Predictor.Tests;

public class PathHelperTests
{
    [Fact]
    public void ExistsOnPathTest()
    {
        Assert.True(PathHelper.ExistsOnPath("carapace"));
    }

    [Fact]
    public void ExistsOnPathFalseTest()
    {
        Assert.False(PathHelper.ExistsOnPath("carapace-not-found"));
    }
}