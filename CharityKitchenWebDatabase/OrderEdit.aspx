<%@ Page Title="Edit Order" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrderEdit.aspx.cs" Inherits="CharityKitchenWebDatabase.OrderEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link rel="stylesheet" href="Content/styles.css" />
    
    <h2>Order Editor</h2>
    <hr />

    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <table id="tblOrder">
                <tr>
                    <td><asp:Button ID="btnReturn" runat="server" Text="<< Return" OnClick="btnReturn_Click"/></td>
                </tr>
                <tr>
                    <td>Order ID:</td>
                    <td><asp:Label ID="lblOrderID" runat="server" Text="0"></asp:Label></td>
                </tr>
                <tr>
                    <td>Delivery address:</td>
                    <td><asp:TextBox ID="txtAddress" runat="server" /></td>
                </tr>
                <tr>
                    <td>Phone number:</td>
                    <td><asp:TextBox ID="txtPhoneNumber" runat="server" /></td>
                </tr>
                <tr>
                    <td><asp:Button ID="btnApply" runat="server" Text="Apply" OnClick="btnApply_Click"/></td>
                </tr>
            </table>

            <br />
            
            <asp:Button ID="btnShowItemCreator" Text="New item" OnClick="btnShowItemCreator_Click" runat="server" />
            
            <asp:Panel ID="pnlItem" Visible="false" runat="server">
                <table id="tblItem">
                    <tr>
                        <td><b>Add/Edit item</b></td>
                    </tr>
                    <tr>
                        <td>Item ID:</td>
                        <td><asp:Label ID="lblItemID" runat="server" Text="0"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Meal/Recipe:</td>
                        <td><asp:DropDownList ID="cboMeals" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Quantity:</td>
                        <td><asp:TextBox ID="txtQuantity" runat="server" TextMode="Number"/></td>
                    </tr>
                    <tr>
                        <td><asp:Button ID="btnNew" runat="server" Text="Save item" OnClick="btnNew_Click" /></td>
                    </tr>
                </table>
            </asp:Panel>
            
            <br />
            <asp:Label ID="lblStatus" runat="server" Text="" />
            <br />

            <asp:GridView ID="gvOrderLines" runat="server" OnSelectedIndexChanged="gvOrderLines_SelectedIndexChanged" AutoGenerateColumns="False" OnRowDeleting="gvOrderLines_RowDeleting">
                <Columns>
                    <asp:CommandField ShowSelectButton="true" ButtonType="Image" SelectImageUrl="~/Images/select.png" />
                    <asp:BoundField DataField="OrderLineID" HeaderText="OrderLineID" />
                    <asp:BoundField DataField="MealID" HeaderText="MealID" Visible="False" />
                    <asp:BoundField DataField="MealName" HeaderText="Meal" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                    <asp:BoundField DataField="OrderID" HeaderText="OrderID" Visible="False" />
                    <asp:CommandField ShowDeleteButton="true" ButtonType="Image" DeleteImageUrl="~/Images/delete.png" />
                </Columns>
            </asp:GridView>
            <asp:Button ID="btnDelete" runat="server" Text="Delete order" OnClick="btnDelete_Click" />

        </ContentTemplate>
    </asp:UpdatePanel>

    <!--Help-->
    <hr />
    <h3>How do I?</h3>
    <ul>
        <li>
            <b>Edit the delivery address?</b> Fill out the form at the top of the page and then click 'Apply' to save the changes.
        </li>
        <li>
            <b>Edit the phone number?</b> Fill out the form at the top of the page and then click 'Apply' to save the changes.
        </li>
        <li>
            <b>Edit an item in the order?</b> Click on the green button on the item you wish to edit, and then fill out the form that appears, and click 'Save item'.
        </li>
        <li>
            <b>add an item to the order?</b> Click on the 'New item' button, and then fill out the form that appears, and click 'Insert New Item'.
        </li>
        <li>
            <b>Delete an item from the order?</b> Click the red 'X' on the item you wish to delete.
        </li>
        <li>
            <b>Delete the entire order?</b> Click the 'Delete order' button to delete the entire order. <p style="color: #b50000;">WARNING: If you want to delete one item from the order, DO NOT click 'Delete order'.</p>
        </li>
    </ul>
    <br />
    <!--End Help-->

</asp:Content>
