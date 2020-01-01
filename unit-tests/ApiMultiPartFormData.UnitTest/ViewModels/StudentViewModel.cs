using System;
using ApiMultiPartFormData.Models;

namespace ApiMultiPartFormData.UnitTest.ViewModels
{
    public class StudentViewModel
    {
        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        public string FullName { get; set; }

        public HttpFileBase Photo { get; set; }
    }
}