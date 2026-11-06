using GestionnaireReseauWindows.Services;

namespace GestionnaireReseauWindows.Tests;

public class HotspotCommandBuilderTests
{
    [Fact]
    public void BuildStartCommand_ShouldIncludeSsidAndKey()
    {
        var command = HotspotCommandBuilder.BuildStartCommand("MonReseau", "motdepasse123");

        Assert.Contains("netsh wlan set hostednetwork", command);
        Assert.Contains("ssid=MonReseau", command);
        Assert.Contains("key=motdepasse123", command);
    }

    [Fact]
    public void BuildStopCommand_ShouldStopHostedNetwork()
    {
        var command = HotspotCommandBuilder.BuildStopCommand();

        Assert.Equal("netsh wlan stop hostednetwork", command);
    }

    [Fact]
    public void BuildStartCommand_ShouldRejectWeakPassword()
    {
        var ex = Assert.Throws<ArgumentException>(() => HotspotCommandBuilder.BuildStartCommand("MonReseau", "1234"));
        Assert.Contains("8 caractères", ex.Message);
    }
}
