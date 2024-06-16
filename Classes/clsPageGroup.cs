using IM999MaxBonum.Models;
using IM999MaxBonum.Data;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IM999MaxBonum.Classes
{
    public class clsPageGroup : PageGroup
    {
        public static vPageGroup GetvPageGroup( int _id)
        {
            var vpg = GetvPageGroups();
            if(vpg == null || vpg.Count() == 0)
                return null;
            
            var pg = vpg.Where(x => x.PageGroupId == _id);
            if(pg==null||pg.Count()==0)
                return null;
            
            return pg.FirstOrDefault();
        }

        public static vPageGroup GetvPageGroup(string _pgName, int _langId)
        {
            var vpg = GetvPageGroups();
            if(vpg == null || vpg.Count() == 0)
                return null;
            
            var pg = vpg.Where(x => x.LangId == _langId && x.PageGroupName.ToLower().Trim() == _pgName.ToLower().Trim());
            if(pg==null||pg.Count()==0)
                return null;
            
            return pg.FirstOrDefault();
        }

        public static vPageGroup GetvPageGroup(int _pageGroupId, string _pgName, int _langId){
            var vpg = GetvPageGroups();
            if(vpg == null || vpg.Count() == 0)
                return null;
            
            var pg = vpg.Where(x => x.PageGroupId != _pageGroupId && x.LangId == _langId && x.PageGroupName.ToLower().Trim() == _pgName.ToLower().Trim());
            if(pg==null||pg.Count()==0)
                return null;
            
            return pg.FirstOrDefault();

        }

        public static bool InsertPageGroup(vPageGroup _vPG){
            var opt = new DbContextOptions<IM999Max_DotNetCore>();
            var db = new IM999Max_DotNetCore(opt);
            var pg = ConvertvPgToPg(_vPG);
            db.Entry(pg).State = EntityState.Added;
            db.SaveChanges();
            return true;
        }

        public static bool SavePageGroup(vPageGroup _vPG)
        {
            var opt = new DbContextOptions<IM999Max_DotNetCore>();
            var db = new IM999Max_DotNetCore(opt);
            var pg = ConvertvPgToPg(_vPG);
            db.Entry(pg).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }

        public static bool DeletePageGroup(int _pageGroupId){
            var opt = new DbContextOptions<IM999Max_DotNetCore>();
            var db = new IM999Max_DotNetCore(opt);
            var pg = ConvertvPgToPg( GetvPageGroup(_pageGroupId));
            using (var dbTran = db.Database.BeginTransaction())
            {
                try {
                    var ps = clsPage.GetPagesByGroup(_pageGroupId);

                    /*
                    //999/حذف صفحاتی که در این گروه صفحه هستند
                    foreach (var item in ps)
                    {
                        db.Entry(item).State = EntityState.Deleted;                        
                    }
                    */
                    //999/تغییر گروه صفحات صفحاتی که در این گروه صفحه هستند به خالی
                    foreach (var item in ps)
                    {
                        item.PageGroupId = 0; 
                        db.Entry(item).State = EntityState.Modified;                        
                    }
                    
                    db.Entry(pg).State = EntityState.Deleted;
                    db.SaveChanges();
                    dbTran.Commit();
                    return true;
                }catch(Exception exp){
                    dbTran.Rollback();
                    throw new Exception(exp.Message);
                }
            }
        }




        public static PageGroup ConvertvPgToPg(vPageGroup _vPageGroup)
        {
            PageGroup pg = new PageGroup();
            pg.LangId = _vPageGroup.LangId;
            pg.PageGroupId = _vPageGroup.PageGroupId;
            pg.PageGroupName = _vPageGroup.PageGroupName;
            pg.CanEdit = _vPageGroup.CanEdit;
            pg.IsActive = _vPageGroup.IsActive;
            return pg;
        }

        public static List<vPageGroup> GetvPageGroups()
        { 
            var opt = new DbContextOptions<IM999Max_DotNetCore>();
            var db = new IM999Max_DotNetCore(opt);
            var pg = db.vPageGroup;
            if(pg == null || pg.Count() == 0)
                return null;
            return pg.ToList();
        }

        public static List<vPageGroup> GetvPageGroups(string langMark)
        { 
            var vpg = GetvPageGroups();
            if(vpg==null || vpg.Count() == 0)
                return null;
            return vpg.Where(x => x.LangMark.Trim().ToLower() == langMark.Trim().ToLower()).ToList();
        }

        public static SelectList GetPageGroupsSelectList(string langMark)
        {
            var pg = clsPageGroup.GetvPageGroups(langMark);
            if(pg == null)
                return null;
            var pgs = pg.Select(x => new SelectListItem() { Text = x.PageGroupName, Value = x.PageGroupId.ToString() }).ToList<SelectListItem>();
            return new SelectList(pgs, "Value", "Text");
        }

        public static List<KeyValuePair<int, string>> GetPageGroupsListKeyValue(){
            var pg = clsPageGroup.GetvPageGroups();
            if(pg == null)
                return null;
            var pgs = pg.Select(x => new KeyValuePair<int, string>(x.PageGroupId, x.PageGroupName) ).ToList<KeyValuePair<int, string>>();
            return pgs;
        }

        public static List<KeyValuePair<int, string>> GetPageGroupsListKeyValue(string _langMark){
            var pg = clsPageGroup.GetvPageGroups().Where(x=> x.LangMark.ToLower().Trim() == _langMark.ToLower().Trim() );
            if(pg == null)
                return null;
            var pgs = pg.Select(x => new KeyValuePair<int, string>(x.PageGroupId, x.PageGroupName) ).ToList<KeyValuePair<int, string>>();
            return pgs;
        }
    }
}