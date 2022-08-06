using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebProjekat.Models;

namespace WebProjekat.BazaPodataka
{
    public class BazaKorisnika : Serijalizacija
    {
        //public static string path = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\";
        public static string KorisnikFile = "ListaKorisnika.xml";
        public static List<Korisnik> sviKorisnici;


        public static void UpisKorisnika(Korisnik k)
        {
            //k.ListaAranzmana = new List<Aranzman>();
            //k.ListaRezervacija = new List<Rezervacija>();
            k.ListaGrupnihTreninga = new List<GrupniTrening>();
            k.FitnesCentri = new List<FitnesCentar>();
            //k. = new FitnesCentar();
            sviKorisnici.Add(k);
            SacuvajListu(sviKorisnici, KorisnikFile);
        }

        public static bool PostojiKorisnickoIme(Korisnik k)
        {
            //sviKorisnici = new List<Korisnik>();
            foreach (Korisnik korisnik in sviKorisnici)
            {
                if (k.KorisnickoIme.Equals(korisnik.KorisnickoIme))
                {
                    return true;
                }
            }
            return false;
        }

        public static Korisnik VratiKorisnika(Korisnik kor)
        {
            foreach (Korisnik k in sviKorisnici)
            {
                if (k.KorisnickoIme == kor.KorisnickoIme && k.Lozinka == kor.Lozinka)
                {
                    return k;
                }
            }
            return null;
        }

        public static Korisnik KorisnikPoKorisnickomImenu(Korisnik kor)
        {
            foreach (Korisnik k in sviKorisnici)
            {
                if (k.KorisnickoIme == kor.KorisnickoIme)
                {
                    return k;
                }
            }
            return null;
        }

        public static void BlokirajKorisnika(Korisnik k)
        {
            try
            {
                int idx = sviKorisnici.IndexOf(k);
                sviKorisnici[idx].Blokiran = !sviKorisnici[idx].Blokiran;

                SacuvajListu(sviKorisnici, KorisnikFile);

                UcitajKorisnike();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void UcitajKorisnike()
        {

            sviKorisnici = UcitajListu<Korisnik>(KorisnikFile);
        }

        public static void IzmeniKorisnika(Korisnik korisnik)
        {
            try
            {
                Korisnik ko = sviKorisnici.Find(k => k.KorisnickoIme == korisnik.KorisnickoIme);
                int idx = sviKorisnici.IndexOf(ko);
                korisnik.Uloga = sviKorisnici[idx].Uloga;
                sviKorisnici[idx] = korisnik;

                SacuvajListu(sviKorisnici, KorisnikFile);

                UcitajKorisnike();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void ObrisiTrening(int idT, Korisnik k)
        {
            Korisnik temp = KorisnikPoKorisnickomImenu(k);
            GrupniTrening gt = temp.ListaGrupnihTreninga.Find(x => x.IDGT == idT);
            int idx = temp.ListaGrupnihTreninga.IndexOf(gt);
            temp.ListaGrupnihTreninga[idx].Obrisan = true;
            IzmeniKorisnika(temp);
        }

        public static void DodajTreningPosetiocu(Korisnik k, GrupniTrening gp)
        {
            BazaGrupnihTreninga.DodajKorisnikaUListu(k, gp);

            k.ListaGrupnihTreninga.Add(gp);
            IzmeniKorisnika(k);
            SacuvajListu(sviKorisnici, KorisnikFile);
            UcitajKorisnike();
        }
    }
}