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
        <br />
        <h3>Change Password</h3>
        <asp:Label ID="LabelChangePasswordMessage" runat="server" ForeColor="Red"></asp:Label>
        <br />
        User Name:
        <br />
        <asp:DropDownList ID="DropDownListUsers" runat="server" CssClass="dropdownStyle"></asp:DropDownList>
        <br />
        New Password:
        <br />
        <asp:TextBox ID="TextBoxNewPassword" runat="server" TextMode="Password"></asp:TextBox>
        <br />
        <asp:Button ID="ButtonChangePassword" CssClass="btnSecondary" runat="server" Text="Change Password" OnClick="ButtonChangePassword_Click" />
        <br />
        <br />
        <asp:GridView ID="GridViewUsers" AutoGenerateColumns="True" CssClass="gridViewStyle" runat="server" DataKeyNames="UserName" OnRowDeleting="GridViewUsers_RowDeleting">
            <Columns>
                <asp:CommandField ShowDeleteButton="True" ControlStyle-CssClass="btnDanger" ButtonType="Button" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

