using KisV4.Common.DependencyInjection;
using KisV4.DAL.EF.Entities;

public class SaleTransactionRequestState : IScopedService {
    public SaleTransactionItem[]? SaleTransactionItems { get; set; }
    public Dictionary<int, (Composite Item, decimal Price)>? Composites { get; set; }
}
