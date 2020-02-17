using ApiBackEndShared.Interfaces;
using ApiBackEndShared.ViewModels.Requests;
using ApiBackEndShared.ViewModels.Responses;
using Microsoft.AspNetCore.Mvc;
namespace ApiBackEndAspNetCore.Controllers
{
    [Route("api/upload")]
    public class ApiUploadController : Controller, IUploadController
    {
        /// <summary>
        ///     Upload attachment to service end-point.
        /// </summary>
        /// <returns></returns>
        [HttpPost("")]
        public IActionResult BasicUpload(UploadRequestViewModel model)
        {
            if (model == null)
            {
                model = new UploadRequestViewModel();
                TryValidateModel(model);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            return Ok(new UploadResponseViewModel(model));
        }
    }
}