using Microsoft.AspNetCore.Mvc;
using ProyectoApi_Sabado.Entidades;
using Microsoft.AspNetCore.Authorization;
using ProyectoApi_Sabado.Services;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using ProyectoApi_Sabado.Entities;

namespace ProyectoApi_Sabado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController(IConfiguration _configuration, IUtilitariosModel _utilitariosModel, IHostEnvironment _hostEnvironment) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("IniciarSesion")]
        public IActionResult IniciarSesion(Usuario entidad)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                UsuarioRespuesta respuesta = new UsuarioRespuesta();
                var resultado = db.Query<Usuario>("IniciarSesion",
                    new { entidad.Correo, entidad.Contrasenna },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

                if (resultado == null)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "Sus credenciales no son correctos";
                }
                else
                {
                    respuesta.Dato = resultado;
                    respuesta.Dato.Token = _utilitariosModel.GenerarToken(resultado.Correo ?? string.Empty);
                }

                return Ok(respuesta);

            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RegistrarUsuario")]
        public IActionResult RegistrarUsuario(Usuario entidad)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                Respuesta respuesta = new Respuesta();
                var resultado = db.Execute("RegistrarUsuario", 
                    new { entidad.Correo, entidad.Contrasenna, entidad.NombreUsuario }, 
                    commandType: CommandType.StoredProcedure);
                if (resultado <= 0)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "Su correo ya se encuentra registrado";
                }
                return Ok(respuesta);
            }
        }



        [AllowAnonymous]
        [HttpPost]
        [Route("RecuperarAcceso")]
        public IActionResult RecuperarAcceso(Usuario entidad)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                UsuarioRespuesta respuesta = new UsuarioRespuesta();

                // Actualiza la contraseña por una contraseña temporal
                string NuevaContrasennaTemporal = _utilitariosModel.GenerarContraseñaTemporal();
                string Contrasenna = _utilitariosModel.Encrypt(NuevaContrasennaTemporal);
                bool EsTemporal = true;

                var resultado = db.Query<Usuario>("RecuperarAcceso",
                new { entidad.Correo, Contrasenna, EsTemporal },
                commandType: CommandType.StoredProcedure).FirstOrDefault();

                if (resultado == null)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "Sus datos no son correctos";
                }
                else
                {
                    // Enviar el correo

                    string contenido = Path.Combine(_hostEnvironment.ContentRootPath, "Mails", "RecuperarContrasenna.html");
                    string htmlBody = System.IO.File.ReadAllText(contenido);

                    htmlBody = htmlBody.Replace("@Usuario@", resultado.NombreUsuario);
                    htmlBody = htmlBody.Replace("@Contrasenna@", NuevaContrasennaTemporal);

                    _utilitariosModel.EnviarCorreo(resultado.Correo!, "Nueva Contraseña!!", htmlBody);
                    respuesta.Dato = resultado;
                }

                return Ok(respuesta);

            }
        }

        [AllowAnonymous]
        [HttpPut]
        [Route("CambiarContrasenna")]
        public IActionResult CambiarContrasenna(Usuario entidad)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                UsuarioRespuesta respuesta = new UsuarioRespuesta();

                // Actualiza la contraseña por una contraseña temporal

                bool EsTemporal = false;

                var resultado = db.Query<Usuario>("CambiarContrasenna",
                new { entidad.Correo, entidad.Contrasenna, entidad.ContrasennaTemporal, EsTemporal },
                commandType: CommandType.StoredProcedure).FirstOrDefault();

                if (resultado == null)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "Sus datos no son correctos";
                }
                else
                {
                    // Enviar el correo
                    string contenido = Path.Combine(_hostEnvironment.ContentRootPath, "Mails" ,"CambioContrasenna.html");
                    string htmlBody = System.IO.File.ReadAllText(contenido);

                    htmlBody = htmlBody.Replace("@Nombre@", resultado.NombreUsuario);

                    _utilitariosModel.EnviarCorreo(resultado.Correo!, "Cambio de Contraseña!!", "Se le informa que se ha realizado un cambio de contraseña " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    respuesta.Dato = resultado;
                }

                return Ok(respuesta);

            }
        }





    }
}
