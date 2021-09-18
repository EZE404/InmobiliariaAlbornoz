using InmobiliariaAlbornoz.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlbornoz.Data
{
    public class RepoPago : RepoBase
    {
        public RepoPago(IConfiguration configuration) : base(configuration)
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
        public Pago Details(int id)
        {
            Pago p = new Pago();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT p.Id, p.IdContrato, p.Fecha, p.FechaCorrespondiente, 
                                p.Monto, p.Tipo FROM pago p
                                WHERE p.Id = @id;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@id", id);

                    conn.Open();
                    var reader = comm.ExecuteReader();

                    if (reader.Read())
                    {
                        Contrato c = new Contrato
                        {
                            Id = reader.GetInt32(1)
                        };

                        p.Id = reader.GetInt32(0);
                        p.IdContrato = reader.GetInt32(1);
                        p.Fecha = reader.GetDateTime(2);
                        p.FechaCorrespondiente = reader.GetDateTime(3);
                        p.Monto = reader.GetDecimal(4);
                        p.Tipo = reader.GetString(5);
                        p.Contrato = c;

                    }

                    conn.Close();
                }
            }
            return p;
        }

        public int Put(Pago p)
        {
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Pago (IdContrato, FechaCorrespondiente, Monto, Tipo) 
                            VALUES(@id_contrato, @fecha_cor, @monto, @tipo);
                            SELECT last_insert_id();";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@id_contrato", p.IdContrato);
                    comm.Parameters.AddWithValue("@fecha_cor", p.FechaCorrespondiente);
                    comm.Parameters.AddWithValue("@monto", p.Monto);
                    comm.Parameters.AddWithValue("@tipo", p.Tipo);

                    conn.Open();

                    res = Convert.ToInt32(comm.ExecuteScalar());
                    p.Id = res;

                    conn.Close();
                }
            }
            return res;
        }

        public IList<Pago> All()
        {
            IList<Pago> lista = new List<Pago>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT p.Id, p.IdContrato, p.Fecha, p.FechaCorrespondiente, 
                                p.Monto, p.Tipo FROM pago p;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();

                    while (reader.Read())
                    {
                        Contrato c = new Contrato
                        {
                            Id = reader.GetInt32(1),
                        };

                        Pago p = new Pago
                        {
                            Id = reader.GetInt32(0),
                            IdContrato = reader.GetInt32(1),
                            Fecha = reader.GetDateTime(2),
                            FechaCorrespondiente = reader.GetDateTime(3),
                            Monto = reader.GetDecimal(4),
                            Tipo = reader.GetString(5),
                            Contrato = c
                        };

                        lista.Add(p);
                    }

                    conn.Close();
                }

            }

            return lista;
        }
    }
}
