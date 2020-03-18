using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ApiBackEnd.IntegrationTest.Shared.Extensions;
using ApiBackEnd.IntegrationTest.Shared.Interfaces;
using ApiBackEnd.IntegrationTest.Shared.Services;
using ApiBackEnd.IntegrationTest.Shared.ViewModels;
using ApiBackEndAspNetCore;
using ApiBackEndShared.ViewModels.Responses;
using ApiMultiPartFormData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AspNetCore.IntegrationTest.ControllerTests
{
    [TestFixture]
    public class GuidUploadTests : IGuidUploadTests
    {
        #region Properties

        private TestServer _testServer; 

        #endregion

        #region Setup & uninstallation

        [SetUp]
        public void Setup()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "integration-test");
            var contentRootFull = Path.GetFullPath(Directory.GetCurrentDirectory());
            var config = new ConfigurationBuilder()
                .SetBasePath(contentRootFull)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("appsettings.Development.json", true, true)
                .AddEnvironmentVariables().Build();

            var webHostBuilder = new WebHostBuilder()
                .UseEnvironment("integration-test")
                .UseConfiguration(config)
                .UseStartup<Startup>()
                .ConfigureServices(services =>
                {
                    services.AddScoped<IAttachmentService, AttachmentService>();

                    services.AddControllers(options =>
                        {
                            options.EnableEndpointRouting = false;

                            options.InputFormatters.Add(new MultipartFormDataFormatter());
                        })
                        .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
                });

            _testServer = new TestServer(webHostBuilder);
        }

        [TearDown]
        public void OneTimeTearDown()
        {
            _testServer?.Dispose();
        }

        #endregion

        #region Methods

        [Test]
        public virtual async Task UploadValidGuidString_Returns_ModelWithValidGuid()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.Id = Guid.NewGuid().ToString("D");

                var httpResponseMessage = await httpClient
                    .PostAsync(new Uri("api/upload", UriKind.Relative), uploadModel.ToMultipartFormDataContent());

                var httpResponse = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.NotNull(httpResponse);
                Assert.AreEqual(uploadModel.Id, httpResponse.Id.ToString("D"));
            }
        }

        [Test]
        public virtual async Task UploadEmptyGuidString_Returns_ModelWithEmptyGuid()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.Id = "";

                var httpResponseMessage = await httpClient
                    .PostAsync(new Uri("api/upload", UriKind.Relative), uploadModel.ToMultipartFormDataContent());

                var httpResponse = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.NotNull(httpResponse);
                Assert.AreEqual(Guid.Empty.ToString("D"), httpResponse.Id.ToString("D"));
            }
        }

        [Test]
        public virtual async Task LeaveIdFieldBlank_Returns_ModelWithEmptyGuid()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();

                var httpResponseMessage = await httpClient
                    .PostAsync(new Uri("api/upload", UriKind.Relative), uploadModel.ToMultipartFormDataContent());

                var httpResponse = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.NotNull(httpResponse);
                Assert.AreEqual(Guid.Empty.ToString("D"), httpResponse.Id.ToString("D"));
            }
        }

        [Test]
        public virtual async Task UploadValidAttachmentIdIntoNullableGuidField_Returns_ModelWithValidGuid()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.AttachmentId = Guid.NewGuid().ToString("D");

                var httpResponseMessage = await httpClient
                    .PostAsync(new Uri("api/upload", UriKind.Relative), uploadModel.ToMultipartFormDataContent());

                var httpResponse = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.NotNull(httpResponse?.AttachmentId);
                Assert.AreEqual(uploadModel.AttachmentId, httpResponse.AttachmentId.Value.ToString("D"));
            }
        }

        [Test]
        public virtual async Task NotIncludeAttachmentIdIntoNullableGuidField_Returns_ModelWithValidGuid()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();

                var httpResponseMessage = await httpClient
                    .PostAsync(new Uri("api/upload", UriKind.Relative), uploadModel.ToMultipartFormDataContent());

                var httpResponse = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.NotNull(httpResponse);
                Assert.Null(httpResponse.AttachmentId);
            }
        }

        [Test]
        public virtual async Task UploadEmptyAttachmentIdIntoNullableGuidField_Returns_ModelWithNullAttachmentId()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.AttachmentId = "";

                var httpResponseMessage = await httpClient
                    .PostAsync(new Uri("api/upload", UriKind.Relative), uploadModel.ToMultipartFormDataContent());

                var httpResponse = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.NotNull(httpResponse);
                Assert.Null(httpResponse.AttachmentId);
            }
        }


        [Test]
        public virtual async Task NotIncludeIdsInIds_Returns_ModelWithNullIds()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();

                var httpResponseMessage = await httpClient
                    .PostAsync(new Uri("api/upload", UriKind.Relative), uploadModel.ToMultipartFormDataContent());

                var httpResponse = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.NotNull(httpResponse);
                Assert.Null(httpResponse.Ids);
            }
        }

        [Test]
        public virtual async Task IncludeIdsInIds_Returns_ModelWithUploadedIds()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.Ids = new List<string>
                {
                    Guid.NewGuid().ToString("D"),
                    Guid.NewGuid().ToString("D"),
                    Guid.NewGuid().ToString("D"),
                    Guid.NewGuid().ToString("D")
                };

                var httpResponseMessage = await httpClient
                    .PostAsync(new Uri("api/upload", UriKind.Relative), uploadModel.ToMultipartFormDataContent());

                var httpResponse = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.NotNull(httpResponse);

                for (var id = 0; id < uploadModel.Ids.Count; id++)
                    Assert.AreEqual(uploadModel.Ids[id], httpResponse.Ids[id].ToString("D"));
            }
        }

        #endregion
    }
}
