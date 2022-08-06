using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebProjekat.Models;

namespace WebProjekat.Models
{
    public enum Uloga { POSETILAC, TRENER, VLASNIK }
    public class Korisnik
    {
        [MinLength(3, ErrorMessage = "Korisnicko ime mora da sadrzi barem 3 karaktera")]
        public string KorisnickoIme { get; set; }
        [MinLength(6, ErrorMessage = "Lozinka mora da sadrzi barem 6 karaktera")]
        public string Lozinka { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Pol { get; set; }
        public string Email { get; set; }
        [XmlIgnore]
        public DateTime DatumRodjenja { get; set; }
        public Uloga Uloga { get; set; }
        [XmlElement("DatumRodjenja")]
        public string DR
        {
            get { return DatumRodjenja.ToString("dd/MM/yyyy").Replace("-", "/"); }
            set { DatumRodjenja = DateTime.ParseExact(value, "dd/MM/yyyy", null); }
        }
        public bool Blokiran { get; set; }
        public List<GrupniTrening> ListaGrupnihTreninga { get; set; }
        public List<FitnesCentar> FitnesCentri { get; set; }
        //public int IDAngazovanja { get; set; }
        public FitnesCentar FitnesCentarUKomJeAngazovan { get; set; }
        public string VlasnikKodKogJeAngazovan { get; set; }


//● Lista grupnih treninga na koje je korisnik prijavljen(ako korisnik ima ulogu Posetioca)
//● Lista grupnih treninga na kojima je korisnik angažovan kao trener(ako korisnik ima
//ulogu Trenera)
//● Fitnes centar gde je korisnik angažovan(ako korisnik ima ulogu Trenera)
//● Fitnes centri čiji je korisnik vlasnik(ako korisnik ima ulogu Vlasnika)
    }
}