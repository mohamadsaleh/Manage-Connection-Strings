using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;

public static class SqliteHelper
{
    private static string DatabasePath
    {
        get
        {
            string relativePath = "~/App_Data/ConnectionStrings.db";
            return HttpContext.Current.Server.MapPath(relativePath);
        }
    }

    public static void InitializeDatabase()
    {
        string dbPath = DatabasePath;

        // Ensure App_Data directory exists
        string appDataPath = Path.GetDirectoryName(dbPath);
        if (!Directory.Exists(appDataPath))
        {
            Directory.CreateDirectory(appDataPath);
        }

        // Check if database exists, if not create it
        if (!File.Exists(dbPath))
        {
            System.Data.SQLite.SQLiteConnection.CreateFile(dbPath);
        }

        // Create tables and seed data
        using (var connection = new System.Data.SQLite.SQLiteConnection("Data Source=" + dbPath + ";Version=3;"))
        {
            connection.Open();

            // Create User table
            string createUserTable = @"
                CREATE TABLE IF NOT EXISTS User (
                    UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserName TEXT NOT NULL UNIQUE,
                    Password TEXT NOT NULL
                );";

            // Create ConnectionString table
            string createConnectionStringTable = @"
                CREATE TABLE IF NOT EXISTS ConnectionString (
                    ConnectionStringId INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER NOT NULL,
                    ConnectionStringName TEXT NOT NULL,
                    ConnectionStringValue TEXT NOT NULL,
                    Type TEXT NOT NULL CHECK(Type IN ('postgresql', 'mysql', 'sqlserver')),
                    FOREIGN KEY (UserId) REFERENCES User(UserId) ON DELETE CASCADE
                );";

            // Create indexes
            string createUserIndex = "CREATE INDEX IF NOT EXISTS idx_user_username ON User(UserName);";
            string createConnectionStringIndex = "CREATE INDEX IF NOT EXISTS idx_connectionstring_userid ON ConnectionString(UserId);";
            string createTypeIndex = "CREATE INDEX IF NOT EXISTS idx_connectionstring_type ON ConnectionString(Type);";

            using (var command = new System.Data.SQLite.SQLiteCommand(createUserTable, connection))
            {
                command.ExecuteNonQuery();
            }

            using (var command = new System.Data.SQLite.SQLiteCommand(createConnectionStringTable, connection))
            {
                command.ExecuteNonQuery();
            }

            using (var command = new System.Data.SQLite.SQLiteCommand(createUserIndex, connection))
            {
                command.ExecuteNonQuery();
            }

            using (var command = new System.Data.SQLite.SQLiteCommand(createConnectionStringIndex, connection))
            {
                command.ExecuteNonQuery();
            }

            using (var command = new System.Data.SQLite.SQLiteCommand(createTypeIndex, connection))
            {
                command.ExecuteNonQuery();
            }

            // Seed admin user if not exists
            string checkAdminUser = "SELECT COUNT(*) FROM User WHERE UserName = 'admin';";
            using (var command = new System.Data.SQLite.SQLiteCommand(checkAdminUser, connection))
            {
                long count = (long)command.ExecuteScalar();
                if (count == 0)
                {
                    string insertAdminUser = "INSERT INTO User (UserName, Password) VALUES ('admin', 'admin');";
                    using (var insertCommand = new System.Data.SQLite.SQLiteCommand(insertAdminUser, connection))
                    {
                        insertCommand.ExecuteNonQuery();
                    }
                }
            }
        }
    }

