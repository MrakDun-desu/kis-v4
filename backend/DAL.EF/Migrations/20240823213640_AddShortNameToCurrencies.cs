using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KisV4.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddShortNameToCurrencies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "Currencies",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortName",
                table: "Currencies");
        }
    }
}
