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

namespace AspNetCore.ApiBackEnd.ControllerTests
{
    [TestFixture]
    public class EnumUploadTests
    {

        #region Properties

        private const string BaseUrl = "http://localhost:44321";

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
                .AddJsonFile($"appsettings.Development.json", true, true)
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

        #endregion

        #region Methods

        [Test]
        public virtual async Task UploadValidQualityToNonNullableQuality_Returns_ModelWithValidQuality()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.NonNullableQuality = nameof(Qualities.Best);

                var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                    uploadModel.ToMultipartFormDataContent());

                var uploadResult = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.AreEqual(Qualities.Best, uploadResult.NonNullableQuality);
            }
            
        }

        [Test]
        public virtual async Task UploadValidIntQualityToNonNullableQuality_Returns_ModelWithValidQuality()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.NonNullableQuality = $"{(int) Qualities.Best}";

                var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                    uploadModel.ToMultipartFormDataContent());

                var uploadResult = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.AreEqual(Qualities.Best, uploadResult.NonNullableQuality);
            }
        }

        [Test]
        public virtual async Task UploadInvalidQualityToNonNullableQuality_Returns_ModelWithDefaultQuality()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.NonNullableQuality = "-1";

                var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                    uploadModel.ToMultipartFormDataContent());

                var uploadResult = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.AreEqual(default(Qualities), uploadResult.NonNullableQuality);
            }

        }

        [Test]
        public virtual async Task UploadOutOfRangeQualityToNonNullableQuality_Returns_ModelWithDefaultQuality()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.NonNullableQuality = "1000";

                var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                    uploadModel.ToMultipartFormDataContent());

                var uploadResult = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.AreEqual(default(Qualities), uploadResult.NonNullableQuality);
            }
        }

        [Test]
        public virtual async Task UploadValidQualityToNullableQuality_Returns_ModelWithUploadedQuality()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.NullableQuality = nameof(Qualities.Best);

                var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                    uploadModel.ToMultipartFormDataContent());

                var uploadResult = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.AreEqual(Qualities.Best, uploadResult.NullableQuality);
            }
        }

        [Test]
        public virtual async Task UploadValidIntQualityToNullableQuality_Returns_ModelWithUploadedQuality()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.NullableQuality = $"{(int)Qualities.Best}";

                var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                    uploadModel.ToMultipartFormDataContent());

                var uploadResult = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.AreEqual(Qualities.Best, uploadResult.NullableQuality);
            }
        }

        [Test]
        public virtual async Task UploadEmptyQualityToNullableQuality_Returns_ModelWithNullQuality()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.NullableQuality = "";

                var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                    uploadModel.ToMultipartFormDataContent());

                var uploadResult = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.IsNull(uploadResult.NullableQuality);
            }
        }

        [Test]
        public virtual async Task UploadNegativeQualityToNullableQuality_Returns_ModelWithNullQuality()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.NullableQuality = "-1";

                var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                    uploadModel.ToMultipartFormDataContent());

                var uploadResult = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.IsNull(uploadResult.NullableQuality);
            }
        }

        [Test]
        public virtual async Task UploadOutOfRangeQualityToNullableQuality_Returns_ModelWithNullQuality()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.NullableQuality = "10000";

                var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                    uploadModel.ToMultipartFormDataContent());

                var uploadResult = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.IsNull(uploadResult.NullableQuality);
            }
        }

        [Test]
        public virtual async Task UploadValidQualityIntoQualities_Returns_ValidList()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.Qualities = new List<string>();

                var values = Enum.GetNames(typeof(Qualities));
                foreach (var value in values)
                    uploadModel.Qualities.Add(value);

                var httpResponseMessage = await httpClient
                    .PostAsync(new Uri("api/upload", UriKind.Relative),
                        uploadModel.ToMultipartFormDataContent());

                var uploadResult = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.NotNull(uploadResult.Qualities);
                for (var id = 0; id < values.Length; id++)
                    Assert.AreEqual(values[id], uploadResult.Qualities[id].ToString("G"));
            }
        }

        [Test]
        public virtual async Task NoUploadQualitiesIntoQualities_Returns_Null()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var httpClient = _testServer.CreateClient();
                var uploadModel = new RawUploadRequestViewModel();

                var content = uploadModel.ToMultipartFormDataContent();
                var httpResponseMessage = await httpClient
                    .PostAsync(new Uri("api/upload", UriKind.Relative), content);

                var uploadResult = await httpResponseMessage.Content
                    .ReadAsAsync<UploadResponseViewModel>();

                Assert.IsNull(uploadResult.Qualities);
            }
        }

        #endregion

    }
}