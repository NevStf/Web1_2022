using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebProjekat.Models;

namespace WebProjekat.BazaPodataka
{
    public class BazaKometara : Serijalizacija
    {
        //public static string path = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\";
        public static string KomentariFile = "ListaKomentara.xml";
        public static List<Komentar> sviK;


        public static void DodajKomentar(Komentar k)
        {
          
            if (sviK == null || sviK.Count == 0)
            {
                sviK = new List<Komentar>();
                k.IDKomentara = 1;
            }
            else
            {
                k.IDKomentara = sviK.LastOrDefault().IDKomentara + 1;
            }

            k.StanjeKomentara = Stanje.U_Obradi;

            sviK.Add(k);
            SacuvajListu(sviK, KomentariFile);
        }

        public static void UcitajKomentare()
        {
            sviK = UcitajListu<Komentar>(KomentariFile);
        }

        public static Komentar VratiKomentar(int id)
        {
            foreach (Komentar k in sviK)
            {
                if (k.IDKomentara == id)
                {
                    return k;
                }
            }
            return null;
        }

        public static void IzmeniKomentar(Komentar k)
        {
            try
            {
                Komentar ko = sviK.Find(x => x.IDKomentara == k.IDKomentara);
                int idx = sviK.IndexOf(ko);
                //k.StanjeKomentara = sviKorisnici[idx].Uloga;
                sviK[idx] = k;

                SacuvajListu(sviK, KomentariFile);

                UcitajKomentare();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static List<Komentar> SviOdobreniKomentariZaFitnesCentar(int idCentra) {
            return sviK.Where(k => k.StanjeKomentara == Stanje.Odobren && k.FitnesCentar.IDCentra == idCentra).ToList();
        }

        public static List<Komentar> SviUObradiKomentariVlasnika(string Vlasnik)
        {
            List<Komentar> temp = sviK.Where(k => k.StanjeKomentara == Stanje.U_Obradi && k.FitnesCentar.KorisnickoImeVlasnika == Vlasnik).ToList();
            return temp;
        }
    }
}