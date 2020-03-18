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

namespace AspNetCore.IntegrationTest.ControllerTests
{
    [TestFixture]
    public class AttachmentUploadTests : IAttachmentUploadTests
    {
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

        #region Properties

        private const string BaseUrl = "http://localhost:44321";

        private TestServer _testServer;

        #endregion

        #region Methods

        [Test]
        public async Task UploadValidAttachment_Returns_AttachmentInfoWhoseInfoSameAsUploadedOne()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var attachmentService = lifeTimeScope.ServiceProvider.GetService<IAttachmentService>();

                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.Attachment = await attachmentService.LoadSampleAttachmentAsync();

                var httpClient = _testServer.CreateClient();
                var multipartFormContent = uploadModel.ToMultipartFormDataContent();
                var httpResponseMessage = await httpClient
                    .PostAsync(new Uri("api/upload", UriKind.Relative), multipartFormContent);

                var uploadResult = await httpResponseMessage
                    .Content.ReadAsAsync<UploadResponseViewModel>();

                Assert.NotNull(uploadResult);
                Assert.AreEqual(uploadModel.Attachment.FileName, uploadResult.Attachment.FileName);
                Assert.AreEqual(uploadModel.Attachment.ContentLength, uploadResult.Attachment.ContentLength);
                Assert.AreEqual(uploadModel.Attachment.ContentType, uploadResult.Attachment.ContentType);
            }
        }

        [Test]
        public async Task UploadValidAttachments_Returns_AttachmentsInfoWhoseInfoSameAsUploadedOnes()
        {
            using (var lifeTimeScope = _testServer.Services.CreateScope())
            {
                var attachmentService = lifeTimeScope.ServiceProvider.GetService<IAttachmentService>();
                var sampleAttachment = await attachmentService.LoadSampleAttachmentAsync();

                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.Attachments = new List<HttpFile>();
                uploadModel.Attachments.Add(new HttpFile(sampleAttachment));

                var httpClient = _testServer.CreateClient();
                var httpResponseMessage = await httpClient
                    .PostAsync(new Uri("api/upload", UriKind.Relative), uploadModel.ToMultipartFormDataContent());

                var uploadResult = await httpResponseMessage
                    .Content.ReadAsAsync<UploadResponseViewModel>();

                Assert.NotNull(uploadResult);
                Assert.NotNull(uploadResult.Attachments);

                for (var id = 0; id < uploadModel.Attachments.Count; id++)
                {
                    Assert.AreEqual(uploadModel.Attachments[id].FileName, uploadResult.Attachments[id].FileName);
                    Assert.AreEqual(uploadModel.Attachments[id].ContentLength,
                        uploadResult.Attachments[id].ContentLength);
                    Assert.AreEqual(uploadModel.Attachments[id].ContentType, uploadResult.Attachments[id].ContentType);
                }
            }
        }

        #endregion
    }
}