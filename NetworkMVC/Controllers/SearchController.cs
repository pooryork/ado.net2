using Entities;
using INetwork.BLL;
using NetworkBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NetworkMVC.Controllers
{
    public class SearchController : Controller
    {

        private readonly INetworkLogic networkLogic;

        public SearchController()
        {
            this.networkLogic = new NetworkLogic();
        }


        [HttpGet]
        [Authorize]
        public ActionResult SearchByName()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult SearchByName(string Name)
        {
            if (ModelState.IsValid)
            {
                var tmp = networkLogic.SearchByName(Name);
                return View(tmp);
            }

            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult SearchByPhone()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult SearchByPhone(string Phone)
        {
            if (ModelState.IsValid)
            {
                var tmp = networkLogic.SearchByPhone(Phone);
                return View(tmp);
            }

            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult SearchBySurname()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult SearchBySurname(string Surname)
        {
            if (ModelState.IsValid)
            {
                var tmp = networkLogic.SearchBySurname(Surname);
                return View(tmp);
            }

            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult SearchByTown()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult SearchByTown(string Town)
        {
            if (ModelState.IsValid)
            {
                var tmp = networkLogic.SearchByTown(Town);
                return View(tmp);
            }

            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.isFriend = networkLogic.GetAllFriends(User.Identity.Name).FirstOrDefault(x => x.IDUser == id) == null ? false : true;
            ViewBag.isUserHimself = networkLogic.GetById(id).Login.Equals(networkLogic.GetByLogin(User.Identity.Name).Login);
            return View(networkLogic.GetById(id));
        }

        [ActionName("Details")]
        [HttpPost]
        [Authorize]
        public ActionResult Details_Post([Bind(Include = "idUser")] int? idUser)
        {
            if (Request["Add"] != null)
            {
                networkLogic.AddFriend(networkLogic.GetByLogin(User.Identity.Name).IDUser, idUser);
            }
            else if (Request["Remove"] != null)
            {
                networkLogic.DeleteFriend(networkLogic.GetByLogin(User.Identity.Name).IDUser, idUser);
            }
            return Redirect("~/MainPage/Friends");
        }
    }
}