using System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

using IM999MaxBonum.Classes;
using IM999MaxBonum.Models;
using IM999MaxBonum.Controllers;

using Microsoft.AspNetCore.Hosting;


namespace IM999MaxBonum.Areas.Admin.Controllers
{
    [Area("Admin")]
    //public class MenusController : BaseController
    public class MenusController(IHostingEnvironment environment) : BaseController(environment)
    {
        /*
        private IHostingEnvironment _hostingEnvironment;

        //999/ برای گرفتن مسیر اصلی سایت
        public  MenusController(IHostingEnvironment environment) {
            _hostingEnvironment = environment;
        }
        */

        [HttpGet]
        public IActionResult Index()
        {
            var ls = clsLanguage.GetLanguagesListKeyValue();
            string create = Resource.GetData(CurrentLang.LangMark, "create");
            string menu = Resource.GetData(CurrentLang.LangMark, "menu");
            var str = create + " " + menu;

            var vms = clsMenu.GetvMenus();
            return View(vms);
        }

        [HttpGet]
        // GET: Admin/{lang}/Menus/Create
        public IActionResult Create()
        {
            var vm = new vMenu();

            //999/ دراپ.داون زبان که باید تابع جاوا اسکریپتش برای انتخاب قرار داده شود
            var ls = clsLanguage.GetLanguagesListKeyValue();
            string lang = Resource.GetData(CurrentLang.LangMark, "Language");
            ViewData["DropDown_Languages"] = clsGeneralFunction.GetHtmlBootstrapDropDown(ls, "MenuLang", "LangId", lang, CurrentLang.LangId);

            ViewData["MenuAndItems"] = "<ul></ul>";

            return View(vm);
        }

        [HttpPost]
        public ActionResult Delete(int menuId)
        {
            var vm = clsMenu.GetvMenu(menuId);
            if(!vm.CanEdit)
                return RedirectToAction("Index", new { lang = CurrentLang.LangMark, Area="Admin", Controller="Menu" });

            var langMark = CurrentLang.LangMark;
            var delete = Resource.GetData(langMark, "Delete");
            var menu = Resource.GetData(langMark, "Menu"); 
            var success = Resource.GetData(langMark, "Exec_Ok"); 
            var faild = Resource.GetData(langMark, "Exec_Nok"); 

            if (clsMenu.DeleteMenu(menuId))
                return Json(new { res = "ok", msg = string.Format(success, delete + " " + menu) });
            else
                return Json(new { res = "nok", msg = string.Format(faild, delete + " " + menu) });
        }


        [HttpPost]
        public IActionResult DoCreate(string menuName, int langId, string menuAndItems)
        {
            var createMenu = Resource.GetData(CurrentLang.LangMark, "Create") + " " + Resource.GetData(CurrentLang.LangMark, "Menu");

            var menu_Exist = Resource.GetData(CurrentLang.LangMark, "Menu_Exist");
            var lang_Not_Exist = Resource.GetData(CurrentLang.LangMark, "Lang_Not_Exist");

            var menu_Failed = string.Format( Resource.GetData(CurrentLang.LangMark, "Exec_Nok"), createMenu);
            var menu_Success = string.Format( Resource.GetData(CurrentLang.LangMark, "Exec_Ok"), createMenu);
            
            if(clsLanguage.GetLanguage(langId)==null)
                return Json(new { res = "nok", msg = lang_Not_Exist });
            
            if(clsPageGroup.GetvPageGroup(menuName, langId) !=null)
                return Json(new { res = "nok", msg = menu_Exist });


            var m = new Menu();
            m.MenuName = menuName;
            m.LangId = langId;
            m.CanEdit = true;
            m.IsMainMenu = false;
            if(clsMenu.InsertMenu(m, "fileName"))
                return Json(new { res = "ok", msg = menu_Success });

            return Json(new { res = "nok", msg = menu_Failed });
        }

        
    }
}
