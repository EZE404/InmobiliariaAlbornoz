using InmobiliariaAlbornoz.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InmobiliariaAlbornoz.Api
{
	[Route("api/[controller]/[action]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ApiController]
    public class PagosController : ControllerBase
	{
		private readonly DataContext contexto;
		private readonly IConfiguration config;
		

		public PagosController(DataContext contexto, IConfiguration config)
		{
			this.contexto = contexto;
			this.config = config;
		}

        //############# INICIO ENDPOINTS ################

        [HttpGet("{id}")]
        public async Task<IActionResult> getPagosDeContrato(int id)
        {
            try
            {
                // Obtener el email del usuario logueado
                var emailUsuarioLogueado = User.Identity.Name;

                // Obtener los pagos del contrato verificando que el propietario del inmueble sea el usuario logueado
                var pagos = await contexto.Pago
                    .Where(p => p.Contrato.Inmueble.Propietario.Email == emailUsuarioLogueado && p.Contrato.Id == id)
                    .Select(p => new 
                    {
                        p.Id,
                        p.Monto,
                        p.Fecha
                    })
                    .ToListAsync();

                // Devolver la lista de pagos en formato JSON
                return Ok(pagos);
            }
            catch (Exception ex)
            {
                // En caso de error, devolver un código 500 y el mensaje de error
                return StatusCode(500, new { mensaje = "Ocurrió un error al obtener los pagos", error = ex.Message });
            }
        }


        //############## FIN ENDPOINTS ###############
    }
}