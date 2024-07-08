using Api.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.DAL.EF;

public class KisDbContext : DbContext {

    public DbSet<AccountEntity> Accounts { get; set; }
    public DbSet<CashboxEntity> Cashboxes { get; set; }
    public DbSet<CompositionEntity> Compositions { get; set; }
    public DbSet<ContainerEntity> Containers { get; set; }
    public DbSet<CurrencyCostEntity> CurrencyCosts { get; set; }
    public DbSet<CurrencyChangeEntity> CurrencyChanges { get; set; }
    public DbSet<DiscountEntity> Discounts { get; set; }
    public DbSet<DiscountUsageEntity> DiscountUsages { get; set; }
    public DbSet<DiscountUsageItemEntity> DiscountUsageItems { get; set; }
    public DbSet<IncompleteTransactionEntity> IncompleteTransactions { get; set; }
    public DbSet<ModifierEntity> Modifiers { get; set; }
    public DbSet<PipeEntity> Pipes { get; set; }
    public DbSet<ProductCategoryEntity> ProductCategories { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<SaleItemEntity> SaleItems { get; set; }
    public DbSet<SaleTransactionEntity> SaleTransactions { get; set; }
    public DbSet<SaleTransactionItemEntity> SaleTransactionItems { get; set; }
    public DbSet<StockTakingEntity> StockTakings { get; set; }
    public DbSet<StoreEntity> Stores { get; set; }
    public DbSet<StoreItemEntity> StoreItems { get; set; }
    public DbSet<StoreTransactionEntity> StoreTransactions { get; set; }
    public DbSet<StoreTransactionItemEntity> StoreTransactionItems { get; set; }
    public DbSet<TransactionEntity> Transactions { get; set; }
    public DbSet<TransactionPriceEntity> TransactionPrices { get; set; }
    public DbSet<UserAccountEntity> UserAccounts { get; set; }

    public KisDbContext(DbContextOptions<KisDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
    }
}
