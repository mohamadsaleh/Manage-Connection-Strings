<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ConnectionStrings.aspx.cs" Inherits="ConnectionStrings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>My Connection Strings</title>
    <link href="style.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="padding: 5px 25px;">
        <h1>My ConnectionStrings</h1>
        <div>
            Connection String Name:
        <asp:TextBox ID="TextBoxName" runat="server" Width="60%"></asp:TextBox>
            <br />
            Connection String Value:
        <asp:TextBox ID="TextBoxValue" runat="server" Width="60%"></asp:TextBox>
            <br />
            Type:
            <asp:DropDownList ID="DropDownListType" CssClass="btnPrimary" runat="server">
                <asp:ListItem Text="Select Type" Value="" Selected="True"></asp:ListItem>
                <asp:ListItem Text="PostgreSQL" Value="postgresql"></asp:ListItem>
                <asp:ListItem Text="MySQL" Value="mysql"></asp:ListItem>
                <asp:ListItem Text="SQL Server" Value="sqlserver"></asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:Button ID="ButtonAddConnection" runat="server" CssClass="btnPrimary" Text="Add" OnClick="ButtonAddConnection_Click" />
            <asp:GridView ID="GridViewConnectionStrings" CssClass="gridViewStyle" runat="server" AutoGenerateColumns="False" DataKeyNames="ConnectionStringId" OnRowCommand="GridViewConnectionStrings_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButtonDelete" runat="server" Text="Delete" CommandName="DeleteRow" CommandArgument='<%# Eval("ConnectionStringId") %>' CssClass="btnDanger btnInGrid" OnClientClick="return confirm('Are you sure you want to delete this connection string?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ConnectionStringName" ItemStyle-Width="120px" HeaderText="Name" />
                    <asp:BoundField DataField="Type" HeaderText="Type" />
                    <asp:BoundField DataField="ConnectionStringValue" ItemStyle-Font-Size="12px" HeaderText="Value" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

