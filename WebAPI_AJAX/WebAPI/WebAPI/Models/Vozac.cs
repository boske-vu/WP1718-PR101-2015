using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class Vozac : Korisnik
    {
        public Automobil Automobil { get; set; }
        public Lokacija Lokacija { get; set; }

        public Vozac() { }

        public Vozac(string korisnicko_ime, string lozinka, string ime, string prezime, Pol pol, string jmbg, string kontakt_telefon, string email, Uloge uloga, Lokacija lokacija, Automobil automobil) : base(korisnicko_ime, lozinka, ime, prezime, pol, jmbg, kontakt_telefon, email, uloga)
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
            Voznja = new List<Models.Voznja>();
        }
    }
}