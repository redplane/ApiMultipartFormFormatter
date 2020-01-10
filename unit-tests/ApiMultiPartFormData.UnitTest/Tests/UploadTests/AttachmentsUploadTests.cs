using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using ApiMultiPartFormData.UnitTest.ViewModels;
using Autofac;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ApiMultiPartFormData.UnitTest.Tests.UploadTests
{
    [TestFixture]
    public class AttachmentsUploadTests
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

            var logger = new Mock<IFormatterLogger>();
            logger.Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()));
            containerBuilder.RegisterInstance(logger.Object)
                .As<IFormatterLogger>()
                .SingleInstance();

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
            var logger = _container.Resolve<IFormatterLogger>();
            var multipartFormDataFormatter = _container.Resolve<MultipartFormDataFormatter>();
            var applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var attachmentPath = Path.Combine(applicationPath, "Data", "grapefruit-slice-332-332.jpg");
            var data = File.ReadAllBytes(attachmentPath);

            var multipartFormContent = new MultipartFormDataContent($"---www-{DateTime.Now:yy-MM-dd}-www---");
            var fileNames = new List<string>();
            fileNames.Add($"{Guid.NewGuid():D}.jpg");
            fileNames.Add($"{Guid.NewGuid():D}.jpg");
            fileNames.Add($"{Guid.NewGuid():D}.jpg");

            for (var i = 0; i < fileNames.Count; i++)
            {
                var content = new ByteArrayContent(data);
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpg");
                multipartFormContent.Add(content, $"{nameof(StudentViewModel.Photos)}[{i}]", fileNames[i]);
            }

            var handledResult = await multipartFormDataFormatter
                .ReadFromStreamAsync(typeof(StudentViewModel), new MemoryStream(), multipartFormContent, logger);

            Assert.IsInstanceOf<StudentViewModel>(handledResult);
            
            var student = handledResult as StudentViewModel;
            Assert.NotNull(student?.Photos);
            Assert.AreEqual(fileNames.Count, student.Photos.Count);

            for (var i = 0; i < fileNames.Count; i++)
                Assert.AreEqual(fileNames[i], student.Photos[i].FileName);
        }

        [Test]
        public async Task UploadPhotosIntoStudentProfile_Returns_StudentWithProfilePhotos()
        {
            var logger = _container.Resolve<IFormatterLogger>();
            var multipartFormDataFormatter = _container.Resolve<MultipartFormDataFormatter>();
            var applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var attachmentPath = Path.Combine(applicationPath, "Data", "grapefruit-slice-332-332.jpg");
            var data = File.ReadAllBytes(attachmentPath);

            var multipartFormContent = new MultipartFormDataContent($"---www-{DateTime.Now:yy-MM-dd}-www---");
            var fileNames = new List<string>();
            fileNames.Add($"{Guid.NewGuid():D}.jpg");
            fileNames.Add($"{Guid.NewGuid():D}.jpg");
            fileNames.Add($"{Guid.NewGuid():D}.jpg");

            for (var i = 0; i < fileNames.Count; i++)
            {
                var content = new ByteArrayContent(data);
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpg");
                multipartFormContent.Add(content, $"{nameof(StudentViewModel.Profile)}[{nameof(ProfileViewModel.Photos)}][{i}]", fileNames[i]);
            }

            var handledResult = await multipartFormDataFormatter
                .ReadFromStreamAsync(typeof(StudentViewModel), new MemoryStream(), multipartFormContent, logger);

            Assert.IsInstanceOf<StudentViewModel>(handledResult);

            var student = handledResult as StudentViewModel;
            Assert.NotNull(student?.Profile);
            Assert.AreEqual(fileNames.Count, student.Profile.Photos.Count);

            for (var i = 0; i < fileNames.Count; i++)
                Assert.AreEqual(fileNames[i], student.Profile.Photos[i].FileName);
        }

        [Test]
        public async Task UploadPhotosIntoStudentProfiles_Returns_StudentWithProfilesPhotos()
        {
            var logger = _container.Resolve<IFormatterLogger>();
            var multipartFormDataFormatter = _container.Resolve<MultipartFormDataFormatter>();
            var applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var attachmentPath = Path.Combine(applicationPath, "Data", "grapefruit-slice-332-332.jpg");
            var data = File.ReadAllBytes(attachmentPath);

            var multipartFormContent = new MultipartFormDataContent($"---www-{DateTime.Now:yy-MM-dd}-www---");
            var fileNames = new List<string>();
            fileNames.Add($"{Guid.NewGuid():D}.jpg");
            fileNames.Add($"{Guid.NewGuid():D}.jpg");
            fileNames.Add($"{Guid.NewGuid():D}.jpg");

            const int profileNumbers = 2;

            for (var profileId = 0; profileId < profileNumbers; profileId++)
            {
                for (var i = 0; i < fileNames.Count; i++)
                {
                    var content = new ByteArrayContent(data);
                    content.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpg");

                    // Param: profiles[0][photos][0]
                    var multipartParam = $"{nameof(StudentViewModel.Profiles)}[{profileId}][{nameof(ProfileViewModel.Photos)}][{i}]";
                    Debug.WriteLine(multipartParam);
                    multipartFormContent
                        .Add(content, multipartParam, fileNames[i]);
                }

            }

            var handledResult = await multipartFormDataFormatter
                .ReadFromStreamAsync(typeof(StudentViewModel), new MemoryStream(), multipartFormContent, logger);
            Assert.IsInstanceOf<StudentViewModel>(handledResult);

            var student = handledResult as StudentViewModel;
            Assert.NotNull(student?.Profiles);

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
