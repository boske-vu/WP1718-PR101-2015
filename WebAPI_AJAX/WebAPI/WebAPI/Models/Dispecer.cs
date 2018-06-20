using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class Dispecer: Korisnik
    {
        public Dispecer(string korisnicko_ime, string lozinka, string ime, string prezime, Pol pol, string jmbg, string kontakt_telefon,
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
        }

        public Dispecer()
        {
            Voznja = new List<Voznja>();
            Ulogovan = false;
        }
    }
}