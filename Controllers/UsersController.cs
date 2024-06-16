using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using IM999MaxBonum.Models;
using IM999MaxBonum.Classes;

using IM999MaxBonum.Data;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Hosting;


namespace IM999MaxBonum.Controllers
{
    //public class UsersController: BaseController
    public class UsersController(IHostingEnvironment environment) : BaseController(environment)
    {
        public IActionResult Index()
        {
            if (CurrentUser != null)
                return RedirectToAction("Profile","Users");
                //return RedirectToAction("Index","Home");

            ViewData["PageName"] = Resource.GetData(CurrentLang.LangMark, "User_Login_Page");
            return View();
        }

        public IActionResult Profile(){
            if (CurrentUser == null)
                return RedirectToAction("Index","Users");
            
            ViewData["PageName"] = Resource.GetData(CurrentLang.LangMark, "User_Profile_Page");                
            return View(CurrentUser);
        }


        #region login
        /*
        [HttpGet]
        public IActionResult Login()
        {
            clsLoginLog.SetLogout(CurrentLoginLog, CurrentUser);
            
            //return PartialView();s
            return RedirectToAction("Index","Users");
        }
        */
        
        [HttpPost]
        public IActionResult UserLogin(string username, string email, string password, bool keepMe, string guid, string captcha = "-1")
        {
            var captcha_Nok = Resource.GetData(CurrentLang.LangMark, "Captcha_Nok");
            var login_Input_Error = Resource.GetData(CurrentLang.LangMark, "Login_Input_Error");
            var login_Error_Online = Resource.GetData(CurrentLang.LangMark, "Login_Error_Online");
            var exec_Ok = Resource.GetData(CurrentLang.LangMark, "Exec_Ok");
            var login = Resource.GetData(CurrentLang.LangMark, "Login");
            exec_Ok = string.Format(exec_Ok, login);
            

            //string captchaSession = HttpContext.Session.Get<string>("Captcha_"+guid);
            //string captchaSession = SC.GetValue("Captcha_"+guid);
            string captchaSession = SD.GetValue("Captcha_"+guid);
         
            if (string.IsNullOrWhiteSpace(captcha) || string.IsNullOrWhiteSpace(captcha.ToString()))
                return Json(new { res = "nok", msg = captcha_Nok });

            if (!(Convert.ToInt32(captchaSession) == Convert.ToInt32(captcha)))
                return Json(new { res = "nok", msg = captcha_Nok });
            
            User u ;
            if(!string.IsNullOrWhiteSpace(username))
                u = clsUser.ExistUserForLoginByUserName(username, password);
            else if(!string.IsNullOrWhiteSpace(email))
                u = clsUser.ExistUserForLoginByEmail(email, password);
            else
                return Json(new { res = "nok", msg = login_Input_Error });
                    
            if (u == null)
                return Json(new { res = "nok", msg = login_Input_Error });

                
            //if (u.IsOnline == true)
            //    throw new Exception("UsersController.UserLogin: "+login_Error_Online);

            //999/بستن لاگهای قبلی این کاربر
            //if (u.IsOnline == true)
            //{
            //    clsLoginLog.DisposeLogins(u.UserId);
            //}
            //clsLoginLog.DisposeLogins(CurrentLL);


            //استفاده از کوکی 
            //if (keepMe != null && keepMe  == true)
            //{
                CurrentLoginLog.ExpireMin = 24*60;
            //}
            clsLoginLog.SetLogin(CurrentLoginLog, u);

            //u = clsUser.CorrectOnlineUser(u);                  

            return Json(new { res = "ok", msg = exec_Ok });
        }
        #endregion login       

        #region logout
        [HttpGet]
        public IActionResult Logout()
        {
            clsLoginLog.SetLogout(CurrentLoginLog, CurrentUser);
            
            //return PartialView();
            return RedirectToAction("Index","Users");
        }

        /*
        [HttpPost]
        public IActionResult UserLogout()
        {
            var u = CurrentUser;
            if (u == null)
                return Json(new { res = "nok", msg = Resource.SeeYouLater });

            var ll = CurrentLoginLog;

            clsLoginLog.SetLogout(ll, u);
            CurrentUser = null;
            CurrentLL = null;


            //Response.Cookies.Clear();
            //var cookie = new HttpCookie("userID");
            //cookie.Value = "";
            //cookie.Expires = DateTime.Now.AddDays(-1);

            return Json(new { res = "ok", msg = Resource.SeeYouLater });
        }
        */
        #endregion logout



    }
}