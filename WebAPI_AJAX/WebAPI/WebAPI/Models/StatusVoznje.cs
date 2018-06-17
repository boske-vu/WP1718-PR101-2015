using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public enum StatusVoznje
    {
        NaCekanju,
        Formirana,
        Obradjena,
        Prihvacena,
        Otkazana,
        Neuspesna,
        Uspesna
    }
}