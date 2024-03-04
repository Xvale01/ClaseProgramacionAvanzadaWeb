using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoWeb_Sabado.Models;
using System.Diagnostics;

namespace ProyectoWeb_Sabado.Controllers
{
    [ResponseCache(NoStore = true, Duration = 0)]
    public class HomeController : Controller
    {
        public HomeController()
        {

        }

        [Seguridad]
        public IActionResult PantallaInicio()
        {
            return View();
        }

        public IActionResult IniciarSesion()
        {
            HttpContext.Session.Clear();
            return View();
        }

        public IActionResult RegistrarUsuario()
        {
            HttpContext.Session.Clear();
            return View();
        }

    }
}
