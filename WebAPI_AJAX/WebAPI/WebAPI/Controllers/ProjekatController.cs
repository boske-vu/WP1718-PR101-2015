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
            Korisnik k = Session["korisnik"] as Korisnik;

            if (k == null)
            {
                k = new Korisnik();
                Session["korisnik"] = k;
                return View();
            }
            else
            {
                foreach (Musterija m in PostojeciKorisnici.ListaMusterija)
                {
                    if (m.Korisnicko_ime == k.Korisnicko_ime && m.Lozinka != k.Lozinka)
                    {
                        return View("PogresnaSifra");
                    }
                    else if (m.Korisnicko_ime == k.Korisnicko_ime && m.Lozinka == k.Lozinka)
                    {
                        m.Ulogovan = true;

                        return View("musterijaView", m);
                    }
                }

                foreach (Vozac v in PostojeciKorisnici.ListaVozaca)
                {
                    if (v.Korisnicko_ime == k.Korisnicko_ime && v.Lozinka != k.Lozinka)
                    {
                        return View("PogresnaSifra");

                    }
                    else if (v.Korisnicko_ime == k.Korisnicko_ime && v.Lozinka == k.Lozinka)
                    {
                        v.Ulogovan = true;

                        return View("vozacView", v);
                    }
                }

                foreach (Dispecer d in PostojeciKorisnici.ListaDispecera)
                {
                    if (d.Korisnicko_ime == k.Korisnicko_ime && d.Lozinka != k.Lozinka)
                    {
                        return View("PogresnaSifra");

                    }
                    else if (d.Korisnicko_ime == k.Korisnicko_ime && d.Lozinka == k.Lozinka)
                    {
                        d.Ulogovan = true;

                        return View("dispecerView", d);
                    }
                }


                return View("NePostojiKorisnik");
            }
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

            Sacuvaj(PostojeciKorisnici.ListaKorisnika);
            return View("musterijaView", ret);
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

            Sacuvaj(PostojeciKorisnici.ListaKorisnika);
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

            PostojeciKorisnici.ListaVozaca.Add(v);

            foreach (Korisnik k in PostojeciKorisnici.ListaKorisnika)
            {
                if (k.Korisnicko_ime == v.Korisnicko_ime)
                {
                    return View("KorisnikPostoji");
                }
            }


            PostojeciKorisnici.ListaKorisnika.Add(v);

            Sacuvaj(PostojeciKorisnici.ListaKorisnika);

            /*
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
            */
            Dispecer d = Session["korisnik"] as Dispecer;

            return View("VozacDodan", v);
        }
