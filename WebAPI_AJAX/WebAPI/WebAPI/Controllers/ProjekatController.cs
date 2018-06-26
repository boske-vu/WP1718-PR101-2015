using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
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

        public ActionResult DodajVozacaStart()
        {
            return View("DodajVozaca");
        }

        public ActionResult ZatraziVoznjuMusterija()
        {
            return View();
        }

        public ActionResult EditVozac()
        {
            Vozac v = HttpContext.Application["Vozac"] as Vozac;
            return View(v);
        }

        public ActionResult IzmeniPodatkeMusterija(string ime, string prezime, string pol, string jmbg, string korisnicko, string lozinka, string mail, string broj)
        {
            Musterija ret = new Musterija();
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

            foreach (Musterija d in PostojeciKorisnici.ListaMusterija)
            {
                if (d.Korisnicko_ime == korisnicko)
                {
                    d.Ime = ime;
                    d.Prezime = prezime;
                    d.Pol = p;
                    d.Jmbg = jmbg;
                    d.Lozinka = lozinka;
                    d.Kontakt_telefon = broj;
                    d.Email = mail;
                    ret = d;
                    break;
                }
            }
            Korisnik korisnik = new Korisnik();

            foreach (Korisnik k in PostojeciKorisnici.ListaMusterija)
            {
                if (k.Korisnicko_ime == ret.Korisnicko_ime)
                {
                    korisnik = ret;
                    break;
                }
            }

            //Sacuvaj(Registrovani.SviZajedno);

            return View("musterijaWelcome", ret);
        }

        public ActionResult IzmeniPodatkeVozac(string ime, string prezime, string pol, string jmbg, string korisnicko, string lozinka, string mail, string broj)
        {
            Vozac ret = new Vozac();
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

            foreach (Vozac d in PostojeciKorisnici.ListaVozaca)
            {
                if (d.Korisnicko_ime == korisnicko)
                {
                    d.Ime = ime;
                    d.Prezime = prezime;
                    d.Pol = p;
                    d.Jmbg = jmbg;
                    d.Lozinka = lozinka;
                    d.Kontakt_telefon = broj;
                    d.Email = mail;
                    ret = d;
                    break;
                }
            }
            Korisnik korisnik = new Korisnik();

            foreach (Korisnik k in PostojeciKorisnici.ListaKorisnika)
            {
                if (k.Korisnicko_ime == ret.Korisnicko_ime)
                {
                    korisnik = ret;
                    break;
                }
            }

            return View("vozacView", ret);
        }

        public ActionResult MusterijaEdit()
        {
            Musterija m = HttpContext.Application["Musterija"] as Musterija;

            return View(m);
        }

        

        #region zatrazenaVoznja
        public ActionResult ZatrazenaVoznja(string ulica, string broj, string mesto, string postanski_broj, string tip_vozila, string x, string y)
        {
            Musterija m = HttpContext.Application["Musterija"] as Musterija;

            Voznja v = new Voznja();
            Adresa a = new Adresa(ulica, broj, mesto, postanski_broj);
            Lokacija l = new Lokacija(x, y, a);

            v.Datum_i_vreme = DateTime.Now;
            v.LokacijaNaKojuTaksiDolazi = l;

            if (tip_vozila == "kombi")
                v.TipAutomobila = TipAutomobila.kombi;
            else
                v.TipAutomobila = TipAutomobila.putnickiAutomobil;

            v.Musterija = m;

            v.StatusVoznje = StatusVoznje.KreiranaNaCekanju;
            /*
            Komentar k = new Komentar("d", DateTime.Now, v, 3);
            Dispecer d = new Dispecer();
            Vozac vozac = new Vozac();
            v.Komentar = k;
            v.Dispecer = d;
            v.Vozac = vozac;
            v.Iznos = 24;
            */
            m.listaVoznja.Add(v);

            PostojeciKorisnici.ListaSvihVoznji.Add(v);
            
                //dodajem voznju u listu musterija
                foreach (Musterija m1 in PostojeciKorisnici.ListaMusterija)
                {
                    if (m1.Korisnicko_ime.Equals(m.Korisnicko_ime))
                    {
                        m1.listaVoznja = m.listaVoznja;
                    }
                }

            foreach (Korisnik k in PostojeciKorisnici.ListaKorisnika)
            {
                if (k.Korisnicko_ime == m.Korisnicko_ime)
                {
                    k.listaVoznja = m.listaVoznja;
                }
            }

            /*
        string path = @"C:\Users\HP\Desktop\Projakat\WP1718-PR101-2015\WebAPI_AJAX\WebAPI\WebAPI\baza.xml";
        //XmlSerializer serializer = new XmlSerializer(typeof(Musterija));

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);

            var allNodes = xDoc.GetElementsByTagName("Voznja");
            var lastNode = allNodes[allNodes.Count - 1];
            // XmlElement node = SerializeToXmlElement(new Voznja(v.Datum_i_vreme, v.LokacijaNaKojuTaksiDolazi, v.TipAutomobila, m, v.Odrediste, v.Dispecer, v.Vozac, v.Iznos, v.Komentar, v.StatusVoznje));
            XmlElement node = SerializeToXmlElement(new Voznja(DateTime.Now, l, TipAutomobila.kombi, m2, l, d, vozac, 55, k, StatusVoznje.Formirana));

            XmlNode importNode = xDoc.ImportNode(node, true);
            xDoc.DocumentElement.AppendChild(importNode);
            xDoc.Save(path);

        */

            return View("ZatrazenaVoznja", v);
        }
