using System.Net.Http.Json;
using System.Text.Json;
using KisV4.Common.Authorization;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Services;

public class KisFoodService : IScopedService {
    private readonly HttpClient _httpClient;

    public KisFoodService(IHttpClientFactory httpClientFactory) {
        _httpClient = httpClientFactory.CreateClient(AuthorizationConstants.KisFoodHttpClientName);
    }

    public async Task<JsonDocument?> CreateFoodOrder(KisFoodOrderRequest req) {
        var kisFoodResponse = await _httpClient.PostAsJsonAsync("https://su-dev.fit.vutbr.cz/food/order", req);

        if (!kisFoodResponse.IsSuccessStatusCode) {
            return null;
        }

        return await kisFoodResponse.Content.ReadFromJsonAsync<JsonDocument>();
    }
}
