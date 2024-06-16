namespace IM999MaxBonum.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsOnline { get; set; }
        public System.DateTime LastLoginDate { get; set; }
        public bool IsPersonel { get; set; }
        public System.DateTime RegisterDate { get; set; }
        public bool IsManager { get; set; }
        public bool IsConfirm { get; set; }
        public string ConfirmCode { get; set; }
        public bool IsActive { get; set; }
    }
}
