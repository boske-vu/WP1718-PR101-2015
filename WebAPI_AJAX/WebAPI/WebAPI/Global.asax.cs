using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Xml;
using System.Xml.Serialization;
using WebAPI.Models;

/*
            Musterija m = new Musterija("d", "d", "dsfj", "djfi", Pol.Muski, "wfeijf", "sfji", "fdjfij", Uloge.Musterija);
            Vozac v = new Vozac("bole", "1", "df", "df", Pol.Muski, "tr", "df", "df", Uloge.Vozac, "df", "3", "vukovar", "2134");
            Automobil a = new Automobil(v, "1993", "veio2", "332", TipAutomobila.kombi);

            Adresa ad = new Adresa("sajmiste", "34", "vukovar", "32000");
            Lokacija l = new Lokacija("43", "43", ad);

            v.Lokacija = l;
            v.Automobil = a;

            PostojeciKorisnici.ListaKorisnika.Add(m);
            PostojeciKorisnici.ListaMusterija.Add(m);
            PostojeciKorisnici.ListaKorisnika.Add(v);
            PostojeciKorisnici.ListaVozaca.Add(v);
            */

namespace WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static XmlElement SerializeToXmlElement(object o)
        {
            XmlDocument doc = new XmlDocument();
            using (XmlWriter writer = doc.CreateNavigator().AppendChild())
            {
                new XmlSerializer(o.GetType()).Serialize(writer, o);
            }
            return doc.DocumentElement;
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            PostojeciKorisnici pk = new PostojeciKorisnici();

            Ucitavanje();
        }
            
            
        private DateTime ToDate(string datum)
        {
            DateTime ret;

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

            ret = new DateTime(year, month, day, hour, minute, second);



            return ret;

        }

        private void Ucitavanje()
        {

            if (File.Exists(@"C:\Users\HP\Desktop\Projakat\WP1718-PR101-2015\WebAPI_AJAX\WebAPI\WebAPI\korisnici.xml"))
            {
                string ime = "";
                string prezime = "";
                string korisnicko = "";
                string lozinka = "";
                string jmbg = "";
                string telefon = "";
                string mail = "";
                Pol pol = Pol.Muski;
                Uloge uloga = Uloge.Dispecer;
                string x = "";
                string y = "";
                string brul = "";
                string posta = "";
                string ulica = "";
                string grad = "";
                string godiste = "";
                string reg = "";
                TipAutomobila tip = TipAutomobila.kombi;
                string taxiBr = "";

                using (XmlReader reader = XmlReader.Create(@"C:\Users\HP\Desktop\Projakat\WP1718-PR101-2015\WebAPI_AJAX\WebAPI\WebAPI\korisnici.xml"))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement() && reader.Name.Equals("Korisnik"))
                        {
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            ime = reader.Value;
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            prezime = reader.Value;
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            jmbg = reader.Value;
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            korisnicko = reader.Value;
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            lozinka = reader.Value;
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            switch (reader.Value)
                            {
                                case "Muski":
                                    pol = Pol.Muski;
                                    break;
                                case "Zenski":
                                    pol = Pol.Zenski;
                                    break;
                            }
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            mail = reader.Value;
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            telefon = reader.Value;
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            switch (reader.Value)
                            {
                                case "Dispecer":
                                    uloga = Uloge.Dispecer;
                                    break;
                                case "Vozac":
                                    uloga = Uloge.Vozac;
                                    break;
                                case "Musterija":
                                    uloga = Uloge.Musterija;
                                    break;
                            }

                            if (uloga == Uloge.Musterija)
                            {
                                Musterija m = new Musterija(korisnicko, lozinka, ime, prezime, pol, jmbg, telefon, mail, uloga);
                                PostojeciKorisnici.ListaMusterija.Add(m);
                                Korisnik k = m;
                                PostojeciKorisnici.ListaKorisnika.Add(k);
                            }
                            else if (uloga == Uloge.Dispecer)
                            {
                                Dispecer m = new Dispecer(korisnicko, lozinka, ime, prezime, pol, jmbg, telefon, mail, uloga);
                                PostojeciKorisnici.ListaDispecera.Add(m);
                                Korisnik k = m;
                                PostojeciKorisnici.ListaKorisnika.Add(k);
                            }
                            else if (uloga == Uloge.Vozac)
                            {
                                Vozac v = new Vozac(korisnicko, lozinka, ime, prezime, pol, jmbg, telefon, mail, uloga, ulica, brul, grad, posta);


                                reader.Read();
                                reader.Read();
                                reader.Read();
                                reader.Read();
                                reader.Read();
                                reader.Read();
                                godiste = reader.Value;
                                reader.Read();
                                reader.Read();
                                reader.Read();
                                reader.Read();
                                reg = reader.Value;
                                reader.Read();
                                reader.Read();
                                reader.Read();
                                reader.Read();
                                taxiBr = reader.Value;
                                reader.Read();
                                reader.Read();
                                reader.Read();
                                reader.Read();
                                switch (reader.Value)
                                {
                                    case "Putnicko":
                                        tip = TipAutomobila.putnickiAutomobil;
                                        break;
                                    case "Kombi":
                                        tip = TipAutomobila.kombi;
                                        break;
                                }
                                Automobil a = new Automobil(v, godiste, reg, taxiBr, tip);

                                reader.Read();
                                reader.Read();
                                reader.Read();
                                reader.Read();
                                reader.Read();
                                reader.Read();
                                reader.Read();
                                reader.Read();
                                ulica = reader.Value;
                                reader.Read();
                                reader.Read();
                                reader.Read();
                                reader.Read();
                                brul = reader.Value;
                                reader.Read();
                                reader.Read();
                                reader.Read();
                                reader.Read();
                                grad = reader.Value;
                                reader.Read();
                                reader.Read();
                                reader.Read();
                                reader.Read();
                                posta = reader.Value;
                                reader.Read();
                                reader.Read();
                                reader.Read();
                                reader.Read();
                                x = reader.Value;
                                reader.Read();
                                reader.Read();
                                reader.Read();
                                reader.Read();
                                y = reader.Value;
                                Adresa adresa = new Adresa(ulica, brul, grad, posta);
                                Lokacija l = new Lokacija(x, y, adresa);

                                v.Automobil = a;
                                v.Lokacija = l;

                                PostojeciKorisnici.ListaVozaca.Add(v);
                                Korisnik k = v;
                                PostojeciKorisnici.ListaKorisnika.Add(k);
                            }
                        }
                    }
                }
            }

            if (PostojeciKorisnici.ListaDispecera.Count() == 0)
            {
                string line;
                // Read the file and display it line by line.  
                System.IO.StreamReader file =
                    new System.IO.StreamReader(@"C:\Users\HP\Desktop\Projakat\WP1718-PR101-2015\WebAPI_AJAX\WebAPI\WebAPI\dispeceri.txt");

                while ((line = file.ReadLine()) != null)
                {
                    string[] polja = line.Split(':');
                    Dispecer d = new Dispecer();

                    d.Korisnicko_ime = polja[0];
                    d.Lozinka = polja[1];
                    d.Ime = polja[2];
                    d.Prezime = polja[3];
                    if (polja[4].Equals("Muski"))
                        d.Pol = Pol.Muski;
                    else
                        d.Pol = Pol.Zenski;
                    d.Jmbg = polja[5];
                    d.Kontakt_telefon = polja[6];
                    d.Email = polja[7];
                    d.Uloga = Uloge.Dispecer;

                    PostojeciKorisnici.ListaKorisnika.Add(d);
                    PostojeciKorisnici.ListaDispecera.Add(d);

                }

                file.Close();

                string path = @"C:\Users\HP\Desktop\Projakat\WP1718-PR101-2015\WebAPI_AJAX\WebAPI\WebAPI\korisnici.xml";
                XmlWriter writer = null;
                try
                {
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.IndentChars = ("\t");
                    settings.OmitXmlDeclaration = true;

                    writer = XmlWriter.Create(path, settings);
                    writer.WriteStartElement("Ulogovani");
                    foreach (Korisnik m in PostojeciKorisnici.ListaKorisnika)
                    {
                        m.Jmbg.ToString();
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
                        writer.WriteStartElement("Voznje");
                        int i = 1;
                        foreach (Voznja v in m.listaVoznja)
                        {
                            writer.WriteStartElement("VoznjaBroj" + i.ToString());
                            writer.WriteElementString("DatumPorudzbine", v.Datum_i_vreme.ToString());
                            writer.WriteElementString("PocetnaLokacija", v.LokacijaNaKojuTaksiDolazi.ToString());
                            writer.WriteElementString("KrajnjaLokacija", v.Odrediste.ToString());
                            writer.WriteElementString("TipVozila", v.TipAutomobila.ToString());
                            writer.WriteElementString("MusterijaIme", v.Musterija.Ime);
                            writer.WriteElementString("MusterijaPrezime", v.Musterija.Prezime);
                            writer.WriteElementString("VozacIme", v.Vozac.Ime);
                            writer.WriteElementString("VozacPrezime", v.Vozac.Prezime);
                            writer.WriteElementString("DispecerIme", v.Dispecer.Ime);
                            writer.WriteElementString("DispececrPrezime", v.Dispecer.Prezime);
                            writer.WriteElementString("Iznos", v.Iznos.ToString());
                            writer.WriteEndElement();
                            i++;
                        }
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();

                    writer.Flush();
                }
                finally
                {
                    if (writer != null)
                    {
                        writer.Close();
                    }
                }
            }
            else
            {
                UcitajVoznje();
            }
        }
        private void UcitajVoznje()
        {
            if (File.Exists(@"C:\Users\HP\Desktop\Projakat\WP1718-PR101-2015\WebAPI_AJAX\WebAPI\WebAPI\voznje.xml"))
            {
                string datum = "";
                string pocetna = "";
                string krajnja = "";
                TipAutomobila tip = TipAutomobila.kombi;
                string musterijaI = "";
                string musterijaP = "";
                string vozacI = "";
                string vozacP = "";
                string dispecerI = "";
                string dispecerP = "";
                StatusVoznje status = StatusVoznje.Formirana;
                string komentar = "";
                string komentarDatum = "";
                Ocene ocena = Ocene.neocenjen;
                string iznos = "0";

                using (XmlReader reader = XmlReader.Create(@"C:\Users\HP\Desktop\Projakat\WP1718-PR101-2015\WebAPI_AJAX\WebAPI\WebAPI\voznje.xml"))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement() && reader.Name.Equals("Voznja"))
                        {
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            datum = reader.Value;
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            pocetna = reader.Value;
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            krajnja = reader.Value;
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            switch (reader.Value)
                            {
                                case "Putnicko":
                                    tip = TipAutomobila.putnickiAutomobil;
                                    break;
                                case "Kombi":
                                    tip = TipAutomobila.kombi;
                                    break;
                            }
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            musterijaI = reader.Value;
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            musterijaP = reader.Value;
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            vozacI = reader.Value;
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            vozacP = reader.Value;
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            dispecerI = reader.Value;
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            dispecerP = reader.Value;
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            switch (reader.Value)
                            {
                                case "Kreirana":
                                    status = StatusVoznje.KreiranaNaCekanju;
                                    break;
                                case "Formirana":
                                    status = StatusVoznje.Formirana;
                                    break;
                                case "Neuspesna":
                                    status = StatusVoznje.Neuspesna;
                                    break;
                                case "Obradjena":
                                    status = StatusVoznje.Obradjena;
                                    break;
                                case "Otkazana":
                                    status = StatusVoznje.Otkazana;
                                    break;
                                case "Prihvacena":
                                    status = StatusVoznje.Prihvacena;
                                    break;
                                case "Uspesna":
                                    status = StatusVoznje.Uspesna;
                                    break;
                            }
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            komentar = reader.Value;
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            komentarDatum = reader.Value;
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            switch (reader.Value)
                            {
                                case "neocenjen":
                                    ocena = Ocene.neocenjen;
                                    break;
                                case "dobro":
                                    ocena = Ocene.dobro;
                                    break;
                                case "lose":
                                    ocena = Ocene.lose;
                                    break;
                                case "odlicno":
                                    ocena = Ocene.odlicno;
                                    break;
                                case "veomaDobro":
                                    ocena = Ocene.veomaDobro;
                                    break;
                                case "veomaLose":
                                    ocena = Ocene.veomaLose;
                                    break;
                            }
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            iznos = reader.Value;

                            SacuvajVoznju(datum, pocetna, krajnja, tip, musterijaI, musterijaP, vozacI, vozacP, dispecerI, dispecerP, status, komentar, komentarDatum, ocena, iznos);
                        }
                    }
                }
            }
        }

        private void SacuvajVoznju(string datum, string pocetna, string krajnja, TipAutomobila tip, string musterijaI, string musterijaP, string vozacI, string vozacP, string dispecerI, string dispecerP, StatusVoznje status, string komentar, string komentarDatum, Ocene ocena, string iznos)
        {
            DateTime date = ToDate(datum);
            string[] podela = pocetna.Split(',');
            string[] ulica = podela[0].Split('_');
            string[] grad = podela[1].Split('_');

            Adresa adresa = new Adresa(ulica[0], ulica[1], grad[0], grad[1]);
            Lokacija poc = new Lokacija("1", "1", adresa);

            podela = krajnja.Split(',');
            ulica = podela[0].Split('_');
            grad = podela[1].Split('_');

            Adresa adresaa = new Adresa(ulica[0], ulica[1], grad[0], grad[1]);
            Lokacija kraj = new Lokacija("1", "1", adresaa);

            Musterija musterija = new Musterija();
            Vozac vozac = new Vozac();
            Dispecer dispecer = new Dispecer();
            if (musterijaI != "nema" && musterijaP != "nema")
            {
                foreach (Musterija m in PostojeciKorisnici.ListaMusterija)
                {
                    if (musterijaI == m.Ime && musterijaP == m.Prezime)
                    {
                        musterija = m;
                        break;
                    }
                }
            }
            else
            {
                musterija = new Musterija("nema", "nema", "nema", "nema", Pol.Muski, "000", "nema", "nema", Uloge.Musterija);
            }

            if (vozacI != "nema" && vozacP != "nema")
            {
                foreach (Vozac m in PostojeciKorisnici.ListaVozaca)
                {
                    if (vozacI == m.Ime && vozacP == m.Prezime)
                    {
                        vozac = m;
                        break;
                    }
                }
            }
            else
            {
                vozac = new Vozac("nema", "nema", "nema", "nema", Pol.Muski, "0000", "nema", "nema", Uloge.Vozac, ulica[0], ulica[1], grad[0], grad[1]);
            }

            if (dispecerI != "nema" && dispecerP != "nema")
            {
                foreach (Dispecer m in PostojeciKorisnici.ListaDispecera)
                {
                    if (dispecerI == m.Ime && dispecerP == m.Prezime)
                    {
                        dispecer = m;
                        break;
                    }
                }
            }
            else
            {
                dispecer = new Dispecer("nema", "nema", "nema", "nema", Pol.Muski, "0000", "nema", "nema", Uloge.Dispecer);
            }
            Voznja v = new Voznja();
            DateTime kom = ToDate(komentarDatum);
            Komentar k = new Komentar();
            if (status == StatusVoznje.Otkazana)
            {
                k = new Komentar(komentar, kom, v, ocena, musterija);
            }
            else if (status == StatusVoznje.Neuspesna)
            {
                k = new Komentar(komentar, kom, v, ocena, musterija);
            }
            else if (status == StatusVoznje.Uspesna)
            {
                k = new Komentar(komentar, kom, v, ocena, musterija);
            }
            else
            {
                k = new Komentar("bez opisa", kom, v, Ocene.neocenjen, new Korisnik("nema", "nema", "nema", "nema", Pol.Muski, "0000", "nema", "nema", Uloge.Dispecer));
            }

            v = new Voznja(date, poc, tip, musterija, kraj, dispecer, vozac, iznos, k, status);

            k.Voznja = v;
            v.Komentar = k;

            if (musterijaI != "nema" && musterijaP != "nema")
            {
                foreach (Musterija m in PostojeciKorisnici.ListaMusterija)
                {
                    if (m.Korisnicko_ime == musterija.Korisnicko_ime)
                    {
                        m.listaVoznja.Add(v);
                    }
                }
            }


            if (vozacI != "nema" && vozacP != "nema")
            {
                foreach (Vozac m in PostojeciKorisnici.ListaVozaca)
                {
                    if (m.Korisnicko_ime == vozac.Korisnicko_ime)
                    {
                        m.listaVoznja.Add(v);
                    }
                }
            }


            if (dispecerI != "nema" && dispecerP != "nema")
            {
                foreach (Dispecer m in PostojeciKorisnici.ListaDispecera)
                {
                    if (m.Korisnicko_ime == dispecer.Korisnicko_ime)
                    {
                        m.listaVoznja.Add(v);
                    }
                }
            }

            PostojeciKorisnici.ListaSvihVoznji.Add(v);


            foreach (Korisnik kor in PostojeciKorisnici.ListaKorisnika)
            {
                if (kor.Korisnicko_ime == musterija.Korisnicko_ime)
                {
                    kor.listaVoznja = musterija.listaVoznja;
                }
                else if (kor.Korisnicko_ime == vozac.Korisnicko_ime)
                {
                    kor.listaVoznja = vozac.listaVoznja;
                }
                else if (kor.Korisnicko_ime == dispecer.Korisnicko_ime)
                {
                    kor.listaVoznja = dispecer.listaVoznja;
                }
            }
        }

    }


}



