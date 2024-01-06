using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipesApi.Models;
using RecipesApi.Models.DTOS.Usuarios;
using System.Security.Cryptography;
using System.Text;

namespace RecipesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly DbRecipiesContext _recipiesContext;
        private ResponseGeneric<UsuarioDto> response;

        public UsuarioController(DbRecipiesContext recipiesContext)
        {
            _recipiesContext = recipiesContext;
            response = new ResponseGeneric<UsuarioDto>();
        }

        [HttpGet]
        public async Task<IActionResult> ListAll()
        {
            try
            {
                var userList = await _recipiesContext.Usuarios.ToListAsync();

                var userDtoList = new List<UsuarioDto>();

                foreach (var user in userList)
                {
                    userDtoList.Add(
                        new UsuarioDto()
                        {
                            CorreoElectronico = user.CorreoElectronico,
                            Idusuario = user.Idusuario,
                            NombreUsuario = user.NombreUsuario,
                        });
                }
                response.Message = "Listado de usuarios";
                response.Data = userDtoList;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UsuarioWithPassword userDto)
        {
            var response = new ResponseGeneric<UsuarioDto>();

            try
            {
                if (!ModelState.IsValid)
                    throw new Exception("La data no es valida");

                var user = new Usuario()
                {
                    NombreUsuario = userDto.NombreUsuario,
                    Contraseña = EncriptText(userDto.Contraseña),
                    CorreoElectronico = userDto.CorreoElectronico,
                };

                var Entity = _recipiesContext.Usuarios.Add(user);
                await _recipiesContext.SaveChangesAsync();

                var NewUser = new UsuarioDto()
                {
                    CorreoElectronico = Entity.Entity.CorreoElectronico,
                    NombreUsuario = Entity.Entity.NombreUsuario,
                    Idusuario = Entity.Entity.Idusuario
                };

                response.Message = "El usuario se ha creado con exito";
                response.Data = new List<UsuarioDto>() { NewUser };
                return Ok(response);

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LogIn([FromBody] UserCredentials credentials)
        {
            try
            {
                var passwordEncripting = EncriptText(credentials.Contraseña);

                var user = await _recipiesContext.Usuarios
                    .FirstOrDefaultAsync(u => u.CorreoElectronico == credentials.CorreoElectronico && u.Contraseña == passwordEncripting);

                if (user == null)
                    throw new Exception("Correo o Contraseña Incorrectos");

                response.Message = "Autenticacion exitosa";

                response.Data = new List<UsuarioDto>() {
                    new() {
                        Idusuario = user.Idusuario,
                        CorreoElectronico = user.CorreoElectronico,
                        NombreUsuario= user.NombreUsuario
                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }
        private string EncriptText(string text)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }


        [HttpPut]
        private async Task<IActionResult> Update([FromBody] UsuarioWithPassword user)
        {
            try
            {

                var userFounded = await _recipiesContext.Usuarios.FirstOrDefaultAsync(u => u.Idusuario == user.Idusuario);

                if (userFounded == null)
                    throw new Exception("No existe un usuario asociado la id dada");

                if (user.Contraseña != null)
                {
                    userFounded.Contraseña = EncriptText(user.Contraseña);
                }

                userFounded.NombreUsuario = user.NombreUsuario;
                userFounded.CorreoElectronico = user.CorreoElectronico;

                var responseUser = _recipiesContext.Update(user);
                await _recipiesContext.SaveChangesAsync();

                response.Message = "Se modifico correctamente el usuario";
                response.Data = new List<UsuarioDto>()
                {
                    new UsuarioDto
                    {
                        NombreUsuario = responseUser.Entity.NombreUsuario,
                        CorreoElectronico = responseUser.Entity.CorreoElectronico,
                        Idusuario= responseUser.Entity.Idusuario
                    }
                };

                return Ok(response);

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpDelete("{id:int}")]
        private async Task<IActionResult> Delete(int id)
        {
            try
            {
                var UserFounded = await _recipiesContext.Usuarios.FindAsync();

                if (UserFounded == null)
                    throw new Exception("no existe un usuario a asociados al id ");

                _recipiesContext.Remove(UserFounded);
                await _recipiesContext.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }
    }
}

