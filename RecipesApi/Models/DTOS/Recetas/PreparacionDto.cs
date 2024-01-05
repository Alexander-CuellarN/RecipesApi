namespace RecipesApi.Models.DTOS.Recetas
{
    public class PreparacionDto
    {
        public int Idpaso { get; set; }
        public int? Idreceta { get; set; }
        public string? Descripcion { get; set; }
        public int? Orden { get; set; }
    }
}
