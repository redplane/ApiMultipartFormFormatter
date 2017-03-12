using System.Net;
using System.Net.Http;
using System.Web.Http;
using HttpFormatter.Test.ViewModels;

namespace HttpFormatter.Test.Controllers
{
    [RoutePrefix("api/account")]
    public class ApiAccountController : ApiController
    {
        [Route("register")]
        [HttpPost]
        public HttpResponseMessage Register([FromUri] int index, [FromBody] AccountRegistrationViewModel parameters)
        {
            if (parameters == null)
            {
                parameters = new AccountRegistrationViewModel();
                Validate(parameters);
            }

            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}