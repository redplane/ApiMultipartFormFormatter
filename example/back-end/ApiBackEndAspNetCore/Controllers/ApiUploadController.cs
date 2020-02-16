using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApiBackEndAspNetCore.Controllers
{
    public class ApiUploadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}