using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using IM999MaxBonum.Data;
using IM999MaxBonum.Models;
using IM999MaxBonum.Classes;

//999/for globalition
using System.Globalization;
using System.Threading;

//999/for useing session
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

//999/for using OnActionExecuting
using Microsoft.AspNetCore.Mvc.Filters;

using Microsoft.AspNetCore.Hosting;

using System.IO;


namespace IM999MaxBonum.Controllers
{

    public class BaseController : Controller
    { 
        private IHostingEnvironment _hostingEnvironment;

        public BaseController(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        } 

        //999/ برای گرفتن مسیر اصلی سایت
        public string GetMainPath(){

            string mainPath;
            if(clsGeneralFunction.InsideIIS()){
                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\wwwroot\\lib\\fileman")) { 
                    mainPath = AppDomain.CurrentDomain.BaseDirectory;
                }
                else { 
                    mainPath = _hostingEnvironment.ContentRootPath;
                }
            }
            else{
                mainPath = _hostingEnvironment.ContentRootPath;
            }

            return mainPath;
        }

        public bool expireISOneDay; 
        private Language __CurrentLang;
        public Language CurrentLang{
            get{
                if(__CurrentLang != null)
                    return __CurrentLang;
                

                object lang = "ir";
                if( RouteData != null)
                    if( RouteData.Values["lang"] != null)
                        lang = RouteData.Values["lang"];

                __CurrentLang = clsLanguage.GetLanguage(lang.ToString());
                if (__CurrentLang == null)
                    throw new Exception("BaseController.CurrentLang : این زبان پشتیبانی نمیشود");
                return __CurrentLang;
            }
        }


        private User __CurrentUser;
        public User CurrentUser{
            get{
                if(__CurrentUser != null)
                    return __CurrentUser;
                
                var uId = CurrentLoginLog.UserId;
                __CurrentUser = clsUser.GetUserById(uId);
                return __CurrentUser;
            }
        }


        private LoginLog __CurrentLoginLog;
        public LoginLog CurrentLoginLog{
            get{
                if(__CurrentLoginLog != null)
                    return __CurrentLoginLog;


                __CurrentLoginLog = clsLoginLog.GetLoginLogByGuid(GUID_Session);
                if(__CurrentLoginLog != null){
                    if(__CurrentLoginLog.DisposeDate!= null || __CurrentLoginLog.LogoutDate!=null 
                        ||__CurrentLoginLog.LastRefresh.AddMinutes(__CurrentLoginLog.ExpireMin)<DateTime.Now)
                    {
                        clsLoginLog.Dispose(__CurrentLoginLog);
                        __GUID_Session = Guid.NewGuid().ToString();
                    }else{
                        __CurrentLoginLog = clsLoginLog.SaveRefresh(__CurrentLoginLog);    
                        return __CurrentLoginLog;
                    }    
                }

                string ip;
                
                //ip = HttpContext.Features.Get<Microsoft.AspNetCore.Http.Features .IHttpConnectionFeature>()?.RemoteIpAddress;
                
                //var connectionFeature = HttpContext.GetFeature<Microsoft.AspNet.HttpFeature.IHttpConnectionFeature>();
                //if (connectionFeature != null)
                //{
                //    ip = connectionFeature.RemoteIpAddress.ToString();
                //}
                
                //ip = HttpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString();

                //HttpContext.Request.Headers["IpAddress"];

                //999/services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                //IHttpContextAccessor _accessor;
                //ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
                //ip = new HttpContextAccessor().HttpContext.Connection.RemoteIpAddress.ToString();

                ip = HttpContext.Connection.RemoteIpAddress?.ToString();;

                //ip = HttpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString();

                string mainPath = GetMainPath() + clsGeneralProperty.PageFilePathFull;

                string os = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
                __CurrentLoginLog =  clsLoginLog.LoginAnonymous(GUID_Session, 
                        HttpContext.Request.Headers["User-Agent"],
                        ip,
                        os,
                        mainPath);
                return __CurrentLoginLog;
            }
        }
        
