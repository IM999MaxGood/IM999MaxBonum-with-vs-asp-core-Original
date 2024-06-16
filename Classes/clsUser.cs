using System;
using System.Linq;
using System.Collections.Generic;

using IM999MaxBonum.Models;
//using System.Web.Mvc;

using IM999MaxBonum.Data;
using Microsoft.EntityFrameworkCore;

namespace IM999MaxBonum.Classes
{
    public class clsUser : User
    {
        public static List<User> GetUsers()
        {
            var opt = new DbContextOptions<IM999Max_DotNetCore>();
            var db = new IM999Max_DotNetCore(opt);
            return db.User.ToList();
        }

        public static User GetUser(int userId)
        {
            var u = GetUsers().Where(x => x.UserId == userId);
            if (u == null || u.Count() == 0)
                return null;
            return u.First();
        }

        public static User GetUserById(int? userId)
        {
            if(userId==null)
                return null;
                
            var u = GetUsers().Where(x => x.UserId == userId);
            if (u == null || u.Count() == 0)
                return null;
            return u.First();
        }

        #region Logout

        #endregion Logout

        #region Login
        public static User ExistUserForLoginByUserName(string userName, string password)
        {
            var us = GetUsers().Where(x => x.UserName.ToLower().Trim() == userName.ToLower().Trim() & x.Password == password & x.IsConfirm == true & x.IsActive == true);
            if (us.Count() == 1)
                return us.First();
            else
                return null;
        }

        public static User ExistUserForLoginByEmail(string email, string password)
        {
            var us = GetUsers().Where(x => x.Email.ToLower().Trim() == email.ToLower().Trim() & x.Password == password & x.IsConfirm == true & x.IsActive == true);
            if (us.Count() == 1)
                return us.First();
            else
                return null;
        }

        #endregion Login

        /*
        public static User SetOffline(int userId)
        {
            var u = clsUser.GetUser(userId);
            if (u == null)
                return null;
            else
                return SetOffline(u);
        }

        public static User SetOnline(User u)
        {
            var opt = new DbContextOptions<IM999Max_DotNetCore>();
            var db = new IM999Max_DotNetCore(opt);


            //999/ اگر کاربر از چند مرورگر لاگین کند
            u.IsOnline = true;
            u.LastLoginDate = DateTime.Now;
            db.Entry(u).State = EntityState.Modified;
            db.SaveChanges();
            return u;
        }
        */

        /*
        public static User SetOffline(User u)
        {
            var opt = new DbContextOptions<IM999Max_DotNetCore>();
            var db = new IM999Max_DotNetCore(opt);

            //999/ اگر کاربر از چند مرورگر لاگین کند
            u.IsOnline = false;
            db.Update(u);

            db.SaveChanges();
            return u;
        }
        */
        public static User CorrectOnlineUser(User user)
        {
            //999/درست کردن وضعیت آنلاین بودن کاربر
            var opt = new DbContextOptions<IM999Max_DotNetCore>();
            var db = new IM999Max_DotNetCore(opt);
            var kv = new List<IM999MaxBonum.Classes.KeyValuePair<string, string>>();
            kv.Add(new IM999MaxBonum.Classes.KeyValuePair<string, string>("userId", user.UserId.ToString()));
            clsGeneralFunction.ExecSqlAndGetReturn(db, "CorrectUserOnline", kv);

            return GetUserById(user.UserId);
        }
    }
}