#endregion

        #region login
        [HttpPost]
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
                    Session["korisnik"] = m;
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
                    Session["korisnik"] = d;
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
                    Session["korisnik"] = v;
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
        [HttpPost]
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

            Sacuvaj(PostojeciKorisnici.ListaKorisnika);
           // string path = @"C:\Users\HP\Desktop\Projakat\WP1718-PR101-2015\WebAPI_AJAX\WebAPI\WebAPI\baza.xml";


            /*
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
            return View("Index");
            }


        public ActionResult DispecerEdit()
        {
            Dispecer d = HttpContext.Application["Dispecer"] as Dispecer;
            return View(d);
        }


        public ActionResult OstaviKomentar(string komentar, string ocena, string korisnicko, string datum)
        {
            Musterija must = new Musterija();
            Komentar k = new Komentar();
            k.DatumObjave = DateTime.Now;
            switch (ocena)
            {
                case "Neocenjen":
                    k.OcenaVoznje = Ocene.veomaLose;
                    break;
                case "Veoma loša":
                    k.OcenaVoznje = Ocene.veomaLose;
                    break;
                case "Loša":
                    k.OcenaVoznje = Ocene.lose;
                    break;
                case "Dobra":
                    k.OcenaVoznje = Ocene.dobro;
                    break;
                case "Veoma Dobra":
                    k.OcenaVoznje = Ocene.veomaDobro;
                    break;
                case "Odlična":
                    k.OcenaVoznje = Ocene.odlicno;
                    break;
            }

            k.Opis = komentar.Trim();

            foreach (Voznja v in PostojeciKorisnici.ListaSvihVoznji)
            {
                if (v.Datum_i_vreme.ToString() == datum && v.Musterija.Korisnicko_ime == korisnicko)
                {
                    k.Voznja = v;
                    k.Korisnik = v.Musterija;
                    k.Voznja.Komentar = k;
                }
            }

            foreach (Musterija m in PostojeciKorisnici.ListaMusterija)
            {
                if (m.Korisnicko_ime == korisnicko)
                {
                    k.Korisnik = m;
                    must = m;

                }
            }


            foreach (Korisnik kor in PostojeciKorisnici.ListaKorisnika)
            {
                if (kor.Korisnicko_ime == korisnicko)
                {
                    kor.listaVoznja = k.Korisnik.listaVoznja;
                }
            }

           Sacuvaj(PostojeciKorisnici.ListaKorisnika);

            return View("musterijaView", must);
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

            Sacuvaj(PostojeciKorisnici.ListaKorisnika);
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

            Sacuvaj(PostojeciKorisnici.ListaKorisnika);
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

            Sacuvaj(PostojeciKorisnici.ListaKorisnika);

            return View("vozacView", v);
        }

        public ActionResult FiltriraMusterija(string filterStatus, string musterija)
        {
            Korisnik ret = new Korisnik();
            foreach (Korisnik must in PostojeciKorisnici.ListaKorisnika)
            {
                if (must.Korisnicko_ime == musterija)
                {
                    ret = must;
                }
            }
            ret.Filter = true;

            StatusVoznje status = StatusVoznje.Formirana;

            switch (filterStatus)
            {
                case "Formirana":
                    status = StatusVoznje.Formirana;
                    break;
                case "Kreirana":
                    status = StatusVoznje.KreiranaNaCekanju;
                    break;
                case "Neuspešna":
                    status = StatusVoznje.Neuspesna;
                    break;
                case "Uspešna":
                    status = StatusVoznje.Uspesna;
                    break;
                case "Prihvaćena":
                    status = StatusVoznje.Prihvacena;
                    break;
                case "Otkazana":
                    status = StatusVoznje.Otkazana;
                    break;
                case "Obrađena":
                    status = StatusVoznje.Obradjena;
                    break;
            }

            foreach (Voznja voz in ret.listaVoznja)
            {
                if (voz.StatusVoznje == status)
                {
                    ret.Filtrirane.Add(voz);
                }
            }
            Dispecer d = new Dispecer();
            Musterija m = new Musterija();
            Vozac v = new Vozac();
            if (ret.Uloga == Uloge.Musterija)
            {
                foreach (Musterija mu in PostojeciKorisnici.ListaMusterija)
                {
                    if (mu.Korisnicko_ime == ret.Korisnicko_ime)
                    {
                        mu.Filtrirane = ret.Filtrirane;
                        mu.Sortirane = ret.Sortirane;
                        mu.Pretrazene = ret.Pretrazene;
                        m = mu;
                        return View("musterijaView", m);
                    }
                }
            }
            else if (ret.Uloga == Uloge.Dispecer)
            {
                foreach (Dispecer di in PostojeciKorisnici.ListaDispecera)
                {
                    if (di.Korisnicko_ime == ret.Korisnicko_ime)
                    {
                        di.Filtrirane = ret.Filtrirane;
                        di.Sortirane = ret.Sortirane;
                        di.Pretrazene = ret.Pretrazene;
                        d = di;
                        return View("dispecerView", d);
                    }
                }
            }
            else if (ret.Uloga == Uloge.Vozac)
            {
                foreach (Vozac vo in PostojeciKorisnici.ListaVozaca)
                {
                    if (vo.Korisnicko_ime == ret.Korisnicko_ime)
                    {
                        vo.Filtrirane = ret.Filtrirane;
                        vo.Sortirane = ret.Sortirane;
                        vo.Pretrazene = ret.Pretrazene;
                        v = vo;
                        return View("vozacView", vo);
                    }
                }
            }

            return View("musterijaView", ret);
        }

        public ActionResult SortirajMusteriju(string datum, string ocena, string musterija)
        {
            Korisnik ret = new Korisnik();
            foreach (Korisnik must in PostojeciKorisnici.ListaKorisnika)
            {
                if (must.Korisnicko_ime == musterija)
                {
                    ret = must;
                }
            }

            ret.Sortiranje = true;

            if (ret.Filter)
            {
                ret.Sortirane = ret.Filtrirane;
            }
            else if (ret.Pretrazivanje)
            {
                ret.Sortirane = ret.Pretrazene;
            }
            else
            {
                foreach (Voznja voz in ret.listaVoznja)
                {
                    ret.Sortirane.Add(voz);
                }
            }

            bool dat = false;
            bool oc = false;

            if (datum != null)
            {
                dat = true;
            }

            if (ocena != null)
            {
                oc = true;
            }

            if (dat)
            {
                ret.Sortirane = ret.listaVoznja.OrderBy(o => o.Datum_i_vreme).ToList();
            }
            else if (oc)
            {
                ret.Sortirane = ret.listaVoznja.OrderBy(o => o.Komentar.OcenaVoznje).ToList();
            }

            Dispecer d = new Dispecer();
            Musterija m = new Musterija();
            Vozac v = new Vozac();
            if (ret.Uloga == Uloge.Musterija)
            {
                foreach (Musterija mu in PostojeciKorisnici.ListaMusterija)
                {
                    if (mu.Korisnicko_ime == ret.Korisnicko_ime)
                    {
                        mu.Filtrirane = ret.Filtrirane;
                        mu.Sortirane = ret.Sortirane;
                        mu.Pretrazene = ret.Pretrazene;
                        m = mu;
                        return View("musterijaView", m);
                    }
                }
            }
            else if (ret.Uloga == Uloge.Dispecer)
            {
                foreach (Dispecer di in PostojeciKorisnici.ListaDispecera)
                {
                    if (di.Korisnicko_ime == ret.Korisnicko_ime)
                    {
                        di.Filtrirane = ret.Filtrirane;
                        di.Sortirane = ret.Sortirane;
                        di.Pretrazene = ret.Pretrazene;
                        d = di;
                        return View("dispecerView", d);
                    }
                }
            }
            else if (ret.Uloga == Uloge.Vozac)
            {
                foreach (Vozac vo in PostojeciKorisnici.ListaVozaca)
                {
                    if (vo.Korisnicko_ime == ret.Korisnicko_ime)
                    {
                        vo.Filtrirane = ret.Filtrirane;
                        vo.Sortirane = ret.Sortirane;
                        vo.Pretrazene = ret.Pretrazene;
                        v = vo;
                        return View("vozacView", vo);
                    }
                }
            }

            return View("musterijaView", ret);
        }

        public ActionResult UspesnoOtkazanaVoznja(string korisnik, string datum)
        {
            Voznja v = new Voznja();

            foreach (Voznja voznja in PostojeciKorisnici.ListaSvihVoznji)
            {
                if (voznja.Datum_i_vreme.ToString() == datum && voznja.Musterija.Korisnicko_ime == korisnik)
                {
                    v = voznja;
                    break;
                }
            }
            return View("UspesnoOtkazanaVoznja", v);
        }

        public ActionResult OtkazujeMusterija(string korisnik, string datum)
        {
            string[] dat = datum.Split(' ', '-', ':');

            int day = int.Parse(dat[0]);
            int month = 0;
            switch (dat[1])
            {
                case "Jan":
                    month = 1;
                    break;
                case "Feb":
                    month = 2;
                    break;
                case "Mar":
                    month = 3;
                    break;
                case "Apr":
                    month = 4;
                    break;
                case "May":
                    month = 5;
                    break;
                case "Jun":
                    month = 6;
                    break;
                case "Jul":
                    month = 7;
                    break;
                case "Aug":
                    month = 8;
                    break;
                case "Sep":
                    month = 9;
                    break;
                case "Oct":
                    month = 10;
                    break;
                case "Nov":
                    month = 11;
                    break;
                case "Dec":
                    month = 12;
                    break;
            }

            int year = int.Parse(dat[2]);
            year = year + 2000;
            int hour = int.Parse(dat[3]);
            int minute = int.Parse(dat[4]);
            int second = int.Parse(dat[5]);

            DateTime d = new DateTime(year, month, day, hour, minute, second);

            Voznja vo = new Voznja();

            foreach (Voznja v in PostojeciKorisnici.ListaSvihVoznji)
            {
                if (v.Datum_i_vreme.Date == d.Date && v.Datum_i_vreme.Day == d.Day && v.Datum_i_vreme.Year == d.Year && v.Datum_i_vreme.Hour == d.Hour && v.Datum_i_vreme.Minute == d.Minute && v.Datum_i_vreme.Second == d.Second && v.Musterija.Korisnicko_ime == korisnik)
                {
                    v.StatusVoznje = StatusVoznje.Otkazana;
                    vo = v;
                }
            }
            Musterija musterija = new Musterija();
            foreach (Musterija m in PostojeciKorisnici.ListaMusterija)
            {
                if (m.Korisnicko_ime == korisnik)
                {
                    foreach (Voznja v in m.listaVoznja)
                    {
                        if (v.Datum_i_vreme.Date == d.Date && v.Datum_i_vreme.Day == d.Day && v.Datum_i_vreme.Year == d.Year && v.Datum_i_vreme.Hour == d.Hour && v.Datum_i_vreme.Minute == d.Minute && v.Datum_i_vreme.Second == d.Second)
                        {
                            v.StatusVoznje = StatusVoznje.Otkazana;
                            musterija = m;
                            break;
                        }
                    }

                }
            }

            Sacuvaj(PostojeciKorisnici.ListaKorisnika);
            return View("UspesnoOtkazanaVoznja", vo);
        }

        public ActionResult IzmeniVoznjuMusterija(string korisnik, string datum)
        {
            string[] dat = datum.Split(' ', '-', ':');

            int day = int.Parse(dat[0]);
            int month = 0;
            switch (dat[1])
            {
                case "Jan":
                    month = 1;
                    break;
                case "Feb":
                    month = 2;
                    break;
                case "Mar":
                    month = 3;
                    break;
                case "Apr":
                    month = 4;
                    break;
                case "May":
                    month = 5;
                    break;
                case "Jun":
                    month = 6;
                    break;
                case "Jul":
                    month = 7;
                    break;
                case "Aug":
                    month = 8;
                    break;
                case "Sep":
                    month = 9;
                    break;
                case "Oct":
                    month = 10;
                    break;
                case "Nov":
                    month = 11;
                    break;
                case "Dec":
                    month = 12;
                    break;
            }

            int year = int.Parse(dat[2]);
            year = year + 2000;
            int hour = int.Parse(dat[3]);
            int minute = int.Parse(dat[4]);
            int second = int.Parse(dat[5]);

            DateTime d = new DateTime(year, month, day, hour, minute, second);

            Voznja vo = new Voznja();

            foreach (Voznja v in PostojeciKorisnici.ListaSvihVoznji)
            {
                if (v.Datum_i_vreme.Date == d.Date && v.Datum_i_vreme.Day == d.Day && v.Datum_i_vreme.Year == d.Year && v.Datum_i_vreme.Hour == d.Hour && v.Datum_i_vreme.Minute == d.Minute && v.Datum_i_vreme.Second == d.Second && v.Musterija.Korisnicko_ime == korisnik)
                {
                    vo = v;
                    break;
                }
            }
            return View("IzmenaVoznjeMusterija", vo);
        }

        public ActionResult PrikaziKomentar(string datum, string korisnik, string vozac, string dispecer)
        {

            Voznja ret = new Voznja();
            foreach (Korisnik k in PostojeciKorisnici.ListaKorisnika)
            {
                foreach (Voznja v in k.listaVoznja)
                {
                    if (v.Datum_i_vreme.ToString() == datum && k.Korisnicko_ime == korisnik && vozac == v.Vozac.Korisnicko_ime && dispecer == v.Dispecer.Korisnicko_ime)
                    {
                        ret = v;
                        break;
                    }
                }
            }
            return View("PrikazKomentara", ret);
        }

        public ActionResult PonistiFilter(string musterija)
        {
            Korisnik ret = new Korisnik();
            foreach (Korisnik must in PostojeciKorisnici.ListaKorisnika)
            {
                if (must.Korisnicko_ime == musterija)
                {
                    ret = must;
                }
            }

            ret.Filter = false;
            ret.Sortiranje = false;
            ret.Pretrazivanje = false;

            ret.Filtrirane = new List<Voznja>();
            ret.Sortirane = new List<Voznja>();
            ret.Pretrazene = new List<Voznja>();

            Dispecer d = new Dispecer();
            Musterija m = new Musterija();
            Vozac v = new Vozac();
            if (ret.Uloga == Uloge.Musterija)
            {
                foreach (Musterija mu in PostojeciKorisnici.ListaMusterija)
                {
                    if (mu.Korisnicko_ime == ret.Korisnicko_ime)
                    {
                        mu.Filtrirane = ret.Filtrirane;
                        mu.Sortirane = ret.Sortirane;
                        mu.Pretrazene = ret.Pretrazene;
                        m = mu;
                        return View("musterijaView", m);
                    }
                }
            }
            else if (ret.Uloga == Uloge.Dispecer)
            {
                foreach (Dispecer di in PostojeciKorisnici.ListaDispecera)
                {
                    if (di.Korisnicko_ime == ret.Korisnicko_ime)
                    {
                        di.Filtrirane = ret.Filtrirane;
                        di.Sortirane = ret.Sortirane;
                        di.Pretrazene = ret.Pretrazene;
                        di.TraziVozac = false;
                        di.TraziMusteriju = false;
                        di.NadjeneMusterije = new List<Voznja>();
                        di.NadjeniVozaci = new List<Voznja>();
                        d = di;
                        return View("dispecerView", d);
                    }
                }
            }
            else if (ret.Uloga == Uloge.Vozac)
            {
                foreach (Vozac vo in PostojeciKorisnici.ListaVozaca)
                {
                    if (vo.Korisnicko_ime == ret.Korisnicko_ime)
                    {
                        vo.Filtrirane = ret.Filtrirane;
                        vo.Sortirane = ret.Sortirane;
                        vo.Pretrazene = ret.Pretrazene;
                        v = vo;
                        return View("vozacView", vo);
                    }
                }
            }

            return View("musterijaView", ret);
        }

        public ActionResult TraziMusterija(string datum, string ocena, string cena, string musterija)
        {
            if (datum == null || ocena == null || cena == null)
            {
                return View("GreskaTrazenje");
            }

            Ocene o = Ocene.neocenjen;

            switch (ocena)
            {
                case "Neocenjen":
                    o = Ocene.neocenjen;
                    break;
                case "Veoma loša":
                    o = Ocene.veomaLose;
                    break;
                case "Loša":
                    o = Ocene.lose;
                    break;
                case "Dobra":
                    o = Ocene.dobro;
                    break;
                case "Veoma dobra":
                    o = Ocene.veomaDobro;
                    break;
                case "Odlična":
                    o = Ocene.odlicno;
                    break;
            }

            List<Voznja> pomocna = new List<Voznja>();

            Korisnik ret = new Korisnik();
            foreach (Korisnik must in PostojeciKorisnici.ListaKorisnika)
            {
                if (must.Korisnicko_ime == musterija)
                {
                    ret = must;
                }
            }

            ret.Pretrazivanje = true;

            if (ret.Filter)
            {
                pomocna = ret.Filtrirane;
            }
            else if (ret.Sortiranje)
            {
                pomocna = ret.Sortirane;
            }
            else
            {
                foreach (Voznja voz in ret.listaVoznja)
                {
                    pomocna.Add(voz);
                }
            }

            foreach (Voznja voznja in pomocna)
            {
                if (voznja.Datum_i_vreme.ToString() == datum && voznja.Komentar.OcenaVoznje == o && voznja.Iznos == cena)
                {
                    ret.Pretrazene.Add(voznja);
                }
            }

            Dispecer d = new Dispecer();
            Musterija m = new Musterija();
            Vozac v = new Vozac();
            if (ret.Uloga == Uloge.Musterija)
            {
                foreach (Musterija mu in PostojeciKorisnici.ListaMusterija)
                {
                    if (mu.Korisnicko_ime == ret.Korisnicko_ime)
                    {
                        mu.Filtrirane = ret.Filtrirane;
                        mu.Sortirane = ret.Sortirane;
                        mu.Pretrazene = ret.Pretrazene;
                        m = mu;
                        return View("musterijaView", m);
                    }
                }
            }
            else if (ret.Uloga == Uloge.Dispecer)
            {
                foreach (Dispecer di in PostojeciKorisnici.ListaDispecera)
                {
                    if (di.Korisnicko_ime == ret.Korisnicko_ime)
                    {
                        di.Filtrirane = ret.Filtrirane;
                        di.Sortirane = ret.Sortirane;
                        di.Pretrazene = ret.Pretrazene;
                        d = di;
                        return View("dispecerView", d);
                    }
                }
            }
            else if (ret.Uloga == Uloge.Vozac)
            {
                foreach (Vozac vo in PostojeciKorisnici.ListaVozaca)
                {
                    if (vo.Korisnicko_ime == ret.Korisnicko_ime)
                    {
                        vo.Filtrirane = ret.Filtrirane;
                        vo.Sortirane = ret.Sortirane;
                        vo.Pretrazene = ret.Pretrazene;
                        v = vo;
                        return View("vozacView", vo);
                    }
                }
            }


            return View("musterijaView", ret);
        }

        public ActionResult TraziVozaca(string vozacI, string vozacP, string korisnik)
        {
            List<Voznja> pomocna = new List<Voznja>();
            Dispecer ret = new Dispecer();
            foreach (Dispecer d in PostojeciKorisnici.ListaDispecera)
            {
                if (d.Korisnicko_ime == korisnik)
                {
                    ret = d;
                }
            }

            ret.TraziVozac = true;

            if (ret.Filter)
            {
                pomocna = ret.Filtrirane;
            }
            else if (ret.Sortiranje)
            {
                pomocna = ret.Sortirane;
            }
            else
            {
                foreach (Voznja voz in ret.listaVoznja)
                {
                    pomocna.Add(voz);
                }
            }

            if (vozacI != "" && vozacP != "")
            {
                foreach (Voznja v in pomocna)
                {
                    if (v.Vozac.Ime == vozacI && v.Vozac.Prezime == vozacP)
                    {
                        ret.NadjeniVozaci.Add(v);
                    }
                }
            }
            else if (vozacP == "" && vozacI != "")
            {
                foreach (Voznja v in pomocna)
                {
                    if (v.Vozac.Ime == vozacI)
                    {
                        ret.NadjeniVozaci.Add(v);
                    }
                }
            }
            else if (vozacI == "" && vozacP != "")
            {
                foreach (Voznja v in pomocna)
                {
                    if (v.Vozac.Prezime == vozacP)
                    {
                        ret.NadjeniVozaci.Add(v);
                    }
                }
            }

            return View("dispecerView", ret);
        }

        public ActionResult TraziMusteriju(string musterijaI, string musterijaP, string korisnik)
        {
            List<Voznja> pomocna = new List<Voznja>();
            Dispecer ret = new Dispecer();
            foreach (Dispecer d in PostojeciKorisnici.ListaDispecera)
            {
                if (d.Korisnicko_ime == korisnik)
                {
                    ret = d;
                }
            }

            ret.TraziMusteriju = true;
            if (ret.Filter)
            {
                pomocna = ret.Filtrirane;
            }
            else if (ret.Sortiranje)
            {
                pomocna = ret.Sortirane;
            }
            else if (ret.TraziVozac)
            {
                pomocna = ret.NadjeniVozaci;
            }
            else
            {
                foreach (Voznja voz in ret.listaVoznja)
                {
                    pomocna.Add(voz);
                }
            }
            if (musterijaI != "" && musterijaP != "")
            {
                foreach (Voznja v in pomocna)
                {
                    if (v.Musterija.Ime == musterijaI && v.Musterija.Prezime == musterijaP)
                    {
                        ret.NadjeniVozaci.Add(v);
                    }
                }
            }
            else if (musterijaP == "" && musterijaI != "")
            {
                foreach (Voznja v in pomocna)
                {
                    if (v.Musterija.Ime == musterijaI)
                    {
                        ret.NadjeniVozaci.Add(v);
                    }
                }
            }
            else if (musterijaI == "" && musterijaP != "")
            {
                foreach (Voznja v in pomocna)
                {
                    if (v.Musterija.Prezime == musterijaP)
                    {
                        ret.NadjeniVozaci.Add(v);
                    }
                }
            }

            return View("dispecerView", ret);
        }

        private void Sacuvaj(List<Korisnik> musterije)
        {
            SacuvajLjude(musterije);
            SacuvajVoznje(PostojeciKorisnici.ListaSvihVoznji);
        }

        private void SacuvajVoznje(List<Voznja> sveVoznje)
        {
            string filename = @"C:\Users\HP\Desktop\Projakat\WP1718-PR101-2015\WebAPI_AJAX\WebAPI\WebAPI\voznje.xml";
            XmlWriter writer = null;
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = ("\t");
                settings.OmitXmlDeclaration = true;

                writer = XmlWriter.Create(filename, settings);
                writer.WriteStartElement("Voznje");

                foreach (Voznja v in sveVoznje)
                {

                    if (v.Odrediste == null)
                    {
                        Adresa a = new Adresa("Nepoznata","0", "Nepoznato", "0");
                        Lokacija l = new Lokacija("0", "0", a);
                        v.Odrediste = l;
                    }
                    if (v.Vozac == null)
                    {
                        Vozac voz = new Vozac("nema", "nema", "nema", "nema", Pol.Muski, "0000", "nema", "nema", Uloge.Vozac ,"nema", "nema","nema","nema");
                        v.Vozac = voz;
                    }
                    if (v.Dispecer == null)
                    {
                        Dispecer d = new Dispecer("nema", "nema", "nema", "nema", Pol.Muski, "0000", "nema", "nema", Uloge.Dispecer);
                        v.Dispecer = d;
                    }

                    if (v.Komentar == null)
                    {
                        Komentar k = new Komentar("bez opisa", DateTime.MinValue, v, Ocene.neocenjen, v.Vozac);
                        v.Komentar = k;
                    }
                    if (v.Musterija == null)
                    {
                        Musterija m = new Musterija("nema", "nema", "nema", "nema", Pol.Muski, "000", "nema", "nema", Uloge.Musterija);
                        v.Musterija = m;
                    }

                    string odrediste = v.Odrediste.Adresa.Mesto + "_" + v.Odrediste.Adresa.Broj.ToString() + "," + v.Odrediste.Adresa.Mesto + "_" + v.Odrediste.Adresa.PostanskiBroj.ToString();
                    string pocetna = v.LokacijaNaKojuTaksiDolazi.Adresa.Mesto + "_" + v.LokacijaNaKojuTaksiDolazi.Adresa.Broj.ToString() + "," + v.LokacijaNaKojuTaksiDolazi.Adresa.Mesto + "_" + v.LokacijaNaKojuTaksiDolazi.Adresa.PostanskiBroj.ToString();

                    writer.WriteStartElement("Voznja");
                    writer.WriteElementString("DatumPorudzbine", v.Datum_i_vreme.ToString());
                    writer.WriteElementString("LokacijaNaKojuTaksiDolazi", pocetna);
                    writer.WriteElementString("KrajnjaLokacija", odrediste);
                    writer.WriteElementString("TipVozila", v.TipAutomobila.ToString());
                    writer.WriteElementString("MusterijaIme", v.Musterija.Ime);
                    writer.WriteElementString("MusterijaPrezime", v.Musterija.Prezime);
                    writer.WriteElementString("VozacIme", v.Vozac.Ime);
                    writer.WriteElementString("VozacPrezime", v.Vozac.Prezime);
                    writer.WriteElementString("DispecerIme", v.Dispecer.Ime);
                    writer.WriteElementString("DispececrPrezime", v.Dispecer.Prezime);
                    writer.WriteElementString("Status", v.StatusVoznje.ToString());
                    writer.WriteElementString("KomentarOpis", v.Komentar.Opis);
                    writer.WriteElementString("KomentarDatum", v.Komentar.DatumObjave.ToString());
                    writer.WriteElementString("KomentarOcena", v.Komentar.OcenaVoznje.ToString());
                    writer.WriteElementString("Iznos", v.Iznos.ToString());
                    writer.WriteEndElement();
                }
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        private void SacuvajLjude(List<Korisnik> korisnici)
        {
            string filename = @"C:\Users\HP\Desktop\Projakat\WP1718-PR101-2015\WebAPI_AJAX\WebAPI\WebAPI\korisnici.xml";
            XmlWriter writer = null;
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = ("\t");
                settings.OmitXmlDeclaration = true;

                writer = XmlWriter.Create(filename, settings);
                writer.WriteStartElement("Ulogovani");
                foreach (Korisnik m in korisnici)
                {
                    if (m.Uloga == Uloge.Vozac)
                    {
                        foreach (Vozac vo in PostojeciKorisnici.ListaVozaca)
                        {
                            if (vo.Korisnicko_ime == m.Korisnicko_ime)
                            {
                                writer.WriteStartElement("Korisnik");
                                writer.WriteElementString("Ime", vo.Ime);
                                writer.WriteElementString("Prezime", vo.Prezime);
                                writer.WriteElementString("Jmbg", vo.Jmbg.ToString());
                                writer.WriteElementString("KorisnickoIme", vo.Korisnicko_ime);
                                writer.WriteElementString("Lozinka", vo.Lozinka);
                                writer.WriteElementString("Pol", vo.Pol.ToString());
                                writer.WriteElementString("E-Mail", vo.Email);
                                writer.WriteElementString("BrojTelefona", vo.Kontakt_telefon);
                                writer.WriteElementString("Uloga", vo.Uloga.ToString());
                                writer.WriteStartElement("Automobil");
                                writer.WriteElementString("Godiste", vo.Automobil.GodisteAutomobila.ToString());
                                writer.WriteElementString("Registracija", vo.Automobil.BrojRegistarskeOznake.ToString());
                                writer.WriteElementString("TaxiBroj", vo.Automobil.BrojTaksiVozila.ToString());
                                writer.WriteElementString("TipVozila", vo.Automobil.Tip.ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("Adresa");
                                writer.WriteElementString("NazivUlice", vo.Lokacija.Adresa.Ulica);
                                writer.WriteElementString("BrojUlice", vo.Lokacija.Adresa.Broj.ToString());
                                writer.WriteElementString("Grad", vo.Lokacija.Adresa.Mesto);
                                writer.WriteElementString("PostanskiBroj", vo.Lokacija.Adresa.PostanskiBroj.ToString());
                                writer.WriteElementString("X", vo.Lokacija.KoordinataX.ToString());
                                writer.WriteElementString("Y", vo.Lokacija.KoordinataY.ToString());
                                writer.WriteEndElement();
                                writer.WriteEndElement();
                            }
                        }
                    }
                    else
                    {
                        writer.WriteStartElement("Korisnik");
                        writer.WriteElementString("Ime", m.Ime);
                        writer.WriteElementString("Prezime", m.Prezime);
                        writer.WriteElementString("Jmbg", m.Jmbg.ToString());
                        writer.WriteElementString("KorisnickoIme", m.Korisnicko_ime);
                        writer.WriteElementString("Lozinka", m.Lozinka);
                        writer.WriteElementString("Pol", m.Pol.ToString());
                        writer.WriteElementString("E-Mail", m.Email);
                        writer.WriteElementString("BrojTelefona", m.Kontakt_telefon);
                        writer.WriteElementString("Uloga", m.Uloga.ToString());
                        writer.WriteEndElement();
                    }
                }
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }

        }
    }

}

