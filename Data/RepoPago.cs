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
        public int Edit(Pago p)
        {
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Pago
                               SET FechaCorrespondiente = @fecha_cor , Monto = @monto , Tipo = @tipo 
                               WHERE Id = @id ;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@fecha_cor", p.FechaCorrespondiente);
                    comm.Parameters.AddWithValue("@monto", p.Monto);
                    comm.Parameters.AddWithValue("@tipo", p.Tipo);
                    comm.Parameters.AddWithValue("@id", p.Id);


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
                string sql = @"UPDATE Pago SET Anulado = 1 WHERE Id = @id ;";

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
                                p.Monto, p.Tipo, p.Anulado FROM pago p;";

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
                            Anulado = reader.GetBoolean(6),
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
