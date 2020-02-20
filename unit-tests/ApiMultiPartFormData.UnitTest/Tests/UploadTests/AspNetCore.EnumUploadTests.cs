#if NETCOREAPP
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using ApiMultiPartFormData.UnitTest.Enums;
using ApiMultiPartFormData.UnitTest.Models;
using ApiMultiPartFormData.UnitTest.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;

using System.Threading.Tasks;
using Moq;

namespace ApiMultiPartFormData.UnitTest.Tests.UploadTests
{
    [TestFixture]
    public class EnumUploadTests
    {
        [Test]
        public async Task UploadStudentTypeWithEnumAsText_Returns_StudentWithTypeEnum()
        {
            var goodStudentType = StudentTypes.Good;
            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var formFileCollection = new FormFileCollection();

            var models = new Dictionary<string, StringValues>();
            models.Add(nameof(StudentViewModel.Type), Enum.GetName(typeof(StudentTypes), goodStudentType));

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
            Assert.AreEqual(goodStudentType, student.Type);
        }

        [Test]
        public async Task UploadStudentTypeWithEnumText_Returns_StudentWithEnum()
        {
            var goodStudentType = StudentTypes.Good;
            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var formFileCollection = new FormFileCollection();

            var models = new Dictionary<string, StringValues>();
            models.Add(nameof(StudentViewModel.NullableStudentType), Enum.GetName(typeof(StudentTypes), goodStudentType));

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
            Assert.AreEqual(goodStudentType, student.NullableStudentType);
        }

        [Test]
        public async Task UploadStudentTypeWithNullEnumText_Returns_StudentWithNullEnum()
        {
            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var formFileCollection = new FormFileCollection();

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
            Assert.IsNull(student.NullableStudentType);
        }
        [Test]
        public async Task UploadStudentWithTypeEnumAsInteger_Returns_StudentWithTypeEnum()
        {
            var goodStudentType = StudentTypes.Good;
            var multipartFormDataFormatter = new MultipartFormDataFormatter();
            var formFileCollection = new FormFileCollection();

            var models = new Dictionary<string, StringValues>();
            models.Add(nameof(StudentViewModel.Type), $"{(int)goodStudentType}");

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
            Assert.AreEqual(goodStudentType, student.Type);
        }
    }
}
#endif