        /*این جی.یو.آی.دی را خودم ایجاد کردم برای مطمئن شدن از این که یک سشن به چند کاربر نسبت داده نشود*/
        private string __GUID_Session;
        public string GUID_Session{
            get{    
                if(__GUID_Session==null || __GUID_Session.Trim() == ""){
                    string guid = null;
                    //var guid = clsGeneralFunction.DecryptAfterFromHtml( HttpContext.Request.Cookies["GUIDS"]);
                    if(HttpContext.Request.QueryString.HasValue)
                        guid = HttpContext.Request.Query["guids"].ToString();
                    if(guid==null|| guid.Trim() == "")
                        guid = HttpContext.Request.Cookies["GUIDS"];
                    //guid = HttpContext.Request.Path;

                    if(guid!=null)
                        __GUID_Session = guid;
                    else
                        __GUID_Session = Guid.NewGuid().ToString();
                }
                return __GUID_Session;
            }
            //set{                
            //    CookieOptions option = new CookieOptions();  
            //    option.Expires = DateTime.Now.AddDays(1);
            //    HttpContext.Response.Cookies.Append( "GUIDS", value, option);
            //}
        }

        public SessionDb SD;
        public SessionCookie SC;
        public override void OnActionExecuting(ActionExecutingContext context){
            //999/ مقداردهی به سشن کوکی از کوکی    
            if(SD == null){
                SD = new SessionDb();
                SD.LoadSD(CurrentLoginLog);
            }

            //999/ مقداردهی به سشن کوکی از کوکی    
            if(SC == null){
                SC = new SessionCookie();
                SC.LoadSC(HttpContext);
            }

            //برای نرفتن غیر مسئول به بخش مدیریت
            //if( RouteData.Values["area"] != null && RouteData.Values["area"].ToString().ToLower().Trim() == "admin"){
            //    if(!CurrentUser.IsPersonel)
            //        throw new Exception("BaseController.OnActionExecuting : Not access.");               
            //}

/*
            var a = SC.GetValue("b");
            if(SC.IsEmpty()){
                SC.SetValue("a", "1");
                SC.SetValue("b", "2");
                SC.SetValue("c", "3");
            }
*/
            var currentLang = CurrentLang;
            var lang = currentLang.LangMark;

            if (lang != null && !string.IsNullOrWhiteSpace(lang.ToString()))
            {
                CultureInfo culture = new CultureInfo(lang.ToString());

                if (lang.ToString().Trim().ToLower() == "ir")
                {
                    if (Thread.CurrentThread.CurrentCulture.Calendar.GetType() != typeof(PersianCalendar))
                    {
                        culture = clsGeneralFunction.GetPersianCulture();
                        Thread.CurrentThread.CurrentCulture = culture;
                        Thread.CurrentThread.CurrentUICulture = culture;
                    }
                }
                else {
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                }

            }

            ViewBag.CurrentLang = currentLang;
            ViewBag.CurrentUser = CurrentUser;
            ViewBag.CurrentLoginLog = CurrentLoginLog;
            
            //999/for saving GUID_Session in cookie
            ViewBag.GUIDS = CurrentLoginLog.GUID;
            //ViewBag.GUIDS = clsGeneralFunction.EncryptAndToHtml(CurrentLoginLog.GUID);
            
            //999/for saving Session From in cookie
            //ViewBag.SF = GetSF();
            //ViewBag.SF = SF.GetSF();



            /*مشخص شدن رنگ سایت*/
            /*
            string cookieColor = "violet";
            if (Request.Cookies["color"] != null)
            {
                cookieColor = Request.Cookies["color"].Value.ToString();
                Session["color"] = cookieColor;
            }
            else if (Session["color"] != null)
            {
                cookieColor = Session["color"].ToString();
            }
            Session["color"] = cookieColor;
            */

            base.OnActionExecuting(context);

            //999/for saving Session cookie in cookie
            //ViewBag.SF = GetSF();
            ViewBag.SC = SC.GetSC();

            //999/for saving Session Db in Db
            //CurrentLoginLog = SD.SaveSD(CurrentLoginLog);
            //SD.SaveSD(CurrentLoginLog);
        }

        protected override void Dispose(bool disposing){
            //999/for saving Session Db in Db
            //CurrentLoginLog = SD.SaveSD(CurrentLoginLog);
            SD.SaveSD(CurrentLoginLog);
        }
        
    }
}
