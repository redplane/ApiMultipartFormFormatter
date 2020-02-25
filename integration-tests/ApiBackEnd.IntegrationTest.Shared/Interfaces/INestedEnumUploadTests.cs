using System.Threading.Tasks;

namespace ApiBackEnd.IntegrationTest.Shared.Interfaces
{
    public interface INestedEnumUploadTests
    {
        Task UploadValidQualityToProfileNonNullableQuality_Returns_ModelWithValidProfileNonNullableQuality();

        Task UploadValidIntQualityToProfileNonNullableQuality_Returns_ModelWithProfileValidQuality();

        Task UploadInvalidQualityToProfileNonNullableQuality_Returns_ModelWithDefaultProfileQuality();

        Task UploadOutOfRangeQualityToProfileNonNullableQuality_Returns_ModelProfileWithDefaultQuality();

        Task UploadValidQualityToProfileNullableQuality_Returns_ModelWithUploadedProfileQuality();

        Task UploadValidIntQualityToProfileNullableQuality_Returns_ModelWithUploadedProfileQuality();

        Task UploadEmptyQualityToNullableQuality_Returns_ModelWithNullQuality();

        Task UploadNegativeQualityToNullableQuality_Returns_ModelWithNullQuality();

        Task UploadOutOfRangeQualityToProfileNullableQuality_Returns_ModelWithNullProfileQuality();

        Task UploadValidQualityIntoProfileQualities_Returns_ValidList();

    }
}