using InmobiliariaAlbornoz.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlbornoz.Data
{
    public class RepoUsuario : RepoBase
    {
        public RepoUsuario(IConfiguration configuration) : base(configuration)
        {

        }

		public int Alta(Usuario e)
		{
			int res = -1;

			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Usuario (Nombre, Apellido, AvatarUrl, Email, Clave, Rol) " +
					$"VALUES (@nombre, @apellido, @avatar, @email, @clave, @rol);" +
					"SELECT last_insert_id();";//devuelve el id insertado (LAST_INSERT_ID para mysql)

				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@nombre", e.Nombre);
					command.Parameters.AddWithValue("@apellido", e.Apellido);
					if (String.IsNullOrEmpty(e.Avatar))
						command.Parameters.AddWithValue("@avatar", DBNull.Value);
					else
						command.Parameters.AddWithValue("@avatar", e.Avatar);
					command.Parameters.AddWithValue("@email", e.Email);
					command.Parameters.AddWithValue("@clave", e.Clave);
					command.Parameters.AddWithValue("@rol", e.Rol);
					
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					e.Id = res;
					connection.Close();
				}
			}
			return res;
		}
		public int Baja(int id)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"DELETE FROM Usuario WHERE Id = @id";

				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{

					command.Parameters.AddWithValue("@id", id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public int Modificacion(Usuario e)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"UPDATE Usuario SET Nombre=@nombre, Apellido=@apellido, AvatarUrl=@avatar, Email=@email, Clave=@clave, Rol=@rol " +
					$"WHERE Id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@nombre", e.Nombre);
					command.Parameters.AddWithValue("@apellido", e.Apellido);
					command.Parameters.AddWithValue("@avatar", e.Avatar);
					command.Parameters.AddWithValue("@email", e.Email);
					command.Parameters.AddWithValue("@clave", e.Clave);
					command.Parameters.AddWithValue("@rol", e.Rol);
					command.Parameters.AddWithValue("@id", e.Id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Usuario> ObtenerTodos()
		{
			IList<Usuario> res = new List<Usuario>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, AvatarUrl, Email, Clave, Rol" +
					$" FROM Usuario";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					connection.Open();
					var reader = command.ExecuteReader();

					while (reader.Read())
					{
						Usuario e = new Usuario
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Avatar = reader["AvatarUrl"].ToString(),
							Email = reader.GetString(4),
							Clave = reader.GetString(5),
							Rol = reader.GetInt32(6),
						};
						res.Add(e);
					}

					connection.Close();
				}
			}
			return res;
		}

		public Usuario ObtenerPorId(int id)
		{
			Usuario e = null;

			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, AvatarUrl, Email, Clave, Rol FROM Usuario" +
					$" WHERE Id=@id";

				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@id", id);
					connection.Open();

					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						e = new Usuario
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Avatar = reader["AvatarUrl"].ToString(),
							Email = reader.GetString(4),
							Clave = reader.GetString(5),
							Rol = reader.GetInt32(6),
						};
					}
					connection.Close();
				}
			}
			return e;
		}

		public Usuario ObtenerPorEmail(string email)
		{
			Usuario e = null;

			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, AvatarUrl, Email, Clave, Rol FROM Usuario" +
					$" WHERE Email=@email";

				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@email", email);
					connection.Open();

					var reader = command.ExecuteReader();
					
					if (reader.Read())
					{
						e = new Usuario
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Avatar = reader["AvatarUrl"].ToString(),
							Email = reader.GetString(4),
							Clave = reader.GetString(5),
							Rol = reader.GetInt32(6),
						};
					}
					connection.Close();
				}
			}
			return e;
		}

        public int Update(Usuario u, bool editRol, bool editAvatar)
        {
			int res = -1;
			string sql;
			int flag = 0;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
				if (!editRol && !editAvatar)
				{
					sql = @"UPDATE Usuario SET Nombre = @nombre,
						Apellido = @apellido, Email = @email 
						WHERE Id = @id ;";
				}
				else if (editRol && !editAvatar)
				{
					sql = @"UPDATE Usuario SET Nombre = @nombre, 
						Apellido = @apellido, Email = @email, 
						Rol = @rol WHERE Id = @id ;";
					flag = 1;
				}
				else if (!editRol && editAvatar)
				{
					sql = @"UPDATE Usuario SET Nombre = @nombre,
						Apellido = @apellido, Email = @email, 
						AvatarUrl = @avatar WHERE Id = @id ;";
					flag = 2;
				}
				else
				{
					sql = @"UPDATE Usuario SET Nombre = @nombre, 
						Apellido = @apellido, Email = @email, 
						Rol = @rol, AvatarUrl = @avatar 
						WHERE Id = @id ;";
					flag = 3;
				}

				using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    switch (flag)
                    {
						case 0:
                            {
								comm.Parameters.AddWithValue("@id", u.Id);
								comm.Parameters.AddWithValue("@nombre", u.Nombre);
								comm.Parameters.AddWithValue("@apellido", u.Apellido);
								comm.Parameters.AddWithValue("@email", u.Email);
							}
							break;

						case 1:
							{
								comm.Parameters.AddWithValue("@id", u.Id);
								comm.Parameters.AddWithValue("@nombre", u.Nombre);
								comm.Parameters.AddWithValue("@apellido", u.Apellido);
								comm.Parameters.AddWithValue("@email", u.Email);
								comm.Parameters.AddWithValue("@rol", u.Rol);
							}
							break;

						case 2:
							{
								comm.Parameters.AddWithValue("@id", u.Id);
								comm.Parameters.AddWithValue("@nombre", u.Nombre);
								comm.Parameters.AddWithValue("@apellido", u.Apellido);
								comm.Parameters.AddWithValue("@email", u.Email);
								comm.Parameters.AddWithValue("@avatar", u.Avatar);
							}
							break;

						case 3:
							{
								comm.Parameters.AddWithValue("@id", u.Id);
								comm.Parameters.AddWithValue("@nombre", u.Nombre);
								comm.Parameters.AddWithValue("@apellido", u.Apellido);
								comm.Parameters.AddWithValue("@email", u.Email);
								comm.Parameters.AddWithValue("@rol", u.Rol);
								comm.Parameters.AddWithValue("@avatar", u.Avatar);
							}
							break;
                    }

					conn.Open();
					res = comm.ExecuteNonQuery();
					conn.Close();
                }
			}
			return res;
        }

        public int UpdatePass(int id, string newPassHashed)
        {
			int res = -1;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
				string sql = @"UPDATE Usuario SET Clave = @clave WHERE Id = @id";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
					comm.Parameters.AddWithValue("@id", id);
					comm.Parameters.AddWithValue("@clave", newPassHashed);

					conn.Open();
					res = comm.ExecuteNonQuery();
					conn.Close();
                }
            }
			return res;
        }
    }
}
