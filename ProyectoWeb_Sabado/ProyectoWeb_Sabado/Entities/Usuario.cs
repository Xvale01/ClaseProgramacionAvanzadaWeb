namespace ProyectoWeb_Sabado.Entities
{
    public class Usuario
    {
        public string correo { get; set; } = string.Empty;
        public string contrasenna { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
        public short idRol { get; set; }
        public bool estado { get; set; }
    }
}
