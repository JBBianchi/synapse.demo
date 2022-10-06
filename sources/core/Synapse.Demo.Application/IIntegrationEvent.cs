using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synapse.Demo.Application;

internal interface IIntegrationEvent
    : Neuroglia.IIntegrationEvent
{
    /// <summary>
    /// Gets the id of the aggregate that has produced the event
    /// </summary>
    string AggregateId { get; set; }

    /// <summary>
    /// Gets the date and time at which the event has been created
    /// </summary>
    DateTimeOffset CreatedAt { get; set; }
}
