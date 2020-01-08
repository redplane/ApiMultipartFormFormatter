using System.Threading.Tasks;
using ApiMultiPartFormData.Models;

namespace ApiMultiPartFormData.UnitTest.Services.Interfaces
{
    public interface IAttachmentService
    {
        #region Methods

        Task<HttpFileBase> LoadSampleAttachmentAsync();

        #endregion
    }
}