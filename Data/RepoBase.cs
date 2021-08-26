using Microsoft.Extensions.Configuration;

namespace InmobiliariaAlbornoz.Data
{
    public abstract class RepoBase
    {
        protected readonly string connectionString;
        protected RepoBase(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:databaseMySql"];
            //connectionString = "server=localhost;user=root;password=;database=inmobiliaria;SslMode=none";
        }
    }
}