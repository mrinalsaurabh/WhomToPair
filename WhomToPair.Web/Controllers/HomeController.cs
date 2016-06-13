using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace WhomToPair.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            List<Pair> allPairs = PairingData.GetAllPairs();
            allPairs = allPairs.OrderBy(q=>q.Count).ToList();
            return View(allPairs);
        }

        public ActionResult Edit()
        {
            List<Pair> allPairs = PairingData.GetAllPairs();
            return View(allPairs);
        }

        [HttpPost]
        public ActionResult Edit(List<Pair> pairs)
        {
            if (ModelState.IsValid)
            {
                PairingData.UpdatePairs(pairs);
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Individual newIndivisual)
        {
            if(ModelState.IsValid)
            {
                PairingData.AddNew(newIndivisual);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}