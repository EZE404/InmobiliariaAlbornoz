using InmobiliariaAlbornoz.Data;
using InmobiliariaAlbornoz.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
namespace InmobiliariaAlbornoz.Api
{
	[Route("api/[controller]/[action]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ApiController]
    public class InmueblesController : ControllerBase
	{
        private readonly DataContext contexto;
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment environment;

        public InmueblesController(DataContext contexto, IConfiguration config, IWebHostEnvironment environment)
        {
            this.contexto = contexto;
            this.config = config;
            this.environment = environment;
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
                            i.TipoNombre,
                            i.UsoNombre,
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

        [HttpPut]
        public async Task<IActionResult> ActualizarDisponibilidadInmueble([FromBody] Inmueble inmueble)
        {
            try
            {
                var emailUsuarioLogueado = User.Identity.Name;
                var propietario = await contexto.Propietario.FirstOrDefaultAsync(p => p.Email == emailUsuarioLogueado);
                // Si el propietario no existe, devolver un error
                if (propietario == null)
                {
                    return NotFound("Propietario no encontrado.");
                }

                var inmuebleExistente = await contexto.Inmueble.SingleOrDefaultAsync(i => i.Id == inmueble.Id);
                if (inmuebleExistente == null)
                {
                    return NotFound("Inmueble no encontrado.");
                }
                if (inmuebleExistente.IdPropietario != propietario.Id)
                {
                    return BadRequest("No tienes permiso para editar este Inmueble");
                }

                inmuebleExistente.Disponible = inmueble.Disponible;
                contexto.Inmueble.Update(inmuebleExistente);
                await contexto.SaveChangesAsync();

				return Ok("Disponibilidad de Inmueble actualizada.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDePropietarioSusInmueblesConContratos()
        {
            try
            {
                var emailUsuarioLogueado = User.Identity.Name;
                var hoy = DateTime.Now;
                var inmueblesConContratos = await contexto.Contrato
                    .Include(c => c.Inmueble)
                    .ThenInclude(i => i.Propietario)
                    .Where(c => c.Desde <= hoy && c.Hasta >= hoy && c.Inmueble.Propietario.Email == emailUsuarioLogueado)
                    .Select(c => new 
                    {
                        c.Inmueble.Id,
                        c.Inmueble.Direccion,
                        c.Inmueble.TipoNombre,  // Ya calculado en la entidad
                        c.Inmueble.UsoNombre,   // Ya calculado en la entidad
                        c.Inmueble.Ambientes,
                        c.Inmueble.Precio,
                        c.Inmueble.Disponible,
                        c.Inmueble.ImageUrl
                    })
                    .Distinct()  // Evitamos duplicados
                    .ToListAsync();

                if (inmueblesConContratos == null || !inmueblesConContratos.Any())
                {
                    return NotFound("No se encontraron inmuebles con contratos para este propietario.");
                }

                return Ok(inmueblesConContratos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // #### ALTA DE INMUEBLE CON FOTO ####
        [HttpPost]
        public async Task<IActionResult> CrearInmueble([FromForm] InmuebleAltaDto inmuebleDTO, [FromForm] IFormFile ImageFile)
        {
            try
            {
                if (!ModelState.IsValid) 
                {
                    return BadRequest(ModelState);
                }
                // Obtener el email del propietario logueado
                var emailPropietario = User.Identity.Name;

                // Buscar el propietario por email
                var propietario = await contexto.Propietario.FirstOrDefaultAsync(p => p.Email == emailPropietario);
                if (propietario == null)
                {
                    return Unauthorized("No se encontró el propietario logueado.");
                }

                // Convertir TipoNombre y UsoNombre a sus valores de enumerados
                if (!Enum.TryParse(inmuebleDTO.TipoNombre, out enTipos tipoEnum))
                {
                    return BadRequest("Tipo de inmueble inválido.");
                }

                if (!Enum.TryParse(inmuebleDTO.UsoNombre, out enUsos usoEnum))
                {
                    return BadRequest("Uso de inmueble inválido.");
                }

                // Crear un nuevo objeto Inmueble
                var nuevoInmueble = new Inmueble
                {
                    Direccion = inmuebleDTO.Direccion,
                    Tipo = (int)tipoEnum,
                    Uso = (int)usoEnum,
                    Ambientes = inmuebleDTO.Ambientes,
                    Precio = inmuebleDTO.Precio,
                    Disponible = false,
                    IdPropietario = propietario.Id // Asignar el propietario logueado
                };

                // Guardar el inmueble en la base de datos para obtener el ID
                contexto.Inmueble.Add(nuevoInmueble);
                await contexto.SaveChangesAsync();

                // Si se subió un archivo de imagen, procesarlo
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Obtener la ruta de almacenamiento
                    string wwwPath = environment.WebRootPath; // ruta raíz del servidor
                    string pathUploads = Path.Combine(wwwPath, "Uploads");

                    // Crear la carpeta "Uploads" si no existe
                    if (!Directory.Exists(pathUploads))
                    {
                        Directory.CreateDirectory(pathUploads);
                    }

                    // Construir el nombre del archivo de imagen
                    string extension = Path.GetExtension(ImageFile.FileName);
                    string fileName = "inmueble_" + nuevoInmueble.Id + extension;
                    string fullPath = Path.Combine(pathUploads, fileName);

                    // Guardar el archivo en el servidor
                    using (FileStream stream = new(fullPath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    // Asignar la ruta relativa al ImageUrl del inmueble
                    nuevoInmueble.ImageUrl = Path.Combine("Uploads", fileName);

                    // Guardar nuevamente el inmueble con la ruta de la imagen
                    contexto.Inmueble.Update(nuevoInmueble);
                    await contexto.SaveChangesAsync();
                }

                return Ok(nuevoInmueble);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}