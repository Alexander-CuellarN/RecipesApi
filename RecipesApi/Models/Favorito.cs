using System;
using System.Collections.Generic;

namespace RecipesApi.Models;

public partial class Favorito
{
    public int IdFavorito { get; set; }

    public int? Idusuario { get; set; }

    public int? Idreceta { get; set; }

    public virtual Receta? IdrecetaNavigation { get; set; }

    public virtual Usuario? IdusuarioNavigation { get; set; }
}
