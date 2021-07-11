using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebLuanVan.Data.ViewModels.ModelBinding
{
    public class User
    {
        public string Id { get; set; }
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        [Display(Name = "Tên")]
        public string FirstName { get; set; }
        [Display(Name = "Họ")]
        public string LastName { get; set; }
        [UIHint("IsActive")]
        public bool Status { get; set; }
    }
}
