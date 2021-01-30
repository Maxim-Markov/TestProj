using System;
using System.Data.SqlClient;
using System.Data;


namespace ConsoleApp9
{
    partial class Program
    {
        public static Random rand = new Random();
        static void Main(string[] args)
        {
            string pointStr = args[0][0].ToString();
            int point;
            int.TryParse(pointStr, out point);
            string insName = "";
            DateTime insDate = new DateTime();
            string insGender = "";
            if (point == 2)
            {
                if (args.Length != 6)
                {
                    Console.WriteLine("Неверное количество аргументов, пример строки ввода\"2 Halavin Nikita Antonovich 23.03.1998 M\"");
                    return;
                }
                insName = args[1] + " " + args[2] + " " + args[3];
                insDate = Convert.ToDateTime(args[4]);
                if (args[5] != "M" && args[5] != "W")
                {
                    Console.WriteLine("Можно ввести только M или W");
                    return;
                }
                insGender = args[5];
            }
            switch (point)
            {
                case 1:
                    {
                        SqlConnection connection = DBSQLServerUtils.GetDBConnection();
                        string sql = "CREATE TABLE Persons" +
                            "(Id INT PRIMARY KEY IDENTITY, Full_name NVARCHAR(100) NOT NULL, Date_of_birth DATE NOT NULL, Gender NVARCHAR(1) NOT NULL)";

                        SqlCommand cmd = new SqlCommand(sql, connection);
                        try
                        {
                            connection.Open();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error: " + e);
                            Console.WriteLine(e.StackTrace);
                        }
                        finally
                        {
                            connection.Close();
                            connection.Dispose();
                            connection = null;
                        }
                        Console.WriteLine("запрос выполнен");
                        break;
                    }
                case 2:
                    {
                        SqlConnection connection = DBSQLServerUtils.GetDBConnection();
                        string sql = "INSERT INTO Persons" +
                            " VALUES (@FullName, @DateOfBirth, @Gender)";
                        SqlCommand cmd = new SqlCommand(sql, connection);

                        SqlParameter nameParam = new SqlParameter("@FullName", SqlDbType.NVarChar);
                        nameParam.Value = insName;
                        cmd.Parameters.Add(nameParam);

                        cmd.Parameters.Add("@DateOfBirth", SqlDbType.Date).Value = insDate;
                        cmd.Parameters.Add("@Gender", SqlDbType.NVarChar).Value = insGender;

                        try
                        {
                            connection.Open();
                            int RowCount = cmd.ExecuteNonQuery();
                            Console.WriteLine("Row Count affected" + RowCount);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error: " + e);
                            Console.WriteLine(e.StackTrace);
                        }
                        finally
                        {
                            connection.Close();
                            connection.Dispose();
                            connection = null;
                        }
                        Console.WriteLine("запрос выполнен,строка вставлена");
                        break;
                    }
                case 3:
                    {
                        SqlConnection connection = DBSQLServerUtils.GetDBConnection();
                        try
                        {
                            connection.Open();
                            Query(connection);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error: " + e);
                            Console.WriteLine(e.StackTrace);
                        }
                        finally
                        {
                            connection.Close();
                            connection.Dispose();
                            connection = null;
                        }
                        Console.WriteLine("запрос выполнен");
                        break;
                    }
                case 4:
                    {
                        const int rowsInsert = 1_000_000;
                        const int maxRowsInsertPerQuery = 699;//больше 2099 параметров сервер за один запрос не поддерживает
                        SqlConnection connection = DBSQLServerUtils.GetDBConnection();
                        connection.Open();
                        int queryCounter = 0;
                        while (rowsInsert >= (maxRowsInsertPerQuery * ++queryCounter))
                        {
                            AutoInsertRows(connection, maxRowsInsertPerQuery);//вставляет рандомные строки, 699 строк
                            Console.WriteLine($"Строк добавлено {queryCounter * maxRowsInsertPerQuery}");
                        }
                        AutoInsertRows(connection, rowsInsert - --queryCounter * maxRowsInsertPerQuery);//вставляет рандомные строки, которых не хватило до maxRowsInsertPerQuery 

                        Insert100RowsWithF(connection);//вставлем 100 строк, где первая F и пол мужской

                        connection.Close();
                        connection.Dispose();
                        connection = null;
                        Console.WriteLine($"Строк добавлено {rowsInsert}");
                        Console.WriteLine("запрос выполнен");
                        break;
                    }
                case 5:
                    {
                        SqlConnection connection = DBSQLServerUtils.GetDBConnection();
                        try
                        {
                            connection.Open();
                            DateTime before = DateTime.Now;
                            QueryWithConstraints(connection);//запрос с условием
                            Console.WriteLine($"Миллисекунд: {(DateTime.Now - before).TotalMilliseconds}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error: " + e);
                            Console.WriteLine(e.StackTrace);
                        }
                        finally
                        {
                            connection.Close();
                            connection.Dispose();
                            connection = null;
                        }
                        break;
                    }
            }

        }

    }
}
