using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ApiBackEnd.Enumerations;
using ApiBackEnd.UnitTests.Extensions;
using ApiBackEnd.UnitTests.ViewModels;
using ApiBackEnd.ViewModels.Responses;
using Autofac;
using Microsoft.Owin.Hosting;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ApiBackEnd.UnitTests.ControllerTests
{
    [TestFixture]
    public class EnumUploadTests
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
        public virtual async Task UploadValidQualityToNonNullableQuality_Returns_ModelWithValidQuality()
        {
            var httpClient = _container.Resolve<HttpClient>();
            var uploadModel = new RawUploadRequestViewModel();
            uploadModel.NonNullableQuality = nameof(Qualities.Best);

            var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                uploadModel.ToMultipartFormDataContent());

            var uploadResult = await httpResponseMessage.Content
                .ReadAsAsync<UploadResponseViewModel>();

            Assert.AreEqual(Qualities.Best, uploadResult.NonNullableQuality);
        }

        [Test]
        public virtual async Task UploadValidIntQualityToNonNullableQuality_Returns_ModelWithValidQuality()
        {
            var httpClient = _container.Resolve<HttpClient>();
            var uploadModel = new RawUploadRequestViewModel();
            uploadModel.NonNullableQuality = $"{(int)Qualities.Best}";

            var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                uploadModel.ToMultipartFormDataContent());

            var uploadResult = await httpResponseMessage.Content
                .ReadAsAsync<UploadResponseViewModel>();

            Assert.AreEqual(Qualities.Best, uploadResult.NonNullableQuality);
        }

        [Test]
        public virtual async Task UploadInvalidQualityToNonNullableQuality_Returns_ModelWithDefaultQuality()
        {
            var httpClient = _container.Resolve<HttpClient>();
            var uploadModel = new RawUploadRequestViewModel();
            uploadModel.NonNullableQuality = "-1";

            var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                uploadModel.ToMultipartFormDataContent());

            var uploadResult = await httpResponseMessage.Content
                .ReadAsAsync<UploadResponseViewModel>();

            Assert.AreEqual(default(Qualities), uploadResult.NonNullableQuality);
        }

        [Test]
        public virtual async Task UploadOutOfRangeQualityToNonNullableQuality_Returns_ModelWithDefaultQuality()
        {
            var httpClient = _container.Resolve<HttpClient>();
            var uploadModel = new RawUploadRequestViewModel();
            uploadModel.NonNullableQuality = "1000";

            var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                uploadModel.ToMultipartFormDataContent());

            var uploadResult = await httpResponseMessage.Content
                .ReadAsAsync<UploadResponseViewModel>();

            Assert.AreEqual(default(Qualities), uploadResult.NonNullableQuality);
        }

        [Test]
        public virtual async Task UploadValidQualityToNullableQuality_Returns_ModelWithUploadedQuality()
        {
            var httpClient = _container.Resolve<HttpClient>();
            var uploadModel = new RawUploadRequestViewModel();
            uploadModel.NullableQuality = nameof(Qualities.Best);

            var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                uploadModel.ToMultipartFormDataContent());

            var uploadResult = await httpResponseMessage.Content
                .ReadAsAsync<UploadResponseViewModel>();

            Assert.AreEqual(Qualities.Best, uploadResult.NullableQuality);
        }

        [Test]
        public virtual async Task UploadValidIntQualityToNullableQuality_Returns_ModelWithUploadedQuality()
        {
            var httpClient = _container.Resolve<HttpClient>();
            var uploadModel = new RawUploadRequestViewModel();
            uploadModel.NullableQuality = $"{(int)Qualities.Best}";

            var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                uploadModel.ToMultipartFormDataContent());

            var uploadResult = await httpResponseMessage.Content
                .ReadAsAsync<UploadResponseViewModel>();

            Assert.AreEqual(Qualities.Best, uploadResult.NullableQuality);
        }

        [Test]
        public virtual async Task UploadEmptyQualityToNullableQuality_Returns_ModelWithNullQuality()
        {
            var httpClient = _container.Resolve<HttpClient>();
            var uploadModel = new RawUploadRequestViewModel();
            uploadModel.NullableQuality = "";

            var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                uploadModel.ToMultipartFormDataContent());

            var uploadResult = await httpResponseMessage.Content
                .ReadAsAsync<UploadResponseViewModel>();

            Assert.IsNull(uploadResult.NullableQuality);
        }

        [Test]
        public virtual async Task UploadNegativeQualityToNullableQuality_Returns_ModelWithNullQuality()
        {
            var httpClient = _container.Resolve<HttpClient>();
            var uploadModel = new RawUploadRequestViewModel();
            uploadModel.NullableQuality = "-1";

            var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                uploadModel.ToMultipartFormDataContent());

            var uploadResult = await httpResponseMessage.Content
                .ReadAsAsync<UploadResponseViewModel>();

            Assert.IsNull(uploadResult.NullableQuality);
        }

        [Test]
        public virtual async Task UploadOutOfRangeQualityToNullableQuality_Returns_ModelWithNullQuality()
        {
            var httpClient = _container.Resolve<HttpClient>();
            var uploadModel = new RawUploadRequestViewModel();
            uploadModel.NullableQuality = "10000";

            var httpResponseMessage = await httpClient.PostAsync(new Uri("api/upload", UriKind.Relative),
                uploadModel.ToMultipartFormDataContent());

            var uploadResult = await httpResponseMessage.Content
                .ReadAsAsync<UploadResponseViewModel>();

            Assert.IsNull(uploadResult.NullableQuality);
        }

        [Test]
        public virtual async Task UploadValidQualityIntoQualities_Returns_ValidList()
        {
            var httpClient = _container.Resolve<HttpClient>();
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

        [Test]
        public virtual async Task NoUploadQualitiesIntoQualities_Returns_Null()
        {
            var httpClient = _container.Resolve<HttpClient>();
            var uploadModel = new RawUploadRequestViewModel();

            var httpResponseMessage = await httpClient
                .PostAsync(new Uri("api/upload", UriKind.Relative),
                    uploadModel.ToMultipartFormDataContent());

            var uploadResult = await httpResponseMessage.Content
                .ReadAsAsync<UploadResponseViewModel>();

            Assert.IsNull(uploadResult.Qualities);
        }

        #endregion

    }
}