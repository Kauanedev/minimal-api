using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using minimal_api.Domain.Dto;
using Test.Helpers;

namespace Test.Requests
{
    [TestClass]
    public class AdminRequestTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            Setup.Initialize(context);
        }

        [ClassCleanup]
        public static void Dispose()
        {
            Setup.Cleanup();
        }

        [TestMethod]
        public async Task TestGetSetProperties()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "admin1@example.com",
                Password = "string"
            };

            var content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");

            // Act
            var response = await Setup.client.PostAsync("/administradores/login", content);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
