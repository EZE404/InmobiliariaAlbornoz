using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InmobiliariaAlbornoz.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InmobiliariaAlbornoz.Api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InquilinosController : ControllerBase
    {
        private readonly DataContext contexto;
		private readonly IConfiguration config;

        public InquilinosController(DataContext contexto, IConfiguration config)
		{
			this.contexto = contexto;
			this.config = config;
		}

        // ########## ARRANCAN LOS ENDPOINTS ##########

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInquilinoDeInmueble(int id)
        {
            try
            {
                // Obtener el email del usuario logueado
                var emailUsuarioLogueado = User.Identity.Name;

                // Buscar el inmueble y el propietario asociado, verificando que el email del propietario coincide
                var inmuebleConPropietario = await contexto.Inmueble
                    .Include(i => i.Propietario) // Incluir el propietario del inmueble
                    .Where(i => i.Id == id && i.Propietario.Email == emailUsuarioLogueado)
                    .FirstOrDefaultAsync();

                // Verificar si el inmueble con el propietario se encontró y si pertenece al usuario logueado
                if (inmuebleConPropietario == null)
                {
                    return Unauthorized("No tienes permiso para acceder a este inmueble.");
                }

                var fechaActual = DateTime.Now;

                // Buscar el contrato válido cuyo inmueble coincida con el id proporcionado
                var contratoActual = await contexto.Contrato
                    .Include(c => c.Inquilino) // Incluir los datos del inquilino relacionado
                    .Where(c => c.IdInmueble == id && 
                                c.Desde <= fechaActual && 
                                c.Hasta >= fechaActual && 
                                c.Valido == true)
                    .FirstOrDefaultAsync();

                // Verificar si se encontró un contrato válido
                if (contratoActual == null)
                {
                    return NotFound("No se encontró un contrato actual para este inmueble.");
                }

                // Obtener el inquilino del contrato encontrado
                var inquilinoActual = contratoActual.Inquilino;

                // Retornar los datos del inquilino
                return Ok(inquilinoActual);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // ######### TERMINAN LOS ENDPOINTS ########
    }
}