using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
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

            foreach (Dispecer d in PostojeciKorisnici.ListaDispecera)
            {
                if (d.Korisnicko_ime == korisnicko_ime && d.Lozinka != password)
                {
                    return View("PogresnaSifra");
                }
                else if (d.Korisnicko_ime == korisnicko_ime && d.Lozinka == password)
                {
                    d.Ulogovan = true;
                    HttpContext.Application["Dispecer"] = d;
                    return View("dispecerView", d);
                }
            }

            foreach (Vozac v in PostojeciKorisnici.ListaVozaca)
            {
                if (v.Korisnicko_ime == korisnicko_ime && v.Lozinka != password)
                {
                    return View("PogresnaSifra");
                }
                else if (v.Korisnicko_ime == korisnicko_ime && v.Lozinka == password)
                {
                    v.Ulogovan = true;
                    HttpContext.Application["Vozac"] = v;
                    return View("VozacView", v);
                }
            }

            return View("NePostojiKorisnik");
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

            if (PostojeciKorisnici.ListaMusterija != null)
            {
                foreach (Korisnik k in PostojeciKorisnici.ListaMusterija)
                {
                    if (k.Korisnicko_ime == m.Korisnicko_ime)
                    {
                        return View("KorisnikPostoji");
                    }
                }
            }

            PostojeciKorisnici.ListaKorisnika.Add(m);
            PostojeciKorisnici.ListaMusterija.Add(m as Musterija);

                using (XmlWriter writer = XmlWriter.Create(@"C:\Users\HP\Desktop\Projakat\WP1718-PR101-2015\WebAPI_AJAX\WebAPI\WebAPI\baza.xml"))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Korisnici");

                    foreach (Korisnik k in PostojeciKorisnici.ListaMusterija)
                    {
                        writer.WriteStartElement("Musterije");


                    writer.WriteElementString("Ime", k.Ime);
                    writer.WriteElementString("Prezime", k.Prezime);
                    writer.WriteElementString("Pol", k.Pol.ToString());
                    writer.WriteElementString("KorisnickoIme", k.Korisnicko_ime);
                    writer.WriteElementString("Sifra", k.Lozinka);
                    writer.WriteElementString("JMBG", k.Jmbg);
                    writer.WriteElementString("KontaktTelefon", k.Kontakt_telefon);
                    writer.WriteElementString("EMail", k.Email);
                    writer.WriteElementString("Uloga", k.Uloga.ToString());

                    writer.WriteEndElement();
                    }

                    /*
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    */
               
                foreach (Korisnik k in PostojeciKorisnici.ListaDispecera)
                {
                    writer.WriteStartElement("Dispeceri");


                    writer.WriteElementString("Ime", k.Ime);
                    writer.WriteElementString("Prezime", k.Prezime);
                    writer.WriteElementString("Pol", k.Pol.ToString());
                    writer.WriteElementString("KorisnickoIme", k.Korisnicko_ime);
                    writer.WriteElementString("Sifra", k.Lozinka);
                    writer.WriteElementString("JMBG", k.Jmbg);
                    writer.WriteElementString("KontaktTelefon", k.Kontakt_telefon);
                    writer.WriteElementString("EMail", k.Email);
                    writer.WriteElementString("Uloga", k.Uloga.ToString());

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            



            return View("Registrovani", m);

        }
    }
}