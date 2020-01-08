using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DADWeb.Controllers
{
    public class RapoarteController : Controller
    {
        // GET: Rapoarte
        public ActionResult Index()
        {
            ViewBag.Title = "Rapoarte Page";

            return View();
        }
    }
}