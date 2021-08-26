namespace InmobiliariaAlbornoz.Data
{
    public abstract class RepoBase
    {
        protected readonly string connectionString;
        protected RepoBase()
        {
            connectionString = "server=localhost;user=root;password=;database=inmobiliaria;SslMode=none";
        }
    }
}