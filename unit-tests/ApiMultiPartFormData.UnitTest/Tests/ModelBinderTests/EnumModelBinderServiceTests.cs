using System.Threading.Tasks;
using ApiMultiPartFormData.Services.Implementations;
using ApiMultiPartFormData.UnitTest.Enums;
using NUnit.Framework;

namespace ApiMultiPartFormData.UnitTest.Tests.ModelBinderTests
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
        public async Task HandleNonNullableEnumWithInvalidIntegerValue_Returns_DefaultValue()
        {
            var enumModelBinderService = new EnumModelBinderService();
            var invalidValue = "-1";

            var handledValue = await enumModelBinderService.BuildModelAsync(typeof(StudentTypes), invalidValue);
            Assert.AreEqual(default(StudentTypes),  (StudentTypes) handledValue);
        }

        [Test]
        public async Task HandleNonNullableEnumWithInvalidStringValue_Returns_DefaultValue()
        {
            var enumModelBinderService = new EnumModelBinderService();
            var invalidValue = "Invalid string";

            var handledValue = await enumModelBinderService.BuildModelAsync(typeof(StudentTypes), invalidValue);
            Assert.AreEqual(default(StudentTypes), (StudentTypes)handledValue);
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
