using System.Threading.Tasks;
using ApiMultiPartFormData.Models;

namespace ApiBackEnd.IntegrationTest.Shared.Interfaces
{
    public interface IAttachmentService
    {
        #region Methods

        Task<HttpFileBase> LoadSampleAttachmentAsync();

        #endregion
    }
}