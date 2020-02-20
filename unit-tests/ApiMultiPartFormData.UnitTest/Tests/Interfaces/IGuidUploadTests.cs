using System.Threading.Tasks;

namespace ApiMultiPartFormData.UnitTest.Tests.Interfaces
{
    public interface IGuidUploadTests
    {
        #region Methods

        Task UploadStudentWithId_Returns_StudentWithId();

        Task UploadStudentWithoutId_Returns_StudentWithGuidEmptyId();

        Task UploadStudentWithoutParentId_Returns_StudentWithNullParentId();

        Task UploadStudentWithParentId_Returns_StudentWithParentId();

        Task UploadStudentWithChildIds_Returns_StudentWithChildIds();

        Task UploadIdIntoProfile_Returns_StudentProfileWithId();

        Task UploadBlankIdIntoProfile_Returns_StudentProfileWithId();

        Task NoUploadIntoProfile_Returns_StudentNullProfile();

        Task UploadWithNestedRelativeIds_Returns_StudentProfileWithRelativeIds();

        #endregion
    }
}
