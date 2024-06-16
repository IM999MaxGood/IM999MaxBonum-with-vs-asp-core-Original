using System;
using System.Linq;
using System.Collections.Generic;

using IM999MaxBonum.Models;
//using System.Web.Mvc;

using IM999MaxBonum.Data;
using Microsoft.EntityFrameworkCore;

namespace IM999MaxBonum.Classes
{
    public class clsLoginLog : LoginLog
    {
        public static List<LoginLog> GetLoginLogs()
        {
            var opt = new DbContextOptions<IM999Max_DotNetCore>();
            var db = new IM999Max_DotNetCore(opt);
            return db.LoginLog.ToList();
        }

        #region GetLoginLog
        public static LoginLog GetLoginLogByGuid(string guid)
        {
            var ll = GetLoginLogs().Where(x => x.GUID.ToLower().Trim() == guid.ToLower().Trim() );
            if (ll == null || ll.Count() == 0)
                return null;
            return ll.First();
        }

        public static LoginLog GetLoginLogById(Int64 LLId)
        {
            var ll = GetLoginLogs().Where(x => x.LLId == LLId);
            if (ll == null || ll.Count() == 0)
                return null;
            return ll.First();
        }
        #endregion GetLoginLog

        public static LoginLog LoginAnonymous(string _guid, string _browser, string _ipAddress, string _os, string _mainPath)
        {
            //im999 daily check and delete temp guid folder in wwwroot\UserFiles\Pages after one week
            clsGeneralFunction.DoCheckDaily(DateTime.Now.Date, _mainPath);

            var opt = new DbContextOptions<IM999Max_DotNetCore>();
            var db = new IM999Max_DotNetCore(opt);

            LoginLog ll = new LoginLog();
            using(var dbTran = db.Database.BeginTransaction()){
                try {
                    ll.CreateDate = DateTime.Now;
                    ll.GUID = _guid;
                    ll.IpAddress = _ipAddress;
                    ll.Browser = _browser;
                    ll.OS = _os;
                    ll.ExpireMin = 24*60;//2; // 10;
                    ll.LastRefresh = DateTime.Now;
                    //db.Entry(ll).State = EntityState.Added;
                    //db.SaveChanges();

                    db.LoginLog.Add(ll);
                    db.SaveChanges();

                    dbTran.Commit();

                }catch(Exception exp){
                    dbTran.Rollback();
                    throw new Exception("Exception at clsLoginLog.LoginAnonymous(): ");
                }
            }

            return ll;
        }

        public static LoginLog SaveRefresh(LoginLog ll)
        {
            var opt = new DbContextOptions<IM999Max_DotNetCore>();
            var db = new IM999Max_DotNetCore(opt);

            ll.LastRefresh = DateTime.Now;
            db.Update(ll);
            db.SaveChanges();
            return ll;
        }

        public static LoginLog SetLogin(LoginLog ll, User user)
        {
            var opt = new DbContextOptions<IM999Max_DotNetCore>();
            var db = new IM999Max_DotNetCore(opt);

            if (ll.DisposeDate != null || ll.LogoutDate != null || ll.LoginDate != null || ll.UserId > 0)
            //if (ll.DisposeDate != null || ll.LogoutDate != null)
                throw new Exception("clsLoginLog.SetLogin: Resource.SaveLLProblem");
            
            using(var dbTran = db.Database.BeginTransaction()){
                try {
                    ll.UserId = user.UserId;
                    ll.LoginDate = DateTime.Now;
                    //db.Entry(ll).State = EntityState.Modified;
                    db.Update(ll);
                    db.SaveChanges();
                    

                    //999/ اگر کاربر از چند مرورگر لاگین کند
                    //user = clsUser.SetOnline(user);
                    user = clsUser.CorrectOnlineUser(user);

                    dbTran.Commit();

                    //999/ شاید بهتر باشد پروسیجیری باشد که فقط مقدار درست و غلط آنلاین بودن را برگرداند
                    //و در همینجا مقدار آنلاین و آفلاین بودن کاربر درست شود
                    //user = clsUser.CorrectOnlineUser(user);

                    return ll;
                }catch(Exception exp){
                    dbTran.Rollback();
                    throw new Exception("clsLoginLog.SetLogin: "+exp.Message );
                }
            }
        }

        public static LoginLog SaveSessionDB(LoginLog ll, string data)
        {
            var opt = new DbContextOptions<IM999Max_DotNetCore>();
            var db = new IM999Max_DotNetCore(opt);

            ll.SD = data;
            db.Update(ll);
            db.SaveChanges();
            return ll;
        }


        //999/ خروج کاربر لاگین کرده و اختصاص سشن جدید بصورت ناشناس
        public static LoginLog SetLogout(LoginLog ll, User u)
        {
            var opt = new DbContextOptions<IM999Max_DotNetCore>();
            var db = new IM999Max_DotNetCore(opt);

            if (ll.DisposeDate != null || ll.LogoutDate != null || ll.LoginDate == null || ll.UserId != u.UserId)
                throw new Exception("clsLoginLog.SetLogout: مشکل در ثبت LogoutLog وجود دارد");

            LoginLog retLL = ll;
            using (var dbTran = db.Database.BeginTransaction())
            {
                try
                {
                    ll.LogoutDate = DateTime.Now;
                    ll.DisposeDate = DateTime.Now;
                    
                    //db.Entry(ll).State = EntityState.Modified;
                    db.Update(ll);
                    
                    db.SaveChanges();

                    //999/ اگر کاربر از چند مرورگر لاگین کند
                    //بهتر است برای مشخص شدن لاگین بودن کاربر پروسیجری نوشته شود سمت بانک داده که به محض خارج
                    //شدن کاربر آی.دی کاربر را بگیرد و تمام لاگها را چک کند اگر کاربر در یکی از آنها لاگین بود
                    //آنلاین ثبت شود و اگر نبود آفلاین ثبت شود
                    //پروسیجر نوشته شد LogoutUser    
                    //و 
                    //یک پروسیجر دیگر براساس مدت زمان آخرین رفرش سیشن و زمان ابطال سشن را دیسپوز کند 
                    //پروسیجر نوشته شد DisposeLoginsAndSetOnline  
                    //و
                    //کاربرانی که وضعیتشان آنلاین ثبت شده و لاگ باز ندارند و بلعکس چه شود ؟؟؟؟؟
                    //clsUser.SetOffline(u);
                    u = clsUser.CorrectOnlineUser(u);

                    //999/در اینجا مشکل پیدا میشود سشن باطل شده و دوبار همان جی.یو.آی.دی را میدهد   
                    //retLL = LoginAnonymous(ll.GUID, ll.Browser, ll.IpAddress, ll.OS);
                    
                    dbTran.Commit();
                    return retLL;
                }
                catch (Exception exp)
                {
                    dbTran.Rollback();
                    return null;
                }
            }
        }   

        public static LoginLog Dispose(LoginLog ll){
            var opt = new DbContextOptions<IM999Max_DotNetCore>();
            var db = new IM999Max_DotNetCore(opt);

            if (ll.DisposeDate != null)
                return ll;

            using (var dbTran = db.Database.BeginTransaction())
            {
                try
                {
                    ll.DisposeDate = DateTime.Now;
                    
                    //db.Entry(ll).State = EntityState.Modified;
                    db.Update(ll);
                    
                    db.SaveChanges();
                    dbTran.Commit();
                    return ll;
                }
                catch (Exception exp)
                {
                    dbTran.Rollback();
                    return null;
                }
            }

        }
    }
}