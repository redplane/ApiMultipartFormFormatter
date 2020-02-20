using System.Threading.Tasks;

namespace ApiMultiPartFormData.UnitTest.Tests.Interfaces
{
    public interface IEnumUploadTests
    {
        Task UploadStudentWithTypeEnumAsInteger_Returns_StudentWithTypeEnum();

        Task UploadStudentTypeWithEnumAsText_Returns_StudentWithTypeEnum();

        Task UploadStudentTypeWithNullEnumText_Returns_StudentWithNullEnum();

        Task UploadStudentTypeWithEnumText_Returns_StudentWithEnum();
    }
}
