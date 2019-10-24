using System.Collections.Generic;
using System.Text;
using System.Web.Http;
using ApiBackEnd.ViewModels;

namespace ApiBackEnd.Controllers
{
    [RoutePrefix("api/upload")]
    public class ApiUploadController : ApiController
    {
        #region Methods

        /// <summary>
        /// Upload attachment to service end-point.
        /// </summary>
        /// <returns></returns>
        [Route("basic-upload")]
        [HttpPost]
        public IHttpActionResult BasicUpload(BasicUploadViewModel info)
        {
            if (info == null)
            {
                info = new BasicUploadViewModel();
                Validate(info);
            }
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var messages = new List<string>();
            messages.Add($"Author information: {info.Author.FullName}");
            messages.Add($"Attachment information: (Mime) {info.Attachment.MediaType} - (File name) {info.Attachment.Name}");

            return Ok(new ClientResponseViewModel(messages));
        }

        /// <summary>
        /// Upload attachment to service end-point using streaming.
        /// </summary>
        /// <returns></returns>
        [Route("basic-upload-streamed")]
        [HttpPost]
        public IHttpActionResult BasicUploadStreamed(BasicUploadStreamedViewModel info)
        {
            if (info == null)
            {
                info = new BasicUploadStreamedViewModel();
                Validate(info);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var messages = new List<string>();
            messages.Add($"Author information: {info.Author.FullName}");
            messages.Add($"Attachment information: (Mime) {info.StreamedAttachment.MediaType} - (File name) {info.StreamedAttachment.Name}");

            return Ok(new ClientResponseViewModel(messages));
        }

        /// <summary>
        /// Upload a list of attachment with author information.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [Route("attachments-list-upload")]
        [HttpPost]
        public IHttpActionResult UploadAttachmentsList(UploadAttachmentListViewModel info)
        {
            if (info == null)
            {
                info = new UploadAttachmentListViewModel();
                Validate(info);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var messages = new List<string>();
            if (info.Author == null)
                messages.Add("No author information has been uploaded");
            else
                messages.Add($"Author information: {info.Author.FullName}");

            foreach (var attachment in info.Attachments)
                messages.Add($"Attachment information: (Mime) {attachment.MediaType} - (File name) {attachment.Name}");

            return Ok(new ClientResponseViewModel(messages));
        }

        /// <summary>
        /// Upload nested file.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [Route("nested-info-upload")]
        [HttpPost]
        public IHttpActionResult UploadNestedInfo(UploadNestedInfoViewModel info)
        {
            if (info == null)
            {
                info = new UploadNestedInfoViewModel();
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
                    $"Profile attachment information: (Mime) {profileAttachment.MediaType} - (File name) {profileAttachment.Name}");
            }
            else
                messages.Add("No profile is added");
            
            return Ok(new ClientResponseViewModel(messages));
        }
        

        #endregion
    }
}