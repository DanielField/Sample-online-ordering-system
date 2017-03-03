<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserEdit.aspx.cs" Inherits="CharityKitchenWebDatabase.Users.UserEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="../Content/styles.css" />

    <h2>User editor</h2>
    <hr />

    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <asp:Label ID="lblInfo" runat="server"/>

            <table id="tblUserCredentials">
                <tr>
                    <td><asp:Label Text="User" Font-Size="15" runat="server"/></td>
                </tr>
                <tr>
                    <td>User ID:</td> <td><asp:Label ID="lblID" runat="server"/></td>
                </tr>
                <tr>
                    <td>Username:</td> <td><asp:TextBox ID="txtUsername" runat="server" /></td>
                </tr>
                <tr>
                    <td>Password:</td> <td><asp:TextBox ID="txtPassword" runat="server" TextMode="Password"/></td>
                </tr>
                <tr>
                    <td>Confirm password:</td> <td><asp:TextBox ID="txtConfirm" runat="server" TextMode="Password"/></td>
                </tr>

                <tr>
                    <td><asp:Button ID="btnSaveUserCredentials" runat="server" OnClick="btnSaveUserCredentials_Click" Text="Update credentials" /></td>
                </tr>
            </table>

            <br />
            
            <asp:Panel ID="pnlUserRoles" runat="server" Visible="false">
                <table id="tblUserRoleEdit">
                    <tr>
                        <td><asp:Label Text="Role Edit" Font-Size="15" runat="server"/></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lblUserRoleID" runat="server" Text="" /></td>
                    </tr>
                    <tr>
                        <td>Role:</td> <td><asp:Label ID="lblRole" runat="server" Text="" /></td>
                    </tr>
                    <tr>
                        <td>Access level:</td> <td><asp:TextBox ID="txtAccessLevel" runat="server" Text=""/></td>
                    </tr>
                    <tr> 
                        <td></td> 
                        <td>DENY=0, READ=1, WRITE=2</td>

                    </tr>
                    <tr>
                        <td><asp:Button ID="btnSaveUserRole" runat="server" OnClick="btnSaveUserRole_Click" Text="Save role changes" /></td>
                    </tr>
                </table>
            </asp:Panel>

            <br />

            <table>
                <tr>
                    <td><asp:Label Text="Roles" Font-Size="15" runat="server"/></td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvRoles" runat="server" OnSelectedIndexChanged="gvRoles_SelectedIndexChanged">
                            <Columns>
                                <asp:CommandField ShowSelectButton="true" ButtonType="Image" SelectImageUrl="~/Images/select.png" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>

        </ContentTemplate>
    </asp:UpdatePanel>

    <!--Help-->
    <hr />
    <h3>How do I?</h3>
    <ul>
        <li>
            <b>Change the Username?</b> Fill out the form at the top with the new user name. NOTE: You must also type a new password into the form (type the old password twice if you do not wish to change it). Click 'Update user credentials' to apply the changes.
        </li>
        <li>
            <b>Change the password?</b> Fill out the form at the top with the new password, and repeat the password to confirm that it was typed correctly. Click 'Update user credentials' to apply the changes.
        </li>
        <li>
            <b>Change the user's role access level?</b> To change a role access level, click on the green button on the role on which you wish to change the access level, and then fill out the access level field and click 'Save role changes'.
        </li>
        <li>
            <b>Delete a role?</b> This is something that cannot be done. If you wish to deny access to a section of the application, simply select the role and change the access level to 0.
        </li>
    </ul>
    <br />
    <!--End Help-->

</asp:Content>
