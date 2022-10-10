using Synapse.Demo.Domain.Models;

namespace Synapse.Demo.Persistence.IntegrationTests.Cases.Write;

public class DeviceTests
{
    [Fact]
    public async void Repository_Should_Work()
    {
        var repository = await RepositoryFactory.Create<IRepository<Device, string>>();
        repository.Should().NotBeNull();
        await Task.CompletedTask;
    }

    [Fact]
    public async void Add_Should_Exist_Work()
    {
        var repository = await RepositoryFactory.Create<IRepository<Device, string>>();
        var device = DomainModelsFactory.CreateDevice();

        await repository.AddAsync(device);
        await repository.SaveChangesAsync();
        var writeModel = await repository.FindAsync(device.Id);

        writeModel.Should().NotBeNull();
        writeModel.Id.Should().Be(device.Id);
        writeModel.Label.Should().Be(device.Label);
        writeModel.Type.Should().Be(device.Type);
        writeModel.Location.Should().Be(device.Location);
        writeModel.State.Should().BeEquivalentTo(device.State);
    }
}
