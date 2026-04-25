using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using KisV4.Common.Authorization;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Services;

public class KisFoodService : IScopedService {
    private readonly HttpClient _httpClient;

    public KisFoodService(IHttpClientFactory httpClientFactory) {
        _httpClient = httpClientFactory.CreateClient(AuthorizationConstants.KisFoodHttpClientName);
    }

    public async Task<KisFoodQueueItemDetails[]?> CreateFoodOrder(
        KisFoodOrderRequest req,
        CancellationToken token = default
    ) {
        var kisFoodResponse = await _httpClient.PostAsJsonAsync("order", req, token);

        if (!kisFoodResponse.IsSuccessStatusCode) {
            return null;
        }

        var jsonOpts = new JsonSerializerOptions(JsonSerializerOptions.Web);
        jsonOpts.Converters.Add(new JsonStringEnumConverter());

        return await kisFoodResponse
            .Content
            .ReadFromJsonAsync<KisFoodQueueItemDetails[]>(jsonOpts);
    }
}
