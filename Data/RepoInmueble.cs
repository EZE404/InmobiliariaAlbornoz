using System;
using System.Collections.Generic;
using InmobiliariaAlbornoz.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace InmobiliariaAlbornoz.Data
{
    public class RepoInmueble : RepoBase
    {
        public RepoInmueble(IConfiguration configuration) : base(configuration)
        {
        }
        public int Edit(Inmueble i, bool isTaken)
        {
            int res = -1;

            string sql = @"UPDATE Inmueble
                            SET Direccion = @direccion , Tipo = @tipo , Uso = @uso , 
                            Ambientes = @ambientes , Precio = @precio , Disponible = @disponible, 
                            IdPropietario = @id_propietario  
                            WHERE Id = @id ;";

            if (isTaken)
            {
                sql = @"UPDATE Inmueble
                            SET Direccion = @direccion , Tipo = @tipo , Uso = @uso , 
                            Ambientes = @ambientes , Precio = @precio , 
                            IdPropietario = @id_propietario  
                            WHERE Id = @id ;";
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@direccion", i.Direccion);
                    comm.Parameters.AddWithValue("@tipo", i.Tipo);
                    comm.Parameters.AddWithValue("@uso", i.Uso);
                    comm.Parameters.AddWithValue("@ambientes", i.Ambientes);
                    comm.Parameters.AddWithValue("@precio", i.Precio);
                    comm.Parameters.AddWithValue("@id_propietario",i.IdPropietario);
                    comm.Parameters.AddWithValue("@id", i.Id);
                    if (!isTaken)
                    {
                        comm.Parameters.AddWithValue("@disponible", i.Disponible);
                    }

                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteNonQuery());
                    conn.Close();
                }
            }
            return res;
        }

        public int Delete(int id)
        {
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {

                string consulta = @"SELECT DISTINCT * FROM Contrato WHERE IdInmueble = @id";

                using (MySqlCommand comm = new MySqlCommand(consulta, conn))
                {
                    comm.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    if (reader.HasRows)
                    {
                        conn.Close();
                        return res;
                    }
                    conn.Close();
                }

                string sql = @"DELETE FROM Inmueble WHERE Id = @id ;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteNonQuery());
                    conn.Close();
                }
            }
            return res;
        }

        public Inmueble Details(int id)
        {
            Inmueble i = new Inmueble();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT Id, Direccion, Tipo, Uso, Ambientes, Precio, Disponible, IdPropietario 
                                FROM Propietario 
                                WHERE Id = @id ;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@id", id);

                    conn.Open();
                    var reader = comm.ExecuteReader();

                    if (reader.Read())
                    {
                        i.Id = reader.GetInt32(0);
                        i.Direccion = reader.GetString(1);
                        i.Tipo = reader.GetInt32(2);
                        i.Uso = reader.GetInt32(3);
                        i.Ambientes = reader.GetInt32(4);
                        i.Precio = reader.GetDecimal(5);
                        i.Disponible = reader.GetBoolean(6);
                        i.IdPropietario = reader.GetInt32(7);
                    }

                    conn.Close();

                }
            }
            return i;
        }

        public bool CheckAvailability(int idInmueble)
        {
            bool res = false;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT i.Id
                                FROM Inmueble i
                                WHERE i.Disponible = 1
                                AND i.id NOT IN (
	                                SELECT DISTINCT c.IdInmueble
	                                FROM Contrato c
	                                WHERE c.Valido = 1
	                                AND current_date() BETWEEN c.Desde AND c.Hasta
                                )
                                AND i.Id = @id ;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@id", idInmueble);
                    conn.Open();
                    var reader = comm.ExecuteReader();

                    if (reader.Read())
                    {
                        res = true;
                    }
                    conn.Close();
                }
            }
            return res;
        }

        public int Put(Inmueble i)
        {
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Inmueble (Direccion, Tipo, Uso, Ambientes, Precio, Disponible, IdPropietario) 
                            VALUES(@direccion, @tipo, @uso, @ambientes, @precio, @disponible, @id_propietario);
                            SELECT last_insert_id();";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@direccion", i.Direccion);
                    comm.Parameters.AddWithValue("@tipo", i.Tipo);
                    comm.Parameters.AddWithValue("@uso", i.Uso);
                    comm.Parameters.AddWithValue("@ambientes", i.Ambientes);
                    comm.Parameters.AddWithValue("@precio", i.Precio);
                    comm.Parameters.AddWithValue("@disponible", i.Disponible);
                    comm.Parameters.AddWithValue("@id_propietario", i.IdPropietario);

                    conn.Open();

                    res = Convert.ToInt32(comm.ExecuteScalar());
                    i.Id = res;

                    conn.Close();
                }
            }
            return res;
        }

        public IList<Inmueble> All()
        {
            IList<Inmueble> list = new List<Inmueble>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT i.Id, i.Direccion, i.Tipo, i.Uso, i.Ambientes, i.Precio, i.Disponible, 
                                i.IdPropietario, p.Nombre, p.Dni, p.Email 
                                FROM Inmueble i INNER JOIN Propietario p
                                ON i.IdPropietario = p.Id;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        var i = new Inmueble {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Tipo = reader.GetInt32(2),
                            Uso = reader.GetInt32(3),
                            Ambientes = reader.GetInt32(4),
                            Precio = reader.GetDecimal(5),
                            Disponible = reader.GetBoolean(6),
                            IdPropietario = reader.GetInt32(7),
                        };

                        var p = new Propietario
                        {
                            Id = reader.GetInt32(7),
                            Nombre = reader.GetString(8),
                            Dni = reader.GetString(9),
                            Email = reader.GetString(10),
                        };

                        i.Propietario = p;

                        list.Add(i);
                    }
                    conn.Close();
                }
            }
            return list;
        }

        public IList<Inmueble> AllByInquilino(int id)
        {
            IList<Inmueble> list = new List<Inmueble>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT i.Id, i.Direccion, i.Tipo, i.Uso, i.Ambientes, i.Precio, i.Disponible, 
                                i.IdPropietario, p.Nombre, p.Dni, p.Email 
                                FROM Inmueble i INNER JOIN Propietario p
                                ON i.IdPropietario = p.Id 
                                WHERE i.IdPropietario = @id ;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        var i = new Inmueble
                        {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Tipo = reader.GetInt32(2),
                            Uso = reader.GetInt32(3),
                            Ambientes = reader.GetInt32(4),
                            Precio = reader.GetDecimal(5),
                            Disponible = reader.GetBoolean(6),
                            IdPropietario = reader.GetInt32(7),
                        };

                        var p = new Propietario
                        {
                            Id = reader.GetInt32(7),
                            Nombre = reader.GetString(8),
                            Dni = reader.GetString(9),
                            Email = reader.GetString(10),
                        };

                        i.Propietario = p;

                        list.Add(i);
                    }
                    conn.Close();
                }
            }
            return list;
        }

        public IList<Inmueble> AllValid()
        {
            IList<Inmueble> list = new List<Inmueble>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT i.Id, i.Direccion, i.Tipo, i.Uso, i.Ambientes, i.Precio,
				               i.Disponible, i.IdPropietario, p.Nombre, p.Dni, p.Email 
                               FROM Inmueble i
                               INNER JOIN Propietario p ON i.IdPropietario = p.Id
                               WHERE i.Disponible = 1
                               AND i.id NOT IN (
	                                SELECT DISTINCT c.IdInmueble
	                                FROM Contrato c
	                                WHERE c.Valido = 1
	                                AND current_date() BETWEEN c.Desde AND c.Hasta
                               );";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        var i = new Inmueble
                        {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Tipo = reader.GetInt32(2),
                            Uso = reader.GetInt32(3),
                            Ambientes = reader.GetInt32(4),
                            Precio = reader.GetDecimal(5),
                            Disponible = reader.GetBoolean(6),
                            IdPropietario = reader.GetInt32(7),
                        };

                        var p = new Propietario
                        {
                            Id = reader.GetInt32(7),
                            Nombre = reader.GetString(8),
                            Dni = reader.GetString(9),
                            Email = reader.GetString(10),
                        };

                        i.Propietario = p;

                        list.Add(i);
                    }
                    conn.Close();
                }
            }
            return list;
        }

        public bool isTaken(int id)
        {
            bool res = false;

            string sql = @"SELECT c.Id FROM contrato c WHERE c.IdInmueble = @id 
                        AND c.Valido = 1 AND current_date() BETWEEN c.Desde AND c.Hasta;";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    res = (reader.HasRows) ? true : false;
                    conn.Close();
                }
            }

            return res;
        }

        public IList<Inmueble> InmueblesAvailableByDates(DateTime desde, DateTime hasta)
        {
            IList<Inmueble> inmuebles = new List<Inmueble>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT i2.Id, i2.Direccion, i2.Tipo, i2.Uso,
                                i2.Ambientes, i2.Precio, i2.Disponible,
                                i2.IdPropietario, p.Nombre
                                FROM propietario p INNER JOIN (
                                SELECT DISTINCT i.* FROM inmueble i INNER JOIN contrato c ON (i.Id = c.IdInmueble)
                                AND @desde NOT BETWEEN c.Desde AND c.Hasta 
                                AND @hasta NOT BETWEEN c.Desde AND c.Hasta 
                                AND i.Disponible = 1
                                UNION
                                SELECT i.* FROM inmueble i WHERE i.Id NOT IN (
	                                SELECT DISTINCT c.IdInmueble FROM contrato c 	
                                )
                                AND i.Disponible = 1) i2
                                ON (i2.IdPropietario = p.Id);";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@desde", desde);
                    comm.Parameters.AddWithValue("@hasta", hasta);

                    conn.Open();
                    var reader = comm.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        var i = new Inmueble
                        {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Tipo = reader.GetInt32(2),
                            Uso = reader.GetInt32(3),
                            Ambientes = reader.GetInt32(4),
                            Precio = reader.GetDecimal(5),
                            Disponible = reader.GetBoolean(6),
                            IdPropietario = reader.GetInt32(7),
                        };

                        var p = new Propietario
                        {
                            Id = reader.GetInt32(7),
                            Nombre = reader.GetString(8)
                        };

                        i.Propietario = p;

                        inmuebles.Add(i);
                    }
                    conn.Close();
                }
            }
            return inmuebles;
        }

        public Inmueble ById(int id) {

            Inmueble i = new Inmueble();
            Propietario p = new Propietario();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT i.Direccion, i.Tipo, i.Uso, i.Ambientes, i.Precio, i.Disponible, 
                                i.IdPropietario, p.Nombre, p.Dni, p.Email 
                                FROM Inmueble i INNER JOIN Propietario p
                                ON i.IdPropietario = p.Id
                                WHERE i.Id = @id ;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@id", id);
                    
                    conn.Open();
                    
                    var reader = comm.ExecuteReader();
                    
                    if (reader.Read()) {

                        i.Id = id;
                        i.Direccion = reader.GetString(0);
                        i.Tipo = reader.GetInt32(1);
                        i.Uso = reader.GetInt32(2);
                        i.Ambientes = reader.GetInt32(3);
                        i.Precio = reader.GetDecimal(4);
                        i.Disponible = (bool)reader.GetBoolean(5);
                        i.IdPropietario = reader.GetInt32(6);
                        p.Id = reader.GetInt32(6);
                        p.Nombre = reader.GetString(7);
                        p.Dni = reader.GetString(8);
                        p.Email = reader.GetString(9);
                        i.Propietario = p;
                    }
                    
                    conn.Close();
                }
            }

            return i;
        }
    }
}