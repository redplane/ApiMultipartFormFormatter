using System;
using ApiMultiPartFormData.Exceptions;
using ApiMultiPartFormData.Services.Implementations;
using NUnit.Framework;

namespace ApiMultiPartFormData.UnitTest.Tests.ModelBinderTests
{
    [TestFixture]
    public class GuidModelBinderServiceTests
    {
        #region Methods

        [Test]
        public void HandleValidGuid_Returns_Guid()
        {
            var guidModelBinderService = new GuidModelBinderService();
            var guid = Guid.NewGuid();
            var result = guidModelBinderService.BuildModelAsync(typeof(Guid), guid.ToString("D"))
                .Result;

            Assert.AreEqual(guid, result);
        }

        [Test]
        public void HandleInvalidGuid_Throws_UnhandledParameterException()
        {
            var guidModelBinderService = new GuidModelBinderService();
            var guid = "this is a string";

            Assert.CatchAsync<UnhandledParameterException>(async () => await guidModelBinderService.BuildModelAsync(typeof(Guid), guid));
        }

        [Test]
        public void HandleEmptyGuid_Returns_EmptyGuid()
        {
            var guidModelBinderService = new GuidModelBinderService();
            var guid = Guid.Empty;

            var result = guidModelBinderService.BuildModelAsync(typeof(Guid), guid.ToString("D"))
                .Result;

            Assert.AreEqual(guid, result);
        }


        [Test]
        public void HandleNullableGuidWithEmptyValue_Returns_Null()
        {
            var guidModelBinderService = new GuidModelBinderService();
            var result = guidModelBinderService.BuildModelAsync(typeof(Guid?), "").Result;
            Assert.IsNull(result);
        }

        [Test]
        public void HandleNullableGuidWithInvalidValue_Returns_Null()
        {
            var guidModelBinderService = new GuidModelBinderService();
            var result = guidModelBinderService.BuildModelAsync(typeof(Guid?), "this is a string").Result;
            Assert.IsNull(result);
        }

        [Test]
        public void HandleNullableGuidWithEmptyValue_Returns_GuidEmpty()
        {
            var guidModelBinderService = new GuidModelBinderService();
            var emptyGuid = Guid.Empty;

            var result = guidModelBinderService.BuildModelAsync(typeof(Guid?), emptyGuid.ToString("D")).Result;
            Assert.AreEqual(emptyGuid, result);
        }

        [Test]
        public void HandleNullableGuidWithValidValue_Returns_GuidValue()
        {
            var guidModelBinderService = new GuidModelBinderService();
            var guid = Guid.NewGuid();

            var result = guidModelBinderService.BuildModelAsync(typeof(Guid?), guid.ToString("D")).Result;
            Assert.AreEqual(guid, result);
        }

        #endregion
    }
}
