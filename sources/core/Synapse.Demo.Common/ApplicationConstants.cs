namespace Synapse.Demo.Common;

public static class ApplicationConstants
{
    /// <summary>
    /// Holds the revers domain for the cloud events type
    /// </summary>
    public const string CloudEventsType = "com.synapse.demo";

    /// <summary>
    /// Holds the types of the devices
    /// </summary>
    public static class DeviceTypes
    {
        public const string SensorPrefix = "sensor.";
        public const string ThermometerSensor = SensorPrefix + "thermometer";
        public const string HydrometerSensor = SensorPrefix + "hydrometer";
        public const string MotionSensor = SensorPrefix + "motion";
        public const string LightsSwitch = "switch.light";
        public const string EquipmentPrefix = "equipment.";
        public const string HeaterEquipment = EquipmentPrefix + "heater";
        public const string AirConditioningEquipment = EquipmentPrefix + "air-conditioning";
        public const string BlindsEquipment = EquipmentPrefix + "blinds";
    }

    /// <summary>
    /// Holds the ids of the devices
    /// </summary>
    public static class DeviceIds
    {
        public const string Thermometer = "thermometer";
        public const string Hydrometer = "hydrometer";
        public const string Heater = "heater";
        public const string AirConditioning = "air-conditioning";
        public const string LightsPrefix = "lights-";
        public const string HallwayLights = LightsPrefix + "hallway";
        public const string LivingLights = LightsPrefix + "living";
        public const string MotionSensorPrefix = "motion-sensor-";
        public const string HallwayMotionSensor = MotionSensorPrefix + "hallway";
        public const string LivingMotionSensor = MotionSensorPrefix + "living";
        public const string BlindsPrefix = "blinds-";
        public const string LivingBlinds = BlindsPrefix + "living";
        public const string KitchenBlinds = BlindsPrefix + "kitchen";
    }
}
