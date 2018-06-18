using System.Web.Http;
using ApiBackEnd.ViewModels;

namespace ApiBackEnd.Controllers
{
    [RoutePrefix("api/student")]
    public class StudentController : ApiController
    {
        [Route("upload")]
        [HttpPost]
        public IHttpActionResult Upload(UploadStudentViewModel upload)
        {
            if (upload == null)
            {
                upload = new UploadStudentViewModel();
                Validate(upload);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok();
        }
    }
}