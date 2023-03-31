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
		// GET: api/<controller>
		[HttpGet] //obtener todos los propietarios
		public async Task<ActionResult<Propietario>> Get()
		{
			try
			{
				
				var usuario =  User.Identity.Name;
				var prop = await contexto.Propietarios.SingleOrDefaultAsync(x => x.Email == usuario);
				//var propView = new PropietarioView(prop);
				return Ok(prop);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
    }
}