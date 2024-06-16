using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

using IM999MaxBonum.Classes;
using IM999MaxBonum.Models;
using IM999MaxBonum.Controllers;

using Microsoft.AspNetCore.Hosting;


namespace IM999MaxBonum.Areas.Admin.Controllers
{
    [Area("Admin")]
    //public class PagesController : BaseController
    public class PagesController(IHostingEnvironment environment) : BaseController(environment)
    {
        /*
        private IHostingEnvironment _hostingEnvironment;

        //999/ برای گرفتن مسیر اصلی سایت
        public  PagesController(IHostingEnvironment environment) {
            _hostingEnvironment = environment;
        }
        */

        [HttpGet]
        public IActionResult Index()
        {
            var langMark = CurrentLang.LangMark;
            if (CurrentUser == null){
                return RedirectToAction("Index", new { lang = langMark, Area="", Controller="Users" });
            }

            if(!CurrentUser.IsPersonel)
                return RedirectToAction("Index", new { lang = langMark, Area="", Controller="Home" });


            var ls = clsLanguage.GetLanguagesListKeyValue();
            string create = Resource.GetData(CurrentLang.LangMark, "create");
            string page = Resource.GetData(CurrentLang.LangMark, "page");
            var str = create + " " + page;
            ViewData["DropDown_Languages"] = clsGeneralFunction.GetHtmlBootstrapDropDownLanguage(ls, str, CurrentLang.LangMark);


            var ps = clsPage.GetvPages();
            return View(ps);
        } 

        
        [HttpGet]
        // GET: Admin/{lang}/Pages/Create
        public IActionResult Create(string pageLang)
        {
            var vp = new vPage();
/*
            //ViewData["Languages"] = clsLanguage.GetLanguagesSelectList();
            //999/ دراپ.داون زبان که باید تابع جاوا اسکریپتش برای انتخاب قرار داده شود
            var ls = clsLanguage.GetLanguagesListKeyValue();
            string lang = Resource.GetData(CurrentLang.LangMark, "Language");
            ViewData["DropDown_Languages"] = clsGeneralFunction.GetHtmlBootstrapDropDown(ls, "PageGroupLang", "LangId", lang, CurrentLang.LangId);
*/
            var Languages_Id = clsLanguage.GetLanguages().Where(x=> x.LangMark.ToLower().Trim()==pageLang.ToLower().Trim()).FirstOrDefault().LangId;
            ViewData["Languages_Id"] = Languages_Id;

            //var pgs = clsPageGroup.GetPageGroupsListKeyValue();
            var pgs = clsPageGroup.GetPageGroupsListKeyValue(pageLang);
            string pageGroup = Resource.GetData(CurrentLang.LangMark, "PageGroup");
            ViewData["DropDown_PageGroups"] = clsGeneralFunction.GetHtmlBootstrapDropDown(pgs, "PageGroups", "PageGroupId", pageGroup, pgs.FirstOrDefault().Key);
                        

            /*            
            var mainPath = "";
            if(clsGeneralFunction.InsideIIS()){
                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\wwwroot\\lib\\fileman")) { 
                    //mainPath = AppDomain.CurrentDomain.BaseDirectory + "\\wwwroot\\UserFiles\\Pages";
                    mainPath = AppDomain.CurrentDomain.BaseDirectory + clsGeneralProperty.PageFilePathFull;
                }
                else { 
                    //mainPath = _hostingEnvironment.ContentRootPath + "\\wwwroot\\UserFiles\\Pages";
                    mainPath = _hostingEnvironment.ContentRootPath + clsGeneralProperty.PageFilePathFull;
                }
            }
            else{
                //mainPath = _hostingEnvironment.ContentRootPath + "\\wwwroot\\UserFiles\\Pages";
                mainPath = _hostingEnvironment.ContentRootPath + clsGeneralProperty.PageFilePathFull;
            }
            */
            var mainPath = GetMainPath() + clsGeneralProperty.PageFilePathFull;


            //ذخیره فایلهای صفحه در یک مسیر موقت
            DirectoryInfo mainRoot = new DirectoryInfo(mainPath);
            ViewData["UnicId"] = Guid.NewGuid().ToString();
            mainRoot.CreateSubdirectory(ViewData["UnicId"].ToString());

            //ذخیره مسیر موقت در سشن برای بخش مدیریت فایل که این شاخه را باز کند
            //HttpContext.Session.Set("RoxyFilemanLastPath", "/wwwroot/UserFiles/Forms");
            SD.SetValue("RoxyFilemanRoot", "/wwwroot/UserFiles/Pages/"+ViewData["UnicId"]);
            //SD.SetValue("RoxyFilemanRoot", "\\wwwroot\\UserFiles\\Pages\\"+ViewData["UnicId"]);

            return View(vp);
        }

        public IActionResult InserPage(string PageName, int LangId, int PageGroupId, string PageTopCode, string PageContentHTML, string UnicId, string PageContentText ){
            var langMark = CurrentLang.LangMark;
            var save = Resource.GetData(langMark, "Save"); 
            var page = Resource.GetData(langMark, "Page"); 
            var success = Resource.GetData(langMark, "Exec_Ok"); 
            var faild = Resource.GetData(langMark, "Exec_Nok"); 

            //var path = _hostingEnvironment.ContentRootPath +"\\wwwroot\\UserFiles\\Pages\\";
            var path = GetMainPath() + clsGeneralProperty.PageFilePathFull;

            try{

                clsPage.InsertPage(CurrentUser.UserId, PageName, LangId, PageGroupId, PageTopCode, PageContentHTML, UnicId, path, PageContentText);
                return Json(new { res = "ok", msg = string.Format(success, save + " " + page) });
            }catch(Exception exp)
            {
                return Json(new { res = "nok", msg = string.Format(faild, save + " " + page) });
            }            
        }

