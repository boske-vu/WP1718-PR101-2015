using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class Lokacija
    {
        public string KoordinataX { get; set; }
        public string KoordinataY { get; set; }

        public Adresa Adresa { get; set; }

        public Lokacija(string kx, string ky, Adresa adresa)
        {
            KoordinataX = kx;
            KoordinataY = ky;
            Adresa = adresa;
        }

    }
}