using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProjekat.Models;

namespace WebProjekat.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View(BazaPodataka.BazaFitnesCentara.sviFC);
        }
        [HttpPost]
        public ActionResult Index(FilterFitnesCentar f)
        {
            if (f.GodinaOtvaranjaMax <= f.GodinaOtvaranjaMin)
            {
                ViewBag.Message = "Gornja granica godine mora da bude veca od donje granice.";
            }
            else
            {
                BazaPodataka.BazaFitnesCentara.Pretraga(f);
            }

            return View(BazaPodataka.BazaFitnesCentara.sviFC);
        }

        public ActionResult SortirajNaziv(int id)
        {
            BazaPodataka.BazaFitnesCentara.SortirajPoNazivu(id);
            return RedirectToAction("Index", "Home");
        }
        public ActionResult SortirajAdresu(int id)
        {
            BazaPodataka.BazaFitnesCentara.SortirajPoAdresi(id);
            return RedirectToAction("Index", "Home");
        }
        public ActionResult SortirajGodinu(int id)
        {
            BazaPodataka.BazaFitnesCentara.SortirajPoGodiniOtvaranja(id);
            return RedirectToAction("Index", "Home");
        }
        //ako neko nema prava pristupa za stranicu
        public ActionResult Error404()
        {
            return View();
        }

    }
}