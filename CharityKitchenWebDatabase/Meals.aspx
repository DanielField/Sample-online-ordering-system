<%@ Page Title="Recipes/Meals" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Meals.aspx.cs" Inherits="CharityKitchenWebDatabase.Meals" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Recipes/Meals</h2>
    <hr />
    <!-- Meals -->
    <asp:Button ID="btnNew" runat="server" Text="New" OnClick="btnNew_Click" />
    <br />
    <asp:Panel runat="server" ID="pnlEdit" Visible="false">
        <table>
            <tr>
                <td>ID: </td><td><asp:Label ID="lblID" runat="server" /></td>
            </tr>
            <tr>
                <td>Meal/Recipe: </td><td><asp:TextBox ID="txtMeal" runat="server" /></td>
            </tr>
            <tr>
                <td><asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="btnSubmit_Click"/></td>
            </tr>
        </table>
    </asp:Panel>

    <asp:GridView ID="gvMeals" runat="server" OnSelectedIndexChanged="gvMeals_SelectedIndexChanged" OnRowDeleting="gvMeals_RowDeleting">
        <Columns>
            <asp:CommandField ShowSelectButton="true" ButtonType="Image" SelectImageUrl="~/Images/select.png" />
            <asp:CommandField ShowDeleteButton="true" ButtonType="Image" DeleteImageUrl="~/Images/delete.png" />
        </Columns>
    </asp:GridView>
    
    <br/>
    <asp:Label ID="lblInfo" runat="server" />
    <br />
    <!-- End Meals -->

    <!-- MealLines -->
    <asp:Button ID="btnNewLine" runat="server" Visible="false" Text="New ingredient" OnClick="btnNewLine_Click" />
    <br />
    <asp:Panel runat="server" ID="pnlEditMealLine" Visible="false">
        <table>
            <tr>
                <td>MealLine ID: </td><td><asp:Label ID="lblMealLineID" runat="server" /></td>
            </tr>
            <tr>
                <td>Ingredient:</td>
                <td><asp:DropDownList ID="cboIngredients" runat="server" /></td>
            </tr>
            <tr>
                <td>Quantity:</td>
                <td><asp:TextBox ID="txtQuantity" runat="server" TextMode="Number"/></td>
            </tr>
            <tr>
                <td><asp:Button ID="btnSubmitLine" Text="Submit" runat="server" OnClick="btnSubmitLine_Click"/></td>
            </tr>
        </table>
    </asp:Panel>

    <asp:GridView ID="gvMealLines" runat="server" OnSelectedIndexChanged="gvMealLines_SelectedIndexChanged" OnRowDeleting="gvMealLines_RowDeleting">
        <Columns>
            <asp:CommandField ShowSelectButton="true" ButtonType="Image" SelectImageUrl="~/Images/select.png" />
            <asp:CommandField ShowDeleteButton="true" ButtonType="Image" DeleteImageUrl="~/Images/delete.png" />
        </Columns>
    </asp:GridView>
    
    <br/>
    <asp:Label ID="lblLineInfo" runat="server" />
    <br />
    <!-- End MealLines -->

    <!--Help-->
    <hr />
    <h3>How do I?</h3>
    <ul>
        <li>
            <b>View existing meals/recipes?</b> To view existing meals/recipes, take a look at the first table on the page, it will contain a list of every meal within the system.
        </li>
        <li>
            <b>Create new meals/recipes?</b> To create a new recipe, click the 'New' button at the top of the page. A form will pop up. In this form, you can set the name of the recipe. Once you have done that, make sure you click 'Submit' to ensure that it has saved. NOTE: continue to the next question to learn how to add ingredients to the recipe.
        </li>
        <li>
            <b>Add ingredients to a recipe?</b> To add new ingredients to a recipe, click on the green button on the row of the recipe you would like to edit. A new section will pop-up at the bottom of the page. Click 'New ingredient' to bring up a form to add the ingredient, and the quantity. Click 'Submit' below the ingredient form to insert it into the recipe.
        </li>
        <li>
            <b>Edit existing meals?</b> In order to edit an existing meal, click the green button beside the meal you are editing. There you can add ingredients (question above), or you can edit existing ingredients by selecting the ingredient row and editing with the form above the recipe ingredients table.
        </li>
        <li>
            <b>Delete a meal?</b> To delete a meal, click the red 'X' beside the meal you would like to delete.
        </li>
        <li>
            <b>Delete an ingredient from the recipe?</b> Simply click the red 'X' beside the ingredient you wish to delete.
        </li>
    </ul>
    <br />
    <!--End Help-->
</asp:Content>
