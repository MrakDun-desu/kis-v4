namespace KisV4.Frontend.Configuration;

public class KisSettings {
    public required string AuthUrl { get; init; }
    public required string BackendUrl { get; init; }
    public required string ClientSecret { get; init; }
    public required string ClientId { get; init; }
    public required string PathBase { get; init; }
}
