using Microsoft.AspNetCore.Mvc;

namespace RecipesApi.Models.DTOS.Recetas
{
    public class DataRecipiesDto
    {
        public RecetasDto recetaDto { get; set; }
        public IngredienteDto[] ingredientesDto { get; set; }
        public PreparacionDto[] preparacionDto { get; set; }
    }
}
