using AutoMapper;

namespace Synapse.Demo.WebUI.Mappging.Configuration;

internal class SpecializedDeviceMappingConfiguration
    : IMappingConfiguration<Device, Thermometer>
    , IMappingConfiguration<Device, Hydrometer>
    , IMappingConfiguration<Device, Switchable>
{

    void IMappingConfiguration<Device, Thermometer>.Configure(IMappingExpression<Device, Thermometer> mapping)
    {
        mapping.ForMember(destination => destination.Temperature, options => options.MapFrom((source, destination) => (source.State as JObject)?["temperature"]?.ToObject<int?>()));
        mapping.ForMember(destination => destination.DesiredTemperature, options => options.MapFrom((source, destination) => (source.State as JObject)?["desired"]?.ToObject<int?>()));
    }

    void IMappingConfiguration<Device, Hydrometer>.Configure(IMappingExpression<Device, Hydrometer> mapping)
    {
        mapping.ForMember(destination => destination.Humidity, options => options.MapFrom((source, destination) => (source.State as JObject)?["humidity"]?.ToObject<int?>()));
    }

    void IMappingConfiguration<Device, Switchable>.Configure(IMappingExpression<Device, Switchable> mapping)
    {
        mapping.ForMember(destination => destination.IsTurnedOn, options => options.MapFrom((source, destination) => 
        {
            bool? on = (source.State as JObject)?["on"]?.ToObject<bool?>();
            return on.HasValue && on.Value;
        }));
    }
}
