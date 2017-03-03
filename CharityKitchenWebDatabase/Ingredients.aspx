<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Ingredients.aspx.cs" Inherits="CharityKitchenWebDatabase.Ingredients.Ingredients" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Ingredients</h2>
    <hr />
    <!-- Ingredients -->
    <asp:Button ID="btnNew" runat="server" Text="New" OnClick="btnNew_Click" />
    <br />
    <asp:Panel runat="server" ID="pnlEdit" Visible="false">
        <table>
            <tr>
                <td>ID: </td><td><asp:Label ID="lblID" runat="server" /></td>
            </tr>
            <tr>
                <td>Ingredient Name: </td><td><asp:TextBox ID="txtIngredientName" runat="server" /></td>
            </tr>
            <tr>
                <td><asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="btnSubmit_Click"/></td>
            </tr>
        </table>
    </asp:Panel>

    <asp:GridView ID="gvIngredients" runat="server" OnSelectedIndexChanged="gvIngredients_SelectedIndexChanged" OnRowDeleting="gvIngredients_RowDeleting">
        <Columns>
            <asp:CommandField ShowSelectButton="true" ButtonType="Image" SelectImageUrl="~/Images/select.png" />
            <asp:CommandField ShowDeleteButton="true" ButtonType="Image" DeleteImageUrl="~/Images/delete.png" />
        </Columns>
    </asp:GridView>
    
    <br/>
    <asp:Label ID="lblInfo" runat="server" />
    <br />
    <!-- End Ingredients -->

    <!--Help-->
    <hr />
    <h3>How do I?</h3>
    <ul>
        <li>
            <b>View existing ingredients?</b> To view existing ingredients, take a look at the first table on the page, it will contain a list of every ingredient within the system.
        </li>
        <li>
            <b>Edit an ingredient?</b> To edit an ingredient, select the row by clicking the green button. Now you can fill in the form that pops up above the table, and click 'submit'.
        </li>
        <li>
            <b>Delete an ingredient?</b> Delete an ingredient by clicking the red 'X' on the row that contains the ingredient you wish to delete.
        </li>
        <li>
            <b>Add a new ingredient?</b> Click 'New' and then you can fill in the form that pops up above the table, and click 'submit'.
        </li>
    </ul>
    <br />
    <!--End Help-->

</asp:Content>
