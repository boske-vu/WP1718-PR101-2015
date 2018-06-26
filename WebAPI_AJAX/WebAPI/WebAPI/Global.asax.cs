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

            Musterija m = new Musterija("d", "d", "dsfj", "djfi", Pol.Muski, "wfeijf", "sfji", "fdjfij", Uloge.Musterija);
            PostojeciKorisnici.ListaKorisnika.Add(m);
            PostojeciKorisnici.ListaMusterija.Add(m);
            
            if(PostojeciKorisnici.ListaDispecera.Count() == 0)
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
            }

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
        }
    }
}
