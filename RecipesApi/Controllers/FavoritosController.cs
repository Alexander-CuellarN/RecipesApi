using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipesApi.Models;
using RecipesApi.Models.DTOS;

namespace RecipesApi.Controllers
{
    [Route("api/[Controller]/")]
    [ApiController]
    public class FavoritosController : ControllerBase
    {
        private readonly DbRecipiesContext _recipiesContext;

        public FavoritosController(DbRecipiesContext recipiesContext)
        {
            _recipiesContext = recipiesContext;
        }

        [HttpGet("{idUser:int}")]
        public async Task<IActionResult> GetMyFavoriesRecipies(int idUser)
        {
            try
            {
                var favoritiesRecipes = _recipiesContext.Recetas.
                     Include(r => r.Favoritos.Where(f => f.Idusuario == idUser))
                     .Select(r => new
                     {
                         Idreceta = r.Idreceta,
                         Nombre = r.Nombre,
                         descripcion = r.Descripción,
                         tiempoPreparacion = r.TiempoPreparacion,
                         NombreUsuario = r.IdusuarioNavigation.NombreUsuario,
                         pasos = r.Preparacions.Select(pasos => new
                         {
                             orden = pasos.Orden,
                             descripcion = pasos.Descripcion
                         }),
                         Ingrediente = r.Ingredientes.Select(i => new
                         {
                             nombre = i.Nombre,
                             cantidad = i.Cantidad
                         })
                     })
                     .ToList();

                return Ok(new
                {
                    Message = "lista de recetas",
                    Data = favoritiesRecipes
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message,
                    Data = Array.Empty<object>()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddFavority([FromBody] FavoritosDto data)
        {
            try
            {
                var newRegister = _recipiesContext.Favoritos.Add(
                        new Favorito()
                        {
                            Idreceta = data.IdReceta,
                            Idusuario = data.IdUsuario,
                        }
                    );

                await _recipiesContext.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Se ha añadido con exito la receta a mis favoritos",
                    Data = new
                    {
                        data,
                        Registro = newRegister.Entity.Idusuario
                    }
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message, Data = Array.Empty<object>() });
            }
        }

        [HttpDelete("{idUser:int}/{idRecipe:int}")]
        public async Task<IActionResult> DeleteFavority([FromBody] int idUser, int idRecipe)
        {
            try
            {
                var register = _recipiesContext.Favoritos
                    .FirstOrDefault(f =>
                        f.Idreceta == idRecipe
                        && f.Idusuario == idUser
                    );

                _recipiesContext.Favoritos.Remove(register);
                await _recipiesContext.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message, Data = Array.Empty<object>() });
            }
        }

        [HttpGet("{idUser:int}/{idRecipe:int}", Name = "IsAFavoriteRecipe")]
        public async Task<IActionResult> IsAFavoriteRecipe(int idUser, int idRecipe)
        {
            try
            {
                var favoriteRecipe = await _recipiesContext.Favoritos
                .Where(f => f.Idusuario == idUser && f.Idreceta == idRecipe)
                .FirstOrDefaultAsync();

                if (favoriteRecipe == null)
                {
                    return Ok(new
                    {
                        message = String.Empty,
                        data = false
                    });
                }
                else
                {
                    return Ok(new
                    {
                        message = String.Empty,
                        data = true
                    });
                }
            }
            catch (Exception ex)
            {

                return BadRequest(new
                {
                    message = ex.Message,
                    data = true
                });
            }
        }
    }
}
