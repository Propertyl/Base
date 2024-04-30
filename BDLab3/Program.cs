using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Security;
class DBMySQLUtils
{
    public static MySqlConnection GetDBConnection(string host, int port, string database, string username, string password)
    {
        string connString = $"Server={host};Database={database};port={port};User Id={username};password={password}";
        MySqlConnection conn = new MySqlConnection(connString);
        return conn;
    }
}

class DBUtils 
{
    public static MySqlConnection GetDBConnection()
    {
        string host = "localhost";
        int port = 3306;
        string database = "injuery";
        string username = "root";
        string password = "ae3036vn";

        return DBMySQLUtils.GetDBConnection(host,port,database,username,password);
    }
    
}
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Getting Connection...");
        MySqlConnection conn = DBUtils.GetDBConnection();
        bool isQuit = true;
        try
        {
            Console.WriteLine("Openning Connection...");
            conn.Open();
            Console.WriteLine("Connection successful!");

            while (isQuit)
            {
                Console.Write("Введите команду:");
                string command = Console.ReadLine().ToLower();

                switch (command)
                {
                    case "пользователи":
                        QueryAllData(conn, "user_gaz");
                        break;
                    case "платежи":
                        QueryAllData(conn, "payment");
                        break;
                    case "тарифы":
                        QueryAllData(conn, "Tarifs");
                        break;
                    case "повысить":
                        UpdateTarif(conn, 1.05);
                        Console.Write("Обновленные тарифы:");
                        QueryAllData(conn, "Tarifs");
                        break;
                    case "понизить":
                        UpdateTarif(conn, 0.95);
                        Console.Write("Обновленные тарифы:");
                        QueryAllData(conn, "Tarifs");
                        break;
                    case "выход":
                        isQuit = false;
                        break;
                    default:
                        Console.WriteLine("Неизвестная команда");
                        break;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
        finally
        {
            conn.Close();
            conn.Dispose();
            Console.WriteLine("Connection closed");
        }
        Console.Read();
    }

    private static void QueryAllData(MySqlConnection conn, string tableName)
    {
        string sql = $"SELECT * FROM {tableName}";
        MySqlCommand cmd = new MySqlCommand(sql, conn);
        using (MySqlDataReader rdr = cmd.ExecuteReader())
        {
            while (rdr.Read())
            {
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    Console.Write($"{rdr.GetName(i)}: {rdr.GetValue(i)}, ");
                }
                Console.WriteLine();
            }
        }
    }

    private static void UpdateTarif(MySqlConnection conn, double factor)
    {
        string sql = $"UPDATE Tarifs SET bill_sum = bill_sum * {factor.ToString(System.Globalization.CultureInfo.InvariantCulture)}";
        MySqlCommand cmd = new MySqlCommand(sql, conn);
        cmd.ExecuteNonQuery();
    }
}


