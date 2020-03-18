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
using ApiBackEndShared.Enumerations;
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
    public class NestedEnumUploadTests : INestedEnumUploadTests
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
        public void TearDown()
        {
            _testServer?.Dispose();
        }

        #endregion

        #region Methods

        [Test]
        public virtual async Task UploadValidQualityToProfileNonNullableQuality_Returns_ModelWithValidProfileNonNullableQuality()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();

                uploadModel.Profile = new RawProfileViewModel();
                uploadModel.Profile.NonNullableQuality = nameof(Qualities.Best);

                var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                    uploadModel.ToMultipartFormDataContent());

                var uploadResult = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.AreEqual(Qualities.Best, uploadResult.Profile.NonNullableQuality);
            }
        }

        [Test]
        public virtual async Task UploadValidIntQualityToProfileNonNullableQuality_Returns_ModelWithProfileValidQuality()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.Profile = new RawProfileViewModel();
                uploadModel.Profile.NonNullableQuality = $"{(int) Qualities.Best}";

                var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                    uploadModel.ToMultipartFormDataContent());

                var uploadResult = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.AreEqual(Qualities.Best, uploadResult.Profile.NonNullableQuality);
            }
        }

        [Test]
        public virtual async Task UploadInvalidQualityToProfileNonNullableQuality_Returns_ModelWithDefaultProfileQuality()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.Profile = new RawProfileViewModel();
                uploadModel.Profile.NonNullableQuality = "-1";

                var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                    uploadModel.ToMultipartFormDataContent());

                var uploadResult = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.AreEqual(default(Qualities), uploadResult.Profile.NonNullableQuality);
            }
        }

        [Test]
        public virtual async Task UploadOutOfRangeQualityToProfileNonNullableQuality_Returns_ModelProfileWithDefaultQuality()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.Profile = new RawProfileViewModel();
                uploadModel.Profile.NonNullableQuality = "1000";

                var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                    uploadModel.ToMultipartFormDataContent());

                var uploadResult = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.AreEqual(default(Qualities), uploadResult.Profile.NonNullableQuality);
            }
        }

        [Test]
        public virtual async Task UploadValidQualityToProfileNullableQuality_Returns_ModelWithUploadedProfileQuality()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.Profile = new RawProfileViewModel();
                uploadModel.Profile.NullableQuality = nameof(Qualities.Best);

                var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                    uploadModel.ToMultipartFormDataContent());

                var uploadResult = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.AreEqual(Qualities.Best, uploadResult.Profile.NullableQuality);
            }
        }

        [Test]
        public virtual async Task UploadValidIntQualityToProfileNullableQuality_Returns_ModelWithUploadedProfileQuality()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.Profile = new RawProfileViewModel();
                uploadModel.Profile.NullableQuality = $"{(int) Qualities.Best}";

                var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                    uploadModel.ToMultipartFormDataContent());

                var uploadResult = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.AreEqual(Qualities.Best, uploadResult.Profile.NullableQuality);
            }
        }

        [Test]
        public virtual async Task UploadEmptyQualityToNullableQuality_Returns_ModelWithNullQuality()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.Profile = new RawProfileViewModel();
                uploadModel.Profile.NullableQuality = "";

                var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                    uploadModel.ToMultipartFormDataContent());

                var uploadResult = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.IsNull(uploadResult.Profile.NullableQuality);
            }
        }

        [Test]
        public virtual async Task UploadNegativeQualityToNullableQuality_Returns_ModelWithNullQuality()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.Profile = new RawProfileViewModel();
                uploadModel.Profile.NullableQuality = "-1";

                var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                    uploadModel.ToMultipartFormDataContent());

                var uploadResult = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.IsNull(uploadResult.Profile.NullableQuality);
            }
        }

        [Test]
        public virtual async Task UploadOutOfRangeQualityToProfileNullableQuality_Returns_ModelWithNullProfileQuality()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.Profile = new RawProfileViewModel();
                uploadModel.Profile.NullableQuality = "10000";

                var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                    uploadModel.ToMultipartFormDataContent());

                var uploadResult = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.IsNull(uploadResult.NullableQuality);
            }
        }

        [Test]
        public virtual async Task UploadValidQualityIntoProfileQualities_Returns_ValidList()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.Profile = new RawProfileViewModel();
                uploadModel.Profile.Qualities = new List<string>();

                var values = Enum.GetNames(typeof(Qualities));
                foreach (var value in values)
                    uploadModel.Profile.Qualities.Add(value);

                var httpResponseMessage = await httpClient
                    .PostAsync(new Uri("api/upload", UriKind.Relative),
                        uploadModel.ToMultipartFormDataContent());

                var uploadResult = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.NotNull(uploadResult.Profile.Qualities);
                for (var id = 0; id < values.Length; id++)
                    Assert.AreEqual(values[id], uploadResult.Profile.Qualities[id].ToString("G"));
            }
        }

        #endregion

    }
}