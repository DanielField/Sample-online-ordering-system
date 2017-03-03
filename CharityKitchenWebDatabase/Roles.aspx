<%@ Page Title="Roles" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Roles.aspx.cs" Inherits="CharityKitchenWebDatabase.Roles.Roles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Roles</h2>
    <hr />

    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <asp:Button ID="btnNewRole" Text="New Role" runat="server" OnClick="btnNewRole_Click"/>
            <br />
            <asp:Panel runat="server" ID="pnlEdit" Visible="false">
                <table>
                    <tr>
                        <td>ID: </td><td><asp:Label ID="lblID" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Description: </td><td><asp:TextBox ID="txtDescription" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="btnSubmit_Click"/></td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:GridView ID="gvRoles" runat="server" OnRowDeleting="gvRoles_RowDeleting" OnSelectedIndexChanged="gvRoles_SelectedIndexChanged">
                <Columns>
                    <asp:CommandField ShowSelectButton="true" ButtonType="Image" SelectImageUrl="~/Images/select.png" />
                    <asp:CommandField ShowDeleteButton="true" ButtonType="Image" DeleteImageUrl="~/Images/delete.png" />
                </Columns>
            </asp:GridView>

            <br/>
            <asp:Label ID="lblInfo" runat="server" />
            <br />

        </ContentTemplate>
    </asp:UpdatePanel>

    <!--Help-->
    <hr />
    <h3>How do I?</h3>
    <ul>
        <li>
            <b>Add a new role?</b> To add a new role, click 'New Role' and fill out the description of the role. Click 'Submit' to save the new role.
        </li>
        <li>
            <b>Edit an existing role?</b> To edit an existing role, click on the green button on the row of the role you wish to edit, and fill out the description, then click 'Submit'.
        </li>
        <li>
            <b>Delete a role?</b> To delete a role, click the red 'X' on the role you wish to delete.
        </li>
    </ul>
    <br />
    <p>
        <p style="color: #b50000">WARNING: If you edit or delete a role, some of the functionality of the system will stop working. Only do this if you KNOW what you are doing.</p>
        <br />
        <br />
        Additional Information: Adding a new role to the system is futile unless the role is implemented throughout the system. Please contact the system developer(s) if you wish to create a new role.
    </p>
    <!--End Help-->

</asp:Content>
