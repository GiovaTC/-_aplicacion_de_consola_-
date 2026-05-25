# -_aplicacion_de_consola_- :.
Promedio de 46 Alumnos:

<img width="1024" height="1024" alt="image" src="https://github.com/user-attachments/assets/9cb86205-f390-46ff-8d9f-34c6b7f8ab19" /> 

<img width="2551" height="1079" alt="image" src="https://github.com/user-attachments/assets/e7611ef8-1f15-43c3-be11-02e74e1492d3" />    

```
Visual Studio 2022 + C# + Oracle Database 19c.
El siguiente proyecto permite:
Registrar información de 46 alumnos
Guardar:
Código
Nombre
Nota 1
Nota 2
Nota 3
Promedio
Estado (APROBADO / REPROBADO)

Conectarse a Oracle Database 19c
Utilizar procedimientos almacenados
Consultar estudiantes registrados
Aplicación de consola en C#

Tecnologías Utilizadas
Visual Studio 2022
C# .NET 8
Oracle Database 19c
Oracle ManagedDataAccess
Procedimientos almacenados

SQL Oracle
1. SCRIPT ORACLE 19c
Crear Tabla
CREATE TABLE ALUMNOS (
    ID_ALUMNO NUMBER GENERATED ALWAYS AS IDENTITY,
    CODIGO VARCHAR2(20),
    NOMBRE VARCHAR2(100),
    NOTA1 NUMBER(5,2),
    NOTA2 NUMBER(5,2),
    NOTA3 NUMBER(5,2),
    PROMEDIO NUMBER(5,2),
    ESTADO VARCHAR2(20),
    FECHA_REGISTRO DATE DEFAULT SYSDATE,

    CONSTRAINT PK_ALUMNOS PRIMARY KEY (ID_ALUMNO)
);

2. PROCEDIMIENTO ALMACENADO
CREATE OR REPLACE PROCEDURE INSERTAR_ALUMNO (
    P_CODIGO   IN VARCHAR2,
    P_NOMBRE   IN VARCHAR2,
    P_NOTA1    IN NUMBER,
    P_NOTA2    IN NUMBER,
    P_NOTA3    IN NUMBER
)
AS
    V_PROMEDIO NUMBER(5,2);
    V_ESTADO   VARCHAR2(20);
BEGIN

    V_PROMEDIO := (P_NOTA1 + P_NOTA2 + P_NOTA3) / 3;

    IF V_PROMEDIO >= 3.0 THEN
        V_ESTADO := 'APROBADO';
    ELSE
        V_ESTADO := 'REPROBADO';
    END IF;

    INSERT INTO ALUMNOS (
        CODIGO,
        NOMBRE,
        NOTA1,
        NOTA2,
        NOTA3,
        PROMEDIO,
        ESTADO
    )
    VALUES (
        P_CODIGO,
        P_NOMBRE,
        P_NOTA1,
        P_NOTA2,
        P_NOTA3,
        ROUND(V_PROMEDIO,2),
        V_ESTADO
    );

    COMMIT;

END;
/

3. CONSULTAR DATOS
SELECT * FROM ALUMNOS;

4. CREAR PROYECTO EN VISUAL STUDIO 2022
Crear proyecto
Console App (.NET)
Nombre del proyecto
PromedioAlumnosOracle

5. INSTALAR PAQUETE ORACLE
Abrir
Herramientas
→ Administrador de paquetes NuGet
→ Consola del Administrador
Instalar
Install-Package Oracle.ManagedDataAccess

6. CADENA DE CONEXIÓN
Datos de ejemplo
Usuario : system
Password: oracle
Puerto  : 1521
XE      : XE

7. PROGRAM.CS
using System;
using Oracle.ManagedDataAccess.Client;

namespace PromedioAlumnosOracle
{
    class Program
    {
        static string connectionString =
            "DATA SOURCE=localhost:1521/XE;" +
            "USER ID=system;" +
            "PASSWORD=oracle;";

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

        static void MostrarAlumnos()
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
                        "FROM ALUMNOS";

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

8. EJEMPLO DE SALIDA
====================================
 REGISTRO DE 46 ALUMNOS
 ORACLE DATABASE 19c
====================================

ALUMNO #1
Codigo: A001
Nombre: Carlos
Nota 1: 4.5
Nota 2: 3.8
Nota 3: 4.0

Alumno registrado correctamente.

9. RESULTADO EN ORACLE
SELECT * FROM ALUMNOS;

Resultado
CODIGO	NOMBRE	PROMEDIO	ESTADO
A001	  Carlos	4.10	    APROBADO
A002	  Ana	    2.80	    REPROBADO

10. ESTRUCTURA DEL PROYECTO
PromedioAlumnosOracle
│
├── Program.cs
│
├── Packages
│     └── Oracle.ManagedDataAccess
│
└── Oracle Database 19c

11. MEJORAS POSIBLES
Puede ampliarse para incluir:
Menú interactivo
Validación de notas
Exportar a PDF
Exportar a Excel
Estadísticas generales
Mejor promedio
Peor promedio
Interfaz gráfica Windows Forms
Reportes Oracle
Resultado Final

La aplicación permite:
Registrar automáticamente 46 alumnos
Calcular el promedio de notas
Determinar si el alumno:
APRUEBA
REPRUEBA
Guardar toda la información en Oracle Database 19c
Consultar los datos desde C#
Utilizar procedimientos almacenados Oracle
Implementar conexión JDBC equivalente mediante Oracle.ManagedDataAccess.
:. . / .
