using IM999MaxBonum.Models;
using IM999MaxBonum.Data;

using Microsoft.EntityFrameworkCore;

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IM999MaxBonum.Classes
{
    public class clsMenu : Menu
    {
        public static List<Menu> GetMenus()
        {
            var opt = new DbContextOptions<IM999Max_DotNetCore>();
            var db = new IM999Max_DotNetCore(opt);
            var ms = db.Menu;

            return ms.ToList();
        }

        public static List<vMenu> GetvMenus()
        {
            var opt = new DbContextOptions<IM999Max_DotNetCore>();
            var db = new IM999Max_DotNetCore(opt);
            var vms = db.vMenu;

            return vms.ToList();
        }

        public static bool InsertMenu(Menu _m, string _fileName){
            var opt = new DbContextOptions<IM999Max_DotNetCore>();
            var db = new IM999Max_DotNetCore(opt);
            //var m = ConvertvMToM(_mv);
            _m.MenuFileName = _fileName;
            db.Entry(_m).State = EntityState.Added;
            db.SaveChanges();
            return true;
        }

        public static vMenu GetvMenu( int _id)
        {
            var vms = GetvMenus();
            if(vms == null || vms.Count() == 0)
                return null;

            var vm = vms.Where(x=> x.MenuId == _id);
            if(vm == null || vm.Count() == 0)
                return null;

            return vm.FirstOrDefault();
        }

        public static Menu GetMenu( int _id)
        {
            var ms = GetMenus();
            if(ms == null || ms.Count() == 0)
                return null;
            
            var m = ms.Where(x=> x.MenuId == _id);
            if(m == null || m.Count() == 0)
                return null;

            return m.FirstOrDefault();
        }

        public static bool DeleteMenu(int _menuId){
            var opt = new DbContextOptions<IM999Max_DotNetCore>();
            var db = new IM999Max_DotNetCore(opt);
            //var pg = ConvertvPgToPg( GetvPageGroup(_menuId));
            var m = GetMenu(_menuId);
            using (var dbTran = db.Database.BeginTransaction())
            {
                try {
                    db.Entry(m).State = EntityState.Deleted;
                    db.SaveChanges();
                    dbTran.Commit();
                    return true;
                }catch(Exception exp){
                    dbTran.Rollback();
                    throw new Exception(exp.Message);
                }
            }
        }       
    }
}

