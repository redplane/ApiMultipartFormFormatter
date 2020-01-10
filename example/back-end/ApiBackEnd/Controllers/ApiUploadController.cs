using System.Collections.Generic;
using System.Web.Http;
using ApiBackEnd.ViewModels;
using ApiBackEnd.ViewModels.Requests;
using ApiBackEnd.ViewModels.Responses;

namespace ApiBackEnd.Controllers
{
    [RoutePrefix("api/upload")]
    public class ApiUploadController : ApiController
    {
        #region Methods

        /// <summary>
        ///     Upload attachment to service end-point.
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public IHttpActionResult BasicUpload(UploadRequestViewModel model)
        {
            if (model == null)
            {
                model = new UploadRequestViewModel();
                Validate(model);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            return Ok(new UploadResponseViewModel(model));
        }

        #endregion
    }
}