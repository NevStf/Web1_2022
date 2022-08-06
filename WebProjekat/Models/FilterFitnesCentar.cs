using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models
{
    public class FilterFitnesCentar
    {
        //nazivu, adresi i godini otvaranja
        public string Naziv { get; set; }
        public string Adresa { get; set; }
        public int? GodinaOtvaranjaMin { get; set; }
        public int? GodinaOtvaranjaMax { get; set; }

    }
}