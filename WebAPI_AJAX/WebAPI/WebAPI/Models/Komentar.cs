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
     public int OcenaVoznje { get; set; }

        public Komentar(string opis, DateTime datumObjave, Voznja voznja, int ocenaVoznje)
        {
            Opis = opis;
            DatumObjave = datumObjave;
            Voznja = voznja;
            OcenaVoznje = ocenaVoznje;
        }
    }
}