using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyCart.Api.Migrations
{
    /// <inheritdoc />
    public partial class updateRoleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "Questions",
                newName: "IsActive");

            migrationBuilder.AddColumn<bool>(
                name: "ISDeleted",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ISDeleted",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "AspNetRoles");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Questions",
                newName: "isActive");
        }
    }
}
