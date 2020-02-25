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
using ApiMultiPartFormData.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AspNetCore.ApiBackEnd.ControllerTests
{
    [TestFixture]
    public class NestedAttachmentUploadTests : INestedAttachmentUploadTests
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
        public virtual async Task UploadValidNestedAttachment_Returns_AttachmentInfoWhoseInfoSameAsUploadedOne()
        {
            var attachmentService = _testServer.Services.GetService<IAttachmentService>();
            var attachment = await attachmentService.LoadSampleAttachmentAsync();

            var uploadModel = new RawUploadRequestViewModel();
            uploadModel.Profile = new RawProfileViewModel();
            uploadModel.Profile.Photo = attachment;

            var httpClient = _testServer.CreateClient();
            var httpResponseMessage = await httpClient
                .PostAsync(new Uri("api/upload", UriKind.Relative), uploadModel.ToMultipartFormDataContent());

            var uploadResult = await httpResponseMessage
                .Content.ReadAsAsync<UploadResponseViewModel>();

            Assert.NotNull(uploadResult);
            Assert.AreEqual(uploadModel.Profile.Photo.FileName, uploadResult.Profile.Photo.FileName);
            Assert.AreEqual(uploadModel.Profile.Photo.ContentLength, uploadResult.Profile.Photo.ContentLength);
            Assert.AreEqual(uploadModel.Profile.Photo.ContentType, uploadResult.Profile.Photo.ContentType);
        }

        [Test]
        public virtual async Task UploadValidProfileAttachments_Returns_AttachmentsInfoWhoseInfoSameAsUploadedOnes()
        {
            var attachmentService = _testServer.Services.GetService<IAttachmentService>();

            var uploadModel = new RawUploadRequestViewModel();
            uploadModel.Profile = new RawProfileViewModel();
            uploadModel.Profile.Photos = new List<HttpFileBase>();
            uploadModel.Profile.Photos.Add(await attachmentService.LoadSampleAttachmentAsync());
            uploadModel.Profile.Photos.Add(await attachmentService.LoadSampleAttachmentAsync());

            var httpClient = _testServer.CreateClient();
            var httpResponseMessage = await httpClient
                .PostAsync(new Uri("api/upload", UriKind.Relative), uploadModel.ToMultipartFormDataContent());

            var uploadResult = await httpResponseMessage
                .Content.ReadAsAsync<UploadResponseViewModel>();

            Assert.NotNull(uploadResult);
            Assert.NotNull(uploadResult.Profile);
            Assert.NotNull(uploadResult.Profile.Photos);

            for (var id = 0; id < uploadModel.Profile.Photos.Count; id++)
            {
                Assert.AreEqual(uploadModel.Profile.Photos[id].FileName, uploadResult.Profile.Photos[id].FileName);
                Assert.AreEqual(uploadModel.Profile.Photos[id].ContentLength,
                    uploadResult.Profile.Photos[id].ContentLength);
                Assert.AreEqual(uploadModel.Profile.Photos[id].ContentType,
                    uploadResult.Profile.Photos[id].ContentType);
            }
        }

        #endregion
    }
}