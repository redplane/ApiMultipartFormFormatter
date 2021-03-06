﻿using System;
using System.Collections.Generic;
using ApiMultiPartFormData.Models;
using ApiMultiPartFormData.UnitTest.Enums;

namespace ApiMultiPartFormData.UnitTest.ViewModels
{
    public class StudentViewModel
    {
        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        public List<Guid> ChildIds { get; set; }

        public string FullName { get; set; }

        public HttpFileBase Photo { get; set; }

        public ProfileViewModel Profile { get; set; }

        public List<ProfileViewModel> Profiles { get; set; }

        public StudentTypes Type { get; set; }

        public StudentTypes? NullableStudentType { get; set; }

        public List<HttpFileBase> Photos { get; set; }

    }
}