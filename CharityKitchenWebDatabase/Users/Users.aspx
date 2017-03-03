<%@ Page Title="Users" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="CharityKitchenWebDatabase.Users.Users" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Content/styles.css" />
    <h2>Users</h2>
    <hr />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <asp:Panel ID="pnlNew" runat="server" Visible="false">
                <table>
                    <tr>
                        <td>Username:</td>
                        <td><asp:TextBox ID="txtNewUser" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><asp:Button ID="btnNewUser" Text="OK" runat="server" OnClick="btnNewUser_Click" /></td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Button ID="btnShowNew" Text="New User" runat="server" OnClick="btnShowNew_Click" />

            <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="gvUsers_SelectedIndexChanged" OnRowDeleting="gvUsers_RowDeleting">
                <Columns>
                    <asp:CommandField ShowSelectButton="true" ButtonType="Image" SelectImageUrl="~/Images/select.png" />
                    <asp:BoundField DataField="ID" HeaderText="ID" />
                    <asp:BoundField DataField="Username" HeaderText="User" />
                    <asp:BoundField DataField="Password" HeaderText="Password" Visible="False" />
                    <asp:CommandField ShowDeleteButton="true" ButtonType="Image" DeleteImageUrl="~/Images/delete.png" />
                </Columns>
            </asp:GridView>
            <br />
            <asp:Label ID="lblInfo" runat="server"/>
            <br />
            <asp:Panel ID="pnlDelete" runat="server" Visible="false" Width="350">
                <table>
                    <tr>
                        <td><p>You are about to delete <%: userDeleting.Username %>. Are you sure you want to delete this user? <p style="color: #b50000">WARNING: All associated permissions/roles will also be deleted.</p></p></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnYes" Text="Yes, delete this user" runat="server" OnClick="btnYes_Click" enabled="false"/>
                            <asp:Button ID="btnNo" Text="No, I made a mistake" runat="server" OnClick="btnNo_Click" enabled="false"/>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>

    <!--Help-->
    <hr />
    <h3>How do I?</h3>
    <ul>
        <li>
            <b>Add a new user account?</b> To add a user account, click 'New User' and fill out the form that appears. Once complete, click on the 'OK' button. NOTE: Make sure you edit the user account to assign a password (if you see an asterisk, it means that the user has the default password, and that it should be changed ASAP), and access levels. Default password is "password" and the default access levels for all roles is zero.
        </li>
        <li>
            <b>Edit a user account?</b> To edit a user account, click on the green button on the user that you wish to modify. You will be redirected to the user editor page.
        </li>
        <li>
            <b>Delete a user account?</b> To delete a user account, click on the red 'X' on the row of the user you wish to delete. You will be asked to confirm your decision.
        </li>
    </ul>
    <br />
    <!--End Help-->

</asp:Content>
