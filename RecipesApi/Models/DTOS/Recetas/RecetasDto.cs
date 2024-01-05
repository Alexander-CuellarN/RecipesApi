namespace RecipesApi.Models.DTOS.Recetas
{
    public class RecetasDto
    {
        public int Idreceta { get; set; }
        public int? Idusuario { get; set; }
        public string? Nombre { get; set; }

        public string? Descripción { get; set; }

        public int? TiempoPreparacion { get; set; }
    }
}
