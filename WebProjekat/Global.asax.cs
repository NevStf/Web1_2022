using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebProjekat
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            BazaPodataka.BazaKorisnika.UcitajKorisnike();
            BazaPodataka.BazaFitnesCentara.UcitajFitnesCentre();
            BazaPodataka.BazaGrupnihTreninga.UcitajTreninge();
            BazaPodataka.BazaKometara.UcitajKomentare();
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
