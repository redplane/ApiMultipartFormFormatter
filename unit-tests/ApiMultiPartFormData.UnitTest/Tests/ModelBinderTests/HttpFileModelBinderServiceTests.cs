using System;
using System.IO;
using System.Reflection;
using ApiMultiPartFormData.Exceptions;
using ApiMultiPartFormData.Models;
using ApiMultiPartFormData.Services.Implementations;
using NUnit.Framework;

namespace ApiMultiPartFormData.UnitTest.Tests.ModelBinderTests
{
    [TestFixture]
    public class HttpFileModelBinderServiceTests
    {
        #region Methods

        [Test]
        public void HandleNullValue_Throws_UnhandledParameterException()
        {
            var httpFileModelBinderService = new HttpFileModelBinderService();
            Assert.CatchAsync<UnhandledParameterException>(() => httpFileModelBinderService.BuildModelAsync(typeof(HttpFile), null));
        }

        [TestCase(typeof(int))]
        [TestCase(typeof(bool))]
        [TestCase(typeof(object))]
        [TestCase(typeof(string))]
        [TestCase(typeof(HttpFileBase))]
        public void HandleNotHttpFileType_Throws_UnhandledParameterException(Type type)
        {
            var httpFileBase = new HttpFileBase("attachment-001.jpg", new MemoryStream(), "image/jpg");
            var httpFileModelBinderService = new HttpFileModelBinderService();
            Assert.CatchAsync<UnhandledParameterException>(() => httpFileModelBinderService
                .BuildModelAsync(type, httpFileBase));
        }

        [Test]
        public void HandleHttpFileType_Throws_UnhandledParameterException()
        {
            var applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var attachmentPath = Path.Combine(applicationPath, "Data", "grapefruit-slice-332-332.jpg");
            var data = File.ReadAllBytes(attachmentPath);

            var httpFileBase = new HttpFile("attachment-001.jpg", "image/jpg", data);
            var httpFileModelBinderService = new HttpFileModelBinderService();
            var handledResult = (HttpFile) httpFileModelBinderService.BuildModelAsync(typeof(HttpFile), httpFileBase)
                .Result;

            Assert.NotNull(handledResult);
            Assert.AreEqual(httpFileBase.Name, handledResult.Name);
            Assert.AreEqual(httpFileBase.Buffer, handledResult.Buffer);
            Assert.AreEqual(httpFileBase.MediaType, handledResult.MediaType);
            Assert.AreEqual(httpFileBase.ContentLength, handledResult.ContentLength);
            Assert.AreEqual(httpFileBase.ContentType, handledResult.ContentType);
            Assert.AreEqual(httpFileBase.FileName, handledResult.FileName);
        }


        #endregion
    }
}