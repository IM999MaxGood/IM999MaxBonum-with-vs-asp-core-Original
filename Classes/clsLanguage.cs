using System.Linq;
using System.Collections.Generic;

using IM999MaxBonum.Models;

using IM999MaxBonum.Data;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IM999MaxBonum.Classes
{
    public class clsLanguage : Language
    {
        public static List<Language> GetLanguages()
        {
            var opt = new DbContextOptions<IM999Max_DotNetCore>();
            var db = new IM999Max_DotNetCore(opt);
            return db.Language.ToList();
        }

        public static Language GetLanguage(int langId)
        {
            var x = GetLanguages().Where(y =>y.LangId== langId);
            if (x == null || x.Count() == 0)
                return null;
            return x.First();
        }


        public static Language GetLanguage(string lang)
        {
            var x = GetLanguages().Where(y =>y.LangMark.Trim().ToLower() == lang.Trim().ToLower());
            if (x == null || x.Count() == 0)
                return null;
            return x.First();
        }

        public static SelectList GetLanguagesSelectList()
        {
            var ls = GetLanguages().Select(x => new SelectListItem() { Text = x.LangName, Value = x.LangId.ToString() }).ToList<SelectListItem>();
            return new SelectList(ls, "Value", "Text");
        }

        public static List<KeyValuePair<int, string>> GetLanguagesListKeyValue()
        {
            var ls = GetLanguages().Select(x => new KeyValuePair<int, string>(x.LangId, x.LangName) ).ToList<KeyValuePair<int, string>>();
            return ls;
        }
    }
}