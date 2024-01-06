using System.ComponentModel.DataAnnotations;

namespace RecipesApi.Models.DTOS
{
    public class CalificacionesDto
    {
        [Required]
        public int? Idusuario { get; set; }
        [Required]
        public int? Idreceta { get; set; }
        [Required]
        public int? Calificacion { get; set; }

        public int IdCalificacion { get; set; }
        public string recetaName { get; set; }
        public string UserName { get; set; }
    }
}