#endregion
        #region dodajVozaca
        public ActionResult DodajVozaca(string ime, string prezime, string pol, string korisnicko_ime, string jmbg, string lozinka, string email, string kontakt_broj, string ulica, string broj, string mesto, string postanski_broj, string godiste, string reg, string taxiBroj, string tip)
        {
            TipAutomobila ti = TipAutomobila.kombi;

            switch (tip)
            {
                case "kombi":
                    ti = TipAutomobila.kombi;
                    break;
                case "putnicki":
                    ti = TipAutomobila.putnickiAutomobil;
                    break;
            }

            Pol p = Pol.Muski;
            if (pol.Equals("muski"))
                p = Pol.Muski;
            else
                p = Pol.Zenski;

            Vozac v = new Vozac(korisnicko_ime, lozinka, ime, prezime, p, jmbg, kontakt_broj, email, Uloge.Vozac, ulica, broj, mesto, postanski_broj);

            Automobil a = new Automobil(v, godiste, reg, taxiBroj, ti);

            v.Automobil = a;

            foreach (Vozac v1 in PostojeciKorisnici.ListaVozaca)
            {
                if (v1.Korisnicko_ime == v.Korisnicko_ime)
                {
                    return View("KorisnikPostoji");
                }
            }

            foreach (Korisnik k in PostojeciKorisnici.ListaKorisnika)
            {
                if (k.Korisnicko_ime == v.Korisnicko_ime)
                {
                    return View("KorisnikPostoji");
                }
            }

            string path = @"C:\Users\HP\Desktop\Projakat\WP1718-PR101-2015\WebAPI_AJAX\WebAPI\WebAPI\baza.xml";

            if (System.IO.File.Exists(path))
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(path);

                var allNodes = xDoc.GetElementsByTagName("Vozac");
                var lastNode = allNodes[allNodes.Count - 1];
                XmlElement node = SerializeToXmlElement(new Vozac(korisnicko_ime, lozinka, ime, prezime, p, jmbg, kontakt_broj, email, Uloge.Vozac, ulica, broj, mesto, postanski_broj));
                XmlNode importNode = xDoc.ImportNode(node, true);
                xDoc.DocumentElement.AppendChild(importNode);
                xDoc.Save(path);
            }

            PostojeciKorisnici.ListaVozaca.Add(v);
            PostojeciKorisnici.ListaKorisnika.Add(v);

            return View("VozacDodan", v);
        }
#endregion

        #region login
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
        #endregion

#region serializeToXMl
        public static XmlElement SerializeToXmlElement(object o)
        {
            XmlDocument doc = new XmlDocument();
            using (XmlWriter writer = doc.CreateNavigator().AppendChild())
            {
                new XmlSerializer(o.GetType()).Serialize(writer, o);
            }
            return doc.DocumentElement;
        }
        #endregion

