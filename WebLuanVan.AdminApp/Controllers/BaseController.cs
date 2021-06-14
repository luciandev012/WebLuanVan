using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLuanVan.AdminApp.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            var session = HttpContext.Session.GetString("Token");
            if (session == null)
            {
                RedirectToAction("Login", "User");
            }
        }
    }
}
