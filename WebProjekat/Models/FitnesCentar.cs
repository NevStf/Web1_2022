using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models
{
    public class FitnesCentar
    {
        public string KorisnickoImeVlasnika { get; set; }
        public int IDCentra { get; set; }
        public string Naziv { get; set; }
        //adresa?
        public string AdresaFitnesCentra { get; set; }
        public int GodinaOtvaranja { get; set; }
        public int MesecnaClanarina { get; set; }
        public int GodisnjaClanarina { get; set; }
        public int CenaJednogTreninga { get; set; }
        public int CenaGrupnogTreninga { get; set; }
        public int CenaJednogTreningaSaPersonalnimTrenerom { get; set; }
        public bool Obrisan { get; set; }
    }
}