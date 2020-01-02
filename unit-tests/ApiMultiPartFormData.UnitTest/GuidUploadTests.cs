using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        [Test]
        public void UploadStudentWithChildIds_Returns_StudentWithChildIds()
        {
            var childIds = new LinkedList<Guid>();
            childIds.AddLast(Guid.NewGuid());
            childIds.AddLast(Guid.NewGuid());

            var logger = new Mock<IFormatterLogger>();
            logger.Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()));

            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var multipartFormContent = new MultipartFormDataContent("---wwww-wwww-wwww-boundary-----");

            var index = 0;
            foreach (var childId in childIds)
            {
                multipartFormContent.Add(new StringContent(childId.ToString("D"), Encoding.UTF8), $"{nameof(StudentViewModel.ChildIds)}[{index}]");
                index++;
            }

            var uploadedModel = multipartFormDataFormatter
                .ReadFromStreamAsync(typeof(StudentViewModel), new MemoryStream(),
                multipartFormContent, logger.Object)
                .Result;

            if (!(uploadedModel is StudentViewModel student))
            {
                Assert.IsInstanceOf<StudentViewModel>(uploadedModel);
                return;
            }

            for (var childId = 0; childId < childIds.Count; childId++)
                Assert.AreEqual(childIds.ElementAt(childId), student.ChildIds[childId]);
        }
    }
}