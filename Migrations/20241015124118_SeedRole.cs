using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class SeedRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b534166a-16f5-4245-b22a-1bb24329c464", "e181b66f-a6ff-42c6-8e9a-7872602e2225", "User", "USER" },
                    { "edbbcca3-547e-421f-b7fb-cbef94469502", "9e0084c8-5117-4307-bd5b-6675737e92df", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b534166a-16f5-4245-b22a-1bb24329c464");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "edbbcca3-547e-421f-b7fb-cbef94469502");
        }
    }
}
