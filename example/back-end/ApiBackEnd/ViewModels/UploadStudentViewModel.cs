using System.Collections.Generic;

namespace ApiBackEnd.ViewModels
{
    public class UploadStudentViewModel
    {
        public string Name { get; set; }

        public List<StudentViewModel> Students { get; set; }
    }
}