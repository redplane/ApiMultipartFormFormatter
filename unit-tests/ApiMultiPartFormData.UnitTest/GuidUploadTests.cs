using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using ApiMultiPartFormData.UnitTest.ViewModels;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ApiMultiPartFormData.UnitTest
{
    [TestFixture]
    public class GuidUploadTests
    {
        [Test]
        public void UploadStudentWithId_Returns_StudentWithId()
        {
            var studentId = Guid.NewGuid().ToString("D");
            var logger = new Mock<IFormatterLogger>();
            logger.Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()));

            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var multipartFormContent = new MultipartFormDataContent("---wwww-wwww-wwww-boundary-----");

            multipartFormContent.Add(new StringContent(studentId, Encoding.UTF8), nameof(StudentViewModel.Id));

            var uploadedModel = multipartFormDataFormatter
                .ReadFromStreamAsync(typeof(StudentViewModel), new MemoryStream(),
                    multipartFormContent, logger.Object)
                .Result;

            if (!(uploadedModel is StudentViewModel student))
            {
                Assert.IsInstanceOf<StudentViewModel>(uploadedModel);
                return;
            }

            Assert.AreEqual(student.Id.ToString("D"), studentId);
        }

        [Test]
        public void UploadStudentWithoutId_Returns_StudentWithGuidEmptyId()
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

            Assert.AreEqual(student.Id, Guid.Empty);
        }

        [Test]
        public void UploadStudentWithoutParentId_Returns_StudentWithNullParentId()
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

            Assert.AreEqual(student.ParentId, null);
        }

        [Test]
        public void UploadStudentWithParentId_Returns_StudentWithParentId()
        {
            var parentId = Guid.NewGuid().ToString("D");

            var logger = new Mock<IFormatterLogger>();
            logger.Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()));

            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var multipartFormContent = new MultipartFormDataContent("---wwww-wwww-wwww-boundary-----");
            multipartFormContent.Add(new StringContent(parentId, Encoding.UTF8), nameof(StudentViewModel.ParentId));
            var uploadedModel = multipartFormDataFormatter
                .ReadFromStreamAsync(typeof(StudentViewModel), new MemoryStream(),
                    multipartFormContent, logger.Object)
                .Result;

            if (!(uploadedModel is StudentViewModel student))
            {
                Assert.IsInstanceOf<StudentViewModel>(uploadedModel);
                return;
            }

            Assert.AreEqual(student.ParentId?.ToString("D"), parentId);
        }
    }
}