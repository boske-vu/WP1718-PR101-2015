using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class Automobil
    {
       

        public Vozac Vozac { get; set; }
        public int GodisteAutomobila { get; set; }
        public int BrojRegistarskeOznake { get; set; }
        public int BrojTaksiVozila { get; set; }
        
        public Automobil(Vozac vozac, int godisteAutomobila, int brojRegistarskeOznake, int brojTaksiVozila)
        {
            Vozac = vozac;
            GodisteAutomobila = godisteAutomobila;
            BrojRegistarskeOznake = brojRegistarskeOznake;
            BrojTaksiVozila = brojTaksiVozila;
        }
    }
}