#region register
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

            string path = @"C:\Users\HP\Desktop\Projakat\WP1718-PR101-2015\WebAPI_AJAX\WebAPI\WebAPI\baza.xml";
            //XmlSerializer serializer = new XmlSerializer(typeof(Musterija));
            if (System.IO.File.Exists(path))
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(path);

                var allNodes = xDoc.GetElementsByTagName("Musterija");
                var lastNode = allNodes[allNodes.Count - 1];
                XmlElement node = SerializeToXmlElement(new Musterija(korisnicko_ime, password, ime, prezime, p, jmbg, broj_telefona, email, Uloge.Musterija));
                XmlNode importNode = xDoc.ImportNode(node, true);
                xDoc.DocumentElement.AppendChild(importNode);
                xDoc.Save(path);
            }
            else
            {
                Console.WriteLine("Greska jer nema dispecera");
            }
            #region commentWriteToXML 
            /* 
            string path = @"C:\Users\HP\Desktop\Projakat\WP1718-PR101-2015\WebAPI_AJAX\WebAPI\WebAPI\baza.xml";
            if (!System.IO.File.Exists(path))
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.Indent = true;
                xmlWriterSettings.NewLineOnAttributes = true;
                using (XmlWriter writer = XmlWriter.Create(path, xmlWriterSettings))
                {
                    foreach (Korisnik k in PostojeciKorisnici.ListaMusterija)
                    {
                        writer.WriteStartElement("Musterije");


                        writer.WriteElementString("Imek", k.Ime);
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
                }
            }
            else
            {
                XDocument xDocument = XDocument.Load(path);
                XElement root = xDocument.Element("Korisnici");
                IEnumerable<XElement> rows = root.Descendants("Musterije");
                
                XElement firstRow = rows.First();
                firstRow.AddBeforeSelf(
                   
                new XElement("Ime", ime);
                new XElement("Prezime", prezime);
                new XElement("Pol", pol.ToString());
                new XElement("KorisnickoIme", korisnicko_ime);
                new XElement("Sifra", password);
                new XElement("JMBG", jmbg);
                new XElement("KontaktTelefon", broj_telefona);
                new XElement("EMail", email);
                new XElement("Uloga", Uloge.Musterija.ToString());
                xDocument.Save(path);
            }

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


                writer.WriteEndElement();
                writer.WriteEndDocument();

                /*
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
            */
