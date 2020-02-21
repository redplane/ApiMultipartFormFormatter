using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ApiBackEnd.IntegrationTest.Shared.Extensions;
using ApiBackEnd.IntegrationTest.Shared.Interfaces;
using ApiBackEnd.IntegrationTest.Shared.Services;
using ApiBackEnd.IntegrationTest.Shared.ViewModels;
using ApiBackEndShared.ViewModels.Responses;
using ApiMultiPartFormData.Models;
using Autofac;
using Microsoft.Owin.Hosting;
using NUnit.Framework;

namespace ApiBackEnd.UnitTests.ControllerTests
{
    [TestFixture]
    public class AttachmentUploadTests : IAttachmentUploadTests
    {
        #region Properties

        private const string BaseUrl = "http://localhost:44321";

        private IDisposable _selfHosted;

        private IContainer _container;

        #endregion

        #region Setup & uninstallation

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _selfHosted = WebApp.Start<Startup>(BaseUrl);

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<HttpClient>()
                .InstancePerLifetimeScope()
                .OnActivating(instance =>
                {
                    var httpClient = new HttpClient();
                    httpClient.BaseAddress = new Uri(BaseUrl, UriKind.Absolute);
                    instance.ReplaceInstance(httpClient);
                });

            containerBuilder.RegisterType<AttachmentService>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            _container = containerBuilder.Build();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _selfHosted?.Dispose();
            _selfHosted = null;

            _container?.Dispose();
            _container = null;
        }

        #endregion

        #region Methods

        [Test]
        public virtual async Task UploadValidAttachment_Returns_AttachmentInfoWhoseInfoSameAsUploadedOne()
        {
            using (var lifeTimeScope = _container.BeginLifetimeScope())
            {
                var attachmentService = lifeTimeScope.Resolve<IAttachmentService>();

                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.Attachment = await attachmentService.LoadSampleAttachmentAsync();

                var httpClient = lifeTimeScope.Resolve<HttpClient>();
                var httpResponseMessage = await httpClient
                    .PostAsync(new Uri("api/upload", UriKind.Relative), uploadModel.ToMultipartFormDataContent());

                var uploadResult = await httpResponseMessage
                    .Content.ReadAsAsync<UploadResponseViewModel>();

                Assert.NotNull(uploadResult);
                Assert.AreEqual(uploadModel.Attachment.FileName, uploadResult.Attachment.FileName);
                Assert.AreEqual(uploadModel.Attachment.ContentLength, uploadResult.Attachment.ContentLength);
                Assert.AreEqual(uploadModel.Attachment.ContentType, uploadResult.Attachment.ContentType);
            }

        }

        [Test]
        public virtual async Task UploadValidAttachments_Returns_AttachmentsInfoWhoseInfoSameAsUploadedOnes()
        {
            using (var lifeTimeScope = _container.BeginLifetimeScope())
            {
                var attachmentService = lifeTimeScope.Resolve<IAttachmentService>();
                var sampleAttachment = await attachmentService.LoadSampleAttachmentAsync();

                var uploadModel = new RawUploadRequestViewModel();
                uploadModel.Attachments = new List<HttpFile>();
                uploadModel.Attachments.Add(new HttpFile(sampleAttachment));

                var httpClient = lifeTimeScope.Resolve<HttpClient>();
                var httpResponseMessage = await httpClient
                    .PostAsync(new Uri("api/upload", UriKind.Relative), uploadModel.ToMultipartFormDataContent());

                var uploadResult = await httpResponseMessage
                    .Content.ReadAsAsync<UploadResponseViewModel>();

                Assert.NotNull(uploadResult);
                Assert.NotNull(uploadResult.Attachments);

                for (var id = 0; id < uploadModel.Attachments.Count; id++)
                {
                    Assert.AreEqual(uploadModel.Attachments[id].FileName, uploadResult.Attachments[id].FileName);
                    Assert.AreEqual(uploadModel.Attachments[id].ContentLength, uploadResult.Attachments[id].ContentLength);
                    Assert.AreEqual(uploadModel.Attachments[id].ContentType, uploadResult.Attachments[id].ContentType);
                }
            }
            
        }

        #endregion
    }
}