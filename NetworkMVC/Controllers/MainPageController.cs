using Entities;
using INetwork.BLL;
using NetworkBLL;
using NetworkDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetworkMVC.Controllers
{
    public class MainPageController : Controller
    {

        private readonly INetworkLogic networkLogic;

        public MainPageController()
        {
            this.networkLogic = new NetworkLogic();
        }

        // GET: MainPage
        
        [HttpGet]
        public ActionResult SingUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SingUp(User user)
        {
            if (ModelState.IsValid)
            {
                networkLogic.SingUp(user);
                return Redirect($"~/LogInLogOut/Login");
            }
            else
            {
                LoggerUtil.getLog("Logger").Info("New user wasnt added cause bad model!");
                return View();
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            return View(networkLogic.GetByLogin(User.Identity.Name));
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditPersonInfo()
        {
            return View(networkLogic.GetByLogin(User.Identity.Name));
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditPersonInfo(User u)
        {
            if (ModelState.IsValid)
            {
                networkLogic.Edit(u);
                return Redirect("Index");
            }
            else
            {
                LoggerUtil.getLog("Logger").Info("Editing was refused cause bad model!");
                return View();
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Friends()
        {
            return View(networkLogic.GetAllFriends(User.Identity.Name));
        }

        [Authorize(Roles ="admin")]
        [HttpGet]
        public ActionResult GetAllUsers()
        {
            return View(networkLogic.GetAllUsers());
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult DeleteUsers()
        {
            if (Request["Delete"] != null)
            {
                networkLogic.DeleteUsers();
            }

            return Redirect("GetAllUsers");
        }
    }
}