using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ApiBackEnd.UnitTests.Extensions;
using ApiBackEnd.UnitTests.ViewModels;
using ApiBackEndShared.Enumerations;
using ApiBackEndShared.ViewModels.Responses;
using Autofac;
using Microsoft.Owin.Hosting;
using NUnit.Framework;

namespace ApiBackEnd.UnitTests.ControllerTests
{
    [TestFixture]
    public class NestedEnumUploadTests
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
        public virtual async Task UploadValidQualityToProfileNonNullableQuality_Returns_ModelWithValidProfileNonNullableQuality()
        {
            using (var lifeTimeScope = _container.BeginLifetimeScope())
            {
                var httpClient = lifeTimeScope.Resolve<HttpClient>();
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
            using (var lifeTimeScope = _container.BeginLifetimeScope())
            {
                var httpClient = lifeTimeScope.Resolve<HttpClient>();
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
            using (var lifeTimeScope = _container.BeginLifetimeScope())
            {
                var httpClient = lifeTimeScope.Resolve<HttpClient>();
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
            using (var lifeTimeScope = _container.BeginLifetimeScope())
            {
                var httpClient = lifeTimeScope.Resolve<HttpClient>();
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
            using (var lifeTimeScope = _container.BeginLifetimeScope())
            {
                var httpClient = lifeTimeScope.Resolve<HttpClient>();
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
            using (var lifeTimeScope = _container.BeginLifetimeScope())
            {
                var httpClient = lifeTimeScope.Resolve<HttpClient>();
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
            using (var lifeTimeScope = _container.BeginLifetimeScope())
            {
                var httpClient = lifeTimeScope.Resolve<HttpClient>();
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
            using (var lifeTimeScope = _container.BeginLifetimeScope())
            {
                var httpClient = lifeTimeScope.Resolve<HttpClient>();
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
            using (var lifeTimeScope = _container.BeginLifetimeScope())
            {
                var httpClient = lifeTimeScope.Resolve<HttpClient>();
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
            using (var lifeTimeScope = _container.BeginLifetimeScope())
            {
                var httpClient = lifeTimeScope.Resolve<HttpClient>();
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