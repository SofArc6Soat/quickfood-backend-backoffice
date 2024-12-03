using Amazon.Extensions.NETCore.Setup;
using Amazon.SQS;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Tests.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddAwsSqsMessageBroker(this IServiceCollection services)
    {
        var awsOptions = new AWSOptions
        {
            Profile = "default",
            Region = Amazon.RegionEndpoint.USEast1
        };

        services.AddDefaultAWSOptions(awsOptions);
        services.AddAWSService<IAmazonSQS>();
    }
}

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddAwsSqsMessageBroker_DeveRegistrarServicosCorretamente()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddAwsSqsMessageBroker();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var awsOptions = serviceProvider.GetService<AWSOptions>();
        Assert.NotNull(awsOptions);
        Assert.Equal("default", awsOptions.Profile);
        Assert.Equal(Amazon.RegionEndpoint.USEast1, awsOptions.Region);

        var amazonSqs = serviceProvider.GetService<IAmazonSQS>();
        Assert.NotNull(amazonSqs);
    }


    [Fact]
    public void AddAwsSqsMessageBroker_DeveRegistrarAWSOptions()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddAwsSqsMessageBroker();
        var serviceProvider = services.BuildServiceProvider();
        var awsOptions = serviceProvider.GetService<AWSOptions>();

        // Assert
        Assert.NotNull(awsOptions);
        Assert.Equal("default", awsOptions.Profile);
        Assert.Equal(Amazon.RegionEndpoint.USEast1, awsOptions.Region);
    }

    [Fact]
    public void AddAwsSqsMessageBroker_DeveRegistrarIAmazonSQS()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddAwsSqsMessageBroker();
        var serviceProvider = services.BuildServiceProvider();
        var amazonSqs = serviceProvider.GetService<IAmazonSQS>();

        // Assert
        Assert.NotNull(amazonSqs);
    }
}