using System.Threading.Tasks;

namespace ApiBackEnd.IntegrationTest.Shared.Interfaces
{
    public interface IAttachmentUploadTests
    {
        Task UploadValidAttachment_Returns_AttachmentInfoWhoseInfoSameAsUploadedOne();

        Task UploadValidAttachments_Returns_AttachmentsInfoWhoseInfoSameAsUploadedOnes();
    }
}
