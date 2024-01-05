using System;
using System.Collections.Generic;

namespace RecipesApi.Models;

public partial class Preparacion
{
    public int Idpaso { get; set; }

    public int? Idreceta { get; set; }

    public string? Descripcion { get; set; }

    public int? Orden { get; set; }

    public virtual Receta? IdrecetaNavigation { get; set; }
}
