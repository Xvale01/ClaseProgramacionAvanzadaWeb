using Microsoft.AspNetCore.Mvc;
using ProyectoApi_Sabado.Entidades;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Authorization;
using ProyectoApi_Sabado.Models;
using ProyectoApi_Sabado.Services;
using System.Data.SqlClient;
using Dapper;
using System.Data;

namespace ProyectoApi_Sabado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUtilitariosModel _utilitariosModel;
        private readonly IConfiguration _configuration;

        public UsuarioController(IUtilitariosModel utilitariosModel, IConfiguration configuration)
        {
            _utilitariosModel = utilitariosModel;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("IniciarSesion")]
        public IActionResult IniciarSesion(Usuario entidad)
        {
             return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("RegistrarUsuario")]
        public IActionResult RegistrarUsuario(Usuario entidad)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                return Ok(db.Execute("sp_RegistrarUsuario", new { entidad.correo, entidad.contrasenna, entidad.nombre }, commandType: CommandType.StoredProcedure));
            }
        }

    }
}
