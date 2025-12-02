using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using System.Linq;
using System.Web;

public static class PostgreSqlDbHelper
{
    public static DataTable GetDataWithAdapter(string connectionString, string sqlQuery)
    {
        DataTable dataTable = new DataTable();

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(sqlQuery, connection))
            {
                try
                {
                    dataAdapter.Fill(dataTable);

                    Console.WriteLine("Successfully retrieved " + dataTable.Rows.Count + " rows.");
                }
                catch (NpgsqlException ex)
                {
                    Console.WriteLine("Database Error: " + ex.Message);
                }
            }
        }
        return dataTable;
    }

    public static string GetStringWithAdapter(string connectionString, string sqlQuery)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            using (NpgsqlCommand command = new NpgsqlCommand(sqlQuery, connection))
            {
                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    string dbString = Convert.ToString(result);
                    return dbString;
                }
                catch (NpgsqlException ex)
                {
                    return "Database Error retrieving label: " + ex.Message;
                }
            }
        }
    }

    public static string GetNoneWithAdapter(string connectionString, string sqlQuery)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            using (NpgsqlCommand command = new NpgsqlCommand(sqlQuery, connection))
            {
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    return "Command Executed Successfully.";
                }
                catch (NpgsqlException ex)
                {
                    return "Database Error retrieving label: " + ex.Message;
                }
            }
        }
    }
}