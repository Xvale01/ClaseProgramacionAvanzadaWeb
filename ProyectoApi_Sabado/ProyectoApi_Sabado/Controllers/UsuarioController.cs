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

namespace ProyectoApi_Sabado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUtilitariosModel _utilitariosModel;
        public UsuarioController(IUtilitariosModel utilitariosModel)
        {
            _utilitariosModel = utilitariosModel;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("IniciarSesion")]
        public IActionResult IniciarSesion(Usuario entidad)
        {
            if (entidad.cedula == "304590415" && entidad.contrasenna == "secreta")
            {
                return Ok(_utilitariosModel.GenerarToken(entidad.cedula));
            }

            return NotFound("Sus credenciales no son correctas");
        }

        [Authorize]
        [HttpGet]
        [Route("ConsultarDia")]
        public IActionResult ConsultarDia()
        {
            var claims = User.Claims;

            return Ok(DateTime.Now);
        }

    }
}
