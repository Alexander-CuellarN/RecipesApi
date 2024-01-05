using System.ComponentModel.DataAnnotations;

namespace RecipesApi.Models.DTOS.Usuarios
{
    public class UsuarioDto
    {
        [Required]
        public int Idusuario { get; set; }
        [Required]
        public string? NombreUsuario { get; set; }
        [Required]
        public string? CorreoElectronico { get; set; }

    }
}
