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
    public class NestedGuidUploadTests : INestedGuidUploadTests
    {
        #region Properties

        private TestServer _testServer;

        #endregion

        #region Installations

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
        public void TearDown()
        {
            _testServer?.Dispose();
        }

        #endregion

        #region Methods

        [Test]
        public virtual async Task UploadValidProfileNonNullableGuidString_Returns_ModelWithValidProfileGuid()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.Profile = new RawProfileViewModel();
                uploadModel.Profile.NonNullableId = Guid.NewGuid().ToString("D");

                var httpResponseMessage = await httpClient
                    .PostAsync(new Uri("api/upload", UriKind.Relative),
                        uploadModel.ToMultipartFormDataContent());

                var httpResponse = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.NotNull(httpResponse);
                Assert.AreEqual(uploadModel.Profile.NonNullableId,
                    httpResponse.Profile.NonNullableId.ToString("D"));
            }
        }

        [Test]
        public virtual async Task UploadEmptyGuidStringIntoProfileNonNullableGuidString_Returns_ModelWithEmptyProfileGuid()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.Profile = new RawProfileViewModel();
                uploadModel.Profile.NonNullableId = Guid.Empty.ToString("D");

                var httpResponseMessage = await httpClient
                    .PostAsync(new Uri("api/upload", UriKind.Relative), uploadModel.ToMultipartFormDataContent());

                var httpResponse = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.NotNull(httpResponse);
                Assert.AreEqual(Guid.Empty.ToString("D"), httpResponse.Profile.NonNullableId.ToString("D"));
            }
        }

        [Test]
        public virtual async Task UploadValidGuidIdIntoNullableGuidField_Returns_ModelWithValidGuid()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.Profile = new RawProfileViewModel();
                uploadModel.Profile.NullableId = Guid.NewGuid().ToString("D");
                var httpResponseMessage = await httpClient
                    .PostAsync(new Uri("api/upload", UriKind.Relative), uploadModel.ToMultipartFormDataContent());

                var httpResponse = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.NotNull(httpResponse?.Profile?.NullableId);
                Assert.AreEqual(uploadModel.Profile.NullableId,
                    httpResponse.Profile.NullableId.Value.ToString("D"));
            }
        }


        [Test]
        public virtual async Task IncludeIdsInProfileIds_Returns_ModelWithUploadedProfileIds()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.Profile = new RawProfileViewModel();
                uploadModel.Profile.Ids = new List<string>
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

                for (var id = 0; id < uploadModel.Profile.Ids.Count; id++)
                    Assert.AreEqual(uploadModel.Profile.Ids[id], httpResponse.Profile.Ids[id].ToString("D"));
            }
        }

        #endregion
    }
}