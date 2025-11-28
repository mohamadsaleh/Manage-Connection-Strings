using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

/// <summary>
/// Manages application configuration, including users.
/// </summary>
public static class ConfigManager
{
    public static User[] Users
    {
        get
        {
            string filePath = HttpContext.Current.Server.MapPath("~/App_Data/config/users.xml");
            User[] result = null;

            if (File.Exists(filePath))
            {
                try
                {
                    // جلوگیری از خطا برای فایل خالی
                    if (new FileInfo(filePath).Length == 0)
                    {
                        result = new User[0];
                    }
                    else
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(User[]));
                        using (TextReader tx = new StreamReader(filePath))
                        {
                            result = (User[])xs.Deserialize(tx);
                        }
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

    public static void SaveConfigs<T>(List<T> list)
    {
        string filePath = HttpContext.Current.Server.MapPath("~/App_Data/config/users.xml");
        
        // اطمینان از وجود دایرکتوری
        string dir = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        XmlSerializer xs = new XmlSerializer(typeof(List<T>));
        using (TextWriter tx = new StreamWriter(filePath))
        {
            xs.Serialize(tx, list);
        }
    }
}