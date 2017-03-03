<%@ Page Title="Lobby" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CharityKitchenWebDatabase._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Kitchen Web Lobby</h2>
    <hr />
    <asp:Button ID="btnOrders" runat="server" Text="Orders" OnClick="btnOrders_Click" />&ensp;
    <asp:Button ID="btnMeals" runat="server" Text="Meals" OnClick="btnMeals_Click" />&ensp;
    <asp:Button ID="btnIngredients" runat="server" Text="Ingredients" OnClick="btnIngredients_Click" />&ensp;
    <asp:Button ID="btnUsers" runat="server" Text="Users" OnClick="btnUsers_Click" />

    <!--Help-->
    <hr />
    <h3>How do I?</h3>
    <ul>
        <li>
            <b>view the current orders?</b> To view the current orders, click 'Orders'.
        </li>
        <li>
            <b>Create a new order?</b> In order to create a new order, click 'Orders', and click the 'New' button on the orders page.
        </li>
        <li>
            <b>View all of the meals?</b> You can view a list of meals by clicking 'Meals'.
        </li>
        <li>
            <b>View a list of ingredients?</b> You can view a list of ingredients by clicking 'Ingredients'.
        </li>
        <li>
            <b>Add/Modify user accounts?</b> You can do this by clicking 'Users' and then following the instructions on the 'Users' page.
        </li>
    </ul>
    <br />
    <p>
        Greetings <%: username %>, this system is designed to deal with all of our orders and recipes. You can navigate the site by clicking the buttons on the navigation bar, or the buttons below the bar (in the content section of the page). 
    </p>
    <p>
        If you have permission to access a page, then it will allow you to visit the page, if not, it will redirect you if you attempt to access the page.
    </p>
    <!--End Help-->

</asp:Content>
