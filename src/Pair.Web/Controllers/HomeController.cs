using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pair.Web.Models;
using Pair.Web.Services;
using Microsoft.Extensions.Options;
using Pair.Web.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Pair.Web.Controllers
{
    public class HomeController : Controller
    {
        public AppConfig AppConfig { get; }
        public HomeController(IOptions<AppConfig> appConfig)
        {
            AppConfig = appConfig.Value;
            PairingData.FilePath = AppConfig.FilePath;
        }

        // GET: Home
        public ActionResult Index()
        {
            List<CodePair> allPairs = PairingData.GetAllCodePairs();
            allPairs = allPairs.OrderBy(q => q.Count).ToList();
            return View(allPairs);
        }

        public ActionResult Edit()
        {
            List<CodePair> allPairs = PairingData.GetAllCodePairs();
            return View(allPairs);
        }

        [HttpPost]
        public ActionResult Edit(List<CodePair> pairs)
        {
            if (ModelState.IsValid)
            {
                PairingData.UpdateCodePairs(pairs);
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
            if (ModelState.IsValid)
            {
                PairingData.AddNew(newIndivisual);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Delete()
        {
            DeleteViewModel deleteViewModel = new DeleteViewModel();
            deleteViewModel.AllIndividuals = new List<Individual>();
            deleteViewModel.AllIndividuals = PairingData.GetAllIndividuals();
            return View(deleteViewModel);
        }


        [HttpPost]
        public ActionResult Delete(FormCollection collection)
        {
            if (ModelState.IsValid)
            {
               // PairingData.DeleteIndividual(individual);
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
