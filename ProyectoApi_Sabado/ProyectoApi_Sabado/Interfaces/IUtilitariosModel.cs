namespace ProyectoApi_Sabado.Services
{
    public interface IUtilitariosModel
    {
        public string GenerarToken(string correo);
        public string GenerarContraseñaTemporal();
        public string Encrypt(string texto);
        public void EnviarCorreo(string Destinatario, string Asunto, string Mensaje);

    }
}
