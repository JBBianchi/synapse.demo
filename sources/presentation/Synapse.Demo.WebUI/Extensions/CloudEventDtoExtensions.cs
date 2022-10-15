namespace Synapse.Demo.WebUI.Extensions;

/// <summary>
/// Extension methods for <see cref="CloudEventDto"/>s
/// </summary>
public static class CloudEventDtoExtensions
{
    /// <summary>
    /// Converts a <see cref="CloudEventDto"/> data payload to the required type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cloudEvent"></param>
    /// <returns></returns>
    public static T As<T>(this CloudEventDto cloudEvent)
    {
        if (cloudEvent.Data == null) return default(T);
        return (T)(cloudEvent.Data as JObject)!.ToObject(typeof(T))!;
    }
}
