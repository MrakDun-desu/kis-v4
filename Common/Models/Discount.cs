namespace KisV4.Common.Models;

public record DiscountReadAllModel(
    int Id,
    string Name
);

public record DiscountReadModel(
    int Id,
    string Name,
    IEnumerable<DiscountUsageReadAllModel> DiscountUsages
);
