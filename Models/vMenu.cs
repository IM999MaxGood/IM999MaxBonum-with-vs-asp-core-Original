namespace IM999MaxBonum.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class vMenu
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public int LangId { get; set; }
        public string LangName { get; set; }
        
        public bool CanEdit { get; set; }
        public bool IsMainMenu { get; set; }
        public string MenuFileName { get; set; }             
    }
}