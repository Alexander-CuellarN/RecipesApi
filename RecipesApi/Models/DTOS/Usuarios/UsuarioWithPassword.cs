using System.ComponentModel.DataAnnotations;

namespace RecipesApi.Models.DTOS.Usuarios
{
    public class UsuarioWithPassword
    {
        [Required]
        public int Idusuario { get; set; }
        [Required]
        public string? NombreUsuario { get; set; }
        [Required]
        public string? CorreoElectronico { get; set; }
        [Required]
        public string? Contraseña { get; set; }
    }
}
