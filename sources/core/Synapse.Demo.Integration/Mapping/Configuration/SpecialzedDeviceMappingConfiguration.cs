using AutoMapper;

namespace Synapse.Demo.Integration.Mapping.Configuration;

internal class SpecializedDeviceMappingConfiguration
    : IMappingConfiguration<Device, Thermometer>
    , IMappingConfiguration<Device, Hydrometer>
    , IMappingConfiguration<Device, Switchable>
{
    private T? GetStateValue<T>(Device source, string property) {
        if (source.State == null) return default(T);
        if (source.State?.GetType() == typeof(JObject))
        {
            var jobjSrc = (JObject)source.State;
            if (!jobjSrc.ContainsKey(property)) return default(T); 
            try
            {
                return jobjSrc[property].ToObject<T>();
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
        else
        {
            var dicSrc = source.State.ToDictionary();
            if (!dicSrc.ContainsKey(property)) return default(T);
            return (T)dicSrc[property];
        }
    }

    void IMappingConfiguration<Device, Thermometer>.Configure(IMappingExpression<Device, Thermometer> mapping)
    {
        mapping.ForMember(destination => destination.Temperature, options => options.MapFrom((source, destination) => this.GetStateValue<int?>(source, "temperature")));
        mapping.ForMember(destination => destination.DesiredTemperature, options => options.MapFrom((source, destination) => this.GetStateValue<int?>(source, "desired")));
    }

    void IMappingConfiguration<Device, Hydrometer>.Configure(IMappingExpression<Device, Hydrometer> mapping)
    {
        mapping.ForMember(destination => destination.Humidity, options => options.MapFrom((source, destination) => this.GetStateValue<int?>(source, "humidity")));
    }

    void IMappingConfiguration<Device, Switchable>.Configure(IMappingExpression<Device, Switchable> mapping)
    {
        mapping.ForMember(destination => destination.IsTurnedOn, options => options.MapFrom((source, destination) =>
        {
            bool? on = this.GetStateValue<bool?>(source, "on");
            return on.HasValue && on.Value;
        }));
    }
}
