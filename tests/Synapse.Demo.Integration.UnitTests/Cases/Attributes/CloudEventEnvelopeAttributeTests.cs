using Neuroglia.Data;

namespace Synapse.Demo.Integration.UnitTests.Attributes;

/// <summary>
/// Holds the tests for <see cref="CloudEventEnvelopeAttribute"/>
/// </summary>
public class CloudEventEnvelopeAttributeTests
{
    /// <summary>
    /// Holds the data used to instanciate a <see cref="Device"/>
    /// </summary>
    public readonly static object MockCloudEventEnvelopeAttribute = new
    {
        AggregateType = "aggregate-type",
        ActionName = "action-name",
        DataSchemaUri = "data://schema"
    };

    /// <summary>
    /// Valid constructor arguments, without data schema uri, should work
    /// </summary>
    [Fact]
    public void Constructor_Without_DataSchemaUri_Should_Work()
    {
        var mockAttribute = (dynamic)MockCloudEventEnvelopeAttribute;

        var attribute = new CloudEventEnvelopeAttribute(mockAttribute.AggregateType, mockAttribute.ActionName);

        attribute.Should().NotBeNull();
        attribute.AggregateType.Should().Be(mockAttribute.AggregateType);
        attribute.ActionName.Should().Be(mockAttribute.ActionName);
        attribute.DataSchemaUri.Should().BeNull();
    }

    /// <summary>
    /// Valid constructor arguments, with a data schema uri, should work
    /// </summary>
    [Fact]
    public void Constructor_With_DataSchemaUri_Should_Work()
    {
        var mockAttribute = (dynamic)MockCloudEventEnvelopeAttribute;

        var attribute = new CloudEventEnvelopeAttribute(mockAttribute.AggregateType, mockAttribute.ActionName, mockAttribute.DataSchemaUri);

        attribute.Should().NotBeNull();
        attribute.AggregateType.Should().Be(mockAttribute.AggregateType);
        attribute.ActionName.Should().Be(mockAttribute.ActionName);
        attribute.DataSchemaUri.Should().Be(mockAttribute.DataSchemaUri);
    }

    /// <summary>
    /// Invalid constructor arguments, without action name, should throw
    /// </summary>
    [Fact]
    public void Null_ActionName_Should_Throw()
    {
        var mockAttribute = (dynamic)MockCloudEventEnvelopeAttribute;

        var task = () => new CloudEventEnvelopeAttribute(mockAttribute.AggregateType, null);

        task.Should().Throw<DomainArgumentException>()
            .Where(ex => ex.ArgumentName == "actionName");
    }

    /// <summary>
    /// Invalid constructor arguments, with an empty action name, should throw
    /// </summary>
    [Fact]
    public void Empty_ActionName_Should_Throw()
    {
        var mockAttribute = (dynamic)MockCloudEventEnvelopeAttribute;

        var task = () => new CloudEventEnvelopeAttribute(mockAttribute.AggregateType, " ");

        task.Should().Throw<DomainArgumentException>()
            .Where(ex => ex.ArgumentName == "actionName");
    }

    /// <summary>
    /// Invalid constructor arguments, without aggregate type, should throw
    /// </summary>
    [Fact]
    public void Null_AggregateType_Should_Throw()
    {
        var mockAttribute = (dynamic)MockCloudEventEnvelopeAttribute;

        var task = () => new CloudEventEnvelopeAttribute(null, mockAttribute.ActionName);

        task.Should().Throw<DomainArgumentException>()
            .Where(ex => ex.ArgumentName == "aggregateType");
    }

    /// <summary>
    /// Invalid constructor arguments, with an empty aggregate type, should throw
    /// </summary>
    [Fact]
    public void Empty_AggregateType_Should_Throw()
    {
        var mockAttribute = (dynamic)MockCloudEventEnvelopeAttribute;

        var task = () => new CloudEventEnvelopeAttribute(" ", mockAttribute.ActionName);

        task.Should().Throw<DomainArgumentException>()
            .Where(ex => ex.ArgumentName == "aggregateType");
    }
}
