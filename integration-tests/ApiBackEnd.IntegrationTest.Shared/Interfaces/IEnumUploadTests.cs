using System.Threading.Tasks;

namespace ApiBackEnd.IntegrationTest.Shared.Interfaces
{
    public interface IEnumUploadTests
    {
        Task UploadValidQualityToNonNullableQuality_Returns_ModelWithValidQuality();

        Task UploadValidIntQualityToNonNullableQuality_Returns_ModelWithValidQuality();

        Task UploadInvalidQualityToNonNullableQuality_Returns_ModelWithDefaultQuality();

        Task UploadOutOfRangeQualityToNonNullableQuality_Returns_ModelWithDefaultQuality();

        Task UploadValidQualityToNullableQuality_Returns_ModelWithUploadedQuality();

        Task UploadValidIntQualityToNullableQuality_Returns_ModelWithUploadedQuality();

        Task UploadEmptyQualityToNullableQuality_Returns_ModelWithNullQuality();

        Task UploadNegativeQualityToNullableQuality_Returns_ModelWithNullQuality();

        Task UploadOutOfRangeQualityToNullableQuality_Returns_ModelWithNullQuality();

        Task UploadValidQualityIntoQualities_Returns_ValidList();

        Task NoUploadQualitiesIntoQualities_Returns_Null();
    }
}