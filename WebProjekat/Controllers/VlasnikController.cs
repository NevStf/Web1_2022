using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProjekat.BazaPodataka;
using WebProjekat.Models;

namespace WebProjekat.Controllers
{
    public class VlasnikController : Controller
    {
        // GET: Vlasnik
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DodajFitnesCentar(FitnesCentar fc, Adresa a)
        {
            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.VLASNIK)
            {
                return RedirectToAction("Error404", "Home");
            }
            if (!BazaFitnesCentara.PostojiFitnesCentar(fc.IDCentra))
            {
                fc.KorisnickoImeVlasnika = BazaKorisnika.KorisnikPoKorisnickomImenu((Korisnik)Session["Korisnik"]).KorisnickoIme;
                string s = a.ToString();
                fc.AdresaFitnesCentra = s;
                fc.Obrisan = false;
                BazaKorisnika.KorisnikPoKorisnickomImenu((Korisnik)Session["Korisnik"]).FitnesCentri.Add(fc);
                BazaFitnesCentara.UpisFitnesCentar(fc);
                BazaKorisnika.IzmeniKorisnika(BazaKorisnika.KorisnikPoKorisnickomImenu((Korisnik)Session["Korisnik"]));
                ViewBag.Message = "Uspesno upisan smestaj";
                //((Korisnik)Session["Korisnik"])..Add(fc);
                return View();
            }
            else
            {
                ViewBag.Message = "Smestaj sa ovim IDjem vec postoji.";
                //((Korisnik)Session["Korisnik"])..Add(fc);
                return View();
            }
        }

