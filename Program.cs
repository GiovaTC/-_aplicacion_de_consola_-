
using System;
using Oracle.ManagedDataAccess.Client;

namespace aplicacion_de_consola
{
    class Program
    {
        static string connectionString = 
            "DATA SOURCE = localhost:1521/orcl;" + 
            "USER ID = system;" +
            "PASSWORD = Tapiero123;";   

        static void Main(string[] args)
        {
            Console.Title = "PROMEDIO DE 46 ALUMNOS";


            Console.WriteLine("====================================");
            Console.WriteLine(" REGISTRO DE 46 ALUMNOS");
            Console.WriteLine(" ORACLE DATABASE 19c");
            Console.WriteLine("====================================");

            for (int i = 1; i <= 46; i++)
            {
                Console.WriteLine("\n================================");
                Console.WriteLine($"ALUMNO #{i}");
                Console.WriteLine("================================");

                Console.Write("Codigo: ");
                string codigo = Console.ReadLine();

                Console.Write("Nombre: ");
                string nombre = Console.ReadLine();

                Console.Write("Nota 1: ");
                double nota1 = Convert.ToDouble(Console.ReadLine());

                Console.Write("Nota 2: ");
                double nota2 = Convert.ToDouble(Console.ReadLine());

                Console.Write("Nota 3: ");
                double nota3 = Convert.ToDouble(Console.ReadLine());

                InsertarAlumno(
                    codigo,
                    nombre,
                    nota1,
                    nota2,
                    nota3
                );

                Console.WriteLine("Alumno registrado correctamente.");
            }

            MostrarAlumnos();


            Console.WriteLine("\nProceso finalizado.");
            Console.ReadKey();
        }

        private static void InsertarAlumno(
            string codigo,
            string nombre,
            double nota1,
            double nota2,
            double nota3)
        {
            try
            {
                using (OracleConnection conn =
                    new OracleConnection(connectionString))
                {
                    conn.Open();

                    using (OracleCommand cmd =
                        new OracleCommand("INSERTAR_ALUMNO", conn))
                    {
                        cmd.CommandType =
                            System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("P_CODIGO",
                            OracleDbType.Varchar2).Value = codigo;

                        cmd.Parameters.Add("P_NOMBRE",
                            OracleDbType.Varchar2).Value = nombre;

                        cmd.Parameters.Add("P_NOTA1",
                            OracleDbType.Double).Value = nota1;

                        cmd.Parameters.Add("P_NOTA2",
                            OracleDbType.Double).Value = nota2;

                        cmd.Parameters.Add("P_NOTA3",
                            OracleDbType.Double).Value = nota3;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Oracle:");
                Console.WriteLine(ex.Message);
            }
        }

        private static void MostrarAlumnos()
        {
            try
            {
                using (OracleConnection conn =
                    new OracleConnection(connectionString))
                {
                    conn.Open();

                    string sql =
                        "SELECT CODIGO, NOMBRE, " +
                        "PROMEDIO, ESTADO " +
                        "FROM ALUMNOS_L";

                    using (OracleCommand cmd =
                        new OracleCommand(sql, conn))
                    {
                        using (OracleDataReader dr =
                            cmd.ExecuteReader())
                        {
                            Console.WriteLine("\n");
                            Console.WriteLine(
                                "========= LISTA DE ALUMNOS =========");

                            while (dr.Read())
                            {
                                Console.WriteLine(
                                    $"Codigo: {dr["CODIGO"]} | " +
                                    $"Nombre: {dr["NOMBRE"]} | " +
                                    $"Promedio: {dr["PROMEDIO"]} | " +
                                    $"Estado: {dr["ESTADO"]}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }   
}