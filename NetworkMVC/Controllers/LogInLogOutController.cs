using Entities;
using INetwork.BLL;
using NetworkBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetworkMVC.Controllers
{
    public class LogInLogOutController : Controller
    {
    
        private readonly INetworkLogic networkLogic;

        public LogInLogOutController()
        {
            this.networkLogic = new NetworkLogic();
        }

        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login([Bind(Include = "Login, Password")] User user)
        {
            bool exist = networkLogic.LogIn(user.Login, user.Password) != null ? true : false;
            ViewBag.LogIn = exist;
            return View();
        }


        [Authorize]
        public ActionResult Logout()
        {
            return View();
        }
    }
}