        public IActionResult Edit(int PageId)
        {
            var vp = clsPage.GetvPageById(PageId);

            var pgs = clsPageGroup.GetPageGroupsListKeyValue(vp.LangMark);
            string pageGroup = Resource.GetData(CurrentLang.LangMark, "PageGroup");
            ViewData["DropDown_PageGroups"] = clsGeneralFunction.GetHtmlBootstrapDropDown(pgs, "PageGroups", "PageGroupId", pageGroup, vp.PageGroupId); //pgs.FirstOrDefault().Key);
            
            //var mainPath = _hostingEnvironment.ContentRootPath //+ clsGeneralProperty.FilePath 
            //    + "\\wwwroot\\UserFiles\\Pages\\";
            //var mainPath = _hostingEnvironment.ContentRootPath + clsGeneralProperty.PageFilePathFull;
            var mainPath = GetMainPath() + clsGeneralProperty.PageFilePathFull;    

            //انتقال فایلهای صفحه به یک مسیر موقت برای اینکه از این مسیر بتوان صفحه را
            //بصورت موقت تغییر داد و اگه تغییرات تصویب شد به صفحه ذخیره شده و تغییرات دائمی شود
            //البته بهتر است کپی صفحه هم گذاشته شود که قبل از اعمال تغییرات یک کپی از صفحه گرفته 
            //شود بمنظور بک آپ
            //شاخههای بدون منها محل نهایی ذخیره صفحه ها
            var newName = vp.UnicId +"_" + DateTime.Now.Year+"-"+DateTime.Now.Month+"-"+DateTime.Now.Day+"_"+ DateTime.Now.Hour+"-"+DateTime.Now.Minute+"-"+DateTime.Now.Second+"-"+DateTime.Now.Millisecond;
            ViewData["NewPath"] = newName;
            var newPath = mainPath  + newName;
            var oldPath = mainPath + vp.UnicId.Replace("-","");
            //Directory.Move(oldPath, newPath);
            clsGeneralFunction.DirectoryCopy(oldPath, newPath, true);

            ViewData["PageContentHTML_"] = vp.PageContentHTML.Replace(vp.UnicId.Replace("-",""), newName);
            
            
            //HttpContext.Session.Set("RoxyFilemanLastPath", "/wwwroot/UserFiles/Forms");
            SD.SetValue("RoxyFilemanRoot", "/wwwroot/UserFiles/Pages/"+newName);

            return View(vp);
            
        }

        public IActionResult SavePage(int PageId, string NewPath, string PageName, int LangId, int PageGroupId, string PageTopCode, string PageContentHTML, string PageContentText ){
            var langMark = CurrentLang.LangMark;
            var save = Resource.GetData(langMark, "Save"); 
            var page = Resource.GetData(langMark, "Page"); 
            var success = Resource.GetData(langMark, "Exec_Ok"); 
            var faild = Resource.GetData(langMark, "Exec_Nok"); 

            //var path = _hostingEnvironment.ContentRootPath +"\\wwwroot\\UserFiles\\Pages\\";
            var path = GetMainPath() + clsGeneralProperty.PageFilePathFull;

            try{

                clsPage.SavePage(PageId, NewPath, CurrentUser.UserId, PageName, LangId, PageGroupId, PageTopCode, PageContentHTML, path, PageContentText);
                return Json(new { res = "ok", msg = string.Format(success, save + " " + page) });
            }catch(Exception exp)
            {
                return Json(new { res = "nok", msg = string.Format(faild, save + " " + page) });
            }            
        }    

        [HttpPost]
        //[HttpGet]
        public IActionResult CancelPage(string pathUnicId ){
        //public JavascriptResult CancelPage(string pathUnicId ){
            var path = GetMainPath() + clsGeneralProperty.PageFilePathFull +'\\'+ pathUnicId;
            clsGeneralFunction.RemoveDir(path);

            //var langMark = CurrentLang.LangMark;
            //return RedirectToAction("Index", new { lang = langMark, Area="", Controller="Pages" });
            //return PartialView();
            //return JavaScript("window.location = 'http://www.google.co.uk'");
            //return JavaScript( "window.location = '" + Url.Action("Index","Pages") + "'" );
            //return Content(@"http://www.google.co.uk");
            return Content(@Url.Action("Index","Pages"));            
        }

        [HttpPost]
        public ActionResult Delete(int pageId)
        {
            //var path = GetMainPath() + clsGeneralProperty.PageFilePathFull;
            //clsPage.DeletePage(pageId, path);

            //return Redirect("/Admin/PageAdmin/");
            //var langMark = CurrentLang.LangMark;
            //return Redirect("/Admin/"+langMark+"/Pages/Index");
            //return Content(@Url.Action("Index","Pages"));            
 
 
            var langMark = CurrentLang.LangMark;
            var delete = Resource.GetData(langMark, "Delete"); 
            var page = Resource.GetData(langMark, "Page"); 
            var success = Resource.GetData(langMark, "Exec_Ok"); 
            var faild = Resource.GetData(langMark, "Exec_Nok"); 

            var path = GetMainPath() + clsGeneralProperty.PageFilePathFull;

            try{
                clsPage.DeletePage(pageId, path);

                //clsPage.SavePage(PageId, NewPath, CurrentUser.UserId, PageName, LangId, PageGroupId, PageTopCode, //PageContentHTML, path, PageContentText);
                return Json(new { res = "ok", msg = string.Format(success, delete + " " + page) });
            }catch(Exception exp)
            {
                return Json(new { res = "nok", msg = string.Format(faild, delete + " " + page) });
            }          
        }
    }
}