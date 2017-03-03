<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CharityKitchenWebDatabase.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Charity Kitchen Login</title>
</head>
<body>
    <form id="form1" runat="server" style="padding-top: 20%;">
        <div>
            <asp:ScriptManager runat="server"/>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <center>
                        <div style="background-color:#e4e4e4; border:2px solid #808080; width:250px; height:110px; padding-top:30px;">
                            <table>
                                <tr>
                                    <td>Username:</td>
                                    <td><asp:TextBox ID="txtUsername" runat="server" width="100%"/></td>
                                </tr>
                                <tr>
                                    <td>Password:</td>
                                    <td><asp:TextBox ID="txtPassword" runat="server" TextMode="Password" width="100%"/></td>
                                </tr>
                                <tr>
                                    <td><asp:Button ID="btnLogin" Text="Login" runat="server" OnClick="btnLogin_Click" /></td>
                                    <td><asp:Label ID="lblInfo" Text="Enter user credentials" runat="server" /></td>
                                </tr>
                            </table>
                        </div>
                    </center>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
