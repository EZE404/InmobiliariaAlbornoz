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
        public int Edit(Inmueble i)
        {
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Inmueble
                            SET Direccion = @direccion , Tipo = @tipo , Uso = @uso , 
                            Ambientes = @ambientes , Precio = @precio , Disponible = @disponible, 
                            IdPropietario = @id_propietario  
                            WHERE Id = @id ;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@direccion", i.Direccion);
                    comm.Parameters.AddWithValue("@tipo", i.Tipo);
                    comm.Parameters.AddWithValue("@uso", i.Uso);
                    comm.Parameters.AddWithValue("@ambientes", i.Ambientes);
                    comm.Parameters.AddWithValue("@precio", i.Precio);
                    comm.Parameters.AddWithValue("@disponible", i.Disponible);
                    comm.Parameters.AddWithValue("@id_propietario",i.IdPropietario);
                    comm.Parameters.AddWithValue("@id", i.Id);

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
                        i.Tipo = reader.GetString(2);
                        i.Uso = reader.GetString(3);
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
                            Tipo = reader.GetString(2),
                            Uso = reader.GetString(3),
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
                        i.Tipo = reader.GetString(1);
                        i.Uso = reader.GetString(2);
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