using FluentAssertions;
using FusionPasswordResetPortal.Application.Interfaces;
using FusionPasswordResetPortal.Application.Services;
using FusionPasswordResetPortal.Domain.Entities;
using Moq;

namespace FusionPasswordResetPortal.UnitTests;

public class FusionInstanceServiceTests
{
    [Fact]
    public async Task GetActiveInstancesAsync_ShouldMapEntitiesToDtos()
    {
        var repository = new Mock<IFusionInstanceRepository>();
        repository.Setup(x => x.GetActiveInstancesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[]
            {
                new FusionResetPasswordInstance
                {
                    Id = 7,
                    InstanceCode = "PROD",
                    InstanceName = "Production",
                    EnvironmentName = "Production",
                    ApiBaseUrl = "https://example",
                    CreatedBy = "test"
                }
            });

        var service = new FusionInstanceService(repository.Object);
        var result = await service.GetActiveInstancesAsync();

        result.Should().ContainSingle();
        result.Single().Id.Should().Be(7);
    }
}
