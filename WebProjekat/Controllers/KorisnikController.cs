using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProjekat.Models;
using WebProjekat.BazaPodataka;

namespace WebProjekat.Controllers
{
    public class KorisnikController : Controller
    {
        public ActionResult IzmeniProfil()
        {
            if (Session["Korisnik"] == null)
            {
                return RedirectToAction("Error404", "Home");
            }
            
            Korisnik k = BazaKorisnika.KorisnikPoKorisnickomImenu((Korisnik)Session["Korisnik"]);

            return View(k);
        }

        [HttpPost]
        public ActionResult IzmeniProfil(Korisnik k)
        {

            if (Session["Korisnik"] == null)
            {
                return RedirectToAction("Error404", "Home");
            }
            //ovo mora zbog disabled taga, ne submituje se formi 
            k.KorisnickoIme = ((Korisnik)Session["Korisnik"]).KorisnickoIme;
            BazaKorisnika.IzmeniKorisnika(k);
            ViewBag.Message = "Uspesno izmenjen profil";
            return View(k);

        }
    }
}