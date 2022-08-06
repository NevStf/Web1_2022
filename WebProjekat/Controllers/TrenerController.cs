using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProjekat.BazaPodataka;
using WebProjekat.Models;

namespace WebProjekat.Controllers
{
    public class TrenerController : Controller
    {
        // GET: Trener
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DodajTrening(GrupniTrening gt)
        {
            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.TRENER)
            {
                return RedirectToAction("Error404", "Home");
            }

            DateTime compare = DateTime.Now.AddDays(3);

            if (gt.DatumiVremeTreninga < compare)
            {
                ViewBag.Message = "Trening mora biti napravljen minimum 3 dana unapred!";
                return View();
            }
            else
            {
                Korisnik k = BazaKorisnika.KorisnikPoKorisnickomImenu((Korisnik)Session["Korisnik"]);
                gt.MestoOdrzavanja = BazaFitnesCentara.VratiFitnesCentar(k.FitnesCentarUKomJeAngazovan);
                gt.ListaKorisnika = new List<string>();
                gt.Trener = k.KorisnickoIme;
                k.ListaGrupnihTreninga.Add(gt);
                BazaKorisnika.IzmeniKorisnika(k);
                BazaGrupnihTreninga.DodajGrupniTrening(gt);
                ViewBag.Message = "Uspesno napravljen grupni trening";
                return View();
            }
        }

        public ActionResult DodajTrening()
        {
            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.TRENER)
            {
                return RedirectToAction("Error404", "Home");
            }

            //ViewData["VlasnikKIme"] = ((Korisnik)Session["Korisnik"]).KorisnickoIme;
            return View();
        }

        public ActionResult PregledTreninga()
        {
            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.TRENER)
            {
                return RedirectToAction("Error404", "Home");
            }
            Korisnik k = BazaKorisnika.KorisnikPoKorisnickomImenu((Korisnik)Session["Korisnik"]);

            List<GrupniTrening> zavrseni = k.ListaGrupnihTreninga.Where(g => g.Zavrsen).ToList();
            List<GrupniTrening> buduci = k.ListaGrupnihTreninga.Where(g => !g.Zavrsen).ToList();
            Tuple<List<GrupniTrening>, List<GrupniTrening>> content = new Tuple<List<GrupniTrening>, List<GrupniTrening>>(zavrseni, buduci);

            return View(content);
        }

        [HttpPost]
        public ActionResult PregledTreninga(FilterTreninga f) {

            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.TRENER)
            {
                return RedirectToAction("Error404", "Home");
            }
            Korisnik k = BazaKorisnika.KorisnikPoKorisnickomImenu((Korisnik)Session["Korisnik"]);

            List<GrupniTrening> temp = BazaGrupnihTreninga.Pretrazi(f, k.ListaGrupnihTreninga);

            List<GrupniTrening> zavrseni = temp.Where(g => g.Zavrsen).ToList();
            List<GrupniTrening> buduci = k.ListaGrupnihTreninga.Where(g => !g.Zavrsen).ToList();
            Tuple<List<GrupniTrening>, List<GrupniTrening>> content = new Tuple<List<GrupniTrening>, List<GrupniTrening>>(zavrseni, buduci);

            return View(content);
        }

        public ActionResult SortirajNaziv(int id)
        {
            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.TRENER)
            {
                return RedirectToAction("Error404", "Home");
            }
            Korisnik k = BazaKorisnika.KorisnikPoKorisnickomImenu((Korisnik)Session["Korisnik"]);
       

            List<GrupniTrening> temp = k.ListaGrupnihTreninga.Where(g => g.Zavrsen).ToList();

            List<GrupniTrening> zavrseni = BazaGrupnihTreninga.SortirajPoNazivu(id,temp);
            List<GrupniTrening> buduci = k.ListaGrupnihTreninga.Where(g => !g.Zavrsen).ToList();
            Tuple<List<GrupniTrening>, List<GrupniTrening>> content = new Tuple<List<GrupniTrening>, List<GrupniTrening>>(zavrseni, buduci);

            return View("../Trener/PregledTreninga", content);
        }
        public ActionResult SortirajTip(int id)
        {
            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.TRENER)
            {
                return RedirectToAction("Error404", "Home");
            }
            Korisnik k = BazaKorisnika.KorisnikPoKorisnickomImenu((Korisnik)Session["Korisnik"]);
            

            List<GrupniTrening> temp = k.ListaGrupnihTreninga.Where(g => g.Zavrsen).ToList();

            List<GrupniTrening> zavrseni = BazaGrupnihTreninga.SortirajPoTipu(id, temp);
            List<GrupniTrening> buduci = k.ListaGrupnihTreninga.Where(g => !g.Zavrsen).ToList();
            Tuple<List<GrupniTrening>, List<GrupniTrening>> content = new Tuple<List<GrupniTrening>, List<GrupniTrening>>(zavrseni, buduci);

            return View("../Trener/PregledTreninga", content);
        }
        public ActionResult SortirajVreme(int id)
        {
            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.TRENER)
            {
                return RedirectToAction("Error404", "Home");
            }
            Korisnik k = BazaKorisnika.KorisnikPoKorisnickomImenu((Korisnik)Session["Korisnik"]);

            List<GrupniTrening> temp = k.ListaGrupnihTreninga.Where(g => g.Zavrsen).ToList();

            List<GrupniTrening> zavrseni = BazaGrupnihTreninga.SortirajPoVremenu(id, temp);
            List<GrupniTrening> buduci = k.ListaGrupnihTreninga.Where(g => !g.Zavrsen).ToList();
            Tuple<List<GrupniTrening>, List<GrupniTrening>> content = new Tuple<List<GrupniTrening>, List<GrupniTrening>>(zavrseni, buduci);

            return View("../Trener/PregledTreninga", content);
        }

        public ActionResult ObrisiTrening(int id)
        {

            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.TRENER)
            {
                return RedirectToAction("Error404", "Home");
            }

            GrupniTrening gp = BazaGrupnihTreninga.VratiGrupniTrening(id);
            if (gp.ListaKorisnika.Count > 0)
            {
                ViewBag.Message = "Trening ima upisane korisnike!";
                return RedirectToAction("PregledTreninga", "Trener");
            }
            else
            {
                BazaGrupnihTreninga.ObrisiTrening(gp);
                BazaKorisnika.ObrisiTrening(id, BazaKorisnika.KorisnikPoKorisnickomImenu((Korisnik)Session["Korisnik"]));
                return RedirectToAction("PregledTreninga", "Trener");
            }

        }

        [HttpPost]
        public ActionResult IzmeniTrening(GrupniTrening gp)
        {
            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.TRENER)
            {
                return RedirectToAction("Error404", "Home");
            }

            if (ModelState.IsValid)
            {
                BazaGrupnihTreninga.IzmeniTrening(gp);
                ViewBag.Message = "Uspesno izmenjen trening";
                return RedirectToAction("PregledTreninga", "Trener");
            }
            return View(gp);
        }

        public ActionResult IzmeniTrening(int id)
        {
            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.TRENER)
            {
                return RedirectToAction("Error404", "Home");
            }
            GrupniTrening gp = BazaGrupnihTreninga.VratiGrupniTrening(id);
            if (gp.IDGT == null)
            {
                RedirectToAction("Error404", "Home");
            }
            return View(gp);
        }

        public ActionResult DetaljiTrening(int id)
        {
            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.TRENER)
            {
                return RedirectToAction("Error404", "Home");
            }

            GrupniTrening gp = BazaGrupnihTreninga.VratiGrupniTrening(id);
            return View(gp);
        }

    }
}