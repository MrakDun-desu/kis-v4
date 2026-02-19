using Audit.EntityFramework;
using KisV4.Common.Enums;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace KisV4.DAL.EF;

[AuditDbContext(Mode = AuditOptionMode.OptOut)]
public class KisDbContext(DbContextOptions<KisDbContext> options) : AuditDbContext(options) {
    public DbSet<Account> Accounts { get; init; } = null!;
    public DbSet<AccountTransaction> AccountTransactions { get; init; } = null!;
    public DbSet<Cashbox> Cashboxes { get; init; } = null!;
    public DbSet<Category> Categories { get; init; } = null!;
    public DbSet<Composite> Composites { get; init; } = null!;
    public DbSet<CompositeAmount> CompositeAmounts { get; init; } = null!;
    public DbSet<Composition> Compositions { get; init; } = null!;
    public DbSet<Container> Containers { get; init; } = null!;
    public DbSet<ContainerChange> ContainerChanges { get; init; } = null!;
    public DbSet<ContainerTemplate> ContainerTemplates { get; init; } = null!;
    public DbSet<Cost> Costs { get; init; } = null!;
    public DbSet<Discount> Discounts { get; init; } = null!;
    public DbSet<DiscountUsage> DiscountUsages { get; init; } = null!;
    public DbSet<Layout> Layouts { get; init; } = null!;
    public DbSet<LayoutItem> LayoutItems { get; init; } = null!;
    public DbSet<LayoutLink> LayoutLinks { get; init; } = null!;
    public DbSet<LayoutPipe> LayoutPipes { get; init; } = null!;
    public DbSet<LayoutSaleItem> LayoutSaleItems { get; init; } = null!;
    public DbSet<Modification> Modifications { get; init; } = null!;
    public DbSet<Modifier> Modifiers { get; init; } = null!;
    public DbSet<Pipe> Pipes { get; init; } = null!;
    public DbSet<PriceChange> PriceChanges { get; init; } = null!;
    public DbSet<SaleItem> SaleItems { get; init; } = null!;
    public DbSet<SaleTransaction> SaleTransactions { get; init; } = null!;
    public DbSet<SaleTransactionItem> SaleTransactionItems { get; init; } = null!;
    public DbSet<Store> Stores { get; init; } = null!;
    public DbSet<StoreItem> StoreItems { get; init; } = null!;
    public DbSet<StoreItemAmount> StoreItemAmounts { get; init; } = null!;
    public DbSet<StoreTransaction> StoreTransactions { get; init; } = null!;
    public DbSet<StoreTransactionItem> StoreTransactionItems { get; init; } = null!;
    public DbSet<Transaction> Transactions { get; init; } = null!;
    public DbSet<User> Users { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AccountTransaction>().HasQueryFilter(e => !e.Cancelled);
        modelBuilder.Entity<Cashbox>().HasQueryFilter(e => !e.Deleted);
        modelBuilder.Entity<Composite>().HasQueryFilter(e => !e.Hidden);
        modelBuilder.Entity<ContainerTemplate>().HasQueryFilter(e => !e.Deleted);
        modelBuilder.Entity<Discount>().HasQueryFilter(e => !e.Deleted);
        modelBuilder.Entity<SaleTransactionItem>().HasQueryFilter(e => !e.Cancelled);
        modelBuilder.Entity<Store>().HasQueryFilter(e => !e.Deleted);
        modelBuilder.Entity<StoreItem>().HasQueryFilter(e => !e.Hidden);
        modelBuilder.Entity<StoreTransactionItem>().HasQueryFilter(e => !e.Cancelled);
        modelBuilder.Entity<Transaction>().HasQueryFilter(e => e.CancelledAt == null);

        modelBuilder.Entity<Category>()
            .HasMany(c => c.Composites)
            .WithMany(c => c.Categories)
            .UsingEntity("CompositeInCategory");

        modelBuilder.Entity<Category>()
            .HasMany(c => c.StoreItems)
            .WithMany(c => c.Categories)
            .UsingEntity("StoreItemInCategory");

        modelBuilder.Entity<Modifier>()
            .HasMany(e => e.Targets)
            .WithMany(e => e.ApplicableModifiers)
            .UsingEntity("ApplicableModifiers");

        modelBuilder.Entity<Transaction>()
            .HasOne(e => e.CancelledBy)
            .WithMany(e => e.CancelledTransactions);

        modelBuilder.Entity<Transaction>()
            .HasOne(e => e.StartedBy)
            .WithMany(e => e.StartedTransactions);

        modelBuilder.Entity<LayoutItem>()
            .HasDiscriminator(e => e.Type)
            .HasValue<LayoutSaleItem>(LayoutItemType.SaleItem)
            .HasValue<LayoutLink>(LayoutItemType.Layout)
            .HasValue<LayoutPipe>(LayoutItemType.Pipe);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) {
        // 11 digits with 2 decimal places
        configurationBuilder.Properties<decimal>().HavePrecision(11, 2);
        // discard seconds for timestamps
        configurationBuilder.Properties<DateTimeOffset>().HavePrecision(0);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        // remove warnings that say that required navigations with global filters will cause
        // unexpected results (we don't want to see the results of global filters even as
        // navigations 99% of the time)
        optionsBuilder.ConfigureWarnings(w => {
            w.Ignore(CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning);
        });
    }
}
