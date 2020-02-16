#if NETFRAMEWORK
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using ApiMultiPartFormData.UnitTest.Enums;
using ApiMultiPartFormData.UnitTest.ViewModels;
using Moq;
using NUnit.Framework;

namespace ApiMultiPartFormData.UnitTest.Tests.UploadTests
{
    [TestFixture]
    public class EnumUploadTests
    {
        [Test]
        public void UploadStudentWithTypeEnumAsInteger_Returns_StudentWithTypeEnum()
        {
            var logger = new Mock<IFormatterLogger>();
            logger.Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()));

            var goodStudentType = StudentTypes.Good;

            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var multipartFormContent = new MultipartFormDataContent("---wwww-wwww-wwww-boundary-----");
            multipartFormContent.Add(new StringContent($"{(int)goodStudentType}", Encoding.UTF8), $"{nameof(StudentViewModel.Type)}");

            var uploadedModel = multipartFormDataFormatter
                .ReadFromStreamAsync(typeof(StudentViewModel), new MemoryStream(),
                    multipartFormContent, logger.Object)
                .Result;

            if (!(uploadedModel is StudentViewModel student))
            {
                Assert.IsInstanceOf<StudentViewModel>(uploadedModel);
                return;
            }

            Assert.AreEqual(student.Type, goodStudentType);
        }

        [Test]
        public void UploadStudentTypeWithEnumAsText_Returns_StudentWithTypeEnum()
        {
            var logger = new Mock<IFormatterLogger>();
            logger.Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()));

            var goodStudentType = StudentTypes.Good;

            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var multipartFormContent = new MultipartFormDataContent("---wwww-wwww-wwww-boundary-----");
            multipartFormContent.Add(new StringContent(Enum.GetName(typeof(StudentTypes), goodStudentType), Encoding.UTF8), $"{nameof(StudentViewModel.Type)}");

            var uploadedModel = multipartFormDataFormatter
                .ReadFromStreamAsync(typeof(StudentViewModel), new MemoryStream(),
                    multipartFormContent, logger.Object)
                .Result;

            if (!(uploadedModel is StudentViewModel student))
            {
                Assert.IsInstanceOf<StudentViewModel>(uploadedModel);
                return;
            }

            Assert.AreEqual(student.Type, goodStudentType);
        }

        [Test]
        public void UploadStudentTypeWithNullEnumText_Returns_StudentWithNullEnum()
        {
            var logger = new Mock<IFormatterLogger>();
            logger.Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()));

            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var multipartFormContent = new MultipartFormDataContent("---wwww-wwww-wwww-boundary-----");

            var uploadedModel = multipartFormDataFormatter
                .ReadFromStreamAsync(typeof(StudentViewModel), new MemoryStream(),
                    multipartFormContent, logger.Object)
                .Result;

            if (!(uploadedModel is StudentViewModel student))
            {
                Assert.IsInstanceOf<StudentViewModel>(uploadedModel);
                return;
            }

            Assert.IsNull(student.NullableStudentType);
        }

        [Test]
        public void UploadStudentTypeWithEnumText_Returns_StudentWithEnum()
        {
            var logger = new Mock<IFormatterLogger>();
            logger.Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()));

            var goodStudentType = StudentTypes.Good;

            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var multipartFormContent = new MultipartFormDataContent("---wwww-wwww-wwww-boundary-----");
            multipartFormContent.Add(new StringContent(Enum.GetName(typeof(StudentTypes), goodStudentType), Encoding.UTF8), $"{nameof(StudentViewModel.NullableStudentType)}");

            var uploadedModel = multipartFormDataFormatter
                .ReadFromStreamAsync(typeof(StudentViewModel), new MemoryStream(),
                    multipartFormContent, logger.Object)
                .Result;

            if (!(uploadedModel is StudentViewModel student))
            {
                Assert.IsInstanceOf<StudentViewModel>(uploadedModel);
                return;
            }

            Assert.AreEqual(student.NullableStudentType, goodStudentType);
        }
    }
}
#endif