using InmobiliariaAlbornoz.Data;
using InmobiliariaAlbornoz.Models;
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