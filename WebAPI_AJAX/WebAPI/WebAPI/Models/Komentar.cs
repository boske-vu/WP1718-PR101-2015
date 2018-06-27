using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class Komentar
    {
    
     public string Opis { get; set; }
     public DateTime DatumObjave { get; set; }
     public Voznja Voznja { get; set; }
     public Ocene OcenaVoznje { get; set; }
        public Korisnik Korisnik { get; set; }
        public Komentar() { }
        public Komentar(string opis, DateTime datumObjave, Voznja voznja, Ocene ocenaVoznje, Korisnik k)
        {
            Opis = opis;
            DatumObjave = datumObjave;
            Voznja = voznja;
            OcenaVoznje = ocenaVoznje;
            Korisnik = k;
        }
    }
}