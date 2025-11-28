using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Users : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["user"] is null)
            Response.Redirect("Login.aspx");
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        // اطمینان از اینکه منبع داده null نیست
        var userList = ConfigManager.Users?.ToList() ?? new List<User>();
        GridViewUsers.DataSource = userList;
        GridViewUsers.DataBind();
    }
    protected void ButtonAddUser_Click(object sender, EventArgs e)
    {
        // 1. دریافت لیست کاربران موجود
        List<User> currentUsers = ConfigManager.Users?.ToList() ?? new List<User>();

        // 2. ایجاد و افزودن کاربر جدید
        User newUser = new User
        {
            UserName = TextBoxUserName.Text,
            Password = TextBoxPassWord.Text
        };
        currentUsers.Add(newUser);

        // 3. ذخیره لیست به‌روز شده
        ConfigManager.SaveConfigs<User>(currentUsers);

        // پاک کردن TextBoxها پس از افزودن
        TextBoxUserName.Text = "";
        TextBoxPassWord.Text = "";
    }

    protected void GridViewUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        // 1. دریافت نام کاربری از کلید داده ردیف انتخاب شده
        string userNameToDelete = GridViewUsers.DataKeys[e.RowIndex].Value.ToString();

        // 2. دریافت لیست کاربران
        List<User> currentUsers = ConfigManager.Users?.ToList() ?? new List<User>();

        // 3. پیدا کردن و حذف کاربر مورد نظر
        User userToRemove = currentUsers.FirstOrDefault(u => u.UserName == userNameToDelete);
        if (userToRemove != null)
        {
            currentUsers.Remove(userToRemove);
        }

        // 4. ذخیره لیست به‌روز شده
        ConfigManager.SaveConfigs<User>(currentUsers);
    }
}