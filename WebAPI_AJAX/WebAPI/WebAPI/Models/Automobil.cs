using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class Automobil
    {
       

        public Vozac Vozac { get; set; }
        public string GodisteAutomobila { get; set; }
        public string BrojRegistarskeOznake { get; set; }
        public string BrojTaksiVozila { get; set; }
        public TipAutomobila Tip { get; set; }


        public Automobil()
        {
        }

        public Automobil(Vozac vozac, string godisteAutomobila, string brojRegistarskeOznake, string brojTaksiVozila, TipAutomobila t)
        {
            Vozac = vozac;
            GodisteAutomobila = godisteAutomobila;
            BrojRegistarskeOznake = brojRegistarskeOznake;
            BrojTaksiVozila = brojTaksiVozila;
            Tip = t;
        }
    }
}