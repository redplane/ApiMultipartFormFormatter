
using ApiMultiPartFormData.UnitTest.Tests.Interfaces;
#if NETCOREAPP
using ApiMultiPartFormData.UnitTest.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ApiMultiPartFormData.UnitTest.ViewModels;
using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Formatters;
using Moq;
using NUnit.Framework;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ApiMultiPartFormData.UnitTest.Tests.UploadTests
{
    [TestFixture]
    public partial class AttachmentsUploadTests : IAttachmentsUploadTests
    {
        #region Properties

        private IContainer _container;

        #endregion

        #region Installations

        [SetUp]
        public void Setup()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<MultipartFormDataFormatter>();

            
            _container = containerBuilder.Build();
        }

        [TearDown]
        public void Uninstall()
        {
            _container.Dispose();
        }

        #endregion

        #region Methods

        [Test]
        public async Task UploadAttachmentsIntoStudent_Returns_StudentWithAttachments()
        {
            var multipartFormDataFormatter = _container.Resolve<MultipartFormDataFormatter>();
            var formFileCollection = new FormFileCollection();
            var applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var attachmentPath = Path.Combine(applicationPath, "Data", "grapefruit-slice-332-332.jpg");
            var data = File.ReadAllBytes(attachmentPath);

            for (var index = 0; index < 3; index++)
            {
                var fileName = $"{Guid.NewGuid().ToString("D")}.jpg";
                var attachment = new FormFile(new MemoryStream(data), 0, data.Length,
                $"{nameof(StudentViewModel.Photos)}[{index}]", fileName);
                attachment.Headers = new HeaderDictionary();
                attachment.ContentType = "image/jpeg";
                formFileCollection.Add(attachment);
            }

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
                .ReadAsync(inputFormatter);

            Assert.IsInstanceOf<InputFormatterResult>(handledResult);

            var student = handledResult.Model as StudentViewModel;
            Assert.NotNull(student?.Photos);
            Assert.AreEqual(formFileCollection.Count, student.Photos.Count);

            for (var i = 0; i < formFileCollection.Count; i++)
            {
                Assert.AreEqual(formFileCollection[i].FileName, student.Photos[i].FileName);
                Assert.AreEqual(formFileCollection[i].ContentType, student.Photos[i].ContentType);
                Assert.AreEqual(formFileCollection[i].Length, student.Photos[i].ContentLength);
            }

        }

        [Test]
        public async Task UploadPhotosIntoStudentProfile_Returns_StudentWithProfilePhotos()
        {
            var multipartFormDataFormatter = _container.Resolve<MultipartFormDataFormatter>();
            var formFileCollection = new FormFileCollection();
            var applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var attachmentPath = Path.Combine(applicationPath, "Data", "grapefruit-slice-332-332.jpg");
            var data = File.ReadAllBytes(attachmentPath);

            for (var index = 0; index < 3; index++)
            {
                var fileName = $"{Guid.NewGuid().ToString("D")}.jpg";
                var attachment = new FormFile(new MemoryStream(data), 0, data.Length,
                $"{nameof(StudentViewModel.Profile)}[{nameof(StudentViewModel.Profile.Photos)}][{index}]", fileName);
                attachment.Headers = new HeaderDictionary();
                attachment.ContentType = "image/jpeg";
                formFileCollection.Add(attachment);
            }

            var httpContextMock = new Mock<HttpContext>();
            var httpRequestMock = new Mock<HttpRequest>();
            var formCollection = new FormCollection(new Dictionary<string, StringValues>(), formFileCollection);
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
                .ReadAsync(inputFormatter);

            Assert.IsInstanceOf<InputFormatterResult>(handledResult);

            var student = handledResult.Model as StudentViewModel;
            Assert.NotNull(student?.Profile?.Photos);
            Assert.AreEqual(formFileCollection.Count, student.Profile.Photos.Count);

            for (var i = 0; i < student.Profile.Photos.Count; i++)
                Assert.AreEqual(formFileCollection[i].FileName, student.Profile.Photos[i].FileName);
        }

        [Test]
        public async Task UploadPhotosIntoStudentProfiles_Returns_StudentWithProfilesPhotos()
        {
            var multipartFormDataFormatter = _container.Resolve<MultipartFormDataFormatter>();
            var applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var attachmentPath = Path.Combine(applicationPath, "Data", "grapefruit-slice-332-332.jpg");
            var data = File.ReadAllBytes(attachmentPath);

            var fileNames = new List<string>();
            fileNames.Add($"{Guid.NewGuid():D}.jpg");
            fileNames.Add($"{Guid.NewGuid():D}.jpg");
            fileNames.Add($"{Guid.NewGuid():D}.jpg");

            const int profileNumbers = 2;
            
            var models = new Dictionary<string, StringValues>();
            var attachments = new FormFileCollection();

            for (var profileId = 0; profileId < profileNumbers; profileId++)
            {
                var profileParam = $"{nameof(StudentViewModel.Profiles)}[{profileId}]";
                models.Add($"{profileParam}[{nameof(ProfileViewModel.Name)}]", new StringValues($"{profileId}"));

                for (var attachmentId = 0; attachmentId < fileNames.Count; attachmentId++)
                {
                    var content = new ByteArrayContent(data);
                    content.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                    var profilePhotoParam = $"{profileParam}[{nameof(ProfileViewModel.Photos)}][{attachmentId}]";

                    var attachment = new FormFile(new MemoryStream(data), 0, data.Length,
                    profilePhotoParam, fileNames[attachmentId]);
                    attachment.Headers = new HeaderDictionary();
                    attachment.ContentType = "image/jpeg";
                    attachments.Add(attachment);

                    
                }

            }

            var httpContextMock = new Mock<HttpContext>();
            var httpRequestMock = new Mock<HttpRequest>();

            httpRequestMock
                .Setup(x => x.ReadFormAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FormCollection(models, attachments));

            httpContextMock.SetupGet(x => x.Request)
                .Returns(httpRequestMock.Object);

            var inputFormatter = new InputFormatterContext(httpContextMock.Object, string.Empty,
            new ModelStateDictionary(), new EmptyModelMetaData(ModelMetadataIdentity.ForType(typeof(StudentViewModel))),
            (stream, encoding) => TextReader.Null);

            var handledResult = await multipartFormDataFormatter
                .ReadRequestBodyAsync(inputFormatter);

            Assert.IsInstanceOf<InputFormatterResult>(handledResult);
            var student = (StudentViewModel) handledResult.Model;

            Assert.NotNull(student);
            Assert.NotNull(student.Profiles);
            Assert.AreEqual(profileNumbers, student.Profiles.Count);

            for (var profileId = 0; profileId < profileNumbers; profileId++)
            {
                Assert.NotNull(student.Profiles[profileId]);
                Assert.AreEqual(fileNames.Count, student.Profiles[profileId].Photos.Count);

                for (var i = 0; i < fileNames.Count; i++)
                    Assert.AreEqual(fileNames[i], student.Profiles[profileId].Photos[i].FileName);
            }
        }
        #endregion
    }
}
#endif