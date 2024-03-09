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
    public class UsuarioController(IConfiguration _configuration, IUtilitariosModel _utilitariosModel) : ControllerBase
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





    }
}
