namespace Synapse.Demo.Common.Extensions;

/// <summary>
/// Extension methods for setting up the <see cref="JsonSerializerSettings"/>
/// </summary>
public static class JsonSerializerSettingsExtensions
{
    /// <summary>
    /// Configures a <see cref="JsonSerializerSettings"/>
    /// </summary>
    /// <param name="settings">The <see cref="JsonSerializerSettings"/> to configured</param>
    /// <returns>The configured <see cref="JsonSerializerSettings"/> so that additional calls can be chained.</returns>
    public static JsonSerializerSettings ConfigureSerializerSettings(this JsonSerializerSettings settings)
    {
        settings.ContractResolver = new NonPublicSetterContractResolver()
        {
            NamingStrategy = new CamelCaseNamingStrategy()
            {
                ProcessDictionaryKeys = false,
                OverrideSpecifiedNames = false,
                ProcessExtensionDataNames = false
            }
        };
        settings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
        settings.NullValueHandling = NullValueHandling.Ignore;
        settings.DefaultValueHandling = DefaultValueHandling.Ignore;
        settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
        settings.Converters.Add(new AbstractClassConverterFactory());
        return settings;
    }
}
