using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigiDock.Data.Migrations
{
    /// <inheritdoc />
    public partial class OrderUpdateUnitPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                schema: "dbo",
                table: "OrderDetails",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitPrice",
                schema: "dbo",
                table: "OrderDetails");
        }
    }
}
