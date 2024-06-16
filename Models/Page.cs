namespace IM999MaxBonum.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Page
    {
        public int PageId { get; set; }
        public int LangId { get; set; }
        public int PageGroupId { get; set; }
        
        public string PageName { get; set; }
        public string PageContentMini { get; set; }
        public string PageContentHTML { get; set; }

        public System.DateTime CreateDate { get; set; }
        public System.DateTime LastModifyDate { get; set; }
        public int CreateUser { get; set; }
        public int LastModifyUser { get; set; }
        public string PageTopCode { get; set; }
        public bool CanDelete { get; set; }

        //999/ این فیلد در خود جی.یو.آی.دی نگه میدارد
        public string UnicId { get; set; }
	    public bool UnderConstract { get; set; }

    }
}