        public ActionResult DodajFitnesCentar()
        {
            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.VLASNIK)
            {
                return RedirectToAction("Error404", "Home");
            }
            ViewData["FitnesCentri"] = BazaFitnesCentara.sviFC;
            return View();
        }

        public ActionResult PregledFitnesCentara()
        {
            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.VLASNIK)
            {
                return RedirectToAction("Error404", "Home");
            }

            ViewData["KorisnickoIme"] = ((Korisnik)Session["Korisnik"]).KorisnickoIme;
            BazaFitnesCentara.UcitajFitnesCentre();
            return View(BazaFitnesCentara.sviFC);
        }

        public ActionResult DodajTrenera()
        {
            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.VLASNIK)
            {
                return RedirectToAction("Error404", "Home");
            }

            ViewData["VlasnikKIme"] = ((Korisnik)Session["Korisnik"]).KorisnickoIme;
            return View();
        }

        [HttpPost]
        public ActionResult DodajTrenera(Korisnik k)
        {
            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.VLASNIK)
            {
                return RedirectToAction("Error404", "Home");
            }
            ViewData["VlasnikKIme"] = ((Korisnik)Session["Korisnik"]).KorisnickoIme;

            if (BazaKorisnika.PostojiKorisnickoIme(k))
            {
                ViewBag.Message = "Postoji korisnik sa tim korisnickim imenom";
                return View();
            }

            if (ModelState.IsValid)
            {
                k.Uloga = Uloga.TRENER;
                k.FitnesCentarUKomJeAngazovan = BazaFitnesCentara.VratiFitnesCentar(k.FitnesCentarUKomJeAngazovan);
                BazaKorisnika.UpisKorisnika(k);
                ViewBag.Message = "Uspesno registrovan";
                return View();
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

        public ActionResult BlokirajTrenera(string korisnickoIme)
        {

            Korisnik temp = BazaKorisnika.KorisnikPoKorisnickomImenu(new Korisnik()
            {
                KorisnickoIme = korisnickoIme
            });

            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.VLASNIK || temp == null || temp.Uloga != Uloga.TRENER)
            {
                return RedirectToAction("Error404", "Home");
            }

            BazaKorisnika.BlokirajKorisnika(temp);

            return RedirectToAction("PregledTrenera", "Vlasnik");
        }

        public ActionResult PregledTrenera()
        {
            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.VLASNIK)
            {
                return RedirectToAction("Error404", "Home");
            }

            string vlasnik = ((Korisnik)Session["Korisnik"]).KorisnickoIme;
            List<Korisnik> lista = BazaKorisnika.sviKorisnici.Where(k => k.Uloga == Uloga.TRENER 
                                        && k.FitnesCentarUKomJeAngazovan.KorisnickoImeVlasnika == vlasnik).ToList();
            return View(lista);
        }

        [HttpPost]
        public ActionResult IzmeniFitnesCentar(FitnesCentar fc)
        {
            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.VLASNIK)
            {
                return RedirectToAction("Error404", "Home");
            }
            if (ModelState.IsValid && ProveriAdresu(fc.AdresaFitnesCentra))
            {

                BazaFitnesCentara.IzmeniFitnesCentar(fc);
                ViewBag.Message = "Uspesno izmenjen fitnes centar";
                return RedirectToAction("PregledFitnesCentara", "Vlasnik");
            }
            else
            {
                ViewBag.Message = "Greska u adresi, probajte ponovo";
                return View(fc);
            }
        }

        public ActionResult IzmeniFitnesCentar(int id)
        {
            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.VLASNIK)
            {
                return RedirectToAction("Error404", "Home");
            }
            FitnesCentar fcen = BazaFitnesCentara.VratiFitnesCentar(new FitnesCentar() { IDCentra = id });
            if (fcen == null)
            {
                RedirectToAction("Error404", "Home");
            }
            return View(fcen);
        }

        
        public ActionResult ObrisiFitnesCentar(int id)
        {
            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.VLASNIK)
            {
                return RedirectToAction("Error404", "Home");
            }
            FitnesCentar fcen = BazaFitnesCentara.VratiFitnesCentar(new FitnesCentar() { IDCentra = id });
            BazaFitnesCentara.ObrisiFitnesCentar(fcen);
            if (fcen == null)
            {
                RedirectToAction("Error404", "Home");
            }
            return RedirectToAction("PregledFitnesCentara", "Vlasnik");
        }

        public ActionResult OdobriKomentare()
        {
            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.VLASNIK)
            {
                return RedirectToAction("Error404", "Home");
            }
            List<Komentar> komentari = BazaKometara.SviUObradiKomentariVlasnika(((Korisnik)Session["Korisnik"]).KorisnickoIme);
            return View(komentari);
            //return View(BazaKometara.sviK.Where(x => x.FitnesCentar.KorisnickoImeVlasnika == BazaKorisnika.KorisnikPoKorisnickomImenu((Korisnik)Session["Korisnik"]).KorisnickoIme));
        }

        public ActionResult OdobriKomentar(int id)
        {
            
            Komentar k = BazaKometara.VratiKomentar(id);

            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.VLASNIK 
                || k==null || k.FitnesCentar.KorisnickoImeVlasnika != ((Korisnik)Session["Korisnik"]).KorisnickoIme)
            {
                return RedirectToAction("Error404", "Home");
            }

            k.StanjeKomentara = Stanje.Odobren;
            BazaKometara.IzmeniKomentar(k);
            return RedirectToAction("OdobriKomentare", "Vlasnik");
        }
        
        public ActionResult OdbijKomentar(int id)
        {
            if (Session["Korisnik"] == null || ((Korisnik)Session["Korisnik"]).Uloga != Uloga.VLASNIK)
            {
                return RedirectToAction("Error404", "Home");
            }
            Komentar k = BazaKometara.VratiKomentar(id);
            k.StanjeKomentara = Stanje.Odbijen;
            BazaKometara.IzmeniKomentar(k);
            return RedirectToAction("OdobriKomentare", "Vlasnik");
        }

        public bool ProveriAdresu(string s)
        {
            string[] temp = s.Split(',');

            //ako ima manje od 3 param
            if (temp.Count() != 3)
            {
                return false;
            }

            //ako je bilo koji prazan
            foreach (string x in temp)
            {
                if (x.Trim().Equals(string.Empty))
                {
                    return false;
                }
            }

            //ako poslednji nije broj 
            if (!int.TryParse(temp[2], out int res))
            {
                return false;
            }

            //ako prvi nema 2 dela 
            if (temp[0].Split(' ').Count() < 2)
            {
                return false;
            }

            return true;

        }
    }

}