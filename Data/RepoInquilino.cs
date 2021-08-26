using System;
using System.Collections.Generic;
using InmobiliariaAlbornoz.Models;
using MySql.Data.MySqlClient;

namespace InmobiliariaAlbornoz.Data
{
    public class RepoInquilino : RepoBase
    {
        //string connectionString = "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=C:\\Users\\Ezequiel\\OneDrive\\ULP\\4to Cuatrimestre\\Programación .NET\\segunda_clase\\WebApplication1\\Data\\WebApp1.mdf";
        //string connectionString = "server=localhost;user=root;password=;database=inmobiliaria;SslMode=none";

        public RepoInquilino()
        {

        }

        public int Edit(Inquilino p)
        {
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Inquilino SET Nombre = @nombre , Dni = @dni , FechaN = @fecha_n , 
                            DomicilioTrabajo = @domicilio , Email = @email , Telefono = @telefono, 
                            DniGarante = @dni_garante, NombreGarante = @nombre_garante, 
                            TelefonoGarante = @tel_garante, EmailGarante = @email_garante 
                            WHERE id = @id ;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@nombre", p.Nombre);
                    comm.Parameters.AddWithValue("@dni", p.Dni);
                    comm.Parameters.AddWithValue("@fecha_n", p.FechaN);
                    comm.Parameters.AddWithValue("@domicilio", p.DireccionTrabajo);
                    comm.Parameters.AddWithValue("@email", p.Email);
                    comm.Parameters.AddWithValue("@telefono", p.Telefono);
                    comm.Parameters.AddWithValue("@dni_garante", p.DniGarante);
                    comm.Parameters.AddWithValue("@nombre_garante", p.NombreGarante);
                    comm.Parameters.AddWithValue("@tel_garante", p.TelefonoGarante);
                    comm.Parameters.AddWithValue("@email_garante", p.EmailGarante);
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
                string sql = @"SELECT Id, Nombre, Dni, FechaN, DomicilioTrabajo, Email, Telefono, 
                                DniGarante, NombreGarante, TelefonoGarante, EmailGarante 
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
                        p.DniGarante = reader.GetString(7);
                        p.NombreGarante = reader.GetString(8);
                        p.TelefonoGarante = reader.GetString(9);
                        p.EmailGarante = reader.GetString(10);
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
                string sql = @"INSERT INTO Inquilino (Nombre, Dni, FechaN, DomicilioTrabajo, Email, Telefono, 
                            DniGarante, NombreGarante, TelefonoGarante, EmailGarante) 
                            VALUES(@nombre, @dni, @fecha_n, @domicilio, @email, @telefono, 
                            @dni_garante, @nombre_garante, @tel_garante, @email_garante);
                            SELECT last_insert_id();";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@nombre", p.Nombre);
                    comm.Parameters.AddWithValue("@dni", p.Dni);
                    comm.Parameters.AddWithValue("@fecha_n", p.FechaN);
                    comm.Parameters.AddWithValue("@Domicilio", p.DireccionTrabajo);
                    comm.Parameters.AddWithValue("@email", p.Email);
                    comm.Parameters.AddWithValue("@telefono", p.Telefono);
                    comm.Parameters.AddWithValue("@dni_garante", p.DniGarante);
                    comm.Parameters.AddWithValue("@nombre_garante", p.NombreGarante);
                    comm.Parameters.AddWithValue("@tel_garante", p.TelefonoGarante);
                    comm.Parameters.AddWithValue("@email_garante", p.EmailGarante);

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
                string sql = @"SELECT Id, Nombre, Dni, FechaN, DomicilioTrabajo, Email, Telefono, 
                           NombreGarante, DniGarante, TelefonoGarante, EmailGarante FROM Inquilino;";

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
                            NombreGarante = reader.GetString(7),
                            DniGarante = reader.GetString(8),
                            TelefonoGarante = reader.GetString(9),
                            EmailGarante = reader.GetString(10),
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
                string sql = @"SELECT Id, Dni, Nombre, FechaN, DomicilioTrabajo, Telefono, Email, 
                                DniGarante, NombreGarante, TelefonoGarante, EmailGarante 
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

                        i.DniGarante = reader.GetString(7);
                        i.NombreGarante = reader.GetString(8);
                        i.TelefonoGarante = reader.GetString(9);
                        i.EmailGarante = reader.GetString(10);
                    }
                    
                    conn.Close();
                }
            }

            return i;
        }
    }
}
