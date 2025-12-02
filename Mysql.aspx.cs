using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class Mysql : System.Web.UI.Page
{

    protected void ButtonTestConnection_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ListBoxConnectionStrings.SelectedValue))
        {
            int connectionStringId = Convert.ToInt32(ListBoxConnectionStrings.SelectedValue);
            ConnectionString selectedCon = SqliteHelper.GetConnectionStrings()
                .FirstOrDefault(c => c.ConnectionStringId == connectionStringId);

            if (selectedCon != null && TestDatabaseConnection(selectedCon.ConnectionStringValue))
            {
                LabelResult.Text = "✅ SUCCESS: Database connection is open and active!";
            }
            else
            {
                LabelResult.Text = "❌ FAILURE: Could not open database connection.";
            }
        }
    }
    private bool TestDatabaseConnection(string connectionString)
    {
        using (MySql.Data.MySqlClient.MySqlConnection connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    return true;
                }
                return false;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Response.Write("⚠️ **MYSQL ERROR:** " + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Response.Write("⚠️ **GENERAL ERROR:** " + ex.Message);
                return false;
            }
        }
    }



    protected void ButtonExecuteReader_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ListBoxConnectionStrings.SelectedValue))
        {
            int connectionStringId = Convert.ToInt32(ListBoxConnectionStrings.SelectedValue);
            ConnectionString selectedCon = SqliteHelper.GetConnectionStrings()
                .FirstOrDefault(c => c.ConnectionStringId == connectionStringId);

            if (selectedCon != null)
            {
                GridViewResults.DataSource = MysqlDbHelper.GetDataWithAdapter(selectedCon.ConnectionStringValue, TextBoxQuery.Text);
                GridViewResults.DataBind();
            }
        }
    }

    protected void ButtonSqaler_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ListBoxConnectionStrings.SelectedValue))
        {
            int connectionStringId = Convert.ToInt32(ListBoxConnectionStrings.SelectedValue);
            ConnectionString selectedCon = SqliteHelper.GetConnectionStrings()
                .FirstOrDefault(c => c.ConnectionStringId == connectionStringId);

            if (selectedCon != null)
            {
                LabelQueryResult.Text = "Result: " + MysqlDbHelper.GetStringWithAdapter(selectedCon.ConnectionStringValue, TextBoxQuery.Text);
            }
        }
    }



    protected void GridViewData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        // 1. Check if the command name is the one we want to handle
        if (e.CommandName == "MySelectCommand")
        {
            // 2. Get the Row Index: The CommandArgument holds the index (0-based)
            int rowIndex;
            if (int.TryParse(e.CommandArgument.ToString(), out rowIndex))
            {
                // 3. Get the specific row object from the GridView using the index
                GridViewRow row = GridViewResults.Rows[rowIndex];

                // 4. Access the Text from the desired column (Index 2)
                // GridView cells are 0-based, so the 3rd column is at index [2].

                // Check if the cell exists (optional but safe)
                if (row.Cells.Count > 2)
                {
                    // The Text property gets the rendered text content of the cell.
                    string descriptionText = row.Cells[2].Text;

                    // 5. Assign the extracted text to the Label control
                    LabelQueryResult.Text = "Selected Description for Row " + (rowIndex + 1) + ": **" + descriptionText + "**";
                    //string SelectTabalesColum
                }
                else
                {
                    LabelQueryResult.Text = "Error: Column index 2 (third column) not found.";
                }
            }
            else
            {
                LabelQueryResult.Text = "Error: Invalid Row Index in CommandArgument.";
            }
        }
    }

    protected void ButtonShowTables_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ListBoxConnectionStrings.SelectedValue))
        {
            int connectionStringId = Convert.ToInt32(ListBoxConnectionStrings.SelectedValue);
            ConnectionString selectedCon = SqliteHelper.GetConnectionStrings()
                .FirstOrDefault(c => c.ConnectionStringId == connectionStringId);

            if (selectedCon != null)
            {
                string showTablesCmd = "SHOW TABLES";
                GridViewTables.DataSource = MysqlDbHelper.GetDataWithAdapter(selectedCon.ConnectionStringValue, showTablesCmd);
                GridViewTables.DataBind();
            }
        }
    }

    protected void GridViewTables_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "GetColumns")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);

            // 3. دریافت شیء GridViewRow
            GridViewRow row = GridViewTables.Rows[rowIndex];

            if (row.Cells.Count > 2)
            {
                // .Text متن رندر شده داخل سلول را برمی‌گرداند.
                string tableName = row.Cells[2].Text;
                string getColumnsCmd = "DESCRIBE " + tableName;
                if (!string.IsNullOrEmpty(ListBoxConnectionStrings.SelectedValue))
                {
                    int connectionStringId = Convert.ToInt32(ListBoxConnectionStrings.SelectedValue);
                    ConnectionString selectedCon = SqliteHelper.GetConnectionStrings()
                        .FirstOrDefault(c => c.ConnectionStringId == connectionStringId);

                    if (selectedCon != null)
                    {
                        GridViewTableColumns.DataSource = MysqlDbHelper.GetDataWithAdapter(selectedCon.ConnectionStringValue, getColumnsCmd);
                        GridViewTableColumns.DataBind();
                    }
                }
                GridViewTableColumns.DataBind();
                // 5. چاپ متن استخراج شده در Label
                LabelResult.Text = "Selected Table: **" + tableName + "** (Row Index: " + rowIndex + ")";
            }
            else
            {
                LabelResult.Text = "Error: Column index 2 (Table Name) not found in the row.";
            }
        }
        else if (e.CommandName == "GenerateInsert")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);

            // 3. دریافت شیء GridViewRow
            GridViewRow row = GridViewTables.Rows[rowIndex];

            if (row.Cells.Count > 2)
            {
                // .Text متن رندر شده داخل سلول را برمی‌گرداند.
                string tableName = row.Cells[2].Text;
                // نام جدول مقصد
                string TargetTableName = row.Cells[2].Text;

                // متغیرها برای نگهداری نام ستون‌ها و مقادیر (با فرض درج مقادیر NULL)
                StringBuilder columnNames = new StringBuilder();
                StringBuilder valuePlaceholders = new StringBuilder();

                // پرچم برای مدیریت کاما (,)
                bool isFirstColumn = true;

                // پیمایش روی تمام سطرهای داده در GridView
                // Rows.Count شامل Header نمی‌شود.
                foreach (GridViewRow rowItem in GridViewTableColumns.Rows)
                {
                    // اطمینان از اینکه سطر از نوع داده است
                    if (rowItem.RowType == DataControlRowType.DataRow)
                    {
                        // Column 0 (ستون اول) حاوی نام ستون است.
                        // Check if the cell exists (optional but safe)
                        if (rowItem.Cells.Count > 0)
                        {
                            // استخراج نام ستون از ستون اول (اندیس 0)
                            string columnName = rowItem.Cells[0].Text.Trim();

                            // اگر ستون نامعتبر یا خالی نبود
                            if (!string.IsNullOrEmpty(columnName) && !columnName.Equals("&nbsp;", StringComparison.OrdinalIgnoreCase))
                            {
                                if (!isFirstColumn)
                                {
                                    columnNames.Append(", ");
                                    valuePlaceholders.Append(", ");
                                }

                                // ساخت قسمت نام ستون‌ها: [ColumnName]
                                columnNames.Append("[" + columnName + "]");

                                // ساخت قسمت مقادیر: NULL (یا @ParameterName)
                                // در این مثال ساده، از NULL استفاده می‌کنیم.
                                valuePlaceholders.Append("NULL");

                                isFirstColumn = false;
                            }
                        }
                    }
                }

                // ساخت دستور INSERT نهایی
                if (columnNames.Length > 0)
                {
                    string insertStatement = "INSERT INTO " + TargetTableName + " (" + columnNames + ") VALUES (" + valuePlaceholders + ");";
                    TextBoxQuery.Text += @"---------------------
" + insertStatement;
                }
                else
                {
                    LabelResult.Text = "No valid column names were found in the GridView.";
                }
            }
            else
            {
                LabelResult.Text = "Error: Column index 2 (Table Name) not found in the row.";
            }
        }
    }



    protected void ButtonNonQuery_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ListBoxConnectionStrings.SelectedValue))
        {
            int connectionStringId = Convert.ToInt32(ListBoxConnectionStrings.SelectedValue);
            ConnectionString selectedCon = SqliteHelper.GetConnectionStrings()
                .FirstOrDefault(c => c.ConnectionStringId == connectionStringId);

            if (selectedCon != null)
            {
                LabelQueryResult.Text = MysqlDbHelper.GetNoneWithAdapter(selectedCon.ConnectionStringValue, TextBoxQuery.Text);
            }
        }
    }

    protected void ButtonAddConnectionString_Click(object sender, EventArgs e)
    {
        // Get current user
        string userName = Session["user"].ToString();
        User currentUser = SqliteHelper.GetUserByUserName(userName);

        if (currentUser != null)
        {
            ConnectionString newCon = new ConnectionString();
            newCon.ConnectionStringName = TextBoxConnectionStringName.Text;
            newCon.ConnectionStringValue = TextBoxConnectionStringValue.Text;
            newCon.Type = "mysql";
            newCon.UserId = currentUser.UserId;

            SqliteHelper.SaveConnectionString(newCon);

            // Refresh the list
            List<ConnectionString> cons = SqliteHelper.GetConnectionStringsByUserId(currentUser.UserId)
                .Where(c => c.Type == "mysql").ToList();
            BindConnectionStringsToListBox(cons);
        }

        TextBoxConnectionStringName.Text = "";
        TextBoxConnectionStringValue.Text = "";
    }
    private void BindConnectionStringsToListBox(List<ConnectionString> cons)
    {
        ListBoxConnectionStrings.Items.Clear();

        foreach (ConnectionString item in cons)
        {
            ListBoxConnectionStrings.Items.Add(new ListItem(item.ConnectionStringName, item.ConnectionStringId.ToString()));
        }
    }
    protected void ButtonDeleteSelectedCon_Click(object sender, EventArgs e)
    {
        if (ListBoxConnectionStrings.SelectedItem == null)
        {
            return;
        }

        int connectionStringId = Convert.ToInt32(ListBoxConnectionStrings.SelectedValue);
        SqliteHelper.DeleteConnectionString(connectionStringId);

        // Get current user and refresh the list
        string userName = Session["user"].ToString();
        User currentUser = SqliteHelper.GetUserByUserName(userName);
        if (currentUser != null)
        {
            List<ConnectionString> cons = SqliteHelper.GetConnectionStringsByUserId(currentUser.UserId)
                .Where(c => c.Type == "mysql").ToList();
            BindConnectionStringsToListBox(cons);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        ButtonDeleteSelectedCon.Visible = true;
        ButtonMoveUp.Visible = false; // Not relevant for database-stored items
        ButtonMoveDown.Visible = false; // Not relevant for database-stored items
        ButtonShowTables.Visible = true;
        ButtonTestConnection.Visible = true;
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ListBoxConnectionStrings.SelectedValue))
        {
            ButtonDeleteSelectedCon.Visible = false;
            ButtonShowTables.Visible = false;
            ButtonTestConnection.Visible = false;
        }
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["user"] == null)
            Response.Redirect("Login.aspx");
        if (!Page.IsPostBack)
        {
            // Get current user
            string userName = Session["user"].ToString();
            User currentUser = SqliteHelper.GetUserByUserName(userName);

            if (currentUser != null)
            {
                // Get connection strings for current user, filtered by type
                List<ConnectionString> cons = SqliteHelper.GetConnectionStringsByUserId(currentUser.UserId)
                    .Where(c => c.Type == "mysql").ToList();
                BindConnectionStringsToListBox(cons);
            }
        }
    }

    protected void ButtonMoveUp_Click(object sender, EventArgs e)
    {
        if (ListBoxConnectionStrings.SelectedItem == null)
            return;

        int selectedIndex = ListBoxConnectionStrings.SelectedIndex;

        // نمی‌توان آیتم اول (Index 0) را بالاتر برد
        if (selectedIndex > 0)
        {
            // 1. دریافت لیست کامل
            List<ConnectionString> consList = ConfigManager.ConnectionStrings.ToList();

            // 2. جابه‌جایی در لیست C#
            Swap(consList, selectedIndex, selectedIndex - 1);

            // 3. ذخیره مجدد لیست به‌روزرسانی شده در XML
            ConfigManager.SaveConfigs<ConnectionString>(consList);

            // 4. به‌روزرسانی UI
            BindConnectionStringsToListBox(consList);

            // 5. حفظ انتخاب کاربر: آیتم جدید (که پایین آمده) را انتخاب می‌کند
            ListBoxConnectionStrings.SelectedIndex = selectedIndex - 1;
        }
    }

    protected void ButtonMoveDown_Click(object sender, EventArgs e)
    {
        if (ListBoxConnectionStrings.SelectedItem == null)
            return;

        int selectedIndex = ListBoxConnectionStrings.SelectedIndex;

        // نمی‌توان آیتم آخر را پایین‌تر برد
        if (selectedIndex < ListBoxConnectionStrings.Items.Count - 1)
        {
            // 1. دریافت لیست کامل
            List<ConnectionString> consList = ConfigManager.ConnectionStrings.ToList();

            // 2. جابه‌جایی در لیست C#
            Swap(consList, selectedIndex, selectedIndex + 1);

            // 3. ذخیره مجدد لیست به‌روزرسانی شده در XML
            ConfigManager.SaveConfigs<ConnectionString>(consList);

            // 4. به‌روزرسانی UI
            BindConnectionStringsToListBox(consList);

            // 5. حفظ انتخاب کاربر: آیتم جدید (که بالا رفته) را انتخاب می‌کند
            ListBoxConnectionStrings.SelectedIndex = selectedIndex + 1;
        }
    }
    private static void Swap<T>(IList<T> list, int indexA, int indexB)
    {
        T tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }

}