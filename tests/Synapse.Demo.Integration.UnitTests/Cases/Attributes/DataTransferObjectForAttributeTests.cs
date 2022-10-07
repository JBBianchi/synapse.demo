using Neuroglia.Data;

namespace Synapse.Demo.Integration.UnitTests.Attributes;

/// <summary>
/// Holds the tests for <see cref="DataTransferObjectForAttribute"/>
/// </summary>
public class DataTransferObjectForAttributeTests
{
    /// <summary>
    /// Holds the data used to instanciate a <see cref="Device"/>
    /// </summary>
    public readonly static object MockDataTransferObjectForAttribute = new
    {
        Type = typeof(DataTransferObjectForAttributeTests),
    };

    /// <summary>
    /// Valid constructor arguments, without data schema uri, should work
    /// </summary>
    [Fact]
    public void Constructor_Should_Work()
    {
        var mockAttribute = (dynamic)MockDataTransferObjectForAttribute;

        var attribute = new DataTransferObjectForAttribute(mockAttribute.Type);

        attribute.Should().NotBeNull();
        attribute.Type.Should().Be(mockAttribute.Type);
    }

    /// <summary>
    /// Invalid constructor arguments, without type, should throw
    /// </summary>
    [Fact]
    public void Null_Type_Should_Throw()
    {
        var mockAttribute = (dynamic)MockDataTransferObjectForAttribute;

        var task = () => new DataTransferObjectForAttribute(null!);

        task.Should().Throw<DomainArgumentException>()
            .Where(ex => ex.ArgumentName == "type");
    }
}
