using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

public partial class ConnectionStrings : System.Web.UI.Page
{
    private int CurrentUserId
    {
        get
        {
            if (Session["user"] == null)
                return 0;
            string userName = Session["user"].ToString();
            User user = SqliteHelper.GetUserByUserName(userName);
            return user != null ? user.UserId : 0;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["user"] == null)
        {
            Response.Redirect("Login.aspx");
        }

        if (!IsPostBack)
        {
            BindGrid();
        }
    }

    private void BindGrid()
    {
        int userId = CurrentUserId;
        if (userId > 0)
        {
            List<ConnectionString> connectionStrings = SqliteHelper.GetConnectionStringsByUserId(userId);
            GridViewConnectionStrings.DataSource = connectionStrings;
            GridViewConnectionStrings.DataBind();
        }
    }

    protected void ButtonAddConnection_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(TextBoxName.Text) || string.IsNullOrEmpty(TextBoxValue.Text) || string.IsNullOrEmpty(DropDownListType.SelectedValue))
        {
            Response.Write("<script>alert('Please fill all fields');</script>");
            return;
        }

        ConnectionString cs = new ConnectionString
        {
            ConnectionStringId = 0, // New
            UserId = CurrentUserId,
            ConnectionStringName = TextBoxName.Text,
            ConnectionStringValue = TextBoxValue.Text,
            Type = DropDownListType.SelectedValue
        };

        SqliteHelper.SaveConnectionString(cs);

        // Clear fields
        TextBoxName.Text = "";
        TextBoxValue.Text = "";
        DropDownListType.SelectedIndex = 0;

        BindGrid();
    }

    protected void GridViewConnectionStrings_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteRow")
        {
            int connectionStringId = Convert.ToInt32(e.CommandArgument);
            SqliteHelper.DeleteConnectionString(connectionStringId);
            BindGrid();
        }
    }
}