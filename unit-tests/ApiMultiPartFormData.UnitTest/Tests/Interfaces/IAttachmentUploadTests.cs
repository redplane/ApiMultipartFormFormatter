using System.Threading.Tasks;

namespace ApiMultiPartFormData.UnitTest.Tests.Interfaces
{
    public interface IAttachmentUploadTests
    {
        #region Methods

        Task UploadStudentWithName_Returns_StudentWithFullName();

        Task UploadStudentWithPhoto_Returns_StudentWithPhoto();

        Task UploadStudentWithProfilePhoto_Returns_StudentWithProfilePhoto();

        Task UploadStudentWithProfilePhoto_Returns_ProfilePhotoIsValidStream();

        Task UploadStudentWithBufferedAttachment_Returns_StudentWithBufferedAttachment();

        #endregion
    }
}
