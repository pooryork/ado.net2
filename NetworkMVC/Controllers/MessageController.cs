using Entities;
using INetwork.BLL;
using NetworkBLL;
using NetworkMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NetworkMVC.Views
{
    public class MessageController : Controller
    {

        private readonly INetworkLogic networkLogic;

        public MessageController()
        {
            this.networkLogic = new NetworkLogic();
        }

        // GET: Message
        [Authorize]
        [HttpGet]
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(GetMs(networkLogic.GetByLogin(User.Identity.Name).IDUser, id));
        }

        [Authorize]
        [HttpPost]
        public ActionResult Index([Bind(Include = "idFriend, message")] int? idFriend, string message)
        {
            if (idFriend == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int? id = networkLogic.GetByLogin(User.Identity.Name).IDUser;
            networkLogic.SendMessage(id, idFriend, message);
           
            return View(GetMs(id, idFriend));
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult GetAllMessages()
        {
            return View(networkLogic.GetAllMessages());
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult DeleteMessages()
        {
            if (Request["Delete"] != null)
            {
                networkLogic.DeleteMessages();
            }

            return Redirect("GetAllMessages");
        }

        private IEnumerable<MessageWithNames> GetMs(int? id, int? idFriend)
        {
            var ms = new List<MessageWithNames>();
            foreach (Message m in networkLogic.GetMessagesByFriend(id, idFriend))
            {
                if (m.IDUser == id)
                {
                    ms.Add(new MessageWithNames(m, networkLogic.GetById(idFriend).Name, networkLogic.GetById(id).Name));
                }
                else
                {
                    ms.Add(new MessageWithNames(m, networkLogic.GetById(id).Name, networkLogic.GetById(idFriend).Name));
                }
            }
            return ms.AsEnumerable();
        }

    }
}