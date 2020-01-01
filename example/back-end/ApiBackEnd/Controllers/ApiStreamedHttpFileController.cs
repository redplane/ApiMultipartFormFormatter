using System.Collections.Generic;
using System.Web.Http;
using ApiBackEnd.ViewModels;
using ApiBackEnd.ViewModels.Requests;

namespace ApiBackEnd.Controllers
{
    [RoutePrefix("api/stream")]
    public class ApiStreamedHttpFileController : ApiController
    {
        /// <summary>
        /// Upload attachment to service end-point using streaming.
        /// </summary>
        /// <returns></returns>
        [Route("basic-upload")]
        [HttpPost]
        public IHttpActionResult UploadBasicStream(StreamBasicUploadViewModel info)
        {
            if (info == null)
            {
                info = new StreamBasicUploadViewModel();
                Validate(info);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var messages = new List<string>();
            messages.Add($"Author information: {info.Author.FullName}");
            messages.Add($"Attachment information: (Mime) {info.Attachment.ContentType} - (File name) {info.Attachment.FileName}");

            return Ok(new ClientResponseViewModel(messages));
        }

        /// <summary>
        /// Upload nested file.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [Route("nested-upload")]
        [HttpPost]
        public IHttpActionResult UploadNestedStream(NestedStreamUploadViewModel info)
        {
            if (info == null)
            {
                info = new NestedStreamUploadViewModel();
                Validate(info);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var messages = new List<string>();
            var attachment = info.Attachment;
            messages.Add($"Root attachment information: (Mime) {attachment.MediaType} - (File name) {attachment.Name}");

            var profile = info.Profile;
            if (profile != null)
            {
                messages.Add($"Profile has been uploaded.");
                messages.Add($"Profile name : {profile.Name}");

                var profileAttachment = profile.Attachment;
                messages.Add(
                    $"Profile attachment information: (Mime) {profileAttachment.ContentType} - (File name) {profileAttachment.FileName}");
            }
            else
                messages.Add("No profile is added");

            return Ok(new ClientResponseViewModel(messages));
        }
    }
}