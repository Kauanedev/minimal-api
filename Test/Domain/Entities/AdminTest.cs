using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using minimal_api.Domain.Entities;

namespace Test.Domain.Entities
{
    [TestClass]
    public class AdminTest
    {
        [TestMethod]
        public void TestGetSetProperties()
        {
            // Arrange
            // Cria uma instância de Admin
            var admin = new Admin();

            // Act
            // Define as propriedades do Admin
            admin.Id = "fc82e878-92b5-43ea-bbb3-d09a1297c1b0";
            admin.Email = "john.doe@example.com";
            admin.Password = "password123";
            admin.Perfil = 0;

            // Assert
            // Verifica se as propriedades foram definidas corretamente
            Assert.AreEqual<string>("fc82e878-92b5-43ea-bbb3-d09a1297c1b0", admin.Id);
            Assert.AreEqual<string>("john.doe@example.com", admin.Email);
            Assert.AreEqual<string>("password123", admin.Password);
            Assert.AreEqual<int>(0, (int)admin.Perfil);

        }

    }
}