using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class main : System.Web.UI.MasterPage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if(Session["user"] != null)
            LinkButtonLogout.Text = "Logout User: " + Session["user"].ToString();
        else 
            Response.Redirect("Login.aspx");
    }

    protected void LinkButtonLogout_Click(object sender, EventArgs e)
    {
        Session["user"] = null;
        Response.Redirect("Login.aspx");
    }
}
