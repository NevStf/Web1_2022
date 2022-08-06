using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models
{
    public class FilterTreninga
    {
        public string NazivTreninga { get; set; }
        public string NazivFC { get; set; }
        public TipTreninga? Tip { get; set; }

        public DateTime? DatumIVremeMin { get; set; }
        public DateTime? DatumIVremeMax { get; set; }

    }
}