#region xmlPogresno
/*
string path = @"C:\Users\HP\Desktop\Projakat\WP1718-PR101-2015\WebAPI_AJAX\WebAPI\WebAPI\baza.xml";
// XmlSerializer serializer = new XmlSerializer(typeof(Korisnik));
if (System.IO.File.Exists(path))
{
    foreach (Korisnik k in PostojeciKorisnici.ListaMusterija)
    {
        XmlDocument xDoc = new XmlDocument();
        xDoc.Load(path);

        var allNodes = xDoc.GetElementsByTagName("Musterija");
        var lastNode = allNodes[allNodes.Count - 1];
        XmlElement node = SerializeToXmlElement(new Korisnik(k.Korisnicko_ime, k.Lozinka, k.Ime, k.Prezime, k.Pol, k.Kontakt_telefon, k.Email, k.Jmbg, k.Uloga));
        XmlNode importNode = xDoc.ImportNode(node, true);
        xDoc.DocumentElement.AppendChild(importNode);
        xDoc.Save(path);
    }

    foreach (Korisnik k in PostojeciKorisnici.ListaVozaca)
    {
        XmlDocument xDoc = new XmlDocument();
        xDoc.Load(path);

        var allNodes = xDoc.GetElementsByTagName("Vozac");
        var lastNode = allNodes[allNodes.Count - 1];
        XmlElement node = SerializeToXmlElement(new Korisnik(k.Korisnicko_ime, k.Lozinka, k.Ime, k.Prezime, k.Pol, k.Kontakt_telefon, k.Email, k.Jmbg, k.Uloga));
        XmlNode importNode = xDoc.ImportNode(node, true);
        xDoc.DocumentElement.AppendChild(importNode);
        xDoc.Save(path);
    }
}
else
{
    using (XmlWriter writer = XmlWriter.Create(@"C:\Users\HP\Desktop\Projakat\WP1718-PR101-2015\WebAPI_AJAX\WebAPI\WebAPI\baza.xml"))
    {
        writer.WriteStartDocument();
        writer.WriteStartElement("Korisnici");

        foreach (Korisnik k in PostojeciKorisnici.ListaKorisnika)
        {
            writer.WriteStartElement("Korisnici");
            writer.WriteElementString("KorisnickoIme", k.Korisnicko_ime);
            writer.WriteElementString("Sifra", k.Lozinka);
            writer.WriteElementString("Ime", k.Ime);
            writer.WriteElementString("Prezime", k.Prezime);
            writer.WriteElementString("Pol", k.Pol.ToString());
            writer.WriteElementString("JMBG", k.Jmbg);
            writer.WriteElementString("KontaktTelefon", k.Kontakt_telefon);
            writer.WriteElementString("EMail", k.Email);
            writer.WriteElementString("Uloga", k.Uloga.ToString());
            writer.WriteElementString("Uloga", k.Ulogovan.ToString());
            writer.WriteEndElement();
        }

        writer.WriteEndElement();
        writer.WriteEndDocument();
    }
}

/*
using (XmlWriter writer = XmlWriter.Create(@"C:\Users\HP\Desktop\Projakat\WP1718-PR101-2015\WebAPI_AJAX\WebAPI\WebAPI\baza.xml"))
{
    writer.WriteStartDocument();
    writer.WriteStartElement("Korisnici");

    foreach (Korisnik k in PostojeciKorisnici.ListaKorisnika)
    {
        writer.WriteStartElement("Korisnici");

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
