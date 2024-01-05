using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipesApi.Models;

public partial class Usuario
{
    [Required]
    public int Idusuario { get; set; }

    [Required]
    public string? NombreUsuario { get; set; }

    [Required]
    public string? CorreoElectronico { get; set; }

    [Required]
    public string? Contraseña { get; set; }

    public virtual ICollection<Calificacione> Calificaciones { get; set; } = new List<Calificacione>();

    public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();

    public virtual ICollection<Receta> Receta { get; set; } = new List<Receta>();
}
