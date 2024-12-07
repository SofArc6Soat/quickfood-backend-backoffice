using Core.WebApi.Configurations;

namespace QuickFood_Backoffice.Tests.Core.WebApi.Configurations;

public class JwtBearerConfigureOptionsTests
{
    [Fact]
    public void JwtBearerConfigureOptions_DeveInicializarComValoresPadrao()
    {
        // Act
        var options = new JwtBearerConfigureOptions();

        // Assert
        Assert.Equal(string.Empty, options.Authority);
        Assert.Equal(string.Empty, options.MetadataAddress);
    }

    [Fact]
    public void JwtBearerConfigureOptions_DevePermitirAtribuicaoDeValores()
    {
        // Arrange
        var authority = "https://example.com";
        var metadataAddress = "https://example.com/.well-known/openid-configuration";

        // Act
        var options = new JwtBearerConfigureOptions
        {
            Authority = authority,
            MetadataAddress = metadataAddress
        };

        // Assert
        Assert.Equal(authority, options.Authority);
        Assert.Equal(metadataAddress, options.MetadataAddress);
    }
}