using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KisV4.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddNickAndGamificationToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "GamificationAllowed",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Nick",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ItemAmount",
                table: "StoreTransactionItems",
                type: "numeric(13,4)",
                precision: 13,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(11,2)",
                oldPrecision: 11,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "StoreTransactionItems",
                type: "numeric(13,4)",
                precision: 13,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(11,2)",
                oldPrecision: 11,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentCost",
                table: "StoreItems",
                type: "numeric(13,4)",
                precision: 13,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(11,2)",
                oldPrecision: 11,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "StoreItemAmounts",
                type: "numeric(13,4)",
                precision: 13,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(11,2)",
                oldPrecision: 11,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "BasePrice",
                table: "SaleTransactionItems",
                type: "numeric(13,4)",
                precision: 13,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(11,2)",
                oldPrecision: 11,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "PriceChanges",
                type: "numeric(13,4)",
                precision: 13,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(11,2)",
                oldPrecision: 11,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "PriceChange",
                table: "Modifications",
                type: "numeric(13,4)",
                precision: 13,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(11,2)",
                oldPrecision: 11,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Costs",
                type: "numeric(13,4)",
                precision: 13,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(11,2)",
                oldPrecision: 11,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "ContainerTemplates",
                type: "numeric(13,4)",
                precision: 13,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(11,2)",
                oldPrecision: 11,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Containers",
                type: "numeric(13,4)",
                precision: 13,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(11,2)",
                oldPrecision: 11,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "NewAmount",
                table: "ContainerChanges",
                type: "numeric(13,4)",
                precision: 13,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(11,2)",
                oldPrecision: 11,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Compositions",
                type: "numeric(13,4)",
                precision: 13,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(11,2)",
                oldPrecision: 11,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "PrestigeAmount",
                table: "Composites",
                type: "numeric(13,4)",
                precision: 13,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(11,2)",
                oldPrecision: 11,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "MarginStatic",
                table: "Composites",
                type: "numeric(13,4)",
                precision: 13,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(11,2)",
                oldPrecision: 11,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "MarginPercent",
                table: "Composites",
                type: "numeric(13,4)",
                precision: 13,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(11,2)",
                oldPrecision: 11,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "AccountTransactions",
                type: "numeric(13,4)",
                precision: 13,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(11,2)",
                oldPrecision: 11,
                oldScale: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GamificationAllowed",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Nick",
                table: "Users");

            migrationBuilder.AlterColumn<decimal>(
                name: "ItemAmount",
                table: "StoreTransactionItems",
                type: "numeric(11,2)",
                precision: 11,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(13,4)",
                oldPrecision: 13,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "StoreTransactionItems",
                type: "numeric(11,2)",
                precision: 11,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(13,4)",
                oldPrecision: 13,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentCost",
                table: "StoreItems",
                type: "numeric(11,2)",
                precision: 11,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(13,4)",
                oldPrecision: 13,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "StoreItemAmounts",
                type: "numeric(11,2)",
                precision: 11,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(13,4)",
                oldPrecision: 13,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "BasePrice",
                table: "SaleTransactionItems",
                type: "numeric(11,2)",
                precision: 11,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(13,4)",
                oldPrecision: 13,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "PriceChanges",
                type: "numeric(11,2)",
                precision: 11,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(13,4)",
                oldPrecision: 13,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "PriceChange",
                table: "Modifications",
                type: "numeric(11,2)",
                precision: 11,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(13,4)",
                oldPrecision: 13,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Costs",
                type: "numeric(11,2)",
                precision: 11,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(13,4)",
                oldPrecision: 13,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "ContainerTemplates",
                type: "numeric(11,2)",
                precision: 11,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(13,4)",
                oldPrecision: 13,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Containers",
                type: "numeric(11,2)",
                precision: 11,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(13,4)",
                oldPrecision: 13,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "NewAmount",
                table: "ContainerChanges",
                type: "numeric(11,2)",
                precision: 11,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(13,4)",
                oldPrecision: 13,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Compositions",
                type: "numeric(11,2)",
                precision: 11,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(13,4)",
                oldPrecision: 13,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "PrestigeAmount",
                table: "Composites",
                type: "numeric(11,2)",
                precision: 11,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(13,4)",
                oldPrecision: 13,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "MarginStatic",
                table: "Composites",
                type: "numeric(11,2)",
                precision: 11,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(13,4)",
                oldPrecision: 13,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "MarginPercent",
                table: "Composites",
                type: "numeric(11,2)",
                precision: 11,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(13,4)",
                oldPrecision: 13,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "AccountTransactions",
                type: "numeric(11,2)",
                precision: 11,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(13,4)",
                oldPrecision: 13,
                oldScale: 4);
        }
    }
}
