using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebProjekat.Models;

namespace WebProjekat.Models
{
    /*Komentar
● Posetilac koji je ostavio komentar
● Fitnes centar na koji se komentar odnosi
● Tekst komentara
● Ocena (na skali od 1 do 5)
*/
    public enum Stanje { Odbijen, U_Obradi, Odobren }
    public class Komentar
    {
        public int IDKomentara { get; set; }
        public string KorisnickoIme { get; set; }

        public FitnesCentar FitnesCentar { get; set; }
        //public int IDAranzmana { get { return Aranzman.ID; } set { Aranzman = BazaPodataka.BazaAranzmana.VratiAranzman(value); } }
        public string TekstKomentara { get; set; }
        public int Ocena { get; set; }
        public Stanje StanjeKomentara { get; set; }
    }
}