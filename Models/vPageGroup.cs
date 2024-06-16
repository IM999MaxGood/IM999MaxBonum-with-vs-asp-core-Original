namespace IM999MaxBonum
{
    using System;
    using System.Collections.Generic;
    
    public partial class vPageGroup
    {
        public int PageGroupId { get; set; }
        public bool IsActive { get; set; }
        public bool CanEdit { get; set; }        
        public string PageGroupName { get; set; }
        public int LangId { get; set; }
        public string LangName { get; set; }
        public string LangMark { get; set; }
    }
}
