using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Manages application configuration for users and connection strings using SQLite database.
/// </summary>
public static class ConfigManager
{
    public static User[] Users
    {
        get
        {
            // Initialize database if not exists
            SqliteHelper.InitializeDatabase();

            // Get users from database
            var users = SqliteHelper.GetUsers();
            return users.ToArray();
        }
    }

    public static ConnectionString[] ConnectionStrings
    {
        get
        {
            // Initialize database if not exists
            SqliteHelper.InitializeDatabase();

            // Get connection strings from database
            var connectionStrings = SqliteHelper.GetConnectionStrings();
            return connectionStrings.ToArray();
        }
    }

    public static void SaveConfigs<T>(List<T> list)
    {
        if (typeof(T) == typeof(User))
        {
            foreach (User user in list.Cast<User>())
            {
                SqliteHelper.SaveUser(user);
            }
        }
        else if (typeof(T) == typeof(ConnectionString))
        {
            foreach (ConnectionString connectionString in list.Cast<ConnectionString>())
            {
                SqliteHelper.SaveConnectionString(connectionString);
            }
        }
    }
}
