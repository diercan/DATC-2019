using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DADWeb.Controllers
{
    public class VotantiController : Controller
    {
        // GET: Votanti
        public ActionResult Index()
        {
            ViewBag.Title = "Votanți";

            return View();
        }
    }
}