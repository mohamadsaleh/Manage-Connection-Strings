using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Web;

public static class MysqlDbHelper
{
    public static DataTable GetDataWithAdapter(string connectionString, string sqlQuery)
    {
        DataTable dataTable = new DataTable();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlQuery, connection))
            {
                try
                {
                    dataAdapter.Fill(dataTable);

                    Console.WriteLine("Successfully retrieved " + dataTable.Rows.Count + " rows.");
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Database Error: " + ex.Message);
                }
            }
        }
        return dataTable;
    }

    public static string GetStringWithAdapter(string connectionString, string sqlQuery)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
            {
                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    string dbString = Convert.ToString(result);
                    return dbString;
                }
                catch (MySqlException ex)
                {
                    return "Database Error retrieving label: " + ex.Message;
                }
            }
        }
    }

    public static string GetNoneWithAdapter(string connectionString, string sqlQuery)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
            {
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    return "Command Executed Successfully.";
                }
                catch (MySqlException ex)
                {
                    return "Database Error retrieving label: " + ex.Message;
                }
            }
        }
    }
}