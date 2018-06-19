using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class ProjekatController : Controller
    {
        // GET: Projekat
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegisterStart()
        {
            return View("Registracija");
        }

        
        public ActionResult LogIn(string korisnicko_ime, string password)
        {
            foreach (Musterija m in PostojeciKorisnici.ListaMusterija)
            {
                if (m.Korisnicko_ime == korisnicko_ime && m.Lozinka != password)
                {
                    return View("PogresnaSifra");
                }
                else if (m.Korisnicko_ime == korisnicko_ime && m.Lozinka == password)
                {
                    m.Ulogovan = true;
                    HttpContext.Application["Musterija"] = m;
                    return View("musterijaView", m);
                }
            }

            return View("PogresnaSifra");
        }

        public ActionResult Register(string ime, string prezime, string pol, string korisnicko_ime, string password, string email, string broj_telefona, string jmbg)
        {
            Pol p = Pol.Muski;
            switch (pol)
            {
                case "muski":
                    p = Pol.Muski;
                    break;
                case "zenski":
                    p = Pol.Zenski;
                    break;
            }

            Korisnik m = new Musterija(korisnicko_ime, password, ime, prezime, p, jmbg, broj_telefona, email, Uloge.Musterija);

            foreach (Korisnik k in PostojeciKorisnici.ListaMusterija)
            {
                if (k.Korisnicko_ime == m.Korisnicko_ime)
                {
                    return View("KorisnikPostoji");
                }
            }

            PostojeciKorisnici.ListaKorisnika.Add(m);
            PostojeciKorisnici.ListaMusterija.Add(m as Musterija);

            return View("Registrovani", m);

        }
    }
}