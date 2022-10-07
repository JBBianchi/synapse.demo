using AutoMapper;

namespace Synapse.Demo.Application.Mapping.Configuration;

internal class LocationMappingConfiguration
    : IMappingConfiguration<string, Location>
{
    void IMappingConfiguration<string, Location>.Configure(IMappingExpression<string, Location> mapping)
    {
        mapping.ConstructUsing((source, context) =>
        {
            var location = Domain.Models.Location.FromString(source);
            return context.Mapper.Map<Location>(location);
        });
    }
}
