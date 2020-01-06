using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using ApiBackEnd.Controllers;
using ApiBackEnd.Enumerations;
using ApiBackEnd.ViewModels;
using ApiBackEnd.ViewModels.Requests;
using ApiBackEnd.ViewModels.Responses;
using Autofac;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ApiBackEnd.UnitTest.ControllerTests
{
    [TestFixture]
    public class EnumUploadTests
    {
        #region Installations

        [SetUp]
        public void Setup()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<ApiUploadController>()
                .OnActivating(instance =>
                {
                    var apiUploadController = new ApiUploadController();
                    apiUploadController.Request = new HttpRequestMessage
                    {
                        RequestUri = new Uri("http://localhost/api/basic-upload")
                    };

                    apiUploadController.Configuration = new HttpConfiguration();
                    apiUploadController.Configuration.Routes.MapHttpRoute(
                        "DefaultApi",
                        "api/{controller}/{id}",
                        new { id = RouteParameter.Optional });

                    apiUploadController.RequestContext.RouteData = new HttpRouteData(
                        new HttpRoute(),
                        new HttpRouteValueDictionary { { "controller", "products" } });

                    instance.ReplaceInstance(apiUploadController);
                })
                .InstancePerLifetimeScope();

            _container = containerBuilder.Build();
        }

        [TearDown]
        public void TearDown()
        {
            _container.Dispose();
        }

        #endregion


        #region Properties

        private IContainer _container;

        #endregion

        #region Methods

        [Test]
        public async Task UploadValidQualityIntoNonNullableQuality_Returns_ValidQualityEnum()
        {
            var controller = _container.Resolve<ApiUploadController>();

            var uploadModel = new UploadRequestViewModel();
            uploadModel.NonNullableQuality = Qualities.Best;

            var actionResult = controller.BasicUpload(uploadModel);
            var httpResponseMessage = await actionResult.ExecuteAsync(default);
            var uploadResponseModel = await httpResponseMessage.Content.ReadAsAsync<UploadResponseViewModel>();

            Assert.AreEqual(uploadModel.NonNullableQuality, uploadResponseModel.NonNullableQuality);
        }

        [Test]
        public async Task UploadValidQualityIntoProfileNonNullableQuality_Returns_ValidProfileQualityEnum()
        {
            var controller = _container.Resolve<ApiUploadController>();

            var uploadModel = new UploadRequestViewModel();
            uploadModel.Profile = new ProfileViewModel();
            uploadModel.Profile.NonNullableQuality = Qualities.Average;

            var actionResult = controller.BasicUpload(uploadModel);
            var httpResponseMessage = await actionResult.ExecuteAsync(default);
            var uploadResponseModel = await httpResponseMessage.Content.ReadAsAsync<UploadResponseViewModel>();

            Assert.AreEqual(uploadModel.Profile.NonNullableQuality, uploadResponseModel.Profile.NonNullableQuality);
        }

        [Test]
        public async Task UploadNullQualityIntoNullableQuality_Returns_NullQuality()
        {
            var controller = _container.Resolve<ApiUploadController>();

            var uploadModel = new UploadRequestViewModel();
            uploadModel.NullableQuality = null;

            var actionResult = controller.BasicUpload(uploadModel);
            var httpResponseMessage = await actionResult.ExecuteAsync(default);
            var uploadResponseModel = await httpResponseMessage.Content.ReadAsAsync<UploadResponseViewModel>();

            Assert.IsNull(uploadResponseModel.NullableQuality);
        }

        [Test]
        public async Task UploadValidQualityIntoNullableQuality_Returns_ValidQuality()
        {
            var controller = _container.Resolve<ApiUploadController>();

            var uploadModel = new UploadRequestViewModel();
            uploadModel.NullableQuality = Qualities.Average;

            var actionResult = controller.BasicUpload(uploadModel);
            var httpResponseMessage = await actionResult.ExecuteAsync(default);
            var uploadResponseModel = await httpResponseMessage.Content.ReadAsAsync<UploadResponseViewModel>();

            Assert.AreEqual(uploadModel.NullableQuality, uploadResponseModel.NullableQuality);
        }

        [Test]
        public async Task UploadValidQualityIntoProfileQualities_Returns_ValidProfileQualities()
        {
            var controller = _container.Resolve<ApiUploadController>();

            var uploadModel = new UploadRequestViewModel();
            uploadModel.Profile = new ProfileViewModel();
            uploadModel.Profile.Qualities = new List<Qualities>(){Qualities.Average, Qualities.Bad, Qualities.Best};

            var actionResult = controller.BasicUpload(uploadModel);
            var httpResponseMessage = await actionResult.ExecuteAsync(default);
            var uploadResponseModel = await httpResponseMessage.Content.ReadAsAsync<UploadResponseViewModel>();

            Assert.AreSame(uploadModel.Profile.Qualities, uploadResponseModel.Profile.Qualities);
        }

        #endregion
    }
}