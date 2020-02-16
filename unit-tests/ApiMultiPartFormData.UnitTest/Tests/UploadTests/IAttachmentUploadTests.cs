using System.Threading.Tasks;

namespace ApiMultiPartFormData.UnitTest.Tests.UploadTests
{
    public interface IAttachmentUploadTests
    {
        Task UploadAttachmentsIntoStudent_Returns_StudentWithAttachments();

        Task UploadPhotosIntoStudentProfile_Returns_StudentWithProfilePhotos();

        Task UploadPhotosIntoStudentProfiles_Returns_StudentWithProfilesPhotos();
    }
}