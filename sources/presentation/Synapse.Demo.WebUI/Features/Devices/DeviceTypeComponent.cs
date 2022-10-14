namespace Synapse.Demo.WebUI;

/// <summary>
/// Used to map a device type to a component type
/// </summary>
public static class DeviceTypeComponent
{
    /// <summary>
    /// Holds the mapping to device type to component type
    /// </summary>
    public static Dictionary<string, Type> ComponentFor = new Dictionary<string, Type>()
    {
        { "sensor.thermometer", typeof(ThermometerWidget) }/*,
        { "sensor.hydrometer", typeof(DeviceWidget) },
        { "switch.light", typeof(DeviceWidget) },
        { "equipment.heater", typeof(DeviceWidget) },
        { "equipment.air-conditioning", typeof(DeviceWidget) }*/
    };
}