#endregion
            return View("Registrovani", m);
            }
        #endregion


        public ActionResult DispecerEdit()
        {
            Dispecer d = HttpContext.Application["Dispecer"] as Dispecer;
            return View(d);
        }


        

        public ActionResult VratiProfilMusterija()
        {
            Musterija m = HttpContext.Application["Musterija"] as Musterija;

            return View("musterijaView", m);
        }

        public ActionResult VratiProfilDisp()
        {
            Dispecer d = HttpContext.Application["Dispecer"] as Dispecer;

            return View("dispecerView", d);
        }

        public ActionResult OdjaviMusterija()
        {
            foreach (Musterija m in PostojeciKorisnici.ListaMusterija)
            {
                if (m == HttpContext.Application["Musterija"])
                {
                    m.Ulogovan = false;
                    HttpContext.Application["Musterija"] = null;
                }
            }
            return View("Index");
        }

        public ActionResult OdjaviDispecer()
        {
            foreach (Dispecer d in PostojeciKorisnici.ListaDispecera)
            {
                if (d == HttpContext.Application["Dispecer"])
                {
                    d.Ulogovan = false;
                    HttpContext.Application["Dispecer"] = null;
                }
            }
            return View("Index");
        }

        public ActionResult OdjaviVozac()
        {
            foreach (Vozac v in PostojeciKorisnici.ListaVozaca)
            {
                if (v == HttpContext.Application["Vozac"])
                {
                    v.Ulogovan = false;
                    HttpContext.Application["Vozac"] = null;
                }
            }
            return View("Index");
        }

        public ActionResult IzmeniPodatkeDispecer(string ime, string prezime, string pol, string jmbg, string korisnicko, string lozinka, string mail, string broj)
        {
            Dispecer ret = new Dispecer();

            Pol p = Pol.Muski;
            switch (pol)
            {
                case "Muski":
                    p = Pol.Muski;
                    break;
                case "Zenski":
                    p = Pol.Zenski;
                    break;
            }

            foreach (Dispecer d in PostojeciKorisnici.ListaDispecera)
            {
                if (d.Korisnicko_ime == korisnicko)
                {
                    d.Ime = ime;
                    d.Prezime = prezime;
                    d.Pol = p;
                    d.Jmbg = jmbg;
                    d.Lozinka = lozinka;
                    d.Kontakt_telefon = broj;
                    d.Email = mail;
                    ret = d;
                    break;
                }
            }

            Korisnik korisnik = new Korisnik();

            foreach (Korisnik k in PostojeciKorisnici.ListaKorisnika)
            {
                if (k.Korisnicko_ime == ret.Korisnicko_ime)
                {
                    korisnik = ret;
                    break;
                }
            }

          //  Sacuvaj(Registrovani.SviZajedno);
            return View("dispecerView", ret);
        }


        public ActionResult DispecerZakazujeVoznju()
        {
            Dispecer d = HttpContext.Application["Dispecer"] as Dispecer;
            return View(d);
        }

        public ActionResult ZatrazioDispecer(string ulica, string broj, string mesto, string postanski, string vozilo, string dispecer, string korisnickoVozac)
        {
            Voznja v = new Voznja();
            Adresa a = new Adresa(ulica, broj, mesto, postanski);
            
            Lokacija l = new Lokacija("1", "1", a);
            v.Datum_i_vreme = DateTime.Now;
            v.LokacijaNaKojuTaksiDolazi = l;
            v.StatusVoznje = StatusVoznje.Formirana;

            switch (vozilo)
            {
                case "putnicko":
                    v.TipAutomobila = TipAutomobila.putnickiAutomobil;
                    break;
                case "kombi":
                    v.TipAutomobila = TipAutomobila.kombi;
                    break;
            }

            Vozac vozac = new Vozac();
           

            foreach (Vozac vo in PostojeciKorisnici.ListaVozaca)
            {
               if(vo.Korisnicko_ime == korisnickoVozac)
                {
                    vozac = vo;
                }
            }

            if(v.TipAutomobila != vozac.Automobil.Tip)
            {
                return View("GreskaAuto");
            }
            else
            {
                vozac.Zauzet = true;
            }


            v.Vozac = vozac;
            v.StatusVoznje = StatusVoznje.KreiranaNaCekanju;
            v.Musterija = new Musterija("", "", "", "", Pol.Muski,"0", "", "", Uloge.Musterija);

            Dispecer disp = new Dispecer();
            foreach (Dispecer d in PostojeciKorisnici.ListaDispecera)
            {
                if (d.Korisnicko_ime == dispecer)
                {
                    disp = d;
                }
            }
            v.Dispecer = disp;

            disp.listaVoznja.Add(v);

            foreach(Vozac voza in PostojeciKorisnici.ListaVozaca)
            {
                if(voza.Korisnicko_ime == korisnickoVozac)
                {
                    voza.listaVoznja.Add(v);
                    vozac = voza;
                    break;
                }
            }

            foreach(Korisnik k in PostojeciKorisnici.ListaKorisnika)
            {
                if(k.Korisnicko_ime == korisnickoVozac)
                {
                    k.listaVoznja = vozac.listaVoznja;
                }
            }

            Session["korisnik"] = disp;

            PostojeciKorisnici.ListaSvihVoznji.Add(v);

            return View("DispecerZatrazioVoznju", v);
        }

        public ActionResult PromeniLokacijuVozaca()
        {
            Vozac v = HttpContext.Application["Vozac"] as Vozac;
            return View(v);
        }

        public ActionResult PromeniLokVozac(string x, string y, string ulica, string broj, string grad, string pozivni)
        {
            
            Adresa a = new Adresa(ulica, broj, grad, pozivni);

            Lokacija l = new Lokacija(x, y, a);

            Vozac v = HttpContext.Application["Vozac"] as Vozac;

            v.Lokacija = l;

            foreach (Korisnik kor in PostojeciKorisnici.ListaKorisnika)
            {
                if (kor.Korisnicko_ime == v.Korisnicko_ime)
                {
                    kor.Korisnicko_ime = v.Korisnicko_ime;
                    kor.Lozinka = v.Lozinka;
                    kor.Ime = v.Ime;
                    kor.Prezime = v.Prezime;
                    kor.Email = v.Email;
                    kor.Kontakt_telefon = v.Kontakt_telefon;
                    kor.Jmbg = v.Jmbg;
                    kor.Pol = v.Pol;
                    break;
                }
            }

            //Sacuvaj(Registrovani.SviZajedno);

            return View("vozacView", v);
        }
    }
    }

