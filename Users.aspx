<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Users.aspx.cs" Inherits="Users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="style.css" rel="stylesheet" />
    <title>Manage Users</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="margin: 20px 20px;">
        <h1>Mange Users</h1>
        <br />
        User Name:
            <br />
        <asp:TextBox ID="TextBoxUserName" runat="server"></asp:TextBox>
        <br />
        Password:
            <br />
        <asp:TextBox ID="TextBoxPassWord" runat="server" TextMode="Password"></asp:TextBox>
        <br />
        <asp:Button ID="ButtonAddUser" CssClass="btnPrimary" runat="server" Text="Add User" OnClick="ButtonAddUser_Click" />
        <br />
        <asp:GridView ID="GridViewUsers" AutoGenerateColumns="True" CssClass="gridViewStyle" runat="server" DataKeyNames="UserName" OnRowDeleting="GridViewUsers_RowDeleting">
            <Columns>
                <asp:CommandField ShowDeleteButton="True" ControlStyle-CssClass="btnDanger" ButtonType="Button" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

