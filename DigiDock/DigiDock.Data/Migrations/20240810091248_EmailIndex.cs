using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigiDock.Data.Migrations
{
    /// <inheritdoc />
    public partial class EmailIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IpAdress",
                schema: "dbo",
                table: "UserLogins",
                newName: "IpAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "dbo",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                schema: "dbo",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "IpAddress",
                schema: "dbo",
                table: "UserLogins",
                newName: "IpAdress");
        }
    }
}
