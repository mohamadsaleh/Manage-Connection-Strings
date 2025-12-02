<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="SqlServer.aspx.cs" Inherits="SqlServer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta charset="utf-8" />
    <title>My Database Tools</title>
    <link href="style.css" rel="stylesheet" />
    <script type="text/javascript">
        function SButtonQueryClear() {
            var textboxQueyId = document.getElementById('<%= TextBoxQuery.ClientID %>');
            textboxQueyId.value = "";
        }
        function SButtonSelectCountStar(clickedButton) {
            const row = clickedButton.closest('tr');
            if (!row) {
                alert("Error: Row not found.");
                return;
            }
            const nameCellIndex = 2;
            const nameCell = row.cells[nameCellIndex];
            const idCellIndex = 0;
            const idCell = row.cells[idCellIndex];

            let tableName = "N/A";


            if (nameCell) {
                tableName = nameCell.innerText.trim();
            }

            var textboxQueyId = document.getElementById('<%= TextBoxQuery.ClientID %>');
            textboxQueyId.value += `--------------------       
SELECT Count(*) FROM ${tableName}`;
        }

        function SButtonSelectStar(clickedButton) {
            const row = clickedButton.closest('tr');
            if (!row) {
                alert("Error: Row not found.");
                return;
            }
            const nameCellIndex = 2;
            const nameCell = row.cells[nameCellIndex];
            const idCellIndex = 0;
            const idCell = row.cells[idCellIndex];

            let tableName = "N/A";


            if (nameCell) {
                tableName = nameCell.innerText.trim();
            }

            var textboxQueyId = document.getElementById('<%= TextBoxQuery.ClientID %>');
            textboxQueyId.value += `--------------------       
SELECT * FROM ${tableName}`;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <div style="width: 100%">
            &nbsp;
                Connection String Name: 
                <asp:TextBox ID="TextBoxConnectionStringName" runat="server"></asp:TextBox>
            &nbsp;
                Connection String Value: 
                <asp:TextBox ID="TextBoxConnectionStringValue" runat="server"></asp:TextBox>
            &nbsp;
                <asp:Button ID="ButtonAddConnectionString" runat="server" CssClass="btnPrimary" Text="Add" OnClick="ButtonAddConnectionString_Click" />
        </div>
        <div style="float: left; width: 350px; border: 1px solid #ccc; width: 350px; padding: 3px; overflow: scroll;">
            <div class="headerSection">Actions</div>
            <asp:Button ID="ButtonDeleteSelectedCon" runat="server" CssClass="btnDanger" OnClick="ButtonDeleteSelectedCon_Click" Text="Delete Selected Connection" />

            <asp:Button ID="ButtonTestConnection" runat="server" CssClass="btnPrimary" OnClick="ButtonTestConnection_Click" Text="Test Connection String" />
            <br />
            <asp:Button ID="ButtonMoveUp" runat="server" Text="▲ Up" CssClass="btnSecondary"
                OnClick="ButtonMoveUp_Click" Width="80px" />
            <asp:Button ID="ButtonMoveDown" runat="server" Text="▼ Down" CssClass="btnSecondary"
                OnClick="ButtonMoveDown_Click" Width="80px" />
            <br />
            <div class="headerSection">Connection Strings</div>
            <asp:ListBox ID="ListBoxConnectionStrings" runat="server" Width="100%" AutoPostBack="True"></asp:ListBox>
            <asp:Button ID="ButtonShowTables" CssClass="btnPrimary" runat="server" OnClick="ButtonShowTables_Click" Text="Show Tables" />
            <asp:GridView ID="GridViewTables" runat="server" OnRowCommand="GridViewTables_RowCommand"
                CssClass="gridViewStyle">
                <Columns>
                    <%--<asp:CommandField HeaderText="Show Columns" SelectText="Show Columns" ShowHeader="True" ShowSelectButton="True" />--%>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButtonGetColumns" runat="server"
                                Text="Schema" CssClass="btnPrimary btnInGrid"
                                CommandName="GetColumns"
                                CommandArgument='<%# Container.DataItemIndex %>' />
                            <asp:LinkButton ID="LinkButtonGenerateInsert" runat="server"
                                Text="Insert" CssClass="btnPrimary btnInGrid"
                                CommandName="GenerateInsert"
                                CommandArgument='<%# Container.DataItemIndex %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <input type="button" class="btnSecondary btnInGrid" onclick="SButtonSelectCountStar(this)" value="Count*" />
                            <input type="button" class="btnSecondary btnInGrid" onclick="SButtonSelectStar(this)" value="Select*" />

                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div style="float: left; border: 1px solid #ccc; width: calc(100% - 380px); margin-left:5px; padding: 5px; ">
            Select Connection String and Click Show Tables:
                <br />

            <asp:Label ID="LabelResult" runat="server"></asp:Label>
            <br />
            <asp:GridView ID="GridViewTableColumns" runat="server" CssClass="gridViewStyle">
            </asp:GridView>
            <div style="width: 80%; float: left">
                <asp:TextBox ID="TextBoxQuery" runat="server" Width="100%" Height="80px" TextMode="MultiLine"></asp:TextBox>
            </div>
            <div style="width: 20%; float: left">
                <input id="ButtonQueryClear" class="btnDanger" type="button" onclick="SButtonQueryClear()" value=" Clear TextBox" />

            </div>
            <div style="clear: both">
                <asp:Label ID="LabelQueryResult" runat="server"></asp:Label>
            </div>
            <div>
                <asp:Button ID="ButtonNonQuery" CssClass="btnPrimary" runat="server" Text="Execute None Query" OnClick="ButtonNonQuery_Click" />
                <asp:Button ID="ButtonSqaler" CssClass="btnPrimary" runat="server" Text="Execute Squaler" OnClick="ButtonSqaler_Click" />
                <asp:Button ID="ButtonExecuteReader" CssClass="btnPrimary" runat="server" OnClick="ButtonExecuteReader_Click" Text="Execute Reader" />
            </div>
            <asp:GridView ID="GridViewResults" runat="server" OnRowCommand="GridViewData_RowCommand"
                CssClass="gridViewStyle">
                <Columns>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>