using System;
using System.Collections.Generic;
using ApiMultiPartFormData.Models;
using NUnit.Framework;

namespace ApiMultiPartFormData.UnitTest.ViewModels
{
    public class StudentViewModel
    {
        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        public List<Guid> ChildIds { get; set; }

        public string FullName { get; set; }

        public HttpFileBase Photo { get; set; }
    }
}