using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProjekat.BazaPodataka;
using WebProjekat.Models;

namespace WebProjekat.Controllers
{
    public class PosetilacController : Controller
    {
        // GET: Posetilac
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PrijaviSeNaTrening(int id)
        {
            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.POSETILAC)
            {
                return RedirectToAction("Error404", "Home");
            }

            Korisnik k = BazaKorisnika.KorisnikPoKorisnickomImenu((Korisnik)Session["Korisnik"]);
            GrupniTrening gp = BazaGrupnihTreninga.VratiGrupniTrening(id);

            if (gp.MaksBrojPosetilaca == gp.BrojPrijavljenihKorisnika)
            {
                ViewBag.Message = "Maksimalan broj prijavljenih za ovaj trening!";
                return RedirectToAction("PregledFitnesCentra", "FitnesCentri", new { id = gp.MestoOdrzavanja.IDCentra });
            }

            foreach (GrupniTrening grup in k.ListaGrupnihTreninga)
            {
                if (grup.IDGT == gp.IDGT)
                {
                    TempData["Message"] = "Vec ste prijavljeni na ovaj trening!";
                    return RedirectToAction("PregledFitnesCentra", "FitnesCentri", new { id = gp.MestoOdrzavanja.IDCentra });
                }
            }

            BazaKorisnika.DodajTreningPosetiocu(k, gp);
            ViewBag.Message = "Uspesno ste se prijavili na trening";
            return RedirectToAction("PregledFitnesCentra", "FitnesCentri", new { id = gp.MestoOdrzavanja.IDCentra });

        }

        public ActionResult PregledSvihTreninga()
        {
            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.POSETILAC)
            {
                return RedirectToAction("Error404", "Home");
            }

            Korisnik k = BazaKorisnika.KorisnikPoKorisnickomImenu((Korisnik)Session["Korisnik"]);
            return View(k.ListaGrupnihTreninga.Where(g=>g.Zavrsen).ToList());
        }

        [HttpPost]
        public ActionResult PregledSvihTreninga(FilterTreninga f)
        {
            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.POSETILAC)
            {
                return RedirectToAction("Error404", "Home");
            }

            Korisnik k = BazaKorisnika.KorisnikPoKorisnickomImenu((Korisnik)Session["Korisnik"]);
            List<GrupniTrening> rez = BazaGrupnihTreninga.Pretrazi(f, k.ListaGrupnihTreninga);

            return View(rez);
        }

        public ActionResult SortirajNaziv(int id)
        {
            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.POSETILAC)
            {
                return RedirectToAction("Error404", "Home");
            }
            Korisnik k = BazaKorisnika.KorisnikPoKorisnickomImenu((Korisnik)Session["Korisnik"]);
            List<GrupniTrening> rez = BazaGrupnihTreninga.SortirajPoNazivu(id, k.ListaGrupnihTreninga);

            return View("../Posetilac/PregledSvihTreninga", rez);
        }
        public ActionResult SortirajTip(int id)
        {
            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.POSETILAC)
            {
                return RedirectToAction("Error404", "Home");
            }
            Korisnik k = BazaKorisnika.KorisnikPoKorisnickomImenu((Korisnik)Session["Korisnik"]);
            List<GrupniTrening> rez = BazaGrupnihTreninga.SortirajPoTipu(id, k.ListaGrupnihTreninga);
            return View("../Posetilac/PregledSvihTreninga", rez);
        }
        public ActionResult SortirajVreme(int id)
        {
            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.POSETILAC)
            {
                return RedirectToAction("Error404", "Home");
            }
            Korisnik k = BazaKorisnika.KorisnikPoKorisnickomImenu((Korisnik)Session["Korisnik"]);
            List<GrupniTrening> rez = BazaGrupnihTreninga.SortirajPoVremenu(id, k.ListaGrupnihTreninga);
            return View("../Posetilac/PregledSvihTreninga", rez);
        }


        public ActionResult Komentarisi(int id)
        {

            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.POSETILAC)
            {
                return RedirectToAction("Error404", "Home");
            }

            ViewData["FitnesCentar"] = BazaFitnesCentara.VratiFitnesCentar(new FitnesCentar() { IDCentra = id });

            return View();
        }

        [HttpPost]
        public ActionResult Komentarisi(Komentar k)
        {
            ViewData.Clear();

            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.POSETILAC)
            {
                return RedirectToAction("Error404", "Home");
            }

            k.KorisnickoIme = ((Korisnik)Session["Korisnik"]).KorisnickoIme;
            k.FitnesCentar = BazaFitnesCentara.VratiFitnesCentar(k.FitnesCentar);

            if (ModelState.IsValid)
            {
                BazaKometara.DodajKomentar(k);
                TempData["Message"] = "Uspesno ste dodali komentar.";
            }

            return RedirectToAction("PregledFitnesCentra", "FitnesCentri", new { id = k.FitnesCentar.IDCentra });
        }

    }
}