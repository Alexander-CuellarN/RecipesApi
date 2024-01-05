using System;
using System.Collections.Generic;

namespace RecipesApi.Models;

public partial class Ingrediente
{
    public int Idingrediente { get; set; }

    public int? Idreceta { get; set; }

    public string? Nombre { get; set; }

    public int? Cantidad { get; set; }

    public virtual Receta? IdrecetaNavigation { get; set; }
}
