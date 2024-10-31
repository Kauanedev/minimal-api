using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using minimal_api.Domain.Entities;
using minimal_api.Domain.Services;
using minimal_api.Infra.Database;
using minimal_api.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Test.Domain.Services
{
    [TestClass]
    public class AdminServiceTest
    {
        private DbContexto CreateTestContext()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Verificar se o path não é nulo
            if (string.IsNullOrEmpty(path)) throw new InvalidOperationException("O caminho base não pode ser nulo.");

            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            return new DbContexto(configuration);
        }

        [TestMethod]
        public void TestSaveAdmin()
        {
            // Arrange
            var context = CreateTestContext();
            using var transaction = context.Database.BeginTransaction();

            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores;");

            var admin = new Admin
            {
                Email = "john.doe@example.com",
                Password = "password123",
                Perfil = PerfilEnum.Admin
            };

            var adminService = new AdminService(context);

            // Act
            adminService.Create(admin);
            var adminById = adminService.GetById(admin.Id); // obtém o admin pelo ID

            // Assert
            Assert.AreEqual(1, adminService.GetAll(1).Count());

            // verificação para garantir que o admin foi criado
            Assert.IsNotNull(adminById);
            Assert.AreEqual(admin.Email, adminById.Email); // Verifica se os detalhes estão corretos

            transaction.Rollback(); // Desfaz as alterações feitas no banco de dados
        }

        [TestMethod]
        public void TestGetAdminById()
        {
            // Arrange
            var context = CreateTestContext();
            using var transaction = context.Database.BeginTransaction();

            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores;");

            var admin = new Admin
            {
                Email = "john.doe@example.com",
                Password = "password123",
                Perfil = PerfilEnum.Admin
            };

            var adminService = new AdminService(context);
            adminService.Create(admin); // Certifica que o admin é criado antes de buscá-lo

            // Act
            var adminById = adminService.GetById(admin.Id); // Passa o ID do admin

            // Assert
            Assert.IsNotNull(adminById); // Verifica se o admin foi encontrado
            Assert.AreEqual<string>(admin.Id, adminById.Id); // Verifica se o ID está correto

            transaction.Rollback(); // Desfaz as alterações feitas no banco de dados
        }
    }
}
