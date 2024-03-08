using ProyectoWeb_Sabado.Entities;
using ProyectoWeb_Sabado.Services;

namespace ProyectoWeb_Sabado.Models
{
    public class UsuarioModel : IUsuarioModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public UsuarioModel(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public int RegistrarUsuario(Usuario entidad)
        {
            string url = _configuration.GetSection("Parametros:UrlWebApi").Value + "api/Usuario/RegistrarUsuario";
            JsonContent body = JsonContent.Create(entidad);
            var resp = _httpClient.PostAsync(url, body).Result;

            if (resp.IsSuccessStatusCode)
                return resp.Content.ReadFromJsonAsync<int>().Result;

            return 0;
        }
    }
}
