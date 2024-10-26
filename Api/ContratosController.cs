using InmobiliariaAlbornoz.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlbornoz.Api
{
	[Route("api/[controller]/[action]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ApiController]
    public class ContratosController : ControllerBase
	{
		private readonly DataContext contexto;
		private readonly IConfiguration config;
		

		public ContratosController(DataContext contexto, IConfiguration config)
		{
			this.contexto = contexto;
			this.config = config;
		}

        //############# INICIO ENDPOINTS ################

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContratoActualDeInmueble(int id)
        {
            try
            {
                // Obtener el email del propietario logueado
                var emailUsuarioLogueado = User.Identity.Name;
                var hoy = DateTime.Now;

                // Verificar si el inmueble pertenece al propietario logueado
                var inmueble = await contexto.Inmueble
                    .Include(i => i.Propietario)
                    .FirstOrDefaultAsync(i => i.Id == id && i.Propietario.Email == emailUsuarioLogueado);

                if (inmueble == null)
                {
                    return Forbid("No tienes acceso a este inmueble o no existe.");
                }

                // Buscar el contrato activo para el inmueble
                var contrato = await contexto.Contrato
                    .Include(c => c.Inquilino)
                    .Where(c => c.IdInmueble == id && c.Valido && c.Desde <= hoy && c.Hasta >= hoy)
                    .FirstOrDefaultAsync();

                if (contrato == null)
                {
                    return NotFound("No se encontró un contrato activo para este inmueble.");
                }

                // Devolver el contrato activo en la respuesta
                return Ok(contrato);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        //############## FIN ENDPOINTS ###############
    }
}