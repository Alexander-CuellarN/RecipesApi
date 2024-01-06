using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipesApi.Models;
using RecipesApi.Models.DTOS;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RecipesApi.Controllers
{
    [Route("/api/[Controller]")]
    [ApiController]
    public class CalificacionesController : ControllerBase
    {
        private readonly DbRecipiesContext _recipiesContext;
        private ResponseGeneric<CalificacionesDto> Response;

        public CalificacionesController(DbRecipiesContext recipiesContext)
        {
            _recipiesContext = recipiesContext;
            Response = new ResponseGeneric<CalificacionesDto>();
        }

        [HttpGet("/{idRecipe:int}")]
        public async Task<IActionResult> GetAllCalificactions(int idRecipe)
        {
            try
            {
                var recipe = _recipiesContext.Recetas
                    .Include(c => c.Calificaciones.Where(ca => ca.Idreceta == idRecipe))
                        .ThenInclude(c => c.IdusuarioNavigation)
                    .FirstOrDefault(p => p.Idreceta == idRecipe);

                var options = new JsonSerializerOptions()
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                var calificaciones = _recipiesContext.Calificaciones
                                   .Include(ca => ca.IdrecetaNavigation)
                                   .Include(ca => ca.IdusuarioNavigation)
                                   .Where(ca => ca.Idreceta == idRecipe)
                                   .ToList();
                var calificacionesDto = new List<CalificacionesDto>();

                foreach (var calificacion in calificaciones)
                {
                    calificacionesDto.Add(
                        new CalificacionesDto()
                        {
                            Calificacion = calificacion.Calificacion,
                            Idreceta = calificacion.Idreceta,
                            Idusuario = calificacion.Idusuario,
                            recetaName = calificacion.IdrecetaNavigation.Nombre,
                            UserName = calificacion.IdusuarioNavigation.NombreUsuario,
                        });
                }

                Response.Message = "Listado de calificaciones";
                Response.Data = calificacionesDto;

                return Ok(Response);
            }
            catch (Exception ex)
            {
                Response.Message = ex.Message;
                return BadRequest(Response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCalificacion([FromBody] CalificacionesDto calificacionesDto)
        {
            try
            {
                var calificacion = new Calificacione()
                {
                    Idreceta = calificacionesDto.Idreceta,
                    Idusuario = calificacionesDto.Idusuario,
                    Calificacion = calificacionesDto.Calificacion
                };

                var calificacionNew = _recipiesContext.Calificaciones.Add(calificacion);
                await _recipiesContext.SaveChangesAsync();

                calificacionesDto.IdCalificacion = calificacionNew.Entity.IdCalificacion;
                Response.Message = "Se agrego la calificacion con exito";
                Response.Data = new List<CalificacionesDto> { calificacionesDto };

                return Ok(Response);
            }
            catch (Exception ex)
            {
                Response.Message = ex.Message;
                return BadRequest(Response);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCalificacion(CalificacionesDto calificacionesDto)
        {
            try
            {
                var calificacio = await _recipiesContext.Calificaciones
                                .Where(c => c.Idreceta == calificacionesDto.Idreceta &&
                                    c.Idusuario == calificacionesDto.Idusuario
                                 )
                                .FirstOrDefaultAsync();

                calificacio.Calificacion = calificacionesDto.Calificacion;

                _recipiesContext.Calificaciones.Update(calificacio);

                Response.Message = "Se modifico correctamente la calificacion";
                Response.Data = new List<CalificacionesDto>() { calificacionesDto };
                return Ok(Response);
            }
            catch (Exception ex)
            {
                Response.Message = ex.Message;
                return BadRequest(Response);
            }
        }
    }
}
