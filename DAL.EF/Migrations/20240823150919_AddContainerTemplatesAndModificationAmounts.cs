using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KisV4.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddContainerTemplatesAndModificationAmounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Containers_StoreItems_ContainedItemId",
                table: "Containers");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTakings_CashBoxes_CashboxId",
                table: "StockTakings");

            migrationBuilder.DropTable(
                name: "ModifierApplications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StockTakings",
                table: "StockTakings");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "StockTakings");

            migrationBuilder.RenameColumn(
                name: "CashboxId",
                table: "StockTakings",
                newName: "CashBoxId");

            migrationBuilder.RenameIndex(
                name: "IX_StockTakings_CashboxId",
                table: "StockTakings",
                newName: "IX_StockTakings_CashBoxId");

            migrationBuilder.RenameColumn(
                name: "ContainedItemId",
                table: "Containers",
                newName: "TemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_Containers_ContainedItemId",
                table: "Containers",
                newName: "IX_Containers_TemplateId");

            migrationBuilder.AddColumn<bool>(
                name: "Cancelled",
                table: "TransactionPrices",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Cancelled",
                table: "StoreTransactionItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Cancelled",
                table: "SaleTransactionItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Cancelled",
                table: "CurrencyChanges",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockTakings",
                table: "StockTakings",
                columns: new[] { "Timestamp", "CashBoxId" });

            migrationBuilder.CreateTable(
                name: "ContainerTemplateEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(11,2)", precision: 11, scale: 2, nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    ContainedItemId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContainerTemplateEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContainerTemplateEntity_StoreItems_ContainedItemId",
                        column: x => x.ContainedItemId,
                        principalTable: "StoreItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModifierAmountEntity",
                columns: table => new
                {
                    ModifierId = table.Column<int>(type: "integer", nullable: false),
                    SaleTransactionItemId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModifierAmountEntity", x => new { x.ModifierId, x.SaleTransactionItemId });
                    table.ForeignKey(
                        name: "FK_ModifierAmountEntity_Modifiers_ModifierId",
                        column: x => x.ModifierId,
                        principalTable: "Modifiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModifierAmountEntity_SaleTransactionItems_SaleTransactionIt~",
                        column: x => x.SaleTransactionItemId,
                        principalTable: "SaleTransactionItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContainerTemplateEntity_ContainedItemId",
                table: "ContainerTemplateEntity",
                column: "ContainedItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ModifierAmountEntity_SaleTransactionItemId",
                table: "ModifierAmountEntity",
                column: "SaleTransactionItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Containers_ContainerTemplateEntity_TemplateId",
                table: "Containers",
                column: "TemplateId",
                principalTable: "ContainerTemplateEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTakings_CashBoxes_CashBoxId",
                table: "StockTakings",
                column: "CashBoxId",
                principalTable: "CashBoxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Containers_ContainerTemplateEntity_TemplateId",
                table: "Containers");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTakings_CashBoxes_CashBoxId",
                table: "StockTakings");

            migrationBuilder.DropTable(
                name: "ContainerTemplateEntity");

            migrationBuilder.DropTable(
                name: "ModifierAmountEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StockTakings",
                table: "StockTakings");

            migrationBuilder.DropColumn(
                name: "Cancelled",
                table: "TransactionPrices");

            migrationBuilder.DropColumn(
                name: "Cancelled",
                table: "StoreTransactionItems");

            migrationBuilder.DropColumn(
                name: "Cancelled",
                table: "SaleTransactionItems");

            migrationBuilder.DropColumn(
                name: "Cancelled",
                table: "CurrencyChanges");

            migrationBuilder.RenameColumn(
                name: "CashBoxId",
                table: "StockTakings",
                newName: "CashboxId");

            migrationBuilder.RenameIndex(
                name: "IX_StockTakings_CashBoxId",
                table: "StockTakings",
                newName: "IX_StockTakings_CashboxId");

            migrationBuilder.RenameColumn(
                name: "TemplateId",
                table: "Containers",
                newName: "ContainedItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Containers_TemplateId",
                table: "Containers",
                newName: "IX_Containers_ContainedItemId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "StockTakings",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockTakings",
                table: "StockTakings",
                column: "Id");

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

            migrationBuilder.CreateIndex(
                name: "IX_ModifierApplications_ModifiersId",
                table: "ModifierApplications",
                column: "ModifiersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Containers_StoreItems_ContainedItemId",
                table: "Containers",
                column: "ContainedItemId",
                principalTable: "StoreItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTakings_CashBoxes_CashboxId",
                table: "StockTakings",
                column: "CashboxId",
                principalTable: "CashBoxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
