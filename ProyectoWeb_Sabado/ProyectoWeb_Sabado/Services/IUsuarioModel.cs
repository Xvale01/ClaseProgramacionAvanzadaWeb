using ProyectoWeb_Sabado.Entities;

namespace ProyectoWeb_Sabado.Services
{
    public interface IUsuarioModel
    {
        public Respuesta RegistrarUsuario(Usuario entidad);
        public UsuarioRespuesta? IniciarSesion(Usuario entidad);
        public UsuarioRespuesta? RecuperarAcceso(Usuario entidad);


    }
}
