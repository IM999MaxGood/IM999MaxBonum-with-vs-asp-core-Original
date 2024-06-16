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
    public class clsPage : Page
    {
        public static List<Page> GetPages()
        {
            var opt = new DbContextOptions<IM999Max_DotNetCore>();
            var db = new IM999Max_DotNetCore(opt);
            var ps = db.Page;

            return ps.ToList();
        }

        public static List<vPage> GetvPages()
        {
            var opt = new DbContextOptions<IM999Max_DotNetCore>();
            var db = new IM999Max_DotNetCore(opt);
            var ps = db.vPage;

            return ps.ToList();
        }

        public static vPage GetvPageById(int _pageId)
        {
            return GetvPages().Where(x=> x.PageId == _pageId).FirstOrDefault();
        }

        public static List<Page> GetPagesByGroup(int _pageGroupId)
        {
            return GetPages().Where(x=> x.PageGroupId == _pageGroupId).ToList();
        }

        public static Page GetPageById(int _pageId){
            return GetPages().Where(x=> x.PageId == _pageId).FirstOrDefault();
        }

        //تابعی برای انتقال تصاویر  صفحه در حالت ایجاد صفحه
        public static string MoveImagesPage(string _html, string _guid, string _path)
        {
            //شاخههای بدون منها محل نهایی ذخیره صفحه ها
            var newPath = _path  + _guid.Replace("-","");
            var oldPath = _path + _guid;
            Directory.Move(oldPath, newPath);
            if(Directory.Exists(oldPath))
                Directory.Delete(oldPath);

            //Directory.Move(System.Web.HttpContext.Current.Server.MapPath(clsGeneralProperty.FilePath+ "Pages/" + guid.Trim()),
            //        System.Web.HttpContext.Current.Server.MapPath(clsGeneralProperty.FilePath + "Pages/" + pageId.ToString()));
            //if (Directory.Exists(HttpContext.Current.Server.MapPath(clsGeneralProperty.FilePath + "Pages/" + guid.Trim())))
            //    Directory.Delete(HttpContext.Current.Server.MapPath(clsGeneralProperty.FilePath + "Pages/" + guid.Trim()));

            return _html.Replace(_guid, _guid.Replace("-",""));
        }

        public static bool InsertPage(int _userId, string _pageName, int _langId, int _pageGroupId, string _pageTopCode, string _pageContentHTML, string _unicId, string _path, string _pageContentText){
            var opt = new DbContextOptions<IM999Max_DotNetCore>();
            var db = new IM999Max_DotNetCore(opt);

            using (var dbTran = db.Database.BeginTransaction())
            {
                try
                {
                    Page p = new Page();

                    p.CanDelete = true;
                    p.CreateDate = DateTime.Now;
                    p.CreateUser = _userId;
                    p.LangId = _langId;
                    p.LastModifyDate = DateTime.Now;
                    p.LastModifyUser = _userId;
                    p.PageContentHTML = _pageContentHTML;
                    p.PageContentMini = _pageContentText;
                    p.PageGroupId = _pageGroupId;
                    p.PageName = _pageName;
                    p.PageTopCode = _pageTopCode;
                    p.UnderConstract = false;
                    p.UnicId = _unicId;

                    //تغییر شاخه عکسهای صفحه
                    p.PageContentHTML = MoveImagesPage( _pageContentHTML, _unicId, _path);

                    db.Entry(p).State = EntityState.Added;
                    db.SaveChanges();

                    dbTran.Commit();
                    return true;
                }catch(Exception exp){
                    dbTran.Rollback();
                    throw new Exception("clsPage.InsertPage: "+exp.Message);
                }
            }
        }

        //تابعی برای انتقال تصاویر  صفحه در حالت ادیت صفحه
        public static string MoveImagesPage(string _html, string _guid, string _path, string _newPath)
        {
            //شاخههای بدون منها محل نهایی ذخیره صفحه ها
            var newPath = _path  + _guid.Replace("-","");
            var oldPath = _path + _newPath;
            if(Directory.Exists(newPath)){
                clsGeneralFunction.DeleteFolderAndAllContent(newPath);
                Directory.Delete(newPath);
            }

            Directory.Move(oldPath, newPath);
            if(Directory.Exists(oldPath)){
                //clsGeneralFunction.DeleteFolderAndAllContent(newPath);
                clsGeneralFunction.DeleteFolderAndAllContent(oldPath);
                Directory.Delete(oldPath);
            }

            return _html.Replace(_newPath, _guid.Replace("-",""));
        }

        public static bool SavePage(int _pageId, string _newPath, int _userId, string _pageName, int _langId, int _pageGroupId, string _pageTopCode, string _pageContentHTML, string _path, string _pageContentText){
            var opt = new DbContextOptions<IM999Max_DotNetCore>();
            var db = new IM999Max_DotNetCore(opt);

            using (var dbTran = db.Database.BeginTransaction())
            {
                try
                {
                    Page p = GetPageById(_pageId);

                    //p.CanDelete = true;
                    //p.CreateDate = DateTime.Now;
                    //p.CreateUser = _userId;
                    //p.LangId = _langId;
                    p.LastModifyDate = DateTime.Now;
                    p.LastModifyUser = _userId;
                    p.PageContentHTML = _pageContentHTML;
                    p.PageContentMini = _pageContentText;
                    p.PageGroupId = _pageGroupId;
                    p.PageName = _pageName;
                    p.PageTopCode = _pageTopCode;
                    p.UnderConstract = false;
                    //p.UnicId = _unicId;

                    //تغییر شاخه عکسهای صفحه
                    p.PageContentHTML = MoveImagesPage( _pageContentHTML, p.UnicId, _path, _newPath);

                    db.Entry(p).State = EntityState.Modified;
                    db.SaveChanges();

                    dbTran.Commit();
                    return true;
                }catch(Exception exp){
                    dbTran.Rollback();
                    throw new Exception("clsPage.SavePage: "+exp.Message);
                }
            }
        }

        public static Page GetPage(int _id)
        {
            var ps = GetPages();
            if(ps == null || ps.Count() == 0)
                return null;
            
            var p = ps.Where(x=> x.PageId == _id);
            if(p == null || p.Count() == 0)
                return null;

            return p.FirstOrDefault();
        }

        public static bool DeletePage(int _pageId, string _path)
        {
            var opt = new DbContextOptions<IM999Max_DotNetCore>();
            var db = new IM999Max_DotNetCore(opt);

            var p = GetPage(_pageId);

            if(p == null)
                throw new Exception("Page not found:DeletePage");
            
            using (var dbTran = db.Database.BeginTransaction())
            {
                try {
                    db.Entry(p).State = EntityState.Deleted;
                    db.SaveChanges();
                    dbTran.Commit();

                    //System.IO.Directory.Delete(System.Web.HttpContext.Current.Server.MapPath("~/UserFiles/Pages/" + p.PageId.ToString()), true);
                    //System.IO.Directory.Delete(System.Web.HttpContext.Current.Server.MapPath(clsGeneralProperty.FilePath+"Pages/" + p.PageId.ToString()), true);
                    var path = _path + p.UnicId.Replace("-", "");
                    try{
                        Directory.Delete(path , true);
                    }catch(Exception exp){
                        throw new Exception(exp.Message);
                    }
                    
                    return true;
                }catch(Exception exp){
                    dbTran.Rollback();
                    throw new Exception(exp.Message);
                }
            }
        }
    }
}