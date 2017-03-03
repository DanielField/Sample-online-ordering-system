<%@ Page Title="Orders" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="CharityKitchenWebDatabase.Orders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Orders</h2>
    <hr />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>


            <asp:GridView ID="gvOrders" runat="server" OnSelectedIndexChanged="gvOrders_SelectedIndexChanged" OnRowDeleting="gvOrders_RowDeleting">
                <Columns>
                    <asp:CommandField ShowSelectButton="true" ButtonType="Image" SelectImageUrl="~/Images/select.png" />
                    <asp:CommandField ShowDeleteButton="true" ButtonType="Image" DeleteImageUrl="~/Images/delete.png" />
                </Columns>
            </asp:GridView>
            <asp:Button ID="btnNew" runat="server" Text="New" OnClick="btnNew_Click" />
            <br />
            <asp:Label ID="lblInfo" runat="server" />
            <hr />
            <h3>How do I?</h3>
            <ul>
                <li>
                    <b>Create a new order?</b> To create a new order click on the 'New' button, and then click 'select' on the new row that has been added.
                </li>
                <li>
                    <b>Edit an existing order?</b> To edit an order, simply click 'select' on the row and the page will redirect you to the order editing page.
                </li>
                <li>
                    <b>Delete an order?</b> To delete an order click 'delete' on the order you wish to delete. 
                    <br /> WARNING: Make SURE that you are deleting the correct order. There is no confirmation dialog implemented yet.
                </li>
            </ul>
            <br />
            <p>
                <b>What does the red row mean?</b> The rows highlighted in red are to let you know that those orders have no data in either the delivery address field, or the phone number field. please ensure that all of the orders have valid data.
            </p>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
