using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebProjekat.Models;

namespace WebProjekat.BazaPodataka
{
    public class BazaGrupnihTreninga : Serijalizacija
    {
        //public static string path = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\";
        public static string GrupniTreningFile = "ListaGrupnihTreninga.xml";
        public static List<GrupniTrening> sviGP;


        public static void DodajGrupniTrening(GrupniTrening gt)
        {
            sviGP.Add(gt);
            SacuvajListu(sviGP, GrupniTreningFile);
        }

        public static void UcitajTreninge()
        {
            sviGP = UcitajListu<GrupniTrening>(GrupniTreningFile);
        }

        public static void ObrisiTrening(GrupniTrening gp)
        {
            try
            {
                gp.Obrisan = true;
                SacuvajListu(sviGP, GrupniTreningFile);
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            } 

            UcitajTreninge();
        }

        public static GrupniTrening VratiGrupniTrening(int id)
        {
            foreach (GrupniTrening gp in sviGP)
            {
                if (gp.IDGT == id)
                {
                    return gp;
                }
            }
            return null;
        }
        public static void IzmeniTrening(GrupniTrening gp)
        {
            try
            {
                GrupniTrening grup = sviGP.Find(x => x.IDGT == gp.IDGT);
                int idx = sviGP.IndexOf(grup);
                //sve sto se ne dobavlja sa forme prepisi 
                gp.ListaKorisnika = grup.ListaKorisnika;
                gp.MestoOdrzavanja = grup.MestoOdrzavanja;
                gp.Trener = grup.Trener;

                sviGP[idx] = gp;
                SacuvajListu(sviGP, GrupniTreningFile);
                UcitajTreninge();

                foreach(Korisnik k in BazaKorisnika.sviKorisnici)
                {
                    for(int i = 0; i < k.ListaGrupnihTreninga.Count; i++)
                    {
                        if(k.ListaGrupnihTreninga[i].IDGT == gp.IDGT)
                        {
                            k.ListaGrupnihTreninga[i] = gp;

                            SacuvajListu(BazaKorisnika.sviKorisnici, BazaKorisnika.KorisnikFile);
                            BazaKorisnika.UcitajKorisnike();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static List<GrupniTrening> sviTreninziPoFitnesCentru(int IDfitnesCentra)
        {
            return sviGP.Where(x => x.MestoOdrzavanja.IDCentra == IDfitnesCentra).ToList();
        }

        public static List<GrupniTrening> Pretrazi(FilterTreninga f, List<GrupniTrening> gt) {

            List<GrupniTrening> rez = gt;
            if (f != null)
            {
                if (!string.IsNullOrEmpty(f.NazivFC))
                {
                    rez = rez.FindAll(s => s.MestoOdrzavanja.Naziv == f.NazivFC);
                }
                if (!string.IsNullOrEmpty(f.NazivTreninga))
                {
                    rez = rez.FindAll(s => s.Naziv == f.NazivTreninga);
                }
                if (f.Tip.HasValue)
                {
                    rez = rez.FindAll(s => s.TipTreninga == f.Tip);
                }
                if (f.DatumIVremeMin.HasValue)
                {
                    rez = rez.FindAll(s => s.DatumiVremeTreninga >= f.DatumIVremeMin);
                }
                if (f.DatumIVremeMax.HasValue)
                {
                    rez = rez.FindAll(s => s.DatumiVremeTreninga <= f.DatumIVremeMax);
                }
            }
            return rez;
        }
        public static List<GrupniTrening> SortirajPoNazivu(int order, List<GrupniTrening> g)
        {
            List<GrupniTrening> rez = g;
            if (order == 0)
            {
                rez.Sort((x, y) => string.Compare(x.Naziv, y.Naziv));
            }
            if (order == 1)
            {
                rez.Sort((x, y) => string.Compare(y.Naziv, x.Naziv));

            }
            return rez;
        }

        public static List<GrupniTrening> SortirajPoTipu(int order, List<GrupniTrening> g)
        {
            List<GrupniTrening> rez = g;
            if (order == 0)
            {
                rez.Sort((x, y) => string.Compare(x.TipTreninga.ToString(), y.TipTreninga.ToString()));
            }
            if (order == 1)
            {
                rez.Sort((x, y) => string.Compare(y.TipTreninga.ToString(), x.TipTreninga.ToString()));

            }
            return rez;
        }

        public static List<GrupniTrening> SortirajPoVremenu(int order, List<GrupniTrening> g)
        {
            List<GrupniTrening> rez = g;

            if (order == 0)
            {
                rez.Sort((x, y) => DateTime.Compare(x.DatumiVremeTreninga, y.DatumiVremeTreninga));
            }
            if (order == 1)
            {
                rez.Sort((x, y) => DateTime.Compare(y.DatumiVremeTreninga, x.DatumiVremeTreninga));

            }
            return rez;

        }

        public static void DodajKorisnikaUListu (Korisnik k, GrupniTrening gp)
        {
            gp.ListaKorisnika.Add(k.KorisnickoIme);
            gp.BrojPrijavljenihKorisnika++;
            IzmeniTrening(gp);
            SacuvajListu(sviGP, GrupniTreningFile);
            UcitajTreninge();
        }
    }
}