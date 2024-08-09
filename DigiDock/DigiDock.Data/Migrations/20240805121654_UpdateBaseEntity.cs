using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigiDock.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateUser",
                schema: "dbo",
                table: "Products",
                newName: "UpdateUserId");

            migrationBuilder.RenameColumn(
                name: "DeleteUser",
                schema: "dbo",
                table: "Products",
                newName: "DeleteUserId");

            migrationBuilder.RenameColumn(
                name: "CreateUser",
                schema: "dbo",
                table: "Products",
                newName: "CreateUserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                schema: "dbo",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "dbo",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "dbo",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "dbo",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateUserId",
                schema: "dbo",
                table: "Products",
                newName: "UpdateUser");

            migrationBuilder.RenameColumn(
                name: "DeleteUserId",
                schema: "dbo",
                table: "Products",
                newName: "DeleteUser");

            migrationBuilder.RenameColumn(
                name: "CreateUserId",
                schema: "dbo",
                table: "Products",
                newName: "CreateUser");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                schema: "dbo",
                table: "Products",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "dbo",
                table: "Products",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "dbo",
                table: "Products",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "dbo",
                table: "Products",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");
        }
    }
}
