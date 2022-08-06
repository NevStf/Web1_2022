using System;
using System.Collections.Generic;
using WebProjekat.Models;

namespace WebProjekat.BazaPodataka
{
    public class BazaFitnesCentara : Serijalizacija
    {

        //public static string path = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\";
        public static string FitnesCentarFile = "ListaFitnesCentara.xml";
        public static List<FitnesCentar> sviFC;

        public static void UpisFitnesCentar(FitnesCentar fc)
        {
            sviFC.Add(fc);
            SacuvajListu(sviFC, FitnesCentarFile);
        }

        public static bool PostojiFitnesCentar(int ID)
        {
            foreach (FitnesCentar fitnesc in sviFC)
            {
                if (ID == fitnesc.IDCentra)
                {
                    return true;
                }
            }
            return false;
        }

        public static FitnesCentar VratiFitnesCentar(FitnesCentar fc)
        {
            foreach (FitnesCentar fitnesc in sviFC)
            {
                if (fitnesc.IDCentra == fc.IDCentra)
                {
                    return fitnesc;
                }
            }
            return null;
        }

        public static void UcitajFitnesCentre()
        {
            sviFC = new List<FitnesCentar>();
            sviFC = UcitajListu<FitnesCentar>(FitnesCentarFile);
            SortirajPoNazivu(0);
        }

        public static void IzmeniFitnesCentar(FitnesCentar fc)
        {
            try
            {
                FitnesCentar fitnesc = sviFC.Find(x => x.IDCentra == fc.IDCentra);
                fc.KorisnickoImeVlasnika = fitnesc.KorisnickoImeVlasnika;

                int idx = sviFC.IndexOf(fitnesc);
                sviFC[idx] = fc;

                SacuvajListu(sviFC, FitnesCentarFile);
                UcitajFitnesCentre();

                foreach (Korisnik k in BazaKorisnika.sviKorisnici)
                {
                    if (k.Uloga == Uloga.TRENER && k.FitnesCentarUKomJeAngazovan.IDCentra == fc.IDCentra)
                    {
                        k.FitnesCentarUKomJeAngazovan = fc;                       
                    }
                    else if (k.Uloga == Uloga.VLASNIK)
                    {
                        for(int i = 0; i < k.FitnesCentri.Count; i++)
                        {
                            if(k.FitnesCentri[i].IDCentra == fc.IDCentra)
                            {
                                k.FitnesCentri[i] = fc;
                            }
                        }
                    }
                }

                SacuvajListu(BazaKorisnika.sviKorisnici, BazaKorisnika.KorisnikFile);
                BazaKorisnika.UcitajKorisnike();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void ObrisiFitnesCentar(FitnesCentar fc)
        {
            try
            {
                fc.Obrisan = true;

                foreach (Korisnik k in BazaKorisnika.sviKorisnici)
                {
                    if (k.FitnesCentarUKomJeAngazovan.IDCentra == fc.IDCentra)
                    {
                        k.Blokiran = true;
                        //sacuvaj korisnike da ne mogu da se uloguju
                        SacuvajListu(BazaKorisnika.sviKorisnici, BazaKorisnika.KorisnikFile);
                    }
                }

                //sacuvaj fitnes centar
                SacuvajListu(sviFC, FitnesCentarFile);

                UcitajFitnesCentre();
                BazaKorisnika.UcitajKorisnike();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void Pretraga(FilterFitnesCentar f) {
            UcitajFitnesCentre();
            if (f != null) {
                if (!string.IsNullOrEmpty(f.Naziv)) {
                    sviFC = sviFC.FindAll(s => s.Naziv == f.Naziv);
                }
                if (!string.IsNullOrEmpty(f.Adresa)) {
                    sviFC = sviFC.FindAll(s => s.AdresaFitnesCentra == f.Adresa);
                }
                if (f.GodinaOtvaranjaMin.HasValue)
                {
                    sviFC = sviFC.FindAll(s => s.GodinaOtvaranja >= f.GodinaOtvaranjaMin);
                }
                if (f.GodinaOtvaranjaMax.HasValue)
                {
                    sviFC = sviFC.FindAll(s => s.GodinaOtvaranja <= f.GodinaOtvaranjaMax);
                }
            }
        }
        public static void SortirajPoAdresi(int order)
        {
            
            if (order == 0)
            {
                sviFC.Sort((x, y) => string.Compare(x.AdresaFitnesCentra, y.AdresaFitnesCentra));
            }
            if (order == 1) {
                sviFC.Sort((x, y) => string.Compare(y.AdresaFitnesCentra, x.AdresaFitnesCentra));
               
            }
        }
        public static void SortirajPoGodiniOtvaranja(int order)
        {
            
            if (order == 0)
            {
                sviFC.Sort((x, y) => x.GodinaOtvaranja.CompareTo(y.GodinaOtvaranja));
            }
            if (order == 1)
            {
                sviFC.Sort((x, y) => y.GodinaOtvaranja.CompareTo(x.GodinaOtvaranja));
                
            }
        }
        public static void SortirajPoNazivu(int order)
        {
            
            if (order == 0)
            {
                sviFC.Sort((x, y) => string.Compare(x.Naziv, y.Naziv));
            }
            if (order == 1)
            {
                sviFC.Sort((x, y) => string.Compare(y.Naziv, x.Naziv));
            }
        }

    }
}
