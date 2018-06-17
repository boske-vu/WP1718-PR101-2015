using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class Korisnik
    {

        public string Korisnicko_ime { get; set; }
        public string Lozinka { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Pol { get; set; }
        public string Jmbg { get; set; }
        public string Kontakt_telefon { get; set; }
        public string Email { get; set; }
        public string Uloga { get; set; }
        public List<Voznja> Voznja { get; set; }

        public Korisnik(string korisnicko_ime, string lozinka, string ime, string prezime, string pol, string jmbg, string kontakt_telefon,
        string email, string uloga, List<Voznja> voznja)
        {
            Korisnicko_ime = korisnicko_ime;
            Lozinka = lozinka;
            Ime = ime;
            Prezime = prezime;
            Pol = pol;
            Jmbg = jmbg;
            Kontakt_telefon = kontakt_telefon;
            Email = email;
            Uloga = uloga;
            Voznja = voznja;
        }

    }
}