using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ApiBackEnd.UnitTests.Extensions;
using ApiBackEnd.UnitTests.ViewModels;
using ApiBackEnd.ViewModels.Responses;
using Autofac;
using Microsoft.Owin.Hosting;
using NUnit.Framework;

namespace ApiBackEnd.UnitTests.ControllerTests
{
    [TestFixture]
    public class GuidUploadTests
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
        public virtual async Task UploadValidGuidString_Returns_ModelWithValidGuid()
        {
            var httpClient = _container.Resolve<HttpClient>();
            var uploadModel = new RawUploadRequestViewModel();
            uploadModel.Id = Guid.NewGuid().ToString("D");

            var httpResponseMessage = await httpClient
                .PostAsync(new Uri("api/upload", UriKind.Relative), uploadModel.ToMultipartFormDataContent());

            var httpResponse = await httpResponseMessage.Content
                .ReadAsAsync<UploadResponseViewModel>();

            Assert.NotNull(httpResponse);
            Assert.AreEqual(uploadModel.Id, httpResponse.Id.ToString("D"));
        }

        [Test]
        public virtual async Task UploadEmptyGuidString_Returns_ModelWithEmptyGuid()
        {
            var httpClient = _container.Resolve<HttpClient>();
            var uploadModel = new RawUploadRequestViewModel();
            uploadModel.Id = "";

            var httpResponseMessage = await httpClient
                .PostAsync(new Uri("api/upload", UriKind.Relative), uploadModel.ToMultipartFormDataContent());

            var httpResponse = await httpResponseMessage.Content
                .ReadAsAsync<UploadResponseViewModel>();

            Assert.NotNull(httpResponse);
            Assert.AreEqual(Guid.Empty.ToString("D"), httpResponse.Id.ToString("D"));
        }

        [Test]
        public virtual async Task LeaveIdFieldBlank_Returns_ModelWithEmptyGuid()
        {
            var httpClient = _container.Resolve<HttpClient>();
            var uploadModel = new RawUploadRequestViewModel();

            var httpResponseMessage = await httpClient
                .PostAsync(new Uri("api/upload", UriKind.Relative), uploadModel.ToMultipartFormDataContent());

            var httpResponse = await httpResponseMessage.Content
                .ReadAsAsync<UploadResponseViewModel>();

            Assert.NotNull(httpResponse);
            Assert.AreEqual(Guid.Empty.ToString("D"), httpResponse.Id.ToString("D"));
        }

        [Test]
        public virtual async Task UploadValidAttachmentIdIntoNullableGuidField_Returns_ModelWithValidGuid()
        {
            var httpClient = _container.Resolve<HttpClient>();
            var uploadModel = new RawUploadRequestViewModel();
            uploadModel.AttachmentId = Guid.NewGuid().ToString("D");

            var httpResponseMessage = await httpClient
                .PostAsync(new Uri("api/upload", UriKind.Relative), uploadModel.ToMultipartFormDataContent());

            var httpResponse = await httpResponseMessage.Content
                .ReadAsAsync<UploadResponseViewModel>();

            Assert.NotNull(httpResponse?.AttachmentId);
            Assert.AreEqual(uploadModel.AttachmentId, httpResponse.AttachmentId.Value.ToString("D"));
        }

        [Test]
        public virtual async Task NotIncludeAttachmentIdIntoNullableGuidField_Returns_ModelWithValidGuid()
        {
            var httpClient = _container.Resolve<HttpClient>();
            var uploadModel = new RawUploadRequestViewModel();

            var httpResponseMessage = await httpClient
                .PostAsync(new Uri("api/upload", UriKind.Relative), uploadModel.ToMultipartFormDataContent());

            var httpResponse = await httpResponseMessage.Content
                .ReadAsAsync<UploadResponseViewModel>();

            Assert.NotNull(httpResponse);
            Assert.Null(httpResponse.AttachmentId);
        }

        [Test]
        public virtual async Task UploadEmptyAttachmentIdIntoNullableGuidField_Returns_ModelWithNullAttachmentId()
        {
            var httpClient = _container.Resolve<HttpClient>();
            var uploadModel = new RawUploadRequestViewModel();
            uploadModel.AttachmentId = "";

            var httpResponseMessage = await httpClient
                .PostAsync(new Uri("api/upload", UriKind.Relative), uploadModel.ToMultipartFormDataContent());

            var httpResponse = await httpResponseMessage.Content
                .ReadAsAsync<UploadResponseViewModel>();

            Assert.NotNull(httpResponse);
            Assert.Null(httpResponse.AttachmentId);
        }


        [Test]
        public virtual async Task NotIncludeIdsInIds_Returns_ModelWithNullIds()
        {
            var httpClient = _container.Resolve<HttpClient>();
            var uploadModel = new RawUploadRequestViewModel();

            var httpResponseMessage = await httpClient
                .PostAsync(new Uri("api/upload", UriKind.Relative), uploadModel.ToMultipartFormDataContent());

            var httpResponse = await httpResponseMessage.Content
                .ReadAsAsync<UploadResponseViewModel>();

            Assert.NotNull(httpResponse);
            Assert.Null(httpResponse.Ids);
        }

        [Test]
        public virtual async Task IncludeIdsInIds_Returns_ModelWithUploadedIds()
        {
            var httpClient = _container.Resolve<HttpClient>();
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

        #endregion
    }
}
