using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipesApi.Models;
using RecipesApi.Models.DTOS.Recetas;
using RecipesApi.Models.DTOS.Usuarios;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace RecipesApi.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class RecipessController : ControllerBase
    {
        private readonly DbRecipiesContext _recipiesContext;
        private ResponseGeneric<RecetasDto> Response;

        public RecipessController(DbRecipiesContext recipiesContext)
        {
            _recipiesContext = recipiesContext;
            Response = new ResponseGeneric<RecetasDto>();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DataRecipiesDto data)
        {
            try
            {
                var recetaDto = data.recetaDto;
                var ingredientesDto = data.ingredientesDto;
                var preparacionDto = data.preparacionDto;

                using (var transaction = await _recipiesContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var Receta = new Receta()
                        {
                            Nombre = recetaDto.Nombre,
                            Descripción = recetaDto.Descripción,
                            TiempoPreparacion = recetaDto.TiempoPreparacion,
                            Idusuario = recetaDto.Idusuario,
                        };

                        var newRecipe = _recipiesContext.Recetas.Add(Receta);
                        await _recipiesContext.SaveChangesAsync();

                        foreach (var ingredienteDto in ingredientesDto)
                        {
                            _recipiesContext.Ingredientes.Add(new Ingrediente()
                            {
                                Cantidad = ingredienteDto.Cantidad,
                                Nombre = ingredienteDto.Nombre,
                                Idreceta = newRecipe.Entity.Idreceta
                            });

                            await _recipiesContext.SaveChangesAsync();
                        }

                        foreach (var preparacion in preparacionDto)
                        {
                            _recipiesContext.Preparacions.Add(new Preparacion()
                            {
                                Descripcion = preparacion.Descripcion,
                                Orden = preparacion.Orden,
                                Idreceta = newRecipe.Entity.Idreceta
                            });

                            await _recipiesContext.SaveChangesAsync();
                        }
                        await _recipiesContext.SaveChangesAsync();
                        await transaction.CommitAsync();

                        Response.Message = "Se ha creado la receta de forma exitosa";
                        Response.Data = new List<RecetasDto>()
                        {
                            new RecetasDto()
                            {
                                Idreceta= newRecipe.Entity.Idreceta,
                                Descripción =  newRecipe.Entity.Descripción,
                                Nombre= newRecipe.Entity.Nombre,
                                TiempoPreparacion = newRecipe.Entity.TiempoPreparacion,
                                Idusuario = recetaDto.Idusuario
                            }
                        };
                        return Ok(Response);
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        Response.Message = ex.Message;
                        return BadRequest(Response);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromBody] DataRecipiesDto data, int id)
        {
            try
            {
                var recetaDto = data.recetaDto;

                if (id != recetaDto.Idreceta)
                    throw new Exception("La receta no coincide el id suministrado");


                var ingredientesDto = data.ingredientesDto;
                var preparacionDto = data.preparacionDto;

                using (var transaction = await _recipiesContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var recetaFounded = _recipiesContext.Recetas.Find(id);

                        if (recetaFounded == null)
                            throw new Exception("La es posible encontrar la receta Solicitada");

                        recetaFounded.Nombre = recetaDto.Nombre;
                        recetaFounded.Descripción = recetaDto.Descripción;
                        recetaFounded.TiempoPreparacion = recetaDto.TiempoPreparacion;
                        recetaFounded.Idusuario = recetaDto.Idusuario;

                        var RecipeUpdate = _recipiesContext.Recetas.Update(recetaFounded);
                        await _recipiesContext.SaveChangesAsync();

                        _recipiesContext.Ingredientes.RemoveRange(
                            _recipiesContext.Ingredientes
                            .Where(r => r.Idreceta == RecipeUpdate.Entity.Idreceta)
                            .ToList()
                            );

                        foreach (var ingredienteDto in ingredientesDto)
                        {
                            _recipiesContext.Ingredientes.Add(new Ingrediente()
                            {
                                Cantidad = ingredienteDto.Cantidad,
                                Nombre = ingredienteDto.Nombre,
                                Idreceta = RecipeUpdate.Entity.Idreceta
                            });

                            await _recipiesContext.SaveChangesAsync();
                        }

                        _recipiesContext.Preparacions.RemoveRange(
                            _recipiesContext.Preparacions
                            .Where(r => r.Idreceta == RecipeUpdate.Entity.Idreceta)
                            .ToList()
                            );

                        foreach (var preparacion in preparacionDto)
                        {
                            _recipiesContext.Preparacions.Add(new Preparacion()
                            {
                                Descripcion = preparacion.Descripcion,
                                Orden = preparacion.Orden,
                                Idreceta = RecipeUpdate.Entity.Idreceta
                            });

                            await _recipiesContext.SaveChangesAsync();
                        }
                        await _recipiesContext.SaveChangesAsync();
                        await transaction.CommitAsync();

                        Response.Message = "Se ha modificado la receta de forma exitosa";
                        Response.Data = new List<RecetasDto>()
                        {
                            new RecetasDto()
                            {
                                Idreceta= RecipeUpdate.Entity.Idreceta,
                                Descripción =  RecipeUpdate.Entity.Descripción,
                                Nombre= RecipeUpdate.Entity.Nombre,
                                TiempoPreparacion = RecipeUpdate.Entity.TiempoPreparacion,
                                Idusuario = RecipeUpdate.Entity.Idusuario
                            }
                        };
                        return Ok(Response);
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        Response.Message = ex.Message;
                        return BadRequest(Response);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                using (var transation = await _recipiesContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var recetaFounded = await _recipiesContext.Recetas.FindAsync(id);

                        if (recetaFounded == null)
                            throw new Exception("No se encuentra una receta para la id dada");

                        _recipiesContext.Ingredientes.RemoveRange(
                            _recipiesContext.Ingredientes
                            .Where(i => i.Idreceta == recetaFounded.Idreceta)
                            .ToList());

                        _recipiesContext.Preparacions.RemoveRange(
                            _recipiesContext.Preparacions
                            .Where(p => p.Idreceta == recetaFounded.Idreceta)
                            .ToList());

                        await _recipiesContext.SaveChangesAsync();
                        await transation.CommitAsync();

                        return NoContent();
                    }
                    catch (Exception ex)
                    {
                        Response.Message = ex.Message;
                        return BadRequest(Response);
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Message = ex.Message;
                return BadRequest(Response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            try
            {
                var Recipes = _recipiesContext.Recetas
                    .Include(I => I.Ingredientes)
                    .Include(p => p.Preparacions)
                    .ToList();

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                string json = JsonSerializer.Serialize(Recipes, options);

                return Ok(new
                {
                    Message = "lista de recetas",
                    Data = json
                });
            }
            catch (Exception ex)
            {
                Response.Message = ex.Message;
                return BadRequest(Response);
            }
        }

        [HttpGet("{id:int}/{idUser:int}")]
        public async Task<IActionResult> GetById(int id, int idUser)
        {
            try
            {
                var recipe = _recipiesContext.Recetas
                .Include(I => I.Ingredientes)
                .Include(p => p.Preparacions)
                .Include(c => c.Calificaciones.Where(ca => ca.Idusuario == idUser))
                .Where(r => r.Idreceta == id)
                .ToList();

                var options = new JsonSerializerOptions()
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                var response = JsonSerializer.Serialize(recipe, options);

                return Ok(new
                {
                    Message = "lista de recetas",
                    Data = response
                });

            }
            catch (Exception ex)
            {
                Response.Message = ex.Message;
                return BadRequest(Response);
            }
        }

    }
}
