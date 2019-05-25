using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Handler.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            Library.App_Code.Test.TEST();
            return View();
        }
    }
}