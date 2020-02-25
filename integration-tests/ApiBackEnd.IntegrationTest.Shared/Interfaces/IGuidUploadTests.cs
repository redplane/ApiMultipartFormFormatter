using System.Threading.Tasks;

namespace ApiBackEnd.IntegrationTest.Shared.Interfaces
{
    public interface IGuidUploadTests
    {
        Task UploadValidGuidString_Returns_ModelWithValidGuid();

        Task UploadEmptyGuidString_Returns_ModelWithEmptyGuid();

        Task LeaveIdFieldBlank_Returns_ModelWithEmptyGuid();

        Task UploadValidAttachmentIdIntoNullableGuidField_Returns_ModelWithValidGuid();

        Task NotIncludeAttachmentIdIntoNullableGuidField_Returns_ModelWithValidGuid();

        Task UploadEmptyAttachmentIdIntoNullableGuidField_Returns_ModelWithNullAttachmentId();

        Task NotIncludeIdsInIds_Returns_ModelWithNullIds();

        Task IncludeIdsInIds_Returns_ModelWithUploadedIds();
    }
}