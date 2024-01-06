using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipesApi.Models;

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
        public async Task<IActionResult> GetMyFavoriesRecipies(int id)
        {
            try
            {
                var favoritiesRecipes = _recipiesContext.Recetas.
                     Include(r => r.Favoritos.Where(f => f.Idusuario == id))
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
        public async Task<IActionResult> AddFavority([FromBody] int[] data)
        {
            try
            {
                var newRegister = _recipiesContext.Favoritos.Add(
                        new Favorito()
                        {
                            Idreceta = data[0],
                            Idusuario = data[1],
                        }
                    );

                _recipiesContext.SaveChanges();

                return Ok(new
                {
                    Message = "Se ha añadido con exito la receta a mis favoritos",
                    Data = new
                    {
                        Receta = data[0],
                        usuario = data[1],
                        Registro = newRegister.Entity.Idusuario
                    }
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message, Data = Array.Empty<object>() });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFavority([FromBody] int[] data)
        {
            try
            {
                var register = _recipiesContext.Favoritos
                    .FirstOrDefault(f => f.Idreceta == data[0] && f.Idusuario == data[1]);

                _recipiesContext.Favoritos.Remove(register);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message, Data = Array.Empty<object>() });
            }
        }
    }
}
