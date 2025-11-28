using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["user"] != null)
            Response.Redirect("Connections.aspx");
    }

    protected void ButtonLogin_Click(object sender, EventArgs e)
    {
        List<User> users = ConfigManager.Users.ToList<User>();
        foreach (var item in users)
        {
            if(item.UserName==TextBoxuserName.Text && item.Password==TextBoxPassword.Text)
            {
                Session.Add("user", TextBoxuserName.Text);
                Response.Redirect("Connections.aspx");

            }
        }

    }
}