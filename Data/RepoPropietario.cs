using System;
using System.Collections.Generic;
using InmobiliariaAlbornoz.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace InmobiliariaAlbornoz.Data
{
    public class RepoPropietario : RepoBase
    {
        //string connectionString = "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=C:\\Users\\Ezequiel\\OneDrive\\ULP\\4to Cuatrimestre\\Programación .NET\\segunda_clase\\WebApplication1\\Data\\WebApp1.mdf";
        //string connectionString = "server=localhost;user=root;password=;database=inmobiliaria;SslMode=none";
        public RepoPropietario(IConfiguration configuration) : base(configuration)
        {

        }

        public int Edit(Propietario p)
        {
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Propietario SET Nombre = @nombre , Dni = @dni , FechaN = @fecha_n , 
                            Domicilio = @domicilio , Email = @email , Telefono = @telefono WHERE id = @id ;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@nombre", p.Nombre);
                    comm.Parameters.AddWithValue("@dni", p.Dni);
                    comm.Parameters.AddWithValue("@fecha_n", p.FechaN);
                    comm.Parameters.AddWithValue("@domicilio", p.Direccion);
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
                string sql = @"DELETE FROM Propietario WHERE Id = @id ;";

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

        public Propietario Details(int id)
        {
            Propietario p = new Propietario();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT Id, Nombre, Dni, FechaN, Domicilio, Email, Telefono FROM Propietario 
                                WHERE Id = @id ;";

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
                        p.Direccion = reader.GetString(4);
                        p.Email = reader.GetString(5);
                        p.Telefono = reader.GetString(6);
                    }

                    conn.Close();

                }
            }
            return p;
        }
        public int Put(Propietario p)
        {
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Propietario (Nombre, Dni, FechaN, Domicilio, Email, Telefono) 
                            VALUES(@nombre, @dni, @fecha_n, @domicilio, @email, @telefono);
                            SELECT last_insert_id();";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@nombre", p.Nombre);
                    comm.Parameters.AddWithValue("@dni", p.Dni);
                    comm.Parameters.AddWithValue("@fecha_n", p.FechaN);
                    comm.Parameters.AddWithValue("@Domicilio", p.Direccion);
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

        public IList<Propietario> All()
        {
            IList<Propietario> list = new List<Propietario>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT Id, Nombre, Dni, FechaN, Domicilio, Email, Telefono FROM Propietario;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        var p = new Propietario
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Dni = reader.GetString(2),
                            FechaN = reader.GetDateTime(3),
                            Direccion = reader.GetString(4),
                            Email = reader.GetString(5),
                            Telefono = reader.GetString(6)
                        };

                        list.Add(p);
                    }
                    conn.Close();
                }
            }
            return list;
        }

        public Propietario ById(int id) {

            Propietario p = new Propietario();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT Id, Dni, Nombre, FechaN, Domicilio, Telefono, Email
                                FROM Propietario WHERE Id = @id ;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@id", id);
                    
                    conn.Open();
                    
                    var reader = comm.ExecuteReader();
                    
                    if (reader.Read()) {
                        p.Id = reader.GetInt32(0);
                        p.Dni = reader.GetString(1);
                        p.Nombre = reader.GetString(2);
                        p.FechaN = reader.GetDateTime(3);
                        p.Direccion = reader.GetString(4);
                        p.Telefono = reader.GetString(5);
                        p.Email = reader.GetString(6);
                    }
                    
                    conn.Close();
                }
            }

            return p;
        }
    }
}
