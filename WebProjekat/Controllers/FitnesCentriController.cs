using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProjekat.BazaPodataka;
using WebProjekat.Models;

namespace WebProjekat.Controllers
{
    public class FitnesCentriController : Controller
    {
        // GET: FitnesCentri
        public ActionResult PregledFitnesCentra(int id)
        {
            ViewBag.Message = TempData["Message"];
            TempData.Clear();
            FitnesCentar fc = BazaFitnesCentara.VratiFitnesCentar(new FitnesCentar() { IDCentra = id });
            List<Komentar> komentari = BazaKometara.SviOdobreniKomentariZaFitnesCentar(fc.IDCentra);
            bool bioSam = false;

            if (Session["Korisnik"] != null)
            {
                Korisnik k = BazaKorisnika.KorisnikPoKorisnickomImenu((Korisnik)Session["Korisnik"]);

                foreach (GrupniTrening g in k.ListaGrupnihTreninga)
                {
                    if (g.MestoOdrzavanja.IDCentra == id)
                    {
                        bioSam = true;
                    }
                }
            }

            Tuple<FitnesCentar, List<Komentar>, bool> content = new Tuple<FitnesCentar, List<Komentar>, bool>(fc, komentari, bioSam);
            return View(content);
        }

    }
}