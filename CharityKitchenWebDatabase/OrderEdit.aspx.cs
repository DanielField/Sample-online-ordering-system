using CharityKitchenWebDatabase.Extensions;
using CharityKitchenWebDatabase.SvcKitchen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CharityKitchenWebDatabase
{
    public partial class OrderEdit : System.Web.UI.Page
    {
        #region vars

        static OrderLine orderLine = new OrderLine();

        static Order order;

        #endregion vars

        #region init

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            switch (this.GetAccessLevel("Orders"))
            {
                case AccessLevel.Deny:
                    Response.Redirect("~/Default");
                    break;
                case AccessLevel.Read:
                    btnNew.Enabled = false;
                    gvOrderLines.Enabled = false;
                    break;
            }

            var result = LoadData();

            if (result.ErrorCode == 0)
                lblStatus.ForeColor = System.Drawing.Color.DarkGreen;
            else
                lblStatus.ForeColor = System.Drawing.Color.DarkRed;

            lblStatus.Text = result.Message;
        }

        /// <summary>
        /// Load the order and order lines to the fields.
        /// </summary>
        /// <returns>Returns a service result.</returns>
        private ServiceResult LoadData()
        {
            SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();
            SvcKitchen.ServiceResult result;
            try
            {
                order = Session["Order"] as Order;
                lblOrderID.Text = order.ID.ToString();
                txtAddress.Text = order.DeliveryAddress;
                txtPhoneNumber.Text = order.PhoneNumber;

                cboMeals.DataSource = svc.GetMeals(out result);
                cboMeals.DataValueField = "ID";
                cboMeals.DataTextField = "Name";
                cboMeals.DataBind();


                gvOrderLines.DataSource = svc.GetOrderLines(order.ID, out result);
                gvOrderLines.DataBind();

                return result;
            }
            catch
            {
                lblStatus.ForeColor = System.Drawing.Color.DarkRed;
                lblStatus.Text = "Oh no... Looks like it failed to load the page correctly.";
                return new ServiceResult();
            }
        }

        #endregion init

        #region events

        /// <summary>
        /// Set order line values to the data given by the user.
        /// run insert query.
        /// Display message.
        /// </summary>
        protected void btnNew_Click(object sender, EventArgs e)
        {
            SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();
            SvcKitchen.ServiceResult result;

            try
            {
                orderLine.MealID = int.Parse(cboMeals.SelectedItem.Value);
                orderLine.OrderID = order.ID;
                orderLine.Quantity = int.Parse(txtQuantity.Text);

                result = svc.AddOrderLine(orderLine);

                LoadData();

                if (result.ErrorCode == 0)
                    lblStatus.ForeColor = System.Drawing.Color.DarkGreen;
                else
                    lblStatus.ForeColor = System.Drawing.Color.DarkRed;
                lblStatus.Text = result.Message;
            }
            catch
            {
                lblStatus.ForeColor = System.Drawing.Color.DarkRed;
                lblStatus.Text = "Error. Please ensure that you have entered valid data into the required fields.";
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();
            SvcKitchen.ServiceResult result;

            try
            {
                result = svc.DeleteOrder(order.ID);

                if (result.ErrorCode == 0)
                    Response.Redirect("~/Orders");

                lblStatus.ForeColor = System.Drawing.Color.DarkRed;
                lblStatus.Text = result.Message;
            }
            catch
            {
                lblStatus.ForeColor = System.Drawing.Color.DarkRed;
                lblStatus.Text = "There was an error attempting to delete the order.";
            }
        }

        /// <summary>
        /// get order line from database, then display on page and set the status text to the ServiceResult message.
        /// </summary>
        protected void gvOrderLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();
            SvcKitchen.ServiceResult result;

            try
            {
                orderLine = svc.GetByOrderLineId(int.Parse(gvOrderLines.SelectedRow.Cells[1].Text), out result);

                displayLine(orderLine);

                if (result.ErrorCode == 0)
                    lblStatus.ForeColor = System.Drawing.Color.DarkGreen;
                else
                    lblStatus.ForeColor = System.Drawing.Color.DarkRed;

                lblStatus.Text = result.Message;

                pnlItem.Visible = true;
                btnNew.Text = "Save item";
            }
            catch
            {
                lblStatus.ForeColor = System.Drawing.Color.DarkRed;
                lblStatus.Text = "There was an error selecting the item.";
            }
            
        }

        /// <summary>
        /// Return to ~/Orders.aspx
        /// </summary>
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Orders");
        }

        /// <summary>
        /// Apply order changes
        /// </summary>
        protected void btnApply_Click(object sender, EventArgs e)
        {
            SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();
            SvcKitchen.ServiceResult result;

            try
            {
                order.PhoneNumber = txtPhoneNumber.Text;
                order.DeliveryAddress = txtAddress.Text;

                result = svc.addOrder(order);

                if (result.ErrorCode == 0)
                    lblStatus.ForeColor = System.Drawing.Color.DarkGreen;
                else
                    lblStatus.ForeColor = System.Drawing.Color.DarkRed;

                lblStatus.Text = result.Message;

            }
            catch
            {
                lblStatus.ForeColor = System.Drawing.Color.DarkRed;
                lblStatus.Text = "Unable to modify the order. Please ensure the submitted data is valid.";
            }
        }

        protected void btnShowItemCreator_Click(object sender, EventArgs e)
        {
            pnlItem.Visible = true;
            btnNew.Text = "Insert New Item";

            orderLine.OrderLineID = 0;
        }

        protected void gvOrderLines_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();
            SvcKitchen.ServiceResult result;
            try
            {
                result = svc.DeleteOrderLine(int.Parse(gvOrderLines.Rows[e.RowIndex].Cells[1].Text));

                if (result.ErrorCode == 0)
                {
                    LoadData();
                    lblStatus.ForeColor = System.Drawing.Color.DarkGreen;
                }
                else
                    lblStatus.ForeColor = System.Drawing.Color.DarkRed;

                lblStatus.Text = result.Message;
            }
            catch
            {
                lblStatus.ForeColor = System.Drawing.Color.DarkRed;
                lblStatus.Text = "There was an error attempting to delete the specified item.";
            }
        }

        #endregion events

        #region methods

        /// <summary>
        /// Display single order line
        /// </summary>
        /// <param name="line">OrderLine object.</param>
        private void displayLine(OrderLine line)
        {
            lblItemID.Text = line.OrderLineID.ToString();
            txtQuantity.Text = line.Quantity.ToString();

            cboMeals.SelectedValue = line.MealID.ToString();
        }

        #endregion methods
    }
}