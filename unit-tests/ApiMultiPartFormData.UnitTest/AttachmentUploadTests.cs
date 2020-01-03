using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using ApiMultiPartFormData.UnitTest.ViewModels;
using Moq;
using NUnit.Framework;

namespace ApiMultiPartFormData.UnitTest
{
    [TestFixture]
    public class AttachmentUploadTests
    {
        [Test]
        public void UploadStudentWithName_Returns_StudentWithFullName()
        {
            var studentName = "Student-001";
            var logger = new Mock<IFormatterLogger>();
            logger.Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()));

            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var multipartFormContent = new MultipartFormDataContent("---wwww-wwww-wwww-boundary-----");
            multipartFormContent.Add(new StringContent(studentName), nameof(StudentViewModel.FullName));
            var uploadedModel = multipartFormDataFormatter
                .ReadFromStreamAsync(typeof(StudentViewModel), new MemoryStream(),
                    multipartFormContent, logger.Object)
                .Result;

            Assert.IsInstanceOf<StudentViewModel>(uploadedModel);
            Assert.AreEqual((uploadedModel as StudentViewModel)?.FullName,studentName);
        }

        [Test]
        public void UploadStudentWithPhoto_Returns_StudentWithPhoto()
        {
            var fileName = $"{Guid.NewGuid():D}.jpg";
            var contentType = "image/jpeg";

            var logger = new Mock<IFormatterLogger>();
            logger.Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()));

            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var multipartFormContent = new MultipartFormDataContent("---wwww-wwww-wwww-boundary-----");

            var applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var attachmentPath = Path.Combine(applicationPath, "Data", "grapefruit-slice-332-332.jpg");
            var data = File.ReadAllBytes(attachmentPath);

            var attachment = new ByteArrayContent(data);
            attachment.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            multipartFormContent.Add(attachment, nameof(StudentViewModel.Photo), fileName);

            var uploadedModel = multipartFormDataFormatter
                .ReadFromStreamAsync(typeof(StudentViewModel), new MemoryStream(),
                    multipartFormContent, logger.Object)
                .Result;

            if (!(uploadedModel is StudentViewModel student))
            {
                Assert.IsInstanceOf<StudentViewModel>(uploadedModel);
                return;
            }

            Assert.AreEqual(student.Photo.ContentLength, data.Length);
            Assert.AreEqual(student.Photo.FileName, fileName);
            Assert.AreEqual(student.Photo.ContentType, contentType);

        }

        [Test]
        public void UploadStudentWithProfilePhoto_Returns_StudentWithProfilePhoto()
        {
            var fileName = $"{Guid.NewGuid():D}.jpg";
            var contentType = "image/jpeg";

            var logger = new Mock<IFormatterLogger>();
            logger.Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()));

            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var multipartFormContent = new MultipartFormDataContent("---wwww-wwww-wwww-boundary-----");

            var applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var attachmentPath = Path.Combine(applicationPath, "Data", "grapefruit-slice-332-332.jpg");
            var data = File.ReadAllBytes(attachmentPath);

            var attachment = new ByteArrayContent(data);
            attachment.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            multipartFormContent.Add(attachment, $"{nameof(StudentViewModel.Profile)}[{nameof(ProfileViewModel.Attachment)}]", fileName);

            var uploadedModel = multipartFormDataFormatter
                .ReadFromStreamAsync(typeof(StudentViewModel), new MemoryStream(),
                multipartFormContent, logger.Object)
                .Result;

            if (!(uploadedModel is StudentViewModel student))
            {
                Assert.IsInstanceOf<StudentViewModel>(uploadedModel);
                return;
            }

            Assert.NotNull(student.Profile);
            Assert.NotNull(student.Profile.Attachment);
            Assert.AreEqual(student.Profile.Attachment.ContentLength, data.Length);
            Assert.AreEqual(student.Profile.Attachment.FileName, fileName);
            Assert.AreEqual(student.Profile.Attachment.ContentType, contentType);

        }
    }
}
