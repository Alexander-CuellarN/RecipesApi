using System;
using System.Collections.Generic;

namespace RecipesApi.Models;

public partial class Receta
{
    public int Idreceta { get; set; }

    public int? Idusuario { get; set; }

    public string? Nombre { get; set; }

    public string? Descripción { get; set; }

    public int? TiempoPreparacion { get; set; }

    public virtual ICollection<Calificacione> Calificaciones { get; set; } = new List<Calificacione>();

    public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();

    public virtual Usuario? IdusuarioNavigation { get; set; }

    public virtual ICollection<Ingrediente> Ingredientes { get; set; } = new List<Ingrediente>();

    public virtual ICollection<Preparacion> Preparacions { get; set; } = new List<Preparacion>();
}
