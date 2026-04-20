using Duende.IdentityModel.Client;

namespace KisV4.BL.EF.Services;

public class KisFoodService {
    private readonly IHttpClientFactory _httpClientFactory;

    public KisFoodService(IHttpClientFactory httpClientFactory) {
        _httpClientFactory = httpClientFactory;

    }

}
