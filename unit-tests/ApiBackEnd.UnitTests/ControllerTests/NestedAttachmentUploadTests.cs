using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.SelfHost;
using ApiBackEnd.UnitTests.Extensions;
using ApiBackEnd.UnitTests.Services.Implementations;
using ApiBackEnd.UnitTests.Services.Interfaces;
using ApiBackEnd.UnitTests.ViewModels;
using ApiBackEndShared.ViewModels.Responses;
using ApiMultiPartFormData.Models;
using Autofac;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Testing;
using NUnit.Framework;

namespace ApiBackEnd.UnitTests.ControllerTests
{
    [TestFixture]
    public class NestedAttachmentUploadTests
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
            _selfHosted = TestServer.Create<ApiBackEnd.Startup>();
            var a = (TestServer) _selfHosted;

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<HttpClient>()
                .InstancePerLifetimeScope()
                .OnActivating(instance =>
                {
                    instance.ReplaceInstance(a.HttpClient);
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
        public virtual async Task UploadValidNestedAttachment_Returns_AttachmentInfoWhoseInfoSameAsUploadedOne()
        {
            var attachmentService = _container.Resolve<IAttachmentService>();
            var attachment = await attachmentService.LoadSampleAttachmentAsync();

            var uploadModel = new RawUploadRequestViewModel();
            uploadModel.Profile = new RawProfileViewModel();
            uploadModel.Profile.Photo = attachment;

            var httpClient = _container.Resolve<HttpClient>();
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
            var attachmentService = _container.Resolve<IAttachmentService>();

            var uploadModel = new RawUploadRequestViewModel();
            uploadModel.Profile = new RawProfileViewModel();
            uploadModel.Profile.Photos = new List<HttpFileBase>();
            uploadModel.Profile.Photos.Add(await attachmentService.LoadSampleAttachmentAsync());
            uploadModel.Profile.Photos.Add(await attachmentService.LoadSampleAttachmentAsync());

            var httpClient = _container.Resolve<HttpClient>();
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
                Assert.AreEqual(uploadModel.Profile.Photos[id].ContentLength, uploadResult.Profile.Photos[id].ContentLength);
                Assert.AreEqual(uploadModel.Profile.Photos[id].ContentType, uploadResult.Profile.Photos[id].ContentType);
            }
        }

        #endregion
    }
}