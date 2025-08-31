using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurveyCart.Api.Migrations
{
    /// <inheritdoc />
    public partial class updateEntitiName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "ISDeleted", "IsDefault", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "bd3d951a-42ef-45a9-aadd-3924ff2a1df8", "0853c1cb-06c8-4eb5-a3e8-6189db5d3d22", false, true, "Member", "MEMBER" },
                    { "d241e2a9-e784-4281-9e80-96c2c67aa6f9", "a4f3c8e2-2b1e-4f0c-8b6e-3f5e5c6d7e8f", false, false, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "5a07861e-6e86-43a4-ac49-99fb8bbd3125", 0, "48500d6c-8583-4875-a801-dce5e4fd65ea", "adminsurvet-cart.com", true, "Survey Cart", "Admine", false, null, "ADMINSURVET-CART.COM", "ADMINSURVET-CART.COM", "AQAAAAIAAYagAAAAEHHdK0joD61zpF465sRaJXs82xL0BROrhvqBnUbe0uFeLqBlLgd7Bn4EAwwA/VTKrA==", null, false, "E33EB181-2A19-4E52-8E87-ED38CEE33C32", false, "adminsurvet-cart.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "d241e2a9-e784-4281-9e80-96c2c67aa6f9", "5a07861e-6e86-43a4-ac49-99fb8bbd3125" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bd3d951a-42ef-45a9-aadd-3924ff2a1df8");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "d241e2a9-e784-4281-9e80-96c2c67aa6f9", "5a07861e-6e86-43a4-ac49-99fb8bbd3125" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d241e2a9-e784-4281-9e80-96c2c67aa6f9");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5a07861e-6e86-43a4-ac49-99fb8bbd3125");
        }
    }
}
