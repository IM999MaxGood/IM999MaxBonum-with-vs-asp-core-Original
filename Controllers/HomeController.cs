using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using IM999MaxBonum.Models;
using IM999MaxBonum.Classes;

using Microsoft.AspNetCore.Hosting;

/*
//999/for using resource
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
//using IM999MaxBonum.App_GlobalResources;

//999/for localization
using Microsoft.Extensions.Localization;
*/

namespace IM999MaxBonum.Controllers
{
    //public class HomeController: BaseController
    
    public class HomeController(IHostingEnvironment environment) : BaseController(environment)
    {


/*
        private readonly IM999Max_DotNetCore _context;
        public HomeController(IM999Max_DotNetCore context) : base(context)
        {
            _context = context;
        }
*/

/*
        private readonly IStringLocalizer<HomeController> _localizer;
        public HomeController(IStringLocalizer<HomeController> localizer)
        {
            _localizer = localizer;
        }

        public string GetLocalizedString()
        {
            return _localizer["My localized string"];
        }
*/

        public IActionResult Index()
        {
            ViewData["ShowSlider"] = true;

            //SharedResource x = new SharedResource("fa-IR");
            //ViewData["PageName"] = x._localizer["HomePage"];

            //ViewData["PageName"] = Resource.HomePage;
            ViewData["PageName"] = Resource.GetData(CurrentLang.LangMark, "HomePage");
            
            ViewData["BreadCrumbLinks"] = null;

            return View();
        }

        public IActionResult testEF()
        {            
            ViewData["ShowSlider"] = true;
            ViewData["PageName"] = Resource.GetData(CurrentLang.LangMark, "HomePage");
            ViewData["BreadCrumbLinks"] = null;

            var langs = clsLanguage.GetLanguages().ToList();
            langs = clsLanguage.GetLanguages().ToList();
            //var langs = clsLanguage.GetLanguages(_DbContext).ToList();

            return View(langs);
        }
    }
}
