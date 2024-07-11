using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Api.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pipes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cashboxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cashboxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cashboxes_Accounts_Id",
                        column: x => x.Id,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAccounts_Accounts_Id",
                        column: x => x.Id,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyCosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    CurrencyId = table.Column<int>(type: "integer", nullable: false),
                    ValidSince = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(11,2)", precision: 11, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyCosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrencyCosts_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurrencyCosts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductInCategory",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "integer", nullable: false),
                    ProductsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInCategory", x => new { x.CategoriesId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_ProductInCategory_ProductCategories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductInCategory_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaleItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    ShowOnWeb = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleItems_Products_Id",
                        column: x => x.Id,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    UnitName = table.Column<string>(type: "text", nullable: false),
                    BarmanCanStock = table.Column<bool>(type: "boolean", nullable: false),
                    IsContainerItem = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreItems_Products_Id",
                        column: x => x.Id,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockTakings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CashboxId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTakings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTakings_Cashboxes_CashboxId",
                        column: x => x.CashboxId,
                        principalTable: "Cashboxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscountUsages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    DiscountId = table.Column<int>(type: "integer", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountUsages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountUsages_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscountUsages_UserAccounts_UserId",
                        column: x => x.UserId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResponsibleUserId = table.Column<int>(type: "integer", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Cancelled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_UserAccounts_ResponsibleUserId",
                        column: x => x.ResponsibleUserId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Modifiers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    ModificationTargetId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modifiers_SaleItems_Id",
                        column: x => x.Id,
                        principalTable: "SaleItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Modifiers_SaleItems_ModificationTargetId",
                        column: x => x.ModificationTargetId,
                        principalTable: "SaleItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Compositions",
                columns: table => new
                {
                    SaleItemId = table.Column<int>(type: "integer", nullable: false),
                    StoreItemId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(11,2)", precision: 11, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compositions", x => new { x.SaleItemId, x.StoreItemId });
                    table.ForeignKey(
                        name: "FK_Compositions_SaleItems_SaleItemId",
                        column: x => x.SaleItemId,
                        principalTable: "SaleItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Compositions_StoreItems_StoreItemId",
                        column: x => x.StoreItemId,
                        principalTable: "StoreItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Containers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    ContainedItemId = table.Column<int>(type: "integer", nullable: false),
                    OpenSince = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PipeId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Containers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Containers_Pipes_PipeId",
                        column: x => x.PipeId,
                        principalTable: "Pipes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Containers_StoreItems_ContainedItemId",
                        column: x => x.ContainedItemId,
                        principalTable: "StoreItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Containers_Stores_Id",
                        column: x => x.Id,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaleTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleTransactions_Transactions_Id",
                        column: x => x.Id,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyChanges",
                columns: table => new
                {
                    CurrencyId = table.Column<int>(type: "integer", nullable: false),
                    SaleTransactionId = table.Column<int>(type: "integer", nullable: false),
                    AccountId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(11,2)", precision: 11, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyChanges", x => new { x.CurrencyId, x.SaleTransactionId, x.AccountId });
                    table.ForeignKey(
                        name: "FK_CurrencyChanges_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurrencyChanges_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurrencyChanges_SaleTransactions_SaleTransactionId",
                        column: x => x.SaleTransactionId,
                        principalTable: "SaleTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IncompleteTransactions",
                columns: table => new
                {
                    SaleTransactionId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncompleteTransactions", x => new { x.SaleTransactionId, x.UserId });
                    table.ForeignKey(
                        name: "FK_IncompleteTransactions_SaleTransactions_SaleTransactionId",
                        column: x => x.SaleTransactionId,
                        principalTable: "SaleTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncompleteTransactions_UserAccounts_UserId",
                        column: x => x.UserId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaleTransactionItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SaleItemId = table.Column<int>(type: "integer", nullable: false),
                    SaleTransactionId = table.Column<int>(type: "integer", nullable: false),
                    ItemAmount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleTransactionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleTransactionItems_SaleItems_SaleItemId",
                        column: x => x.SaleItemId,
                        principalTable: "SaleItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleTransactionItems_SaleTransactions_SaleTransactionId",
                        column: x => x.SaleTransactionId,
                        principalTable: "SaleTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    TransactionReason = table.Column<int>(type: "integer", nullable: false),
                    SaleTransactionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreTransactions_SaleTransactions_SaleTransactionId",
                        column: x => x.SaleTransactionId,
                        principalTable: "SaleTransactions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreTransactions_Transactions_Id",
                        column: x => x.Id,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscountUsageItems",
                columns: table => new
                {
                    DiscountUsageId = table.Column<int>(type: "integer", nullable: false),
                    CurrencyId = table.Column<int>(type: "integer", nullable: false),
                    SaleTransactionItemId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(11,2)", precision: 11, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountUsageItems", x => new { x.DiscountUsageId, x.CurrencyId, x.SaleTransactionItemId });
                    table.ForeignKey(
                        name: "FK_DiscountUsageItems_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscountUsageItems_DiscountUsages_DiscountUsageId",
                        column: x => x.DiscountUsageId,
                        principalTable: "DiscountUsages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscountUsageItems_SaleTransactionItems_SaleTransactionItem~",
                        column: x => x.SaleTransactionItemId,
                        principalTable: "SaleTransactionItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModifierApplications",
                columns: table => new
                {
                    ApplicationsId = table.Column<int>(type: "integer", nullable: false),
                    ModifiersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModifierApplications", x => new { x.ApplicationsId, x.ModifiersId });
                    table.ForeignKey(
                        name: "FK_ModifierApplications_Modifiers_ModifiersId",
                        column: x => x.ModifiersId,
                        principalTable: "Modifiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModifierApplications_SaleTransactionItems_ApplicationsId",
                        column: x => x.ApplicationsId,
                        principalTable: "SaleTransactionItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionPrices",
                columns: table => new
                {
                    CurrencyId = table.Column<int>(type: "integer", nullable: false),
                    SaleTransactionItemId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(11,2)", precision: 11, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionPrices", x => new { x.SaleTransactionItemId, x.CurrencyId });
                    table.ForeignKey(
                        name: "FK_TransactionPrices_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionPrices_SaleTransactionItems_SaleTransactionItemId",
                        column: x => x.SaleTransactionItemId,
                        principalTable: "SaleTransactionItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreTransactionItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StoreItemId = table.Column<int>(type: "integer", nullable: false),
                    StoreId = table.Column<int>(type: "integer", nullable: false),
                    StoreTransactionId = table.Column<int>(type: "integer", nullable: false),
                    ItemAmount = table.Column<decimal>(type: "numeric(11,2)", precision: 11, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreTransactionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreTransactionItems_StoreItems_StoreItemId",
                        column: x => x.StoreItemId,
                        principalTable: "StoreItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreTransactionItems_StoreTransactions_StoreTransactionId",
                        column: x => x.StoreTransactionId,
                        principalTable: "StoreTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreTransactionItems_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Compositions_StoreItemId",
                table: "Compositions",
                column: "StoreItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Containers_ContainedItemId",
                table: "Containers",
                column: "ContainedItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Containers_PipeId",
                table: "Containers",
                column: "PipeId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyChanges_AccountId",
                table: "CurrencyChanges",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyChanges_SaleTransactionId",
                table: "CurrencyChanges",
                column: "SaleTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyCosts_CurrencyId",
                table: "CurrencyCosts",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyCosts_ProductId",
                table: "CurrencyCosts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountUsageItems_CurrencyId",
                table: "DiscountUsageItems",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountUsageItems_SaleTransactionItemId",
                table: "DiscountUsageItems",
                column: "SaleTransactionItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountUsages_DiscountId",
                table: "DiscountUsages",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountUsages_UserId",
                table: "DiscountUsages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IncompleteTransactions_UserId",
                table: "IncompleteTransactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModifierApplications_ModifiersId",
                table: "ModifierApplications",
                column: "ModifiersId");

            migrationBuilder.CreateIndex(
                name: "IX_Modifiers_ModificationTargetId",
                table: "Modifiers",
                column: "ModificationTargetId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInCategory_ProductsId",
                table: "ProductInCategory",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleTransactionItems_SaleItemId",
                table: "SaleTransactionItems",
                column: "SaleItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleTransactionItems_SaleTransactionId",
                table: "SaleTransactionItems",
                column: "SaleTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTakings_CashboxId",
                table: "StockTakings",
                column: "CashboxId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTransactionItems_StoreId",
                table: "StoreTransactionItems",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTransactionItems_StoreItemId",
                table: "StoreTransactionItems",
                column: "StoreItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTransactionItems_StoreTransactionId",
                table: "StoreTransactionItems",
                column: "StoreTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTransactions_SaleTransactionId",
                table: "StoreTransactions",
                column: "SaleTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionPrices_CurrencyId",
                table: "TransactionPrices",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ResponsibleUserId",
                table: "Transactions",
                column: "ResponsibleUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Compositions");

            migrationBuilder.DropTable(
                name: "Containers");

            migrationBuilder.DropTable(
                name: "CurrencyChanges");

            migrationBuilder.DropTable(
                name: "CurrencyCosts");

            migrationBuilder.DropTable(
                name: "DiscountUsageItems");

            migrationBuilder.DropTable(
                name: "IncompleteTransactions");

            migrationBuilder.DropTable(
                name: "ModifierApplications");

            migrationBuilder.DropTable(
                name: "ProductInCategory");

            migrationBuilder.DropTable(
                name: "StockTakings");

            migrationBuilder.DropTable(
                name: "StoreTransactionItems");

            migrationBuilder.DropTable(
                name: "TransactionPrices");

            migrationBuilder.DropTable(
                name: "Pipes");

            migrationBuilder.DropTable(
                name: "DiscountUsages");

            migrationBuilder.DropTable(
                name: "Modifiers");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "Cashboxes");

            migrationBuilder.DropTable(
                name: "StoreItems");

            migrationBuilder.DropTable(
                name: "StoreTransactions");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "SaleTransactionItems");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "SaleItems");

            migrationBuilder.DropTable(
                name: "SaleTransactions");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
