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

            var admin = new Admin();

            admin.Email = "john.doe@example.com";
            admin.Password = "password123";
            admin.Perfil = (PerfilEnum)1;

            var adminService = new AdminService(context);

            // Act
            adminService.Create(admin);

            // Assert
            Assert.AreEqual(1, adminService.GetAll(1).Count());

            transaction.Rollback(); // Desfaz as alterações feitas no banco de dados
        }
    }
}