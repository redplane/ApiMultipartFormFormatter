#if NETFRAMEWORK
using System.Web.Http;
#elif NETCOREAPP
using IHttpActionResult = Microsoft.AspNetCore.Mvc.IActionResult;
#endif
using ApiBackEndShared.ViewModels.Requests;

namespace ApiBackEndShared.Interfaces
{
    public interface IUploadController
    {
        IHttpActionResult BasicUpload(UploadRequestViewModel model);

    }
}