
using ApiMultiPartFormData.UnitTest.Tests.Interfaces;
#if NETCOREAPP
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using ApiMultiPartFormData.UnitTest.ViewModels;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using ApiMultiPartFormData.UnitTest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Primitives;

namespace ApiMultiPartFormData.UnitTest.Tests.UploadTests
{
    [TestFixture]
    public class AttachmentUploadTests : IAttachmentUploadTests
    {
        [Test]
        public async Task UploadStudentWithName_Returns_StudentWithFullName()
        {
            var studentName = "Student-001";

            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var formFileCollection = new FormFileCollection();

            var models = new Dictionary<string, StringValues>();
            models.Add($"{nameof(StudentViewModel.FullName)}", studentName);

            var formCollection = new FormCollection(new Dictionary<string, StringValues>(), formFileCollection);
            var httpContextMock = new Mock<HttpContext>();
            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock.Setup(x => x.Form)
                .Returns(formCollection);

            httpRequestMock.Setup(x => x.ReadFormAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(formCollection);

            httpContextMock.SetupGet(x => x.Request)
                .Returns(httpRequestMock.Object);

            var inputFormatter = new InputFormatterContext(httpContextMock.Object, string.Empty,
                new ModelStateDictionary(), new EmptyModelMetaData(ModelMetadataIdentity.ForType(typeof(StudentViewModel))),
                (stream, encoding) => TextReader.Null);

            var handledResult = await multipartFormDataFormatter
                .ReadRequestBodyAsync(inputFormatter);

            Assert.IsInstanceOf<InputFormatterResult>(handledResult);

            var student = handledResult.Model as StudentViewModel;
            Assert.NotNull(student);
            Assert.AreEqual(student.FullName, student.FullName);
        }

        [Test]
        public async Task UploadStudentWithPhoto_Returns_StudentWithPhoto()
        {
            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var formFileCollection = new FormFileCollection();

            var applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var attachmentPath = Path.Combine(applicationPath, "Data", "grapefruit-slice-332-332.jpg");
            var data = File.ReadAllBytes(attachmentPath);

            var fileName = $"{Guid.NewGuid().ToString("D")}.jpg";
            var attachment = new FormFile(new MemoryStream(data), 0, data.Length,
            $"{nameof(StudentViewModel.Photo)}", fileName);
            formFileCollection.Add(attachment);
            attachment.Headers = new HeaderDictionary();
            attachment.ContentType = "image/jpeg";

            var formCollection = new FormCollection(new Dictionary<string, StringValues>(), formFileCollection);
            var httpContextMock = new Mock<HttpContext>();
            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock.Setup(x => x.Form)
                .Returns(formCollection);

            httpRequestMock.Setup(x => x.ReadFormAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(formCollection);

            httpContextMock.SetupGet(x => x.Request)
                .Returns(httpRequestMock.Object);

            var inputFormatter = new InputFormatterContext(httpContextMock.Object, string.Empty,
            new ModelStateDictionary(), new EmptyModelMetaData(ModelMetadataIdentity.ForType(typeof(StudentViewModel))),
            (stream, encoding) => TextReader.Null);

            var handledResult = await multipartFormDataFormatter
                .ReadRequestBodyAsync(inputFormatter);

            Assert.IsInstanceOf<InputFormatterResult>(handledResult);

            var student = handledResult.Model as StudentViewModel;
            Assert.NotNull(student);
            Assert.NotNull(student.Photo);
            Assert.AreEqual(attachment.FileName, student.Photo.FileName);
            Assert.AreEqual(attachment.ContentType, student.Photo.ContentType);
            Assert.AreEqual(attachment.Length, student.Photo.ContentLength);

        }

        [Test]
        public async Task UploadStudentWithProfilePhoto_Returns_StudentWithProfilePhoto()
        {
            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var formFileCollection = new FormFileCollection();

            var applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var attachmentPath = Path.Combine(applicationPath, "Data", "grapefruit-slice-332-332.jpg");
            var data = File.ReadAllBytes(attachmentPath);

            var fileName = $"{Guid.NewGuid().ToString("D")}.jpg";
            var attachment = new FormFile(new MemoryStream(data), 0, data.Length,
            $"{nameof(StudentViewModel.Profile)}[{nameof(ProfileViewModel.Attachment)}]", fileName);
            formFileCollection.Add(attachment);
            attachment.Headers = new HeaderDictionary();
            attachment.ContentType = "image/jpeg";

            var formCollection = new FormCollection(new Dictionary<string, StringValues>(), formFileCollection);
            var httpContextMock = new Mock<HttpContext>();
            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock.Setup(x => x.Form)
                .Returns(formCollection);

            httpRequestMock.Setup(x => x.ReadFormAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(formCollection);

            httpContextMock.SetupGet(x => x.Request)
                .Returns(httpRequestMock.Object);

            var inputFormatter = new InputFormatterContext(httpContextMock.Object, string.Empty,
            new ModelStateDictionary(), new EmptyModelMetaData(ModelMetadataIdentity.ForType(typeof(StudentViewModel))),
            (stream, encoding) => TextReader.Null);

            var handledResult = await multipartFormDataFormatter
                .ReadRequestBodyAsync(inputFormatter);

            Assert.IsInstanceOf<InputFormatterResult>(handledResult);

            var student = handledResult.Model as StudentViewModel;
            Assert.NotNull(student);
            Assert.NotNull(student.Profile);
            Assert.NotNull(student.Profile.Attachment);
            Assert.AreEqual(attachment.FileName, student.Profile.Attachment.FileName);
            Assert.AreEqual(attachment.ContentType, student.Profile.Attachment.ContentType);
            Assert.AreEqual(attachment.Length, student.Profile.Attachment.ContentLength);
        }

        [Test]
        public async Task UploadStudentWithProfilePhoto_Returns_ProfilePhotoIsValidStream()
        {
            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var formFileCollection = new FormFileCollection();

            var applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var attachmentPath = Path.Combine(applicationPath, "Data", "grapefruit-slice-332-332.jpg");
            var data = File.ReadAllBytes(attachmentPath);

            var fileName = $"{Guid.NewGuid().ToString("D")}.jpg";
            var attachment = new FormFile(new MemoryStream(data), 0, data.Length,
            $"{nameof(StudentViewModel.Profile)}[{nameof(ProfileViewModel.Attachment)}]", fileName);
            formFileCollection.Add(attachment);
            attachment.Headers = new HeaderDictionary();
            attachment.ContentType = "image/jpeg";

            var formCollection = new FormCollection(new Dictionary<string, StringValues>(), formFileCollection);
            var httpContextMock = new Mock<HttpContext>();
            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock.Setup(x => x.Form)
                .Returns(formCollection);

            httpRequestMock.Setup(x => x.ReadFormAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(formCollection);

            httpContextMock.SetupGet(x => x.Request)
                .Returns(httpRequestMock.Object);

            var inputFormatter = new InputFormatterContext(httpContextMock.Object, string.Empty,
            new ModelStateDictionary(), new EmptyModelMetaData(ModelMetadataIdentity.ForType(typeof(StudentViewModel))),
            (stream, encoding) => TextReader.Null);

            var handledResult = await multipartFormDataFormatter
                .ReadRequestBodyAsync(inputFormatter);

            Assert.IsInstanceOf<InputFormatterResult>(handledResult);

            var student = handledResult.Model as StudentViewModel;
            Assert.NotNull(student);
            Assert.NotNull(student.Profile);
            Assert.NotNull(student.Profile.Attachment);
            Assert.AreEqual(data.Length, student.Profile.Attachment.InputStream.Length);
        }

        [Test]
        public async Task UploadStudentWithBufferedAttachment_Returns_StudentWithBufferedAttachment()
        {
            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var formFileCollection = new FormFileCollection();

            var applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var attachmentPath = Path.Combine(applicationPath, "Data", "grapefruit-slice-332-332.jpg");
            var data = File.ReadAllBytes(attachmentPath);

            var fileName = $"{Guid.NewGuid().ToString("D")}.jpg";
            var attachment = new FormFile(new MemoryStream(data), 0, data.Length,
            $"{nameof(StudentViewModel.Profile)}[{nameof(ProfileViewModel.BufferedAttachment)}]", fileName);
            formFileCollection.Add(attachment);
            attachment.Headers = new HeaderDictionary();
            attachment.ContentType = "image/jpeg";

            var formCollection = new FormCollection(new Dictionary<string, StringValues>(), formFileCollection);
            var httpContextMock = new Mock<HttpContext>();
            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock.Setup(x => x.Form)
                .Returns(formCollection);

            httpRequestMock.Setup(x => x.ReadFormAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(formCollection);

            httpContextMock.SetupGet(x => x.Request)
                .Returns(httpRequestMock.Object);

            var inputFormatter = new InputFormatterContext(httpContextMock.Object, string.Empty,
            new ModelStateDictionary(), new EmptyModelMetaData(ModelMetadataIdentity.ForType(typeof(StudentViewModel))),
            (stream, encoding) => TextReader.Null);

            var handledResult = await multipartFormDataFormatter
                .ReadRequestBodyAsync(inputFormatter);

            Assert.IsInstanceOf<InputFormatterResult>(handledResult);

            var student = handledResult.Model as StudentViewModel;
            Assert.NotNull(student);
            Assert.NotNull(student.Profile);
            Assert.NotNull(student.Profile.BufferedAttachment);
            Assert.AreEqual(attachment.FileName, student.Profile.BufferedAttachment.FileName);
            Assert.AreEqual(attachment.ContentType, student.Profile.BufferedAttachment.ContentType);
            Assert.AreEqual(attachment.Length, student.Profile.BufferedAttachment.ContentLength);
        }
    }
}
#endif