using AutoMapper;

namespace Synapse.Demo.Application.Mapping.Configuration;

internal class CloudEventMappingConfiguration
    : IMappingConfiguration<CloudEvent, CloudEventDto>
{

    void IMappingConfiguration<CloudEvent, CloudEventDto>.Configure(IMappingExpression<CloudEvent, CloudEventDto> mapping)
    {

    }

}
