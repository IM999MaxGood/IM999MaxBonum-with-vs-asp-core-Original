using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using IM999MaxBonum.Controllers;

using Microsoft.AspNetCore.Hosting;


namespace IM999MaxBonum.Areas.Admin.Controllers
{
    [Area("Admin")]
    //public class HomeController : BaseController
    public class HomeController(IHostingEnvironment environment) : BaseController(environment)
    {
        // GET: /Products/Home/Index
        public IActionResult Index()
        {
            if (CurrentUser == null){
                //return RedirectToAction("Index","Users");
                //esponse.Redirect("/");
                return RedirectToAction("Index", new { lang = CurrentLang.LangMark, Area="", Controller="Users" });
            }

            if(!CurrentUser.IsPersonel)
                //return RedirectToAction("Index","Home");
                return RedirectToAction("Index", new { lang = CurrentLang.LangMark, Area="", Controller="Home" });
        
            ViewData["PageName"] = Resource.GetData(CurrentLang.LangMark, "Admin_Page");;
            return View();
        }
    }
}