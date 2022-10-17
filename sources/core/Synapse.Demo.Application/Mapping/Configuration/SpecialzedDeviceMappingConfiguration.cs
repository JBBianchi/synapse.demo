﻿using AutoMapper;

namespace Synapse.Demo.Application.Mapping.Configuration;

internal class SpecializedDeviceMappingConfiguration
    : IMappingConfiguration<Device, Thermometer>
    , IMappingConfiguration<Device, Hydrometer>
    , IMappingConfiguration<Device, Switchable>
{

    void IMappingConfiguration<Device, Thermometer>.Configure(IMappingExpression<Device, Thermometer> mapping)
    {
        mapping.ForMember(destination => destination.Temperature, options => options.MapFrom((source, destination) => (int?)source.State?.ToDictionary()["temperature"]));
        mapping.ForMember(destination => destination.DesiredTemperature, options => options.MapFrom((source, destination) => (int?)source.State?.ToDictionary()["desired"]));
    }

    void IMappingConfiguration<Device, Hydrometer>.Configure(IMappingExpression<Device, Hydrometer> mapping)
    {
        mapping.ForMember(destination => destination.Humidity, options => options.MapFrom((source, destination) => (int?)source.State?.ToDictionary()["humidity"]));
    }

    void IMappingConfiguration<Device, Switchable>.Configure(IMappingExpression<Device, Switchable> mapping)
    {
        mapping.ForMember(destination => destination.IsTurnedOn, options => options.MapFrom((source, destination) =>
        {
            bool? on = (bool?)source.State?.ToDictionary()["on"];
            return on.HasValue && on.Value;
        }));
    }
}