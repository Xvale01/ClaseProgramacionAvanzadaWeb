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

                HttpContext.Session.SetString("Correo", resp?.Dato?.Correo!);
                HttpContext.Session.SetString("Nombre", resp?.Dato?.NombreUsuario!);
                HttpContext.Session.SetString("Categoria", resp?.Dato?.NombreCategoria!);


                if ((bool)(resp?.Dato?.EsTemporal!)) //Si se tiene una contraseña temporal 
                    return RedirectToAction("CambiarContrasenna", "Home");
                else
                {
                    HttpContext.Session.SetString("Login", "true");
                    return RedirectToAction("PantallaInicio", "Home");
                }

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

        [HttpGet]
        public IActionResult RecuperarAcceso()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [HttpPost]
        public IActionResult RecuperarAcceso(Usuario entidad)
        {
            var resp = _usuarioModel.RecuperarAcceso(entidad);

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

        [HttpGet]
        public IActionResult CambiarContrasenna()
        {
            var usuario = new Usuario();

            //estamos haciendo uso de la varible de sesion "Correo" creada al iniciar sesion
            usuario.Correo = HttpContext.Session.GetString("Correo"); 
            return View(usuario);
        }

        [HttpPost]
        public IActionResult CambiarContrasenna(Usuario entidad)
        {
            //el método Trin elimina los espacios en blanco
            if (entidad.Contrasenna?.Trim() == entidad.ContrasennaTemporal?.Trim())
            {
                ViewBag.MsjPantalla = "Debe utilizar una contraseña distinta";
                return View();
            }

            entidad.Contrasenna = _utilitariosModel.Encrypt(entidad.Contrasenna!);
            entidad.ContrasennaTemporal = _utilitariosModel.Encrypt(entidad.ContrasennaTemporal!);

            var resp = _usuarioModel.CambiarContrasenna(entidad);

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


        [Seguridad] //Este filtro pregunta si uno esta logueago, se utiliza para las vistas que si o si necesitamos estar logueados
        [HttpGet] //Salir o cerrar sesion es de tipo get porque la estamos llamando desde un hipervinculo "href"
        public IActionResult Salir(Usuario entidad)
        {

            HttpContext.Session.Clear();
            return RedirectToAction("IniciarSesion", "Home");

        }


    }
}
