using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class Voznja
    {
        public DateTime Datum_i_vreme { get; set; }
        public Lokacija LokacijaNaKojuTaksiDolazi { get; set; }
        public TipAutomobila TipAutomobila { get; set; }
        public Musterija Musterija { get; set; }
        public Lokacija Odrediste { get; set; }
        public Dispecer Dispecer { get; set; }
        public Vozac Vozac { get; set; }
        public string Iznos { get; set; }
        public Komentar Komentar { get; set; }
        public StatusVoznje StatusVoznje { get; set; }

        public Voznja() { }
        public Voznja(DateTime datum_i_vreme, Lokacija lokacijaNaKojuTaksiDolazi, TipAutomobila tipAutomobila, Musterija m, Lokacija odrediste, Dispecer dispecer, Vozac vozac, string iznos, Komentar komentar, StatusVoznje statusVoznje)
        {
            Datum_i_vreme = datum_i_vreme;
            LokacijaNaKojuTaksiDolazi = lokacijaNaKojuTaksiDolazi;
            TipAutomobila = tipAutomobila;
            Musterija = m;
            Odrediste = odrediste;
            Dispecer = dispecer;
            Vozac = vozac;
            Iznos = iznos;
            Komentar = komentar;
            StatusVoznje = statusVoznje;
        }
        
    }
}