using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class PostojeciKorisnici
    {
        public static List<Musterija> ListaMusterija { get; set; }
        public static List<Vozac> ListaVozaca { get; set; }
        public static List<Dispecer> ListaDispecera { get; set; }

        public static List<Korisnik> ListaKorisnika { get; set; }

        public PostojeciKorisnici()
        {
            ListaMusterija = new List<Musterija>();
            ListaVozaca = new List<Vozac>();
            ListaDispecera = new List<Dispecer>();
            ListaKorisnika = new List<Korisnik>();
        }
    }
}