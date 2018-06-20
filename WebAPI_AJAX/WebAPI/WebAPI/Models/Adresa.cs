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
        public string Broj { get; set; }

        public string Mesto { get; set; }
        public string PostanskiBroj { get; set; }

        public Adresa() { }
        public Adresa(string ulica, string broj, string mesto, string postanskiBroj)
        {
            Ulica = ulica;
            Broj = broj;
            Mesto = mesto;
            PostanskiBroj = postanskiBroj;
        }

    }
}