using InmobiliariaAlbornoz.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlbornoz.Data
{
    public class RepoContrato : RepoBase
    {
        public RepoContrato(IConfiguration config) : base(config)
        {

        }
        public int Edit(Contrato c)
        {
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Contrato
                            SET IdInmueble = @id_inmueble , IdInquilino = @id_inquilino , Desde = @desde , 
                            Hasta = @hasta , DniGarante = @dni_garante , NombreGarante = @nombre_garante, 
                            TelefonoGarante = @tel_garante, EmailGarante = @email_garante 
                            WHERE Id = @id ;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@id_inmueble", c.IdInmueble);
                    comm.Parameters.AddWithValue("@id_inquilino", c.IdInquilino);
                    comm.Parameters.AddWithValue("@desde", c.Desde);
                    comm.Parameters.AddWithValue("@hasta", c.Hasta);
                    comm.Parameters.AddWithValue("@dni_garante", c.DniGarante);
                    comm.Parameters.AddWithValue("@nombre_garante", c.NombreGarante);
                    comm.Parameters.AddWithValue("@tel_garante", c.TelefonoGarante);
                    comm.Parameters.AddWithValue("@email_garante", c.EmailGarante);
                    comm.Parameters.AddWithValue("@id", c.Id);


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
                string sql = @"DELETE FROM Contrato WHERE Id = @id ;";

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
        public Contrato Details(int id)
        {
            Contrato c = new Contrato();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT c.Id, c.IdInmueble, c.IdInquilino, c.Desde, c.Hasta, 
                                c.DniGarante, c.NombreGarante, c.TelefonoGarante, c.EmailGarante, 
                                i.IdPropietario, p.Nombre, 
                                i2.Nombre
                                FROM Contrato c INNER JOIN Inmueble i ON c.IdInmueble = i.Id 
                                INNER JOIN Propietario p ON i.IdPropietario = p.Id 
                                INNER JOIN Inquilino i2 ON c.IdInquilino = i2.Id 
                                WHERE c.Id = @id;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@id", id);

                    conn.Open();
                    var reader = comm.ExecuteReader();

                    if (reader.Read())
                    {
                        Propietario p = new Propietario
                        {
                            Id = reader.GetInt32(9),
                            Nombre = reader.GetString(10),
                        };

                        Inmueble i = new Inmueble
                        {
                            Id = reader.GetInt32(1),
                            Propietario = p,
                        };

                        Inquilino i2 = new Inquilino
                        {
                            Id = reader.GetInt32(2),
                            Nombre = reader.GetString(11)
                        };


                        c.Id = reader.GetInt32(0);
                        c.IdInmueble = reader.GetInt32(1);
                        c.IdInquilino = reader.GetInt32(2);
                        c.Desde = reader.GetDateTime(3);
                        c.Hasta = reader.GetDateTime(4);
                        c.DniGarante = reader.GetString(5);
                        c.NombreGarante = reader.GetString(6);
                        c.TelefonoGarante = reader.GetString(7);
                        c.EmailGarante = reader.GetString(8);
                        c.Inmueble = i;
                        c.Inquilino = i2;

                    }

                    conn.Close();
                }
            }
            return c;
        }

        public int Put(Contrato c)
        {
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Contrato (IdInmueble, IdInquilino, Desde, Hasta, 
                            DniGarante, NombreGarante, TelefonoGarante, EmailGarante) 
                            VALUES(@id_inmueble, @id_inquilino, @desde, @hasta, 
                            @dni_garante, @nombre_garante, @tel_garante, @email_garante);
                            SELECT last_insert_id();";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@id_inmueble", c.IdInmueble);
                    comm.Parameters.AddWithValue("@id_inquilino", c.IdInquilino);
                    comm.Parameters.AddWithValue("@desde", c.Desde);
                    comm.Parameters.AddWithValue("@hasta", c.Hasta);
                    comm.Parameters.AddWithValue("@dni_garante", c.DniGarante);
                    comm.Parameters.AddWithValue("@nombre_garante", c.NombreGarante);
                    comm.Parameters.AddWithValue("@tel_garante", c.TelefonoGarante);
                    comm.Parameters.AddWithValue("@email_garante", c.EmailGarante);

                    conn.Open();

                    res = Convert.ToInt32(comm.ExecuteScalar());
                    c.Id = res;

                    conn.Close();
                }
            }
            return res;
        }

        public IList<Contrato> All()
        {
            IList<Contrato> lista = new List<Contrato>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT c.Id, c.IdInmueble, c.IdInquilino, c.Desde, c.Hasta, c.NombreGarante, 
                                i.IdPropietario, p.Nombre, 
                                i2.Nombre
                                FROM Contrato c INNER JOIN Inmueble i ON c.IdInmueble = i.Id 
                                INNER JOIN Propietario p ON i.IdPropietario = p.Id 
                                INNER JOIN Inquilino i2 ON c.IdInquilino = i2.Id";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();

                    while (reader.Read())
                    {
                        Propietario p = new Propietario
                        {
                            Id = reader.GetInt32(6),
                            Nombre = reader.GetString(7),
                        };

                        Inmueble i = new Inmueble
                        {
                            Id = reader.GetInt32(1),
                            Propietario = p,
                        };

                        Inquilino i2 = new Inquilino
                        {
                            Id = reader.GetInt32(2),
                            Nombre = reader.GetString(8)
                        };

                        Contrato c = new Contrato
                        {
                            Id = reader.GetInt32(0),
                            IdInmueble = reader.GetInt32(1),
                            IdInquilino = reader.GetInt32(2),
                            Desde = reader.GetDateTime(3),
                            Hasta = reader.GetDateTime(4),
                            NombreGarante = reader.GetString(5),
                            Inmueble = i,
                            Inquilino = i2,
                        };

                        lista.Add(c);
                    }

                    conn.Close();
                }

            }

            return lista;
        }
        public IList<Contrato> AllByInquilino(int id)
        {
            IList<Contrato> lista = new List<Contrato>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT c.Id, c.IdInmueble, c.IdInquilino, c.Desde, c.Hasta, c.NombreGarante, 
                                i.IdPropietario, i.Direccion, i.Precio, p.Nombre, 
                                i2.Nombre
                                FROM Contrato c INNER JOIN Inmueble i ON c.IdInmueble = i.Id 
                                INNER JOIN Propietario p ON i.IdPropietario = p.Id 
                                INNER JOIN Inquilino i2 ON c.IdInquilino = i2.Id WHERE i2.Id = @id ";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    var reader = comm.ExecuteReader();

                    while (reader.Read())
                    {
                        Propietario p = new Propietario
                        {
                            Id = reader.GetInt32(6),
                            Nombre = reader.GetString(9),
                        };

                        Inmueble i = new Inmueble
                        {
                            Id = reader.GetInt32(1),
                            Direccion = reader.GetString(7),
                            Precio = reader.GetDecimal(8),
                            Propietario = p,
                        };

                        Inquilino i2 = new Inquilino
                        {
                            Id = reader.GetInt32(2),
                            Nombre = reader.GetString(10)
                        };

                        Contrato c = new Contrato
                        {
                            Id = reader.GetInt32(0),
                            IdInmueble = reader.GetInt32(1),
                            IdInquilino = reader.GetInt32(2),
                            Desde = reader.GetDateTime(3),
                            Hasta = reader.GetDateTime(4),
                            NombreGarante = reader.GetString(5),
                            Inmueble = i,
                            Inquilino = i2,
                        };

                        lista.Add(c);
                    }

                    conn.Close();
                }

            }

            return lista;
        }
    }
}
