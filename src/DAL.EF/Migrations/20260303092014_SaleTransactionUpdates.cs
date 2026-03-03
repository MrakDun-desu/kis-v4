using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KisV4.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class SaleTransactionUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modifications_SaleTransactionItems_SaleTransactionItemId",
                table: "Modifications");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceChanges_SaleTransactionItems_SaleTransactionItemId",
                table: "PriceChanges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SaleTransactionItems",
                table: "SaleTransactionItems");

            migrationBuilder.DropIndex(
                name: "IX_PriceChanges_SaleTransactionItemId",
                table: "PriceChanges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Modifications",
                table: "Modifications");

            migrationBuilder.DropIndex(
                name: "IX_Modifications_SaleTransactionItemId",
                table: "Modifications");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "AccountTransactions");

            migrationBuilder.RenameColumn(
                name: "ItemAmount",
                table: "SaleTransactionItems",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SaleTransactionItems",
                newName: "LineNumber");

            migrationBuilder.RenameColumn(
                name: "SaleTransactionItemId",
                table: "Modifications",
                newName: "SaleTransactionId");

            migrationBuilder.AlterColumn<int>(
                name: "LineNumber",
                table: "SaleTransactionItems",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<decimal>(
                name: "BasePrice",
                table: "SaleTransactionItems",
                type: "numeric(11,2)",
                precision: 11,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SaleTransactionItemLineNumber",
                table: "PriceChanges",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SaleTransactionItemSaleTransactionId",
                table: "PriceChanges",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SaleTransactionItemLineNumber",
                table: "Modifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceChange",
                table: "Modifications",
                type: "numeric(11,2)",
                precision: 11,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SaleTransactionItems",
                table: "SaleTransactionItems",
                columns: new[] { "LineNumber", "SaleTransactionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Modifications",
                table: "Modifications",
                columns: new[] { "ModifierId", "SaleTransactionItemLineNumber", "SaleTransactionId" });

            migrationBuilder.CreateIndex(
                name: "IX_PriceChanges_SaleTransactionItemLineNumber_SaleTransactionI~",
                table: "PriceChanges",
                columns: new[] { "SaleTransactionItemLineNumber", "SaleTransactionItemSaleTransactionId" });

            migrationBuilder.CreateIndex(
                name: "IX_Modifications_SaleTransactionItemLineNumber_SaleTransaction~",
                table: "Modifications",
                columns: new[] { "SaleTransactionItemLineNumber", "SaleTransactionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Modifications_SaleTransactionItems_SaleTransactionItemLineN~",
                table: "Modifications",
                columns: new[] { "SaleTransactionItemLineNumber", "SaleTransactionId" },
                principalTable: "SaleTransactionItems",
                principalColumns: new[] { "LineNumber", "SaleTransactionId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceChanges_SaleTransactionItems_SaleTransactionItemLineNu~",
                table: "PriceChanges",
                columns: new[] { "SaleTransactionItemLineNumber", "SaleTransactionItemSaleTransactionId" },
                principalTable: "SaleTransactionItems",
                principalColumns: new[] { "LineNumber", "SaleTransactionId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modifications_SaleTransactionItems_SaleTransactionItemLineN~",
                table: "Modifications");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceChanges_SaleTransactionItems_SaleTransactionItemLineNu~",
                table: "PriceChanges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SaleTransactionItems",
                table: "SaleTransactionItems");

            migrationBuilder.DropIndex(
                name: "IX_PriceChanges_SaleTransactionItemLineNumber_SaleTransactionI~",
                table: "PriceChanges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Modifications",
                table: "Modifications");

            migrationBuilder.DropIndex(
                name: "IX_Modifications_SaleTransactionItemLineNumber_SaleTransaction~",
                table: "Modifications");

            migrationBuilder.DropColumn(
                name: "BasePrice",
                table: "SaleTransactionItems");

            migrationBuilder.DropColumn(
                name: "SaleTransactionItemLineNumber",
                table: "PriceChanges");

            migrationBuilder.DropColumn(
                name: "SaleTransactionItemSaleTransactionId",
                table: "PriceChanges");

            migrationBuilder.DropColumn(
                name: "SaleTransactionItemLineNumber",
                table: "Modifications");

            migrationBuilder.DropColumn(
                name: "PriceChange",
                table: "Modifications");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "SaleTransactionItems",
                newName: "ItemAmount");

            migrationBuilder.RenameColumn(
                name: "LineNumber",
                table: "SaleTransactionItems",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "SaleTransactionId",
                table: "Modifications",
                newName: "SaleTransactionItemId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SaleTransactionItems",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Timestamp",
                table: "AccountTransactions",
                type: "timestamp(0) with time zone",
                precision: 0,
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddPrimaryKey(
                name: "PK_SaleTransactionItems",
                table: "SaleTransactionItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Modifications",
                table: "Modifications",
                columns: new[] { "ModifierId", "SaleTransactionItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_PriceChanges_SaleTransactionItemId",
                table: "PriceChanges",
                column: "SaleTransactionItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Modifications_SaleTransactionItemId",
                table: "Modifications",
                column: "SaleTransactionItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modifications_SaleTransactionItems_SaleTransactionItemId",
                table: "Modifications",
                column: "SaleTransactionItemId",
                principalTable: "SaleTransactionItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceChanges_SaleTransactionItems_SaleTransactionItemId",
                table: "PriceChanges",
                column: "SaleTransactionItemId",
                principalTable: "SaleTransactionItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
