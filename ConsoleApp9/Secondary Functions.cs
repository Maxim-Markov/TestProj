using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp9
{
    partial class Program
    {
        private static void Query(SqlConnection conn)
        {
            string sql = "SELECT DISTINCT Full_name, Date_of_Birth, Gender FROM Persons ORDER BY Full_name";

            // Создать объект Command.
            SqlCommand cmd = new SqlCommand(sql, conn);

            using (DbDataReader reader = cmd.ExecuteReader())
            {

                if (reader.HasRows)
                {
                    int queryCounter = 1;
                    while (reader.Read())
                    {
                        //Console.WriteLine(reader.GetSchemaTable().Rows.Count);
                        Console.WriteLine($"Строка N {queryCounter++}");
                        // Индекс столбца FullName в комманде SQL.
                        int nameIndex = reader.GetOrdinal("Full_name");
                        string fullName = Convert.ToString(reader.GetValue(nameIndex));

                        int dateIndex = reader.GetOrdinal("Date_of_birth");
                        DateTime dateOfBirth = Convert.ToDateTime(reader.GetValue(dateIndex));

                        int genderIndex = reader.GetOrdinal("Gender");
                        string Gender = Convert.ToString(reader.GetValue(genderIndex));

                        int Age = (int)((DateTime.Now - dateOfBirth).TotalDays / 365.25);
                        Console.WriteLine($"Full Name: {fullName},\ndate of birth: {dateOfBirth.ToString("dd.MM.yyyy")},\ngender: {Gender}\nage {Age}\n");

                    }
                }
            }
        }

        private static void QueryWithConstraints(SqlConnection conn)
        {
            string sql = "SELECT Full_name, Date_of_Birth, Gender FROM Persons WHERE Gender='M' AND Full_name LIKE 'F%'";

            // Создать объект Command.
            SqlCommand cmd = new SqlCommand(sql, conn);

            using (DbDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    int queryCounter = 1;
                    while (reader.Read())
                    {

                        int nameIndex = reader.GetOrdinal("Full_name");
                        string fullName = Convert.ToString(reader.GetValue(nameIndex));

                        int dateIndex = reader.GetOrdinal("Date_of_birth");
                        DateTime dateOfBirth = Convert.ToDateTime(reader.GetValue(dateIndex));

                        int genderIndex = reader.GetOrdinal("Gender");
                        string Gender = Convert.ToString(reader.GetValue(genderIndex));



                    }
                }
            }
        }

        private static void AutoInsertRows(SqlConnection connection, in int rowsInsert)
        {
            if (rowsInsert == 0) return;

            string sql = "INSERT INTO Persons VALUES";
            for (int i = 1; i < rowsInsert; i++)
            {
                sql += $"(@FullName{i}, @DateOfBirth{i}, @Gender{i}),";
            }
            sql += $"(@FullName{rowsInsert}, @DateOfBirth{rowsInsert}, @Gender{rowsInsert})";
            SqlCommand cmd = new SqlCommand(sql, connection);
            for (int i = 1; i <= rowsInsert; i++)
            {
                cmd.Parameters.Add($"@FullName{i}", SqlDbType.NVarChar).Value = GetRandName();

                cmd.Parameters.Add($"@DateOfBirth{i}", SqlDbType.Date).Value = new DateTime(1998, 12, 2);
                cmd.Parameters.Add($"@Gender{i}", SqlDbType.NVarChar).Value = RandomString();
            }
            try
            {
                int RowCount = cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
                Console.WriteLine(e.StackTrace);
            }
        }

        private static void Insert100RowsWithF(SqlConnection connection)
        {


            string sql = "INSERT INTO Persons VALUES";
            for (int i = 1; i < 100; i++)
            {
                sql += $"(@FullName{i}, @DateOfBirth{i}, @Gender{i}),";
            }
            sql += $"(@FullName100, @DateOfBirth100, @Gender100)";
            SqlCommand cmd = new SqlCommand(sql, connection);
            for (int i = 1; i <= 100; i++)
            {
                cmd.Parameters.Add($"@FullName{i}", SqlDbType.NVarChar).Value = "F" + GetRandName();

                cmd.Parameters.Add($"@DateOfBirth{i}", SqlDbType.Date).Value = new DateTime(1998, 12, 2);
                cmd.Parameters.Add($"@Gender{i}", SqlDbType.NVarChar).Value = "M";
            }
            try
            {
                int RowCount = cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
                Console.WriteLine(e.StackTrace);
            }
        }

        private static string GetRandName()
        {
            string name = "$rnd $rnd $rnd";
            string output = "";
            Regex rx = new Regex("\\$rnd");
            int count = name.Split(new string[] { "$rnd" }, StringSplitOptions.None).Count() - 1;

            for (int i = 0; i < count; i++)
            {
                output = rx.Replace(name, RandomString(rand.Next(3, 9)), 1);
                name = output;
            }
            return output;
        }

        private static string RandomString(int length)
        {
            string chars = "abcdefghijklmnopqrstuvwxyz";
            StringBuilder builder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                builder.Append(chars[rand.Next(chars.Length)]);

            }
            builder[0] = char.ToUpper(builder[0]);
            return builder.ToString();
        }

        private static string RandomString()
        {
            string chars = "WM";
            return chars[rand.Next(chars.Length)].ToString();
        }
    }
}
