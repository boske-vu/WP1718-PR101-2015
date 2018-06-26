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
        public string BrojRegistarskeOznake { get; set; }
        public int BrojTaksiVozila { get; set; }
        public TipAutomobila Tip { get; set; }


        public Automobil()
        {
        }

        public Automobil(Vozac vozac, int godisteAutomobila, string brojRegistarskeOznake, int brojTaksiVozila, TipAutomobila t)
        {
            Vozac = vozac;
            GodisteAutomobila = godisteAutomobila;
            BrojRegistarskeOznake = brojRegistarskeOznake;
            BrojTaksiVozila = brojTaksiVozila;
            Tip = t;
        }
    }
}