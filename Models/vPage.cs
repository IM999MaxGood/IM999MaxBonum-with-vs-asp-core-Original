namespace IM999MaxBonum.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class vPage
    {
        public int PageId { get; set; }

        public int LangId { get; set; }
        public string LangName { get; set; }
        public string LangMark { get; set; }


        public int PageGroupId { get; set; }
        public string PageGroupName { get; set; }
        
        public string PageName { get; set; }
        public string PageContentMini { get; set; }
        public string PageContentHTML { get; set; }

        public System.DateTime CreateDate { get; set; }
        public System.DateTime LastModifyDate { get; set; }
        public int CreateUser { get; set; }
        public int LastModifyUser { get; set; }
        public string PageTopCode { get; set; }
        public bool CanDelete { get; set; }

        
        public string UnicId { get; set; }
	    public bool UnderConstract { get; set; }
    }
}