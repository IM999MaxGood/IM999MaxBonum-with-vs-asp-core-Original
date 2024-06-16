using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using IM999MaxBonum.Classes;
using IM999MaxBonum.Controllers;

using Microsoft.AspNetCore.Hosting;


namespace IM999MaxBonum.Areas.Admin.Controllers
{
    [Area("Admin")]
    //public class PageGroupsController : BaseController
    public class PageGroupsController(IHostingEnvironment environment) : BaseController(environment)
    {
        public IActionResult Index()
        {
            var langMark = CurrentLang.LangMark;
            if (CurrentUser == null){
                return RedirectToAction("Index", new { lang = langMark, Area="", Controller="Users" });
            }

            if(!CurrentUser.IsPersonel)
                return RedirectToAction("Index", new { lang = langMark, Area="", Controller="Home" });

            //var pgs = clsPageGroup.GetvPageGroups(langMark);
            var pgs = clsPageGroup.GetvPageGroups();
            return View(pgs);
        }

        [HttpGet]
        // GET: Admin/{lang}/PageGroups/Create
        public IActionResult Create()
        {
            var vpg = new vPageGroup();

            //ViewData["Languages"] = clsLanguage.GetLanguagesSelectList();
            //999/ دراپ.داون زبان که باید تابع جاوا اسکریپتش برای انتخاب قرار داده شود
            var ls = clsLanguage.GetLanguagesListKeyValue();
            string lang = Resource.GetData(CurrentLang.LangMark, "Language");
            ViewData["DropDown_Languages"] = clsGeneralFunction.GetHtmlBootstrapDropDown(ls, "PageGroupLang", "LangId", lang, CurrentLang.LangId);

            //ViewData["PageName"] = string.Format("{0} {1}", Resource.Create, Resource.Menus_Groups);
            return View(vpg);
        }

        [HttpGet]
        // GET: Admin/{lang}/PageGroups/Edit/id
        public IActionResult Edit(int id)
        {
            var vpg = clsPageGroup.GetvPageGroup(id);
            if(!vpg.CanEdit)
                return RedirectToAction("Index", new { lang = CurrentLang.LangMark, Area="Admin", Controller="PageGroup" });

            var ls = clsLanguage.GetLanguagesListKeyValue();
            string lang = Resource.GetData(CurrentLang.LangMark, "Language");
            ViewData["DropDown_Languages"] = clsGeneralFunction.GetHtmlBootstrapDropDown(ls, "PageGroupLang", "LangId", lang, vpg.LangId);

            return View(vpg);
        }

        [HttpPost]
        public ActionResult Delete(int pageGroupId)
        {
            var vpg = clsPageGroup.GetvPageGroup(pageGroupId);
            if(!vpg.CanEdit)
                return RedirectToAction("Index", new { lang = CurrentLang.LangMark, Area="Admin", Controller="PageGroup" });

            var langMark = CurrentLang.LangMark;
            var delete = Resource.GetData(langMark, "Delete");
            var pageGroup = Resource.GetData(langMark, "PageGroup"); 
            var success = Resource.GetData(langMark, "Exec_Ok"); 
            var faild = Resource.GetData(langMark, "Exec_Nok"); 

            if (clsPageGroup.DeletePageGroup(pageGroupId))
                return Json(new { res = "ok", msg = string.Format(success, delete + " " + pageGroup) });
            else
                return Json(new { res = "nok", msg = string.Format(faild, delete + " " + pageGroup) });
        }

        [HttpPost]
        public IActionResult DoCreate(string pageGroupName, int langId)
        {
            var createPageGroup = Resource.GetData(CurrentLang.LangMark, "Create") + " " + Resource.GetData(CurrentLang.LangMark, "PageGroup");

            var pageGroup_Exist = Resource.GetData(CurrentLang.LangMark, "PageGroup_Exist");
            var lang_Not_Exist = Resource.GetData(CurrentLang.LangMark, "Lang_Not_Exist");

            var pageGroup_Failed = string.Format( Resource.GetData(CurrentLang.LangMark, "Exec_Nok"), createPageGroup);
            var pageGroup_Success = string.Format( Resource.GetData(CurrentLang.LangMark, "Exec_Ok"), createPageGroup);
            
            if(clsLanguage.GetLanguage(langId)==null)
                return Json(new { res = "nok", msg = lang_Not_Exist });
            
            if(clsPageGroup.GetvPageGroup(pageGroupName, langId) !=null)
                return Json(new { res = "nok", msg = pageGroup_Exist });


            var pg = new vPageGroup();
            pg.PageGroupName = pageGroupName;
            pg.LangId = langId;
            pg.CanEdit = true;
            pg.IsActive = true;
            if(clsPageGroup.InsertPageGroup(pg))
                return Json(new { res = "ok", msg = pageGroup_Success });

            return Json(new { res = "nok", msg = pageGroup_Failed });
        }

        [HttpPost]
        public IActionResult DoEdit(string PageGroupName, int LangId, int pageGroupId,  int isActive)
        {
            var editPageGroup = Resource.GetData(CurrentLang.LangMark, "Edit") + " " + Resource.GetData(CurrentLang.LangMark, "PageGroup");

            var pageGroup_Exist = Resource.GetData(CurrentLang.LangMark, "PageGroup_Exist");
            var lang_Not_Exist = Resource.GetData(CurrentLang.LangMark, "Lang_Not_Exist");

            var pageGroup_Failed = string.Format( Resource.GetData(CurrentLang.LangMark, "Exec_Nok"), editPageGroup);
            var pageGroup_Success = string.Format( Resource.GetData(CurrentLang.LangMark, "Exec_Ok"), editPageGroup);
            
            if(clsLanguage.GetLanguage(LangId)==null)
                return Json(new { res = "nok", msg = lang_Not_Exist });
            
            if(clsPageGroup.GetvPageGroup(pageGroupId, PageGroupName, LangId) != null)
                return Json(new { res = "nok", msg = pageGroup_Exist });


            var pg = clsPageGroup.GetvPageGroup(pageGroupId);
            pg.PageGroupName = PageGroupName;
            pg.LangId = LangId;
            //pg.CanEdit = true;
            pg.IsActive = Convert.ToBoolean( isActive);
            if(clsPageGroup.SavePageGroup(pg))
                return Json(new { res = "ok", msg = pageGroup_Success });

            return Json(new { res = "nok", msg = pageGroup_Failed });
        }
    }
}