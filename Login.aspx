<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="style.css" rel="stylesheet" />
    <title>Login to Use Connection String Manager</title>
</head>
<body>
    <form id="form1" runat="server">
        <h1 style="text-align:center; margin:10px auto; padding:20px">Login to Use Connection String Manager</h1>
        <div style="width:350px; margin:20px auto; border:1px solid #ccc; padding:20px">
            UserName:
            <br />
            <asp:TextBox ID="TextBoxuserName" Width="100%" runat="server"></asp:TextBox>
            <br />
            Password:
            <br />
            <asp:TextBox ID="TextBoxPassword" Width="100%" runat="server" TextMode="Password"></asp:TextBox>
            <br />
            <asp:Button ID="ButtonLogin" CssClass="btnPrimary" runat="server" Text="Login" OnClick="ButtonLogin_Click" />
        </div>
    </form>
</body>
</html>
