#if NETCOREAPP
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ApiMultiPartFormData.UnitTest.ViewModels;
using Moq;
using NUnit.Framework;
using System.Threading;
using ApiMultiPartFormData.UnitTest.Enums;
using ApiMultiPartFormData.UnitTest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Primitives;
using ApiMultiPartFormData.UnitTest.Tests.Interfaces;

namespace ApiMultiPartFormData.UnitTest.Tests.UploadTests
{
    [TestFixture]
    public class GuidUploadTests : IGuidUploadTests
    {
        [Test]
        public async Task UploadStudentWithId_Returns_StudentWithId()
        {
            var studentId = Guid.NewGuid().ToString("D");

            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var formFileCollection = new FormFileCollection();

            var models = new Dictionary<string, StringValues>();
            models.Add(nameof(StudentViewModel.Id), studentId);

            var formCollection = new FormCollection(models, formFileCollection);
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
            Assert.AreEqual(studentId, student.Id.ToString("D"));
        }

        [Test]
        public async Task UploadStudentWithoutId_Returns_StudentWithGuidEmptyId()
        {

            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var formFileCollection = new FormFileCollection();

            var models = new Dictionary<string, StringValues>();

            var formCollection = new FormCollection(models, formFileCollection);
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
            Assert.AreEqual(Guid.Empty, student.Id);
        }

        [Test]
        public async Task UploadStudentWithoutParentId_Returns_StudentWithNullParentId()
        {
            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var formFileCollection = new FormFileCollection();

            var models = new Dictionary<string, StringValues>();

            var formCollection = new FormCollection(models, formFileCollection);
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
            Assert.IsNull(student.ParentId);
        }

        [Test]
        public async Task UploadStudentWithParentId_Returns_StudentWithParentId()
        {
            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var formFileCollection = new FormFileCollection();

            var parentId = Guid.NewGuid().ToString("D");
            var models = new Dictionary<string, StringValues>();
            models.Add($"{nameof(StudentViewModel.ParentId)}", parentId);

            var formCollection = new FormCollection(models, formFileCollection);
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
            Assert.AreEqual(parentId, student.ParentId?.ToString("D"));
        }

        [Test]
        public async Task UploadStudentWithChildIds_Returns_StudentWithChildIds()
        {
            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var formFileCollection = new FormFileCollection();

            var childIds = Enumerable.Range(1, 3)
                .Select(x => Guid.NewGuid())
                .ToList();

            var models = new Dictionary<string, StringValues>();

            for (var index = 0; index < childIds.Count; index++)
                models.Add($"{nameof(StudentViewModel.ChildIds)}[{index}]", childIds[index].ToString("D"));

            var formCollection = new FormCollection(models, formFileCollection);
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

            for (var id = 0; id < childIds.Count; id++)
                Assert.AreEqual(childIds[id].ToString("D"), student.ChildIds[id].ToString("D"));
        }

        [Test]
        public async Task UploadIdIntoProfile_Returns_StudentProfileWithId()
        {
            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var formFileCollection = new FormFileCollection();

            var id = Guid.NewGuid().ToString("D");

            var models = new Dictionary<string, StringValues>();
            models.Add($"{nameof(StudentViewModel.Profile)}[{nameof(ProfileViewModel.Id)}]", id);

            var formCollection = new FormCollection(models, formFileCollection);
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
            Assert.IsNotNull(student?.Profile);
            Assert.AreEqual(id, student.Profile.Id.ToString("D"));
        }

        [Test]
        public async Task UploadBlankIdIntoProfile_Returns_StudentProfileWithId()
        {
            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var formFileCollection = new FormFileCollection();

            var id = Guid.Empty.ToString("D");

            var models = new Dictionary<string, StringValues>();
            models.Add($"{nameof(StudentViewModel.Profile)}[{nameof(ProfileViewModel.Id)}]", id);

            var formCollection = new FormCollection(models, formFileCollection);
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
            Assert.IsNotNull(student?.Profile);
            Assert.AreEqual(id, student.Profile.Id.ToString("D"));
        }

        [Test]
        public async Task NoUploadIntoProfile_Returns_StudentNullProfile()
        {
            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var formFileCollection = new FormFileCollection();

            var models = new Dictionary<string, StringValues>();

            var formCollection = new FormCollection(models, formFileCollection);
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
            Assert.IsNull(student?.Profile);
        }

        [Test]
        public async Task UploadWithNestedRelativeIds_Returns_StudentProfileWithRelativeIds()
        {
            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var formFileCollection = new FormFileCollection();

            var models = new Dictionary<string, StringValues>();
            var relativeIds = Enumerable.Range(1, 3)
                .Select(x => Guid.NewGuid())
                .ToList();

            for (var id = 0; id < relativeIds.Count; id++)
            {
                models.Add($"{nameof(StudentViewModel.Profile)}[{nameof(ProfileViewModel.RelativeIds)}][{id}]", relativeIds[id].ToString("D"));
            }
            
            var formCollection = new FormCollection(models, formFileCollection);
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
            Assert.IsNotNull(student);

            for (var id = 0; id < relativeIds.Count; id++)
                Assert.AreEqual(relativeIds[id].ToString("D"), student.Profile.RelativeIds[id].ToString("D"));
        }
    }
}
#endif