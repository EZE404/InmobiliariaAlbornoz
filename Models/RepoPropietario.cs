using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlbornoz.Models
{
    public class RepoPropietario
    {
        //string connectionString = "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=C:\\Users\\Ezequiel\\OneDrive\\ULP\\4to Cuatrimestre\\Programación .NET\\segunda_clase\\WebApplication1\\Data\\WebApp1.mdf";
        string connectionString = "Server=(localdb)\\mssqllocaldb;Database=InmobiliariaAlbornoz;Trusted_Connection=True;MultipleActiveResultSets=true";
        //string conString = Configuration.getStringConnection("database");
        public RepoPropietario()
        {

        }

        public int Edit(Propietario p)
        {
            int res = -1;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"UPDATE Propietarios SET Nombre = @nombre , Dni = @dni , FechaN = @fecha_n 
                                Domicilio = @domicilio , Email = @email , Telefono = @telefono WHERE id = @id ;";

                using (SqlCommand comm = new SqlCommand(sql, conn))
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
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"DELETE FROM Propietarios WHERE Id = @id ;";

                using (SqlCommand comm = new SqlCommand(sql, conn))
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
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT Id, Nombre, Dni, FechaN, Domicilio, Email, Telefono FROM Propietarios 
                                WHERE Id = @id ;";

                using (SqlCommand comm = new SqlCommand(sql, conn))
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
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Propietarios (Nombre, Dni, FechaN, Domicilio, Email, Telefono) 
                            VALUES(@nombre, @dni, @fecha_n, @domicilio, @email, @telefono);
                            SELECT SCOPE_IDENTITY();";

                using (SqlCommand comm = new SqlCommand(sql, conn))
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

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT Id, Nombre, Dni, FechaN, Domicilio, Email, Telefono FROM Propietarios;";

                using (SqlCommand comm = new SqlCommand(sql, conn))
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
    }
}
