using System.Threading.Tasks;

namespace ApiMultiPartFormData.UnitTest.Tests.Interfaces
{
    public interface IAttachmentsUploadTests
    {
        Task UploadAttachmentsIntoStudent_Returns_StudentWithAttachments();

        Task UploadPhotosIntoStudentProfile_Returns_StudentWithProfilePhotos();

        Task UploadPhotosIntoStudentProfiles_Returns_StudentWithProfilesPhotos();
    }
}