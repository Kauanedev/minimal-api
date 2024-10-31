using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace minimal_api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminPerfilEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Administradores",
                keyColumn: "Id",
                keyValue: "1fb3aea4-2e49-457d-9aa7-0672217dedb3");

            migrationBuilder.InsertData(
                table: "Administradores",
                columns: new[] { "Id", "Email", "Password", "Perfil" },
                values: new object[] { "375b648d-e1e2-4dae-b166-89bb7896982f", "admin@email.com", "123456", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Administradores",
                keyColumn: "Id",
                keyValue: "375b648d-e1e2-4dae-b166-89bb7896982f");

            migrationBuilder.InsertData(
                table: "Administradores",
                columns: new[] { "Id", "Email", "Password", "Perfil" },
                values: new object[] { "1fb3aea4-2e49-457d-9aa7-0672217dedb3", "admin@email.com", "123456", 1 });
        }
    }
}
