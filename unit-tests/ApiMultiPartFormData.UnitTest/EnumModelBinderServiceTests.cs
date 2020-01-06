using ApiMultiPartFormData.Exceptions;
using ApiMultiPartFormData.Services.Implementations;
using ApiMultiPartFormData.UnitTest.Enums;
using NUnit.Framework;

namespace ApiMultiPartFormData.UnitTest
{
    [TestFixture]
    public class EnumModelBinderServiceTests
    {
        #region Methods

        [Test]
        public void HandleNonNullableEnumWithValidIntegerValue_Returns_ValidEnum()
        {
            var enumModelBinderService = new EnumModelBinderService();
            var goodString = $"{(int)StudentTypes.Good}";

            var handledResult = enumModelBinderService.BuildModelAsync(typeof(StudentTypes), goodString)
                .Result;

            Assert.AreEqual(StudentTypes.Good, handledResult);
        }

        [Test]
        public void HandleNonNullableEnumWithValidStringValue_Returns_ValidEnum()
        {
            var enumModelBinderService = new EnumModelBinderService();
            var goodString = $"{nameof(StudentTypes.Good)}";

            var handledResult = enumModelBinderService.BuildModelAsync(typeof(StudentTypes), goodString)
                .Result;

            Assert.AreEqual(StudentTypes.Good, handledResult);
        }

        [Test]
        public void HandleNonNullableEnumWithInvalidIntegerValue_Throws_UnhandledParameterException()
        {
            var enumModelBinderService = new EnumModelBinderService();
            var invalidValue = "-1";

            Assert.CatchAsync<UnhandledParameterException>(() => enumModelBinderService.BuildModelAsync(typeof(StudentTypes), invalidValue));
        }

        [Test]
        public void HandleNonNullableEnumWithInvalidStringValue_Throws_UnhandledParameterException()
        {
            var enumModelBinderService = new EnumModelBinderService();
            var invalidValue = "Invalid string";

            Assert.CatchAsync<UnhandledParameterException>(() => enumModelBinderService.BuildModelAsync(typeof(StudentTypes), invalidValue));
        }

        [Test]
        public void HandleNullableEnumWithValidIntegerValue_Returns_ValidEnum()
        {
            var enumModelBinderService = new EnumModelBinderService();
            var goodString = $"{(int)StudentTypes.Good}";

            var handledResult = enumModelBinderService.BuildModelAsync(typeof(StudentTypes?), goodString)
                .Result;

            Assert.AreEqual(StudentTypes.Good, handledResult);
        }

        [Test]
        public void HandleNullableEnumWithValidStringValue_Returns_ValidEnum()
        {
            var enumModelBinderService = new EnumModelBinderService();
            var goodString = $"{nameof(StudentTypes.Good)}";

            var handledResult = enumModelBinderService.BuildModelAsync(typeof(StudentTypes?), goodString)
                .Result;

            Assert.AreEqual(StudentTypes.Good, handledResult);
        }

        #endregion
    }
}
