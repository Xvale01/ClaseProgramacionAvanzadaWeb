using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoWeb_Sabado.Entities;
using ProyectoWeb_Sabado.Models;
using ProyectoWeb_Sabado.Services;
using System.Diagnostics;

namespace ProyectoWeb_Sabado.Controllers
{
    [ResponseCache(NoStore = true, Duration = 0)]
    public class HomeController(IUsuarioModel _usuarioModel, IUtilitariosModel _utilitariosModel) : Controller
    {
        [Seguridad]
        public IActionResult PantallaInicio()
        {
            return View();
        }

        [HttpGet]
        public IActionResult IniciarSesion()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [HttpPost]
        public IActionResult IniciarSesion(Usuario entidad)
        {
            entidad.Contrasenna = _utilitariosModel.Encrypt(entidad.Contrasenna);
            var resp = _usuarioModel.IniciarSesion(entidad);

            if (resp?.Codigo == "00")
            {
                HttpContext.Session.SetString("Login", "true");
                return RedirectToAction("PantallaInicio", "Home");
            }
            else
            {
                ViewBag.MsjPantalla = resp?.Mensaje;
                return View();
            }
        }

        [HttpGet]
        public IActionResult RegistrarUsuario()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [HttpPost]
        public IActionResult RegistrarUsuario(Usuario entidad)
        {
            entidad.Contrasenna = _utilitariosModel.Encrypt(entidad.Contrasenna);
            var resp = _usuarioModel.RegistrarUsuario(entidad);

            if (resp?.Codigo == "00")
            {
                return RedirectToAction("IniciarSesion", "Home");
            }
            else
            {
                ViewBag.MsjPantalla = resp?.Mensaje;
                return View();
            }


        }

    }
}
