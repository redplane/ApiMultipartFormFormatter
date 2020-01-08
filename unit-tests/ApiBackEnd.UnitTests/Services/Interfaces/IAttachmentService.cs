using System.Threading.Tasks;
using ApiMultiPartFormData.Models;

namespace ApiBackEnd.UnitTests.Services.Interfaces
{
    public interface IAttachmentService
    {
        #region Methods

        Task<HttpFileBase> LoadSampleAttachmentAsync();

        #endregion
    }
}