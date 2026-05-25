using System;
using Oracle.ManagedDataAccess.Client;

namespace aplicacion_de_consola
{
    class Program
    {
        // ============================================
        // CADENA DE CONEXION ORACLE 19c
        // ============================================

        static string connectionString =
            "User Id=system;" +
            "Password=Tapiero123;" +
            "Data Source=(DESCRIPTION=" +
            "(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))" +
            "(CONNECT_DATA=(SERVICE_NAME=orcl)));";

        // ============================================
        // MAIN
        // ============================================

        static void Main(string[] args)
        {
            Console.Title = "PROMEDIO DE 46 ALUMNOS";

            Console.WriteLine("====================================");
            Console.WriteLine(" REGISTRO DE 46 ALUMNOS");
            Console.WriteLine(" ORACLE DATABASE 19c");
            Console.WriteLine("====================================");

            // ============================================
            // PROBAR CONEXION
            // ============================================

            if (!ProbarConexion())
            {
                Console.WriteLine("\nNo fue posible conectar con Oracle.");
                Console.ReadKey();
                return;
            }

            // ============================================
            // REGISTRAR 46 ALUMNOS
            // ============================================

            for (int i = 1; i <= 46; i++)
            {
                Console.WriteLine("\n================================");
                Console.WriteLine($"ALUMNO #{i}");
                Console.WriteLine("================================");

                Console.Write("Codigo: ");
                string codigo = Console.ReadLine();

                Console.Write("Nombre: ");
                string nombre = Console.ReadLine();

                double nota1 = LeerNota("Nota 1");
                double nota2 = LeerNota("Nota 2");
                double nota3 = LeerNota("Nota 3");

                InsertarAlumno(
                    codigo,
                    nombre,
                    nota1,
                    nota2,
                    nota3
                );
            }

            // ============================================
            // MOSTRAR ALUMNOS
            // ============================================

            MostrarAlumnos();

            Console.WriteLine("\nProceso finalizado.");
            Console.ReadKey();
        }

        // ============================================
        // PROBAR CONEXION
        // ============================================

        static bool ProbarConexion()
        {
            try
            {
                using (OracleConnection conn =
                    new OracleConnection(connectionString))
                {
                    conn.Open();

                    Console.WriteLine("\nConexion Oracle exitosa.");

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nERROR DE CONEXION");
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        // ============================================
        // LEER NOTAS
        // ============================================

        static double LeerNota(string mensaje)
        {
            double nota;

            while (true)
            {
                Console.Write($"{mensaje}: ");

                if (double.TryParse(
                    Console.ReadLine(),
                    out nota))
                {
                    if (nota >= 0 && nota <= 5)
                    {
                        return nota;
                    }
                }

                Console.WriteLine(
                    "Ingrese una nota valida entre 0 y 5.");
            }
        }

        // ============================================
        // INSERTAR ALUMNO
        // ============================================

        static void InsertarAlumno(
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
                        new OracleCommand(
                            "INSERTAR_ALUMNO",
                            conn))
                    {
                        cmd.CommandType =
                            System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(
                            "P_CODIGO",
                            OracleDbType.Varchar2).Value = codigo;

                        cmd.Parameters.Add(
                            "P_NOMBRE",
                            OracleDbType.Varchar2).Value = nombre;

                        cmd.Parameters.Add(
                            "P_NOTA1",
                            OracleDbType.Double).Value = nota1;

                        cmd.Parameters.Add(
                            "P_NOTA2",
                            OracleDbType.Double).Value = nota2;

                        cmd.Parameters.Add(
                            "P_NOTA3",
                            OracleDbType.Double).Value = nota3;

                        cmd.ExecuteNonQuery();

                        Console.WriteLine(
                            "Alumno registrado correctamente.");
                    }
                }
            }
            catch (OracleException ex)
            {
                Console.WriteLine("\nERROR ORACLE");
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nERROR GENERAL");
                Console.WriteLine(ex.Message);
            }
        }

        // ============================================
        // MOSTRAR ALUMNOS
        // ============================================

        static void MostrarAlumnos()
        {
            try
            {
                using (OracleConnection conn =
                    new OracleConnection(connectionString))
                {
                    conn.Open();

                    string sql =
                        "SELECT CODIGO, " +
                        "NOMBRE, " +
                        "PROMEDIO, " +
                        "ESTADO " +
                        "FROM ALUMNOS_L";

                    using (OracleCommand cmd =
                        new OracleCommand(sql, conn))
                    {
                        using (OracleDataReader dr =
                            cmd.ExecuteReader())
                        {
                            Console.WriteLine("\n");
                            Console.WriteLine(
                                "==============================================");

                            Console.WriteLine(
                                "LISTA DE ALUMNOS");

                            Console.WriteLine(
                                "==============================================");

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
                Console.WriteLine("\nERROR CONSULTANDO DATOS");
                Console.WriteLine(ex.Message);
            }
        }
    }
}