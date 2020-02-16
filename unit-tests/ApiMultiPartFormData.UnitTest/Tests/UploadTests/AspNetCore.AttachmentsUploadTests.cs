#if NETCOREAPP__
using ApiMultiPartFormData.UnitTest.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
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

namespace ApiMultiPartFormData.UnitTest.Tests.UploadTests
{
    [TestFixture]
    public partial class AttachmentsUploadTests : IAttachmentUploadTests
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
            try
            {
                var multipartFormDataFormatter = _container.Resolve<MultipartFormDataFormatter>();
                var applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var attachmentPath = Path.Combine(applicationPath, "Data", "grapefruit-slice-332-332.jpg");
                var data = File.ReadAllBytes(attachmentPath);

                var formFileCollection = new FormFileCollection();

                var fileNames = new List<string>();
                fileNames.Add($"{Guid.NewGuid():D}.jpg");
                fileNames.Add($"{Guid.NewGuid():D}.jpg");
                fileNames.Add($"{Guid.NewGuid():D}.jpg");

                for (var i = 0; i < fileNames.Count; i++)
                {
                    var formFile = new FormFile(new MemoryStream(data), 0, data.Length,
                        $"{nameof(StudentViewModel.Photos)}[{i}]", fileNames[i]);
                    formFile.Headers = new HeaderDictionary();
                    formFile.ContentType = "image/jpg";
                    //formFile.ContentDisposition = $"{nameof(StudentViewModel.Photo)}";

                    formFileCollection.Add(formFile);
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

                Assert.IsInstanceOf<StudentViewModel>(handledResult);

                var student = handledResult.Model as StudentViewModel;
                Assert.NotNull(student?.Photos);
                Assert.AreEqual(fileNames.Count, student.Photos.Count);

                for (var i = 0; i < fileNames.Count; i++)
                    Assert.AreEqual(fileNames[i], student.Photos[i].FileName);
            }
            catch (Exception e)
            {
                var a = 1;
            }
        }

        [Test]
        public async Task UploadPhotosIntoStudentProfile_Returns_StudentWithProfilePhotos()
        {
            //var logger = _container.Resolve<IFormatterLogger>();
            //var multipartFormDataFormatter = _container.Resolve<MultipartFormDataFormatter>();
            //var applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //var attachmentPath = Path.Combine(applicationPath, "Data", "grapefruit-slice-332-332.jpg");
            //var data = File.ReadAllBytes(attachmentPath);

            //var multipartFormContent = new MultipartFormDataContent($"---www-{DateTime.Now:yy-MM-dd}-www---");
            //var fileNames = new List<string>();
            //fileNames.Add($"{Guid.NewGuid():D}.jpg");
            //fileNames.Add($"{Guid.NewGuid():D}.jpg");
            //fileNames.Add($"{Guid.NewGuid():D}.jpg");

            //for (var i = 0; i < fileNames.Count; i++)
            //{
            //    var content = new ByteArrayContent(data);
            //    content.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpg");
            //    multipartFormContent.Add(content, $"{nameof(StudentViewModel.Profile)}[{nameof(ProfileViewModel.Photos)}][{i}]", fileNames[i]);
            //}

            //var handledResult = await multipartFormDataFormatter
            //    .ReadFromStreamAsync(typeof(StudentViewModel), new MemoryStream(), multipartFormContent, logger);

            //Assert.IsInstanceOf<StudentViewModel>(handledResult);

            //var student = handledResult as StudentViewModel;
            //Assert.NotNull(student?.Profile);
            //Assert.AreEqual(fileNames.Count, student.Profile.Photos.Count);

            //for (var i = 0; i < fileNames.Count; i++)
            //    Assert.AreEqual(fileNames[i], student.Profile.Photos[i].FileName);
            throw new NotImplementedException();
        }

        [Test]
        public async Task UploadPhotosIntoStudentProfiles_Returns_StudentWithProfilesPhotos()
        {
            //var logger = _container.Resolve<IFormatterLogger>();
            //var multipartFormDataFormatter = _container.Resolve<MultipartFormDataFormatter>();
            //var applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //var attachmentPath = Path.Combine(applicationPath, "Data", "grapefruit-slice-332-332.jpg");
            //var data = File.ReadAllBytes(attachmentPath);

            //var multipartFormContent = new MultipartFormDataContent($"---www-{DateTime.Now:yy-MM-dd}-www---");
            //var fileNames = new List<string>();
            //fileNames.Add($"{Guid.NewGuid():D}.jpg");
            //fileNames.Add($"{Guid.NewGuid():D}.jpg");
            //fileNames.Add($"{Guid.NewGuid():D}.jpg");

            //const int profileNumbers = 2;

            //for (var profileId = 0; profileId < profileNumbers; profileId++)
            //{
            //    for (var i = 0; i < fileNames.Count; i++)
            //    {
            //        var content = new ByteArrayContent(data);
            //        content.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpg");

            //        // Param: profiles[0][photos][0]
            //        var multipartParam = $"{nameof(StudentViewModel.Profiles)}[{profileId}][{nameof(ProfileViewModel.Photos)}][{i}]";
            //        Debug.WriteLine(multipartParam);
            //        multipartFormContent
            //            .Add(content, multipartParam, fileNames[i]);
            //    }

            //}

            //var handledResult = await multipartFormDataFormatter
            //    .ReadFromStreamAsync(typeof(StudentViewModel), new MemoryStream(), multipartFormContent, logger);
            //Assert.IsInstanceOf<StudentViewModel>(handledResult);

            //var student = handledResult as StudentViewModel;
            //Assert.NotNull(student?.Profiles);

            //for (var profileId = 0; profileId < profileNumbers; profileId++)
            //{
            //    Assert.NotNull(student.Profiles[profileId]);
            //    Assert.AreEqual(fileNames.Count, student.Profiles[profileId].Photos.Count);

            //    for (var i = 0; i < fileNames.Count; i++)
            //        Assert.AreEqual(fileNames[i], student.Profiles[profileId].Photos[i].FileName);
            //}
            throw new NotImplementedException();
        }
        #endregion
    }
}
#endif