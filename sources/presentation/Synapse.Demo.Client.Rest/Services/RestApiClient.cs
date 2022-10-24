namespace Synapse.Demo.Client.Rest.Services;

/// <summary>
/// Represents the service used to interact with the HTTP REST API
/// </summary>
public class RestApiClient
    : IRestApiClient
{

    /// <summary>
    /// Gets the service used to perform logging
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Gets the <see cref="System.Net.Http.HttpClient"/> used to request the Synapse HTTP REST API
    /// </summary>
    protected HttpClient HttpClient { get; }

    /// <summary>
    /// Gets the service used to serialize/deserialize to/from JSON
    /// </summary>
    protected IJsonSerializer Serializer { get; }

    /// <summary>
    /// Initializes a new <see cref="SynapseHttpManagementApiClient"/>
    /// </summary>
    /// <param name="logger">The service used to perform logging</param>
    /// <param name="httpClientFactory">The service used to create <see cref="System.Net.Http.HttpClient"/>s</param>
    /// <param name="serializer">The service used to serialize/deserialize to/from JSON</param>
    public RestApiClient(ILogger<RestApiClient> logger, IHttpClientFactory httpClientFactory, IJsonSerializer serializer)
    {
        this.Logger = logger;
        this.HttpClient = httpClientFactory.CreateClient(this.GetType().Name);
        this.Serializer = serializer;
    }

    /// <summary>
    /// Queries the <see cref="Device"/>s
    /// </summary>
    /// <param name="query">The potential OData query</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A list of <see cref="Device"/>s</returns>
    public async Task<IEnumerable<Device>> GetDevices(string? query = null, CancellationToken cancellationToken = default)
    {
        var requestUri = "api/v1/devices";
        if (!string.IsNullOrWhiteSpace(query)) requestUri += $"?{query}";
        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        using var response = await this.HttpClient.SendAsync(request, cancellationToken);
        var json = await response.Content?.ReadAsStringAsync(cancellationToken)!;
        if (!response.IsSuccessStatusCode)
            this.Logger.LogError("An error occured while querying devices: {details}", json);
        response.EnsureSuccessStatusCode();
        return await this.Serializer.DeserializeAsync<List<Device>>(json, cancellationToken);
    }

    /// <summary>
    /// Queries the <see cref="Device"/>s
    /// </summary>
    /// <param name="query">The potential OData query</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A list of <see cref="Device"/>s</returns>
    public async Task<Device> GetDeviceById(string id, CancellationToken cancellationToken = default)
    {
        var requestUri = $"api/v1/devices/{id}";
        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        using var response = await this.HttpClient.SendAsync(request, cancellationToken);
        var json = await response.Content?.ReadAsStringAsync(cancellationToken)!;
        if (!response.IsSuccessStatusCode)
            this.Logger.LogError("An error occured while getting the device: {details}", json);
        response.EnsureSuccessStatusCode();
        return await this.Serializer.DeserializeAsync<Device>(json, cancellationToken);
    }

    /// <summary>
    /// Creates a new <see cref="Device"/>
    /// </summary>
    /// <param name="command">The <see cref="CreateDeviceCommand"/> used to create the device</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The created <see cref="Device"/></returns>
    public async Task<Device> CreateDevice(CreateDeviceCommand command, CancellationToken cancellationToken = default)
    {
        var requestUri = "api/v1/devices";
        var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
        var json = await this.Serializer.SerializeAsync(command, cancellationToken);
        request.Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
        using var response = await this.HttpClient.SendAsync(request, cancellationToken);
        json = await response.Content?.ReadAsStringAsync(cancellationToken)!;
        if (!response.IsSuccessStatusCode)
            this.Logger.LogError("An error occured while creating a new device: {details}", json);
        response.EnsureSuccessStatusCode();
        return await this.Serializer.DeserializeAsync<Device>(json, cancellationToken);
    }

    /// <summary>
    /// Updates a <see cref="Device"/> state
    /// </summary>
    /// <param name="command">The <see cref="UpdateDeviceStateCommand"/> used to update the device state</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The updated <see cref="Device"/></returns>
    public async Task<Device> UpdateDeviceState(UpdateDeviceStateCommand command, CancellationToken cancellationToken = default)
    {
        var requestUri = "api/v1/devices";
        var request = new HttpRequestMessage(HttpMethod.Put, requestUri);
        var json = await this.Serializer.SerializeAsync(command, cancellationToken);
        request.Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
        using var response = await this.HttpClient.SendAsync(request, cancellationToken);
        json = await response.Content?.ReadAsStringAsync(cancellationToken)!;
        if (!response.IsSuccessStatusCode)
            this.Logger.LogError("An error occured while updating a device state: {details}", json);
        response.EnsureSuccessStatusCode();
        return await this.Serializer.DeserializeAsync<Device>(json, cancellationToken);
    }

    /// <summary>
    /// Patches a <see cref="Device"/> state
    /// </summary>
    /// <param name="command">The <see cref="PatchDeviceStateCommand"/> used to patch the device state</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The patched <see cref="Device"/></returns>
    public async Task<Device> PatchDeviceState(PatchDeviceStateCommand command, CancellationToken cancellationToken = default)
    {
        var requestUri = "api/v1/devices";
        var request = new HttpRequestMessage(HttpMethod.Patch, requestUri);
        var json = await this.Serializer.SerializeAsync(command, cancellationToken);
        request.Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
        using var response = await this.HttpClient.SendAsync(request, cancellationToken);
        json = await response.Content?.ReadAsStringAsync(cancellationToken)!;
        if (!response.IsSuccessStatusCode)
            this.Logger.LogError("An error occured while patching a device state: {details}", json);
        response.EnsureSuccessStatusCode();
        return await this.Serializer.DeserializeAsync<Device>(json, cancellationToken);
    }
}
