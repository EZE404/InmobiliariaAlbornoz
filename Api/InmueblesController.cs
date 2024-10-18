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
using System.Linq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
namespace InmobiliariaAlbornoz.Api
{
	[Route("api/[controller]/[action]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ApiController]
    public class InmueblesController : ControllerBase
	{
		private readonly DataContext contexto;
		private readonly IConfiguration config;
		

		public InmueblesController(DataContext contexto, IConfiguration config)
		{
			this.contexto = contexto;
			this.config = config;
			
		}

        [HttpGet]
        public async Task<IActionResult> GetInmueblesDePropietario()
        {
            try
            {
                // Obtener el email del propietario desde las claims
                var emailPropietario = User.Identity.Name;

                // Buscar al propietario con ese email
                var propietario = await contexto.Propietario
                    .FirstOrDefaultAsync(p => p.Email == emailPropietario);

                // Si el propietario no existe, devolver un error
                if (propietario == null)
                {
                    return NotFound("Propietario no encontrado.");
                }

                // Obtener los inmuebles asociados al propietario
                var inmuebles = await contexto.Inmueble
                    .Where(i => i.IdPropietario == propietario.Id)
                    .Select(i => new
                        {
                            i.Id,
                            i.Direccion,
                            i.Tipo,
                            i.Uso,
                            i.Ambientes,
                            i.Precio,
                            i.Disponible,
                            i.ImageUrl
                        })
                    .ToListAsync();

                // Devolver la lista de inmuebles
                return Ok(inmuebles);
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción
                return BadRequest(ex.Message);
            }
        }
    }
}