    public static List<User> GetUsers()
    {
        List<User> users = new List<User>();
        string dbPath = DatabasePath;

        if (!File.Exists(dbPath))
        {
            InitializeDatabase();
        }

        using (var connection = new System.Data.SQLite.SQLiteConnection("Data Source=" + dbPath + ";Version=3;"))
        {
            connection.Open();
            string query = "SELECT UserId, UserName, Password FROM User;";

            using (var command = new System.Data.SQLite.SQLiteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
                            UserName = reader["UserName"].ToString(),
                            Password = reader["Password"].ToString()
                        });
                    }
                }
            }
        }

        return users;
    }

    public static User GetUserByUserName(string userName)
    {
        User user = null;
        string dbPath = DatabasePath;

        if (!File.Exists(dbPath))
        {
            InitializeDatabase();
        }

        using (var connection = new System.Data.SQLite.SQLiteConnection("Data Source=" + dbPath + ";Version=3;"))
        {
            connection.Open();
            string query = "SELECT UserId, UserName, Password FROM User WHERE UserName = @UserName;";

            using (var command = new System.Data.SQLite.SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserName", userName);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
                            UserName = reader["UserName"].ToString(),
                            Password = reader["Password"].ToString()
                        };
                    }
                }
            }
        }

        return user;
    }

    public static void DeleteConnectionString(int connectionStringId)
    {
        string dbPath = DatabasePath;

        if (!File.Exists(dbPath))
        {
            InitializeDatabase();
        }

        using (var connection = new System.Data.SQLite.SQLiteConnection("Data Source=" + dbPath + ";Version=3;"))
        {
            connection.Open();

            string query = "DELETE FROM ConnectionString WHERE ConnectionStringId = @ConnectionStringId;";

            using (var command = new System.Data.SQLite.SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ConnectionStringId", connectionStringId);
                command.ExecuteNonQuery();
            }
        }
    }

    public static List<ConnectionString> GetConnectionStrings()
    {
        List<ConnectionString> connectionStrings = new List<ConnectionString>();
        string dbPath = DatabasePath;

        if (!File.Exists(dbPath))
        {
            InitializeDatabase();
        }

        using (var connection = new System.Data.SQLite.SQLiteConnection("Data Source=" + dbPath + ";Version=3;"))
        {
            connection.Open();
            string query = @"
                SELECT cs.ConnectionStringId, cs.UserId, cs.ConnectionStringName,
                       cs.ConnectionStringValue, cs.Type
                FROM ConnectionString cs;";

            using (var command = new System.Data.SQLite.SQLiteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        connectionStrings.Add(new ConnectionString
                        {
                            ConnectionStringId = Convert.ToInt32(reader["ConnectionStringId"]),
                            UserId = Convert.ToInt32(reader["UserId"]),
                            ConnectionStringName = reader["ConnectionStringName"].ToString(),
                            ConnectionStringValue = reader["ConnectionStringValue"].ToString(),
                            Type = reader["Type"].ToString()
                        });
                    }
                }
            }
        }

        return connectionStrings;
    }

    public static List<ConnectionString> GetConnectionStringsByUserId(int userId)
    {
        List<ConnectionString> connectionStrings = new List<ConnectionString>();
        string dbPath = DatabasePath;

        if (!File.Exists(dbPath))
        {
            InitializeDatabase();
        }

        using (var connection = new System.Data.SQLite.SQLiteConnection("Data Source=" + dbPath + ";Version=3;"))
        {
            connection.Open();
            string query = @"
                SELECT cs.ConnectionStringId, cs.UserId, cs.ConnectionStringName,
                       cs.ConnectionStringValue, cs.Type
                FROM ConnectionString cs
                WHERE cs.UserId = @UserId;";

            using (var command = new System.Data.SQLite.SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        connectionStrings.Add(new ConnectionString
                        {
                            ConnectionStringId = Convert.ToInt32(reader["ConnectionStringId"]),
                            UserId = Convert.ToInt32(reader["UserId"]),
                            ConnectionStringName = reader["ConnectionStringName"].ToString(),
                            ConnectionStringValue = reader["ConnectionStringValue"].ToString(),
                            Type = reader["Type"].ToString()
                        });
                    }
                }
            }
        }

        return connectionStrings;
    }

    public static void SaveUser(User user)
    {
        string dbPath = DatabasePath;

        if (!File.Exists(dbPath))
        {
            InitializeDatabase();
        }

        using (var connection = new System.Data.SQLite.SQLiteConnection("Data Source=" + dbPath + ";Version=3;"))
        {
            connection.Open();

            if (user.UserId == 0)
            {
                // Insert new user
                string query = "INSERT INTO User (UserName, Password) VALUES (@UserName, @Password);";
                using (var command = new System.Data.SQLite.SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", user.UserName);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                // Update existing user
                string query = "UPDATE User SET UserName = @UserName, Password = @Password WHERE UserId = @UserId;";
                using (var command = new System.Data.SQLite.SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", user.UserName);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@UserId", user.UserId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    public static void DeleteUser(string userName)
    {
        string dbPath = DatabasePath;

        if (!File.Exists(dbPath))
        {
            InitializeDatabase();
        }

        using (var connection = new System.Data.SQLite.SQLiteConnection("Data Source=" + dbPath + ";Version=3;"))
        {
            connection.Open();

            string query = "DELETE FROM User WHERE UserName = @UserName;";

            using (var command = new System.Data.SQLite.SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserName", userName);
                command.ExecuteNonQuery();
            }
        }
    }

    public static void SaveConnectionString(ConnectionString connectionString)
    {
        string dbPath = DatabasePath;

        if (!File.Exists(dbPath))
        {
            InitializeDatabase();
        }

        using (var connection = new System.Data.SQLite.SQLiteConnection("Data Source=" + dbPath + ";Version=3;"))
        {
            connection.Open();

            if (connectionString.ConnectionStringId == 0)
            {
                // Insert new connection string
                string query = @"
                    INSERT INTO ConnectionString
                    (UserId, ConnectionStringName, ConnectionStringValue, Type)
                    VALUES (@UserId, @ConnectionStringName, @ConnectionStringValue, @Type);";

                using (var command = new System.Data.SQLite.SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", connectionString.UserId);
                    command.Parameters.AddWithValue("@ConnectionStringName", connectionString.ConnectionStringName);
                    command.Parameters.AddWithValue("@ConnectionStringValue", connectionString.ConnectionStringValue);
                    command.Parameters.AddWithValue("@Type", connectionString.Type);
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                // Update existing connection string
                string query = @"
                    UPDATE ConnectionString
                    SET UserId = @UserId, ConnectionStringName = @ConnectionStringName,
                        ConnectionStringValue = @ConnectionStringValue, Type = @Type
                    WHERE ConnectionStringId = @ConnectionStringId;";

                using (var command = new System.Data.SQLite.SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", connectionString.UserId);
                    command.Parameters.AddWithValue("@ConnectionStringName", connectionString.ConnectionStringName);
                    command.Parameters.AddWithValue("@ConnectionStringValue", connectionString.ConnectionStringValue);
                    command.Parameters.AddWithValue("@Type", connectionString.Type);
                    command.Parameters.AddWithValue("@ConnectionStringId", connectionString.ConnectionStringId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}