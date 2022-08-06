using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProjekat.BazaPodataka;
using WebProjekat.Models;

namespace WebProjekat.Controllers
{
    public class RegLogController : Controller
    {
        // GET: RegLog
        public ActionResult Register()
        {
            Session.Remove("Korisnik");
            return View();
        }

        [HttpPost]
        public ActionResult Register(Korisnik k)
        {

            if (BazaKorisnika.PostojiKorisnickoIme(k))
            {
                ViewBag.Message = "Postoji korisnik sa tim korisnickim imenom";
                return View();
            }

            if (ModelState.IsValid)
            {
                k.Uloga = Uloga.POSETILAC;
                k.Blokiran = false;
                //k.BrojOtkazanihRegistracija = 0;
                BazaKorisnika.UpisKorisnika(k);
                TempData["Success"] = "Uspesno registrovan";
                return RedirectToAction("Login", "RegLog");
            }

            if (ModelState.Values.ElementAt(2).Errors.Count > 0)
            {
                ViewBag.KorisnickoIme = ModelState.Values.ElementAt(2).Errors[0].ErrorMessage;
            }
            if (ModelState.Values.ElementAt(3).Errors.Count > 0)
            {
                ViewBag.Lozinka = ModelState.Values.ElementAt(3).Errors[0].ErrorMessage;
            }

            return View();
        }
        public ActionResult Login()
        {
            Session.Remove("Korisnik");
            if (TempData["Success"] != null)
            {
                ViewBag.Message = TempData["Success"];
                TempData.Remove("Success");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(Korisnik k)
        {
            //DataBase.UcitajKorisnike();
            Korisnik korisnik = BazaKorisnika.VratiKorisnika(k);
            if (korisnik != null && !korisnik.Blokiran)
            {
                Session["Korisnik"] = korisnik;
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Message = "Neispravno korisnicko ime ili lozinka.";
            return View();
        }

        public ActionResult Logout()
        {
            Session.Remove("Korisnik");
            return RedirectToAction("Index", "Home");
        }
    }
}