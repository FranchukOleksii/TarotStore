using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotStore.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceAtPurchaseField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PriceAtPurchase",
                table: "Order",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceAtPurchase",
                table: "Order");
        }
    }
}
