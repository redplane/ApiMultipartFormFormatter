using System.Collections.Generic;
using ApiMultiPartFormData.Models;

namespace ApiBackEnd.ViewModels
{
    public class StudentViewModel
    {
        public string FullName { get; set; }

        public int Age { get; set; }

        public List<HttpFile> Attachments { get; set; }
    }
}