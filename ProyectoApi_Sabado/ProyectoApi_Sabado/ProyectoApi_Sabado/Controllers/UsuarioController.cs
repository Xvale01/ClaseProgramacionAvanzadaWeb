using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data.SqlClient;

namespace ProyectoApi_Sabado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        [HttpGet]
        [Route("ConsultarUsuarios")]
        public IActionResult ConsultarUsuarios(string cedula)
        {
            using (var context = new SqlConnection("Server=localhost\\MSSQLSERVER01; Database=BD_SABADO; Trusted_Connection=True"))
            {
                return Ok(context.Query<Usuario>("TraerUsuarios", new { cedula }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault());
            }
        }

    }
}
