
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

/// <summary>
/// Manages application configuration for users and connection strings.
/// </summary>
public static class ConfigManager
{
    public static User[] Users
    {
        get
        {
            string fileName = $"{typeof(User).Name}s.xml";
            string relativePath = $"~/App_Data/config/{fileName}";
            string filePath = HttpContext.Current.Server.MapPath(relativePath);
            User[] result = null;

            if (File.Exists(filePath) && new FileInfo(filePath).Length > 0)
            {
                try
                {
                    XmlSerializer xs = new XmlSerializer(typeof(User[]));
                    using (TextReader tx = new StreamReader(filePath))
                    {
                        result = (User[])xs.Deserialize(tx);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error deserializing users: " + ex.Message);
                    result = new User[0];
                }
            }

            // اگر فایل وجود نداشت یا هیچ کاربری در آن نبود، کاربر ادمین را ایجاد کن
            if (result == null || result.Length == 0)
            {
                var adminUser = new User { UserName = "admin", Password = "admin" };
                var userList = new List<User> { adminUser };
                SaveConfigs<User>(userList);
                return userList.ToArray();
            }

            return result;
        }
    }

    public static ConnectionString[] ConnectionStrings
    {
        get
        {
            string fileName = $"{typeof(ConnectionString).Name}s.xml";
            string relativePath = $"~/App_Data/config/{fileName}";
            string filePath = HttpContext.Current.Server.MapPath(relativePath);

            if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0)
            {
                return new ConnectionString[0];
            }

            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(ConnectionString[]));
                using (TextReader tx = new StreamReader(filePath))
                {
                    return (ConnectionString[])xs.Deserialize(tx) ?? new ConnectionString[0];
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error deserializing connection strings: " + ex.Message);
                return new ConnectionString[0];
            }
        }
    }

    public static void SaveConfigs<T>(List<T> list)
    {
        string fileName;
        string typeName = typeof(T).Name;


        fileName = $"{typeName}s.xml";
        string relativePath = $"~/App_Data/config/{fileName}";
        string filePath = HttpContext.Current.Server.MapPath(relativePath);

        // اطمینان از وجود دایرکتوری
        string dir = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        // برای سازگاری با نحوه خواندن که آرایه است، به صورت آرایه ذخیره می‌کنیم
        XmlSerializer xs = new XmlSerializer(typeof(T[]));
        using (TextWriter tx = new StreamWriter(filePath))
        {
            xs.Serialize(tx, list.ToArray());
        }
    }
}
