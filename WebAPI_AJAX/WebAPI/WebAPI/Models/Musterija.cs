using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    [Serializable]
    public class Musterija : Korisnik
    {
        public Musterija(string korisnicko_ime, string lozinka, string ime, string prezime, Pol pol, string jmbg, string kontakt_telefon,
       string email, Uloge uloga) : base(korisnicko_ime, lozinka, ime, prezime, pol, jmbg, kontakt_telefon, email, uloga)
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
            Ulogovan = false;
            listaVoznja = new List<Voznja>();
            Filtrirane = new List<Voznja>();
            Sortirane = new List<Voznja>();
            Pretrazene = new List<Voznja>();
        }

        public Musterija()
        {
            listaVoznja = new List<Voznja>();
            Ulogovan = false;
            Filtrirane = new List<Voznja>();
            Sortirane = new List<Voznja>();
            Pretrazene = new List<Voznja>();
        }

    }
}