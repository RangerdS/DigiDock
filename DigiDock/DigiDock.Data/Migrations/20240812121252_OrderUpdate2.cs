using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigiDock.Data.Migrations
{
    /// <inheritdoc />
    public partial class OrderUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                schema: "dbo",
                table: "Orders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                schema: "dbo",
                table: "Orders",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserId",
                schema: "dbo",
                table: "Orders");
        }
    }
}
