﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProjekat.Models
{
    public class Adresa
    {
        public string Ulica { get; set; }
        public int Broj { get; set; }
        public string Mesto { get; set; }
        public string PostanskiBroj { get; set; }

        public override string ToString()
        {
            return Ulica + " " + Broj + ", " + Mesto + ", " + PostanskiBroj;
        }
    }
}