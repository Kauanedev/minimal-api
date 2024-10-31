using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using minimal_api.Domain.Interfaces;
using minimal_api;
using Test.Mocks;

namespace Test.Helpers
{
    public class Setup
    {
        public const string PORT = "8080";
        public static TestContext TestContext = default!;
        public static WebApplicationFactory<Startup> http = default!;
        public static HttpClient client = default!;

        public static void ClassInit(TestContext testContext)
        {
            Setup.TestContext = testContext;
            http = new WebApplicationFactory<Startup>();
            http = Setup.http.WithWebHostBuilder(builder =>
            {
                builder.UseSetting("https_port", Setup.PORT).UseEnvironment("Testing");

                builder.ConfigureServices(services =>
                {
                    services.AddScoped<IAdminService, AdminServiceMock>();
                });
            });

            Setup.client = Setup.http.CreateClient();
        }

        public static void Cleanup()
        {
            Setup.http.Dispose();
        }
    }
}
