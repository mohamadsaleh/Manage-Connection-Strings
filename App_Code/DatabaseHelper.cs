using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DatabaseHelper
/// </summary>
public static class DatabaseHelper
{
    public static DataTable GetDataWithAdapter(string connectionString, string sqlQuery)
    {
        DataTable dataTable = new DataTable();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlQuery, connection))
            {
                try
                {
                    dataAdapter.Fill(dataTable);

                    Console.WriteLine("Successfully retrieved " + dataTable.Rows.Count + " rows.");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Database Error: " + ex.Message);
                }
            }
        }
        return dataTable;
    }
    public static string GetStringWithAdapter(string connectionString, string sqlQuery)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    string dbString = Convert.ToString(result);
                    return dbString;
                }
                catch (SqlException ex)
                {
                    return "Database Error retrieving label: " + ex.Message;
                }
            }
        }
    }
    public static string GetNoneWithAdapter(string connectionString, string sqlQuery)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();                    
                    return "Command Executed Successfully.";
                }
                catch (SqlException ex)
                {
                    return "Database Error retrieving label: " + ex.Message;
                }
            }
        }
    }
}