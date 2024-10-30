using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace minimal_api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEnumToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Administradores",
                keyColumn: "Id",
                keyValue: "962ecb22-aac6-426c-8c96-2662dade74d5");

            migrationBuilder.InsertData(
                table: "Administradores",
                columns: new[] { "Id", "Email", "Password", "Perfil" },
                values: new object[] { "1fb3aea4-2e49-457d-9aa7-0672217dedb3", "admin@email.com", "123456", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Administradores",
                keyColumn: "Id",
                keyValue: "1fb3aea4-2e49-457d-9aa7-0672217dedb3");

            migrationBuilder.InsertData(
                table: "Administradores",
                columns: new[] { "Id", "Email", "Password", "Perfil" },
                values: new object[] { "962ecb22-aac6-426c-8c96-2662dade74d5", "admin@email.com", "123456", 0 });
        }
    }
}
