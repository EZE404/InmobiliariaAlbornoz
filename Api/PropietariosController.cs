using InmobiliariaAlbornoz.Data;
using InmobiliariaAlbornoz.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace InmobiliariaAlbornoz.Api
{
	[Route("api/[controller]")]
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
        // GET api/<controller>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id) => Ok(contexto.Usuarios.Find(id));
	}
}