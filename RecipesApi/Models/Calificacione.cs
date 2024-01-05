using System;
using System.Collections.Generic;

namespace RecipesApi.Models;

public partial class Calificacione
{
    public int IdCalificacion { get; set; }

    public int? Idusuario { get; set; }

    public int? Idreceta { get; set; }

    public int? Calificacion { get; set; }

    public virtual Receta? IdrecetaNavigation { get; set; }

    public virtual Usuario? IdusuarioNavigation { get; set; }
}
