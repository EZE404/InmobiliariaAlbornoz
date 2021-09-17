using System;
using System.Collections.Generic;
using InmobiliariaAlbornoz.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace InmobiliariaAlbornoz.Data
{
    public class RepoInquilino : RepoBase
    {
        //string connectionString = "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=C:\\Users\\Ezequiel\\OneDrive\\ULP\\4to Cuatrimestre\\Programación .NET\\segunda_clase\\WebApplication1\\Data\\WebApp1.mdf";
        //string connectionString = "server=localhost;user=root;password=;database=inmobiliaria;SslMode=none";

        public RepoInquilino(IConfiguration configuration) : base(configuration)
        {

        }

        public int Edit(Inquilino p)
        {
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Inquilino SET Nombre = @nombre , Dni = @dni , FechaN = @fecha_n , 
                            DomicilioTrabajo = @domicilio , Email = @email , Telefono = @telefono 
                            WHERE id = @id ;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@nombre", p.Nombre);
                    comm.Parameters.AddWithValue("@dni", p.Dni);
                    comm.Parameters.AddWithValue("@fecha_n", p.FechaN);
                    comm.Parameters.AddWithValue("@domicilio", p.DireccionTrabajo);
                    comm.Parameters.AddWithValue("@email", p.Email);
                    comm.Parameters.AddWithValue("@telefono", p.Telefono);
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
                string sql = @"DELETE FROM Inquilino WHERE Id = @id ;";

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

        public Inquilino Details(int id)
        {
            Inquilino p = new Inquilino();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT Id, Nombre, Dni, FechaN, DomicilioTrabajo, Email, Telefono 
                                FROM Inquilino WHERE Id = @id ;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@id", id);

                    conn.Open();
                    var reader = comm.ExecuteReader();

                    while (reader.Read())
                    {
                        p.Id = reader.GetInt32(0);
                        p.Nombre = reader.GetString(1);
                        p.Dni = reader.GetString(2);
                        p.FechaN = reader.GetDateTime(3);
                        p.DireccionTrabajo = reader.GetString(4);
                        p.Email = reader.GetString(5);
                        p.Telefono = reader.GetString(6);
                    }

                    conn.Close();

                }
            }
            return p;
        }

        public Inquilino Details(string dni)
        {
            Inquilino p = new Inquilino();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT Id, Nombre, Dni, FechaN, DomicilioTrabajo, Email, Telefono 
                                FROM Inquilino WHERE Dni = @dni ;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@dni", dni);

                    conn.Open();
                    var reader = comm.ExecuteReader();

                    while (reader.Read())
                    {
                        p.Id = reader.GetInt32(0);
                        p.Nombre = reader.GetString(1);
                        p.Dni = reader.GetString(2);
                        p.FechaN = reader.GetDateTime(3);
                        p.DireccionTrabajo = reader.GetString(4);
                        p.Email = reader.GetString(5);
                        p.Telefono = reader.GetString(6);
                    }

                    conn.Close();

                }
            }
            return p;
        }
        public int Put(Inquilino p)
        {
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Inquilino (Nombre, Dni, FechaN, DomicilioTrabajo, Email, Telefono) 
                            VALUES(@nombre, @dni, @fecha_n, @domicilio, @email, @telefono);
                            SELECT last_insert_id();";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@nombre", p.Nombre);
                    comm.Parameters.AddWithValue("@dni", p.Dni);
                    comm.Parameters.AddWithValue("@fecha_n", p.FechaN);
                    comm.Parameters.AddWithValue("@Domicilio", p.DireccionTrabajo);
                    comm.Parameters.AddWithValue("@email", p.Email);
                    comm.Parameters.AddWithValue("@telefono", p.Telefono);

                    conn.Open();

                    res = Convert.ToInt32(comm.ExecuteScalar());
                    p.Id = res;

                    conn.Close();
                }
            }
            return res;
        }

        public IList<Inquilino> All()
        {
            IList<Inquilino> list = new List<Inquilino>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT Id, Nombre, Dni, FechaN, DomicilioTrabajo, Email, Telefono
                                FROM Inquilino;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        var p = new Inquilino
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Dni = reader.GetString(2),
                            FechaN = reader.GetDateTime(3),
                            DireccionTrabajo = reader.GetString(4),
                            Email = reader.GetString(5),
                            Telefono = reader.GetString(6),
                        };

                        list.Add(p);
                    }
                    conn.Close();
                }
            }
            return list;
        }
        public Inquilino ById(int id) {

            Inquilino i = new Inquilino();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT Id, Dni, Nombre, FechaN, DomicilioTrabajo, Telefono, Email 
                                FROM Inquilino WHERE Id = @id ;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@id", id);
                    
                    conn.Open();
                    
                    var reader = comm.ExecuteReader();
                    
                    if (reader.Read()) {

                        i.Id = reader.GetInt32(0);
                        i.Dni = reader.GetString(1);
                        i.Nombre = reader.GetString(2);
                        i.FechaN = reader.GetDateTime(3);
                        i.DireccionTrabajo = reader.GetString(4);
                        i.Telefono = reader.GetString(5);
                        i.Email = reader.GetString(6);
                    }
                    
                    conn.Close();
                }
            }

            return i;
        }
    }
}
