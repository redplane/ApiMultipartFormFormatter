using System.Threading.Tasks;

namespace ApiBackEnd.IntegrationTest.Shared.Interfaces
{
    public interface INestedGuidUploadTests
    {
        Task UploadValidProfileNonNullableGuidString_Returns_ModelWithValidProfileGuid();

        Task UploadEmptyGuidStringIntoProfileNonNullableGuidString_Returns_ModelWithEmptyProfileGuid();

        Task UploadValidGuidIdIntoNullableGuidField_Returns_ModelWithValidGuid();

        Task IncludeIdsInProfileIds_Returns_ModelWithUploadedProfileIds();
    }
}