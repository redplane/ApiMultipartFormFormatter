using System.Threading.Tasks;

namespace ApiBackEnd.IntegrationTest.Shared.Interfaces
{
    public interface INestedAttachmentUploadTests
    {
        Task UploadValidNestedAttachment_Returns_AttachmentInfoWhoseInfoSameAsUploadedOne();

        Task UploadValidProfileAttachments_Returns_AttachmentsInfoWhoseInfoSameAsUploadedOnes();
    }
}