using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ApiBackEnd.IntegrationTest.Shared.Extensions;
using ApiBackEnd.IntegrationTest.Shared.ViewModels;
using ApiBackEndShared.ViewModels.Responses;
using Autofac;
using Microsoft.Owin.Hosting;
using NUnit.Framework;

namespace ApiBackEnd.UnitTests.ControllerTests
{
    [TestFixture]
    public class NestedGuidUploadTests
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
        public virtual async Task UploadValidProfileNonNullableGuidString_Returns_ModelWithValidProfileGuid()
        {
            using (var lifeTimeScope = _container.BeginLifetimeScope())
            {
                var httpClient = lifeTimeScope.Resolve<HttpClient>();
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
            using (var lifeTimeScope = _container.BeginLifetimeScope())
            {
                var httpClient = lifeTimeScope.Resolve<HttpClient>();
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
            using (var lifeTimeScope = _container.BeginLifetimeScope())
            {
                var httpClient = lifeTimeScope.Resolve<HttpClient>();
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
            using (var lifeTimeScope = _container.BeginLifetimeScope())
            {
                var httpClient = lifeTimeScope.Resolve<HttpClient>();
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