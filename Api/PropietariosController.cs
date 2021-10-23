using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using InmobiliariaAlbornoz.Data;
using InmobiliariaAlbornoz.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InmobiliariaAlbornoz.Api
{
	[Route("api/[controller]")]
	[ApiController]
	public class PropietariosController : ControllerBase//
	{
		private readonly InmobiliariaContext contexto;
		private readonly IConfiguration config;

		public PropietariosController(InmobiliariaContext contexto, IConfiguration config)
		{
			this.contexto = contexto;
			this.config = config;
		}
		// GET: api/<controller>
		[HttpGet]
		public async Task<ActionResult<Propietario>> Get()
		{
			try
			{
				/*contexto.Inmuebles
                    .Include(x => x.Duenio)
                    .Where(x => x.Duenio.Nombre == "")//.ToList() => lista de inmuebles
                    .Select(x => x.Duenio)
                    .ToList();//lista de propietarios*/
				var usuario = User.Identity.Name;
				/*contexto.Contratos.Include(x => x.Inquilino).Include(x => x.Inmueble).ThenInclude(x => x.Duenio)
                    .Where(c => c.Inmueble.Duenio.Email....);*/
				/*var res = contexto.Propietarios.Select(x => new { x.Nombre, x.Apellido, x.Email })
                    .SingleOrDefault(x => x.Email == usuario);*/
				return await contexto.Propietario.SingleOrDefaultAsync(x => x.Email == usuario);
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

		// GET api/<controller>/5
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			try
			{
				var entidad = await contexto.Propietario.SingleOrDefaultAsync(x => x.Id == id);
				return entidad != null ? Ok(entidad) : NotFound();
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
				var lista = await contexto.Propietario.ToListAsync();
				
				return Ok(lista);
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

		// POST api/<controller>/login
		//[HttpPost("login")]
		//[AllowAnonymous]
		//public async Task<IActionResult> Login([FromForm] LoginView loginView)
		//{
		//	try
		//	{
		//		string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
		//			password: loginView.Clave,
		//			salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
		//			prf: KeyDerivationPrf.HMACSHA1,
		//			iterationCount: 1000,
		//			numBytesRequested: 256 / 8));
		//		var p = await contexto.Propietario.FirstOrDefaultAsync(x => x.Email == loginView.Usuario);
		//		if (p == null || p.Clave != hashed)
		//		{
		//			return BadRequest("Nombre de usuario o clave incorrecta");
		//		}
		//		else
		//		{
		//			var key = new SymmetricSecurityKey(
		//				System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
		//			var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
		//			var claims = new List<Claim>
		//			{
		//				new Claim(ClaimTypes.Name, p.Email),
		//				new Claim("FullName", p.Nombre + " " + p.Apellido),
		//				new Claim(ClaimTypes.Role, "Propietario"),
		//			};

		//			var token = new JwtSecurityToken(
		//				issuer: config["TokenAuthentication:Issuer"],
		//				audience: config["TokenAuthentication:Audience"],
		//				claims: claims,
		//				expires: DateTime.Now.AddMinutes(60),
		//				signingCredentials: credenciales
		//			);
		//			return Ok(new JwtSecurityTokenHandler().WriteToken(token));
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		return BadRequest(ex);
		//	}
		//}

		// POST api/<controller>
		[HttpPost]
		public async Task<IActionResult> Post([FromForm] Propietario entidad)
		{
			try
			{
				if (ModelState.IsValid)
				{
					await contexto.Propietario.AddAsync(entidad);
					contexto.SaveChanges();
					return CreatedAtAction(nameof(Get), new { id = entidad.Id }, entidad);
				}
				return BadRequest();
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

		// PUT api/<controller>/5
		//[HttpPut("{id}")]
		//public async Task<IActionResult> Put(int id, [FromForm] Propietario entidad)
		//{
		//	try
		//	{
		//		if (ModelState.IsValid)
		//		{
		//			entidad.Id = id;
		//			Propietario original = await contexto.Propietario.FindAsync(id);
		//			if (String.IsNullOrEmpty(entidad.Clave))
		//			{
		//				entidad.Clave = original.Clave;
		//			}
		//			else
		//			{
		//				entidad.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
		//					password: entidad.Clave,
		//					salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
		//					prf: KeyDerivationPrf.HMACSHA1,
		//					iterationCount: 1000,
		//					numBytesRequested: 256 / 8));
		//			}
		//			contexto.Propietario.Update(entidad);
		//			await contexto.SaveChangesAsync();
		//			return Ok(entidad);
		//		}
		//		return BadRequest();
		//	}
		//	catch (Exception ex)
		//	{
		//		return BadRequest(ex);
		//	}
		//}

		// DELETE api/<controller>/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var p = contexto.Propietario.Find(id);
					if (p == null)
						return NotFound();
					contexto.Propietario.Remove(p);
					contexto.SaveChanges();
					return Ok(p);
				}
				return BadRequest();
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

		// GET: api/Propietarios/test
		[HttpGet("test")]
		[AllowAnonymous]
		public IActionResult Test()
		{
			try
			{
				return Ok("anduvo");
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

		// GET: api/Propietarios/test/5
		[HttpGet("test/{codigo}")]
		[AllowAnonymous]
		public IActionResult Code(int codigo)
		{
			try
			{
				//StatusCodes.Status418ImATeapot //constantes con códigos
				return StatusCode(codigo, new { Mensaje = "Anduvo", Error = false });
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}
	}
}
