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
        if (Session["user"] == null)
            Response.Redirect("Login.aspx");
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        // Load users from SQLite database
        var userList = SqliteHelper.GetUsers();
        GridViewUsers.DataSource = userList;
        GridViewUsers.DataBind();

        // Populate dropdown list for password change
        DropDownListUsers.Items.Clear();
        foreach (var user in userList)
        {
            DropDownListUsers.Items.Add(new ListItem(user.UserName, user.UserName));
        }
    }

    protected void ButtonAddUser_Click(object sender, EventArgs e)
    {
        // Create new user
        User newUser = new User
        {
            UserName = TextBoxUserName.Text,
            Password = TextBoxPassWord.Text
        };

        // Save to SQLite database
        SqliteHelper.SaveUser(newUser);

        // Clear TextBox fields
        TextBoxUserName.Text = "";
        TextBoxPassWord.Text = "";

        // Refresh the GridView
        Page_PreRender(sender, e);
    }

    protected void GridViewUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        // Get username from the data key of the selected row
        string userNameToDelete = GridViewUsers.DataKeys[e.RowIndex].Value.ToString();

        // Delete user from SQLite database
        SqliteHelper.DeleteUser(userNameToDelete);

        // Refresh the GridView
        Page_PreRender(sender, e);
    }

    protected void ButtonChangePassword_Click(object sender, EventArgs e)
    {
        // Clear previous messages
        LabelChangePasswordMessage.Text = "";
        LabelChangePasswordMessage.ForeColor = System.Drawing.Color.Red;

        // Validate inputs
        if (string.IsNullOrEmpty(DropDownListUsers.SelectedValue))
        {
            LabelChangePasswordMessage.Text = "Please select a user.";
            return;
        }

        if (string.IsNullOrEmpty(TextBoxNewPassword.Text))
        {
            LabelChangePasswordMessage.Text = "Please enter a new password.";
            return;
        }

        // Change password directly
        SqliteHelper.ChangePassword(
            DropDownListUsers.SelectedValue,
            TextBoxNewPassword.Text
        );

        LabelChangePasswordMessage.ForeColor = System.Drawing.Color.Green;
        LabelChangePasswordMessage.Text = "Password changed successfully!";

        // Clear form field
        TextBoxNewPassword.Text = "";
    }
}