#if NETFRAMEWORK
using System.Web.Http;
#endif
using ApiBackEndShared.ViewModels.Requests;

namespace ApiBackEndShared.Interfaces
{
    public interface IUploadController
    {
        IHttpActionResult BasicUpload(UploadRequestViewModel model);

    }
}