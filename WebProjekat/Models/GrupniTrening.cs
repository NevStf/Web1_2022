using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebProjekat.Models;

namespace WebProjekat.Models
{
    public enum TipTreninga {Yoga, LesMillesTone, BodyPump, Zumba, Drugo}
    public class GrupniTrening
    {
        /*Grupni Trening
● Naziv
● Tip treninga (yoga, les mills tone, body pump itd. )
● Fitnes centar gde se održava trening (veza sa klasom Fitnes centar)
● Trajanje treninga (izraženo u minutima)
● Datum i vreme treninga (čuvati u formatu dd/MM/yyyy HH:mm)
● Maksimalan broj posetilaca
● Spisak posetilaca (lista Korisnika sa ulogom Posetilac koji su se prijavili da pohađaju
trening)*/

        public int IDGT { get; set; }
        public string Trener { get; set; }
        public string Naziv { get; set; }
        public TipTreninga TipTreninga { get; set; }
        public FitnesCentar MestoOdrzavanja { get; set; }

        //trajanje treninga 
        [XmlIgnore]
        public DateTime DatumiVremeTreninga { get; set; }
        [XmlElement("DatumiVremeTreninga")]
        public string DR
        {
            get { return DatumiVremeTreninga.ToString("dd/MM/yyyy HH:mm").Replace("-", "/"); }
            set { DatumiVremeTreninga = DateTime.ParseExact(value, "dd/MM/yyyy HH:mm", null); }
        }
        public int MaksBrojPosetilaca { get; set; }

        public int VremeTrajanja { get; set; }

        public List<string> ListaKorisnika { get; set; }

        public bool Obrisan { get; set; }

        public int BrojPrijavljenihKorisnika { get; set; } 

        [XmlIgnore]
        public bool Zavrsen { get { return DatumiVremeTreninga.AddMinutes(VremeTrajanja) < DateTime.Now; } }
    }
}