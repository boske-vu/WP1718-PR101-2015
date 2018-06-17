using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class Adresa
    {
        //u formatu: Ulica broj, Naseljeno mesto Pozivni broj mesta(npr.Sutjeska 3, Novi Sad 21000)
        public string Ulica { get; set; }
        public int Broj { get; set; }

        public string Mesto { get; set; }
        public string PozivniBroj { get; set; }

        public Adresa(string ulica, int broj, string mesto, string pozitivniBroj)
        {
            Ulica = ulica;
            Broj = broj;
            Mesto = mesto;
            PozivniBroj = pozitivniBroj;
        }

    }
}