using KisV4.Common.Enums;
using KisV4.DAL.EF.Entities;

namespace KisV4.DAL.EF.Seeding;

public class TestSeeder(KisDbContext dbContext) {
    private readonly KisDbContext _dbContext = dbContext;

    public void Seed() {
        var seedingUser = new User {
            Id = "Seeder",
            Nick = "Seeder",
            Accounts = [
                new UserAccount {
                    Type = AccountType.Prestige
                }
            ]
        };
        var categoryToasts = new Category { Name = "Tousty" };
        var categoryDrinks = new Category { Name = "Pití" };
        var storeItemBread = new StoreItem {
            Name = "Chleba",
            UnitName = "kg",
            Categories = [categoryToasts],
            Costs = [
                new Cost {
                    Amount = 40,
                    Description = "Seedovaná cena",
                    User = seedingUser,
                    Timestamp = DateTimeOffset.UtcNow
                }
            ],
        };
        var storeItemCheese = new StoreItem {
            Name = "Sýr",
            UnitName = "dkg",
            Categories = [categoryToasts],
            Costs = [
                new Cost {
                    Amount = 1.1m,
                    Description = "Seedovaná cena",
                    User = seedingUser,
                    Timestamp = DateTimeOffset.UtcNow
                }
            ]
        };
        var storeItemHam = new StoreItem {
            Name = "Šunka",
            UnitName = "dkg",
            Categories = [categoryToasts],
            Costs = [
                new Cost {
                    Amount = 1m,
                    Description = "Seedovaná cena",
                    User = seedingUser,
                    Timestamp = DateTimeOffset.UtcNow
                }
            ]
        };
        var storeItemBottleCola = new StoreItem {
            Name = "Kofola flaška 0.5l",
            UnitName = "ks",
            Categories = [categoryDrinks],
            Costs = [
                new Cost {
                    Amount = 15,
                    Description = "Seedovaná cena",
                    User = seedingUser,
                    Timestamp = DateTimeOffset.UtcNow
                }
            ]
        };
        var storeItemKegCola = new StoreItem {
            Name = "Kofola keg",
            UnitName = "l",
            IsContainerItem = true,
            Categories = [categoryDrinks],
            Costs = [
                new Cost {
                    Amount = 20,
                    Description = "Seedovaná cena",
                    User = seedingUser,
                    Timestamp = DateTimeOffset.UtcNow
                }
            ]
        };

        var saleItemToast = new SaleItem {
            Name = "Toust",
            Compositions = [
                new Composition {
                    StoreItem = storeItemBread,
                    Amount = 0.05m
                },
                new Composition {
                    StoreItem = storeItemCheese,
                    Amount = 1
                },
                new Composition {
                    StoreItem = storeItemHam,
                    Amount = 1
                },
            ],
            Categories = [categoryToasts],
            MarginPercent = 10,
        };
        var saleItemBottleCola = new SaleItem {
            Name = "Kofola flaška 0.5l",
            Compositions = [
                new Composition {
                    StoreItem = storeItemBottleCola,
                    Amount = 1
                }
            ]
        };
        var saleItemKegColaHalfLiter = new SaleItem {
            Name = "Kofola keg 0.5l",
            Compositions = [
                new Composition {
                    StoreItem = storeItemKegCola,
                    Amount = 0.5m
                }
            ]
        };

        var cashBox = new Cashbox {
            Name = "Kachna",
            Accounts = [
                new CashBoxAccount {
                    Type = AccountType.SalesMoney
                },
                new CashBoxAccount {
                    Type = AccountType.DonationMoney
                },
            ]
        };
        var store = new Store { Name = "Kachna" };
        var pipe = new Pipe { Name = "Kachna" };

        var container = new Container {
            Amount = 50,
            Store = store,
            Template = new ContainerTemplate {
                Amount = 50,
                Name = "Kofola 50l",
                StoreItem = storeItemKegCola
            }
        };

        _dbContext.Users.Add(seedingUser);

        _dbContext.Categories.Add(categoryToasts);
        _dbContext.Categories.Add(categoryDrinks);

        _dbContext.StoreItems.Add(storeItemBread);
        _dbContext.StoreItems.Add(storeItemCheese);
        _dbContext.StoreItems.Add(storeItemHam);
        _dbContext.StoreItems.Add(storeItemBottleCola);
        _dbContext.StoreItems.Add(storeItemKegCola);

        _dbContext.StoreItemAmounts.AddRange([
            new () {
                Amount = 0,
                StoreItem = storeItemBread,
                Store = store
            },
            new () {
                Amount = 0,
                StoreItem = storeItemCheese,
                Store = store
            },
            new () {
                Amount = 0,
                StoreItem = storeItemHam,
                Store = store
            },
            new () {
                Amount = 0,
                StoreItem = storeItemBottleCola,
                Store = store
            },
            new () {
                Amount = 0,
                StoreItem = storeItemKegCola,
                Store = store
            },
        ]);

        _dbContext.SaleItems.Add(saleItemToast);
        _dbContext.SaleItems.Add(saleItemBottleCola);
        _dbContext.SaleItems.Add(saleItemKegColaHalfLiter);

        _dbContext.CompositeAmounts.AddRange([
            new () {
                Amount = 0,
                Composite = saleItemToast,
                Store = store
            },
            new () {
                Amount = 0,
                Composite = saleItemBottleCola,
                Store = store
            },
            new () {
                Amount = 0,
                Composite = saleItemKegColaHalfLiter,
                Store = store
            },
        ]);

        _dbContext.Cashboxes.Add(cashBox);
        _dbContext.Stores.Add(store);
        _dbContext.Pipes.Add(pipe);
        _dbContext.Containers.Add(container);

        _dbContext.SaveChanges();
    }
}
