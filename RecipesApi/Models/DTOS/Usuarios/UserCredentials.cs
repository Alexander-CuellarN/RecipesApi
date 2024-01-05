using System.ComponentModel.DataAnnotations;

namespace RecipesApi.Models.DTOS.Usuarios
{
    public class UserCredentials
    {
        [Required]
        public string? CorreoElectronico { get; set; }
        [Required]
        public string? Contraseña { get; set; }
    }
}
