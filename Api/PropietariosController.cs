using InmobiliariaAlbornoz.Data;
using InmobiliariaAlbornoz.Models;
using InmobiliariaAlbornoz.ModelsAux;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InmobiliariaAlbornoz.Api
{
	[Route("api/[controller]/[action]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ApiController]
	public class PropietariosController : ControllerBase
	{
		private readonly DataContext contexto;
		private readonly IConfiguration config;
		

		public PropietariosController(DataContext contexto, IConfiguration config)
		{
			this.contexto = contexto;
			this.config = config;
			
		}
		
		// POST api/<controller>/login
		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromForm] LoginView loginView)
		{
			try
			{
				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
					password: loginView.Clave,
					salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
					prf: KeyDerivationPrf.HMACSHA1,
					iterationCount: 1000,
					numBytesRequested: 256 / 8));
				var p = await contexto.Propietario.FirstOrDefaultAsync(x => x.Email == loginView.Usuario);
				if (p == null || p.Clave != hashed)
				{
					return BadRequest("Nombre de usuario o clave incorrecta");
				}
				else
				{
					var key = new SymmetricSecurityKey(
						System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
					var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
					var claims = new List<Claim>
					{
						//new Claim("Id", p.Id),
						new Claim(ClaimTypes.Name, p.Email),
						new Claim("FullName", p.Nombre),
						new Claim(ClaimTypes.Role, "Propietario"),
					};

					var token = new JwtSecurityToken(
						issuer: config["TokenAuthentication:Issuer"],
						audience: config["TokenAuthentication:Audience"],
						claims: claims,
						expires: DateTime.Now.AddMinutes(60),
						signingCredentials: credenciales
					);
					return Ok(new JwtSecurityTokenHandler().WriteToken(token));
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

		// GET: api/<controller>/getpropietario						// Ok
		[HttpGet]
		public async Task<ActionResult<Propietario>> GetPropietario()	
		{
			try
			{
				var usuario = User.Identity.Name;
				Propietario prop = await contexto.Propietario.SingleOrDefaultAsync(x => x.Email == usuario);
				return Ok(prop);
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

		[HttpPut]
		public async Task<IActionResult> ActualizarPropietario([FromBody] PropietarioPutDto propietario)
		{
			try
			{
				if (propietario == null)
				{
					return BadRequest("Propietario inválido.");
				}

				// Obtener el email del usuario autenticado desde el token JWT
				var emailUsuarioLogueado = User.Identity.Name;  // Este es el email del usuario en el token

				// Buscar el propietario a actualizar en la base de datos
				var propietarioExistente = await contexto.Propietario.SingleOrDefaultAsync(x => x.Email == emailUsuarioLogueado);
				if (propietarioExistente == null)
				{
					return NotFound("Propietario no encontrado.");
				}

				// Validar que el propietario autenticado es el mismo que está intentando actualizar su información
				if (propietarioExistente.Email != emailUsuarioLogueado)
				{
					return Forbid("No tienes permisos para actualizar este propietario.");
				}

				propietarioExistente.Dni = propietario.Dni;
				propietarioExistente.Nombre = propietario.Nombre;
				propietarioExistente.Apellido = propietario.Apellido;
				propietarioExistente.Email = propietario.Email;
				propietarioExistente.Telefono = propietario.Telefono;
				propietarioExistente.FechaN = propietario.FechaN;
				propietarioExistente.Direccion = propietario.Direccion;

				// Guardar los cambios
				contexto.Propietario.Update(propietarioExistente);
				await contexto.SaveChangesAsync();

				return Ok(propietarioExistente);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut]
		public async Task<IActionResult> ActualizarClave([FromForm] string currentpass, [FromForm] string newpass)
		{
			try
			{
				// Obtener el email del usuario autenticado desde el token JWT
				var emailUsuarioLogueado = User.Identity.Name; // Este es el email del usuario en el token

				// Buscar el propietario a actualizar en la base de datos
				var propietarioExistente = await contexto.Propietario.SingleOrDefaultAsync(x => x.Email == emailUsuarioLogueado);
				if (propietarioExistente == null)
				{
					return NotFound("Propietario no encontrado.");
				}

				// Validar la contraseña actual
				var currentFromBodyHashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
					password: currentpass,
					salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
					prf: KeyDerivationPrf.HMACSHA1,
					iterationCount: 1000,
					numBytesRequested: 256 / 8));

				if (currentFromBodyHashed != propietarioExistente.Clave)
				{
					return BadRequest("La contraseña actual es incorrecta.");
				}

				// Hashear la nueva contraseña
				var newPassHashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
					password: newpass,
					salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
					prf: KeyDerivationPrf.HMACSHA1,
					iterationCount: 1000,
					numBytesRequested: 256 / 8));

				// Actualizar la contraseña
				propietarioExistente.Clave = newPassHashed;

				// Guardar los cambios
				contexto.Propietario.Update(propietarioExistente);
				await contexto.SaveChangesAsync();

				return Ok("Contraseña actualizada correctamente.");
			}
			catch (Exception ex)
			{
				return BadRequest($"Error: {ex.Message}");
			}
		}


        // GET api/<controller>/GetAll
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await contexto.Propietario.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
		[AllowAnonymous]
        public IActionResult GetPropietario(int id) => Ok(contexto.Propietario.Find(id));


		[HttpGet("{id}")]
		//[AllowAnonymous]
        public IActionResult GetUsuario(int id) => Ok(contexto.Usuario.Find(id));


	}
}