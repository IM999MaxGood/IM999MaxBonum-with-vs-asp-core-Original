namespace IM999MaxBonum.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class LoginLog
    {
        public long LLId { get; set; }

        //999/ این فیلد جی.یو.آی.دی نگه میدارد که این جی.یو.آی.دی در کوکی در کلاینت ذخیره میشود
        //و این فیلد است که مشخص میکند سشین کلاینت کدام است پس باید کوکی سمت کلاینت ذخیره شود
        public string GUID { get; set; }
        public Nullable<int> UserId { get; set; }

        //999/ این دو فیلد برای خاتمه دادن به سیشن از سمت سرور است و تنظیم زمان خاتمه کوکی سمت کلاینت
        public int ExpireMin { get; set; }
        public System.DateTime LastRefresh { get; set; }



        public System.DateTime CreateDate { get; set; }
        public Nullable<System.DateTime> LoginDate { get; set; }
        public Nullable<System.DateTime> LogoutDate { get; set; }
        public Nullable<System.DateTime> DisposeDate { get; set; }
        public string Location { get; set; }
        public string IpAddress { get; set; }
        public string Browser { get; set; }
        public string OS { get; set; }
        public string SD { get; set; }
        
    }
}
