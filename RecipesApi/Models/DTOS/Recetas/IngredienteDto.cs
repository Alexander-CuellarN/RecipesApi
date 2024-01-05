namespace RecipesApi.Models.DTOS.Recetas
{
    public class IngredienteDto
    {
        public int Idingrediente { get; set; }

        public int? Idreceta { get; set; }

        public string? Nombre { get; set; }

        public int? Cantidad { get; set; }
    }
}
