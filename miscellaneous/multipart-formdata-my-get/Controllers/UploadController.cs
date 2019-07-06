using System.Collections.Generic;
using System.Web.Http;
using MultipartFormDataMyGet.ViewModels;

namespace MultipartFormDataMyGet.Controllers
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
            messages.Add($"Attachment information: (Mime) {info.Attachment.MediaType} - (File name) {info.Attachment.Name}");

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

            return Ok(new ClientResponseViewModel(messages));
        }


        #endregion
    }
}