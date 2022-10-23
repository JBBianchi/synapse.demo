namespace Synapse.Demo.Common;

public static class ApplicationConstants
{
    /// <summary>
    /// Holds the revers domain for the cloud events type
    /// </summary>
    public const string CloudEventsType = "com.synapse.demo";

    /// <summary>
    /// Holds the ids of the devices
    /// </summary>
    public static class DeviceIds
    {
        public const string Thermometer = "thermometer";
        public const string Hydrometer = "hydrometer";
        public const string Heater = "heater";
        public const string AirConditioning = "air-conditioning";
        public const string HallwayLights = "lights-hallway";
        public const string LivingLights = "lights-living";
        public const string HallwayMotionSensor = "motion-sensor-hallway";
        public const string LivingMotionSensor = "motion-sensor-living";
    }
}
