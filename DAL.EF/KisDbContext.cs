using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF;

public class KisDbContext(DbContextOptions<KisDbContext> options) : DbContext(options) {
    public DbSet<AccountEntity> Accounts { get; init; } = null!;
    public DbSet<CashBoxEntity> CashBoxes { get; init; } = null!;
    public DbSet<CompositionEntity> Compositions { get; init; } = null!;
    public DbSet<ContainerEntity> Containers { get; init; } = null!;
    public DbSet<CurrencyChangeEntity> CurrencyChanges { get; init; } = null!;
    public DbSet<CurrencyCostEntity> CurrencyCosts { get; init; } = null!;
    public DbSet<CurrencyEntity> Currencies { get; init; } = null!;
    public DbSet<DiscountEntity> Discounts { get; init; } = null!;
    public DbSet<DiscountUsageEntity> DiscountUsages { get; init; } = null!;
    public DbSet<DiscountUsageItemEntity> DiscountUsageItems { get; init; } = null!;
    public DbSet<IncompleteTransactionEntity> IncompleteTransactions { get; init; } = null!;
    public DbSet<ModifierEntity> Modifiers { get; init; } = null!;
    public DbSet<PipeEntity> Pipes { get; init; } = null!;
    public DbSet<ProductCategoryEntity> ProductCategories { get; init; } = null!;
    public DbSet<ProductEntity> Products { get; init; } = null!;
    public DbSet<SaleItemEntity> SaleItems { get; init; } = null!;
    public DbSet<SaleTransactionEntity> SaleTransactions { get; init; } = null!;
    public DbSet<SaleTransactionItemEntity> SaleTransactionItems { get; init; } = null!;
    public DbSet<StockTakingEntity> StockTakings { get; init; } = null!;
    public DbSet<StoreEntity> Stores { get; init; } = null!;
    public DbSet<StoreItemEntity> StoreItems { get; init; } = null!;
    public DbSet<StoreTransactionEntity> StoreTransactions { get; init; } = null!;
    public DbSet<StoreTransactionItemEntity> StoreTransactionItems { get; init; } = null!;
    public DbSet<TransactionEntity> Transactions { get; init; } = null!;
    public DbSet<TransactionPriceEntity> TransactionPrices { get; init; } = null!;
    public DbSet<UserAccountEntity> UserAccounts { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        // for better naming of the join tables
        modelBuilder.Entity<ProductEntity>()
            .HasMany(e => e.Categories)
            .WithMany(e => e.Products)
            .UsingEntity("ProductInCategory");

        modelBuilder.Entity<ModifierEntity>()
            .HasMany(e => e.Applications)
            .WithMany(e => e.Modifiers)
            .UsingEntity("ModifierApplications");

        modelBuilder.Entity<UserAccountEntity>()
            .HasMany(e => e.Transactions)
            .WithOne(e => e.ResponsibleUser);

        // using table-per-type instead of table-per-hierarchy for saving space
        modelBuilder.Entity<ProductEntity>().ToTable("Products");
        modelBuilder.Entity<SaleItemEntity>().ToTable("SaleItems");
        modelBuilder.Entity<StoreItemEntity>().ToTable("StoreItems");
        modelBuilder.Entity<ModifierEntity>().ToTable("Modifiers");

        modelBuilder.Entity<AccountEntity>().ToTable("Accounts");
        modelBuilder.Entity<UserAccountEntity>().ToTable("UserAccounts");
        modelBuilder.Entity<CashBoxEntity>().ToTable("CashBoxes");

        modelBuilder.Entity<TransactionEntity>().ToTable("Transactions");
        modelBuilder.Entity<StoreTransactionEntity>().ToTable("StoreTransactions");
        modelBuilder.Entity<SaleTransactionEntity>().ToTable("SaleTransactions");

        modelBuilder.Entity<StoreEntity>().ToTable("Stores");
        modelBuilder.Entity<ContainerEntity>().ToTable("Containers");
    }
}
