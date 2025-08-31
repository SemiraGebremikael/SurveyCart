using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurveyCart.Api.Migrations
{
    /// <inheritdoc />
    public partial class seedIdentityTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "Permissions", "polls:read", "d241e2a9-e784-4281-9e80-96c2c67aa6f9" },
                    { 2, "Permissions", "polls:add", "d241e2a9-e784-4281-9e80-96c2c67aa6f9" },
                    { 3, "Permissions", "polls:create", "d241e2a9-e784-4281-9e80-96c2c67aa6f9" },
                    { 4, "Permissions", "polls:update", "d241e2a9-e784-4281-9e80-96c2c67aa6f9" },
                    { 5, "Permissions", "polls:delete", "d241e2a9-e784-4281-9e80-96c2c67aa6f9" },
                    { 6, "Permissions", "question:read", "d241e2a9-e784-4281-9e80-96c2c67aa6f9" },
                    { 7, "Permissions", "question:add", "d241e2a9-e784-4281-9e80-96c2c67aa6f9" },
                    { 8, "Permissions", "question:update", "d241e2a9-e784-4281-9e80-96c2c67aa6f9" },
                    { 9, "Permissions", "user:read", "d241e2a9-e784-4281-9e80-96c2c67aa6f9" },
                    { 10, "Permissions", "user:add", "d241e2a9-e784-4281-9e80-96c2c67aa6f9" },
                    { 11, "Permissions", "user:update", "d241e2a9-e784-4281-9e80-96c2c67aa6f9" },
                    { 12, "Permissions", "role:read", "d241e2a9-e784-4281-9e80-96c2c67aa6f9" },
                    { 13, "Permissions", "role:add", "d241e2a9-e784-4281-9e80-96c2c67aa6f9" },
                    { 14, "Permissions", "role:update", "d241e2a9-e784-4281-9e80-96c2c67aa6f9" },
                    { 15, "Permissions", "results:read", "d241e2a9-e784-4281-9e80-96c2c67aa6f9" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5a07861e-6e86-43a4-ac49-99fb8bbd3125",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECHycjQCfuMWZB4ZmHZpXj8OxFcMCF5qwm+p5ICYM5knYcEAICGOEjRL/lMseFyhlg==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5a07861e-6e86-43a4-ac49-99fb8bbd3125",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHHdK0joD61zpF465sRaJXs82xL0BROrhvqBnUbe0uFeLqBlLgd7Bn4EAwwA/VTKrA==");
        }
    }
}
