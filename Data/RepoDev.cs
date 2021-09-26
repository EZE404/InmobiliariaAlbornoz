using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;

namespace InmobiliariaAlbornoz.Data
{
    public class RepoDev : RepoBase
    {
        public RepoDev(IConfiguration configuration) : base(configuration)
        {

        }

        public int SaveException(string controller, string action, string message, string user)
        {
            int res = -1;

            string sql = @"INSERT INTO error_log (controller, action, message, user)
                        VALUES(@c, @a, @m, @u);
                        SELECT last_insert_id();";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand comm = new MySqlCommand(sql, conn))
                    {
                        comm.Parameters.AddWithValue("@c", controller);
                        comm.Parameters.AddWithValue("@a", action);
                        comm.Parameters.AddWithValue("@m", message);
                        comm.Parameters.AddWithValue("@u", user);

                        conn.Open();
                        res = Convert.ToInt32(comm.ExecuteScalar());
                        conn.Close();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return res;
        }
    }


}
