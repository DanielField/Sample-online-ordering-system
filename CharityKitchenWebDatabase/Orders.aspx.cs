using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CharityKitchenWebDatabase.Extensions;

namespace CharityKitchenWebDatabase
{
    public partial class Orders : System.Web.UI.Page
    {
        #region init
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (this.GetAccessLevel("Orders"))
            {
                case AccessLevel.Deny:
                    Response.Redirect("~/Default");
                    break;
                case AccessLevel.Read:
                    btnNew.Enabled = false;
                    gvOrders.Enabled = false;
                    break;
            }

            try
            {
                GetOrders();
            }
            catch{}

            foreach (GridViewRow row in gvOrders.Rows)
            {
                if (string.IsNullOrWhiteSpace(row.Cells[4].Text) || string.IsNullOrWhiteSpace(row.Cells[5].Text)
                        || row.Cells[4].Text == "&nbsp;" || row.Cells[5].Text == "&nbsp;")
                {
                    row.BackColor = System.Drawing.Color.FromArgb(255, 150, 150);

                    lblInfo.Text = "There are orders with an empty delivery address and/or phone number. Please edit the order(s) to change the order details.";
                }
            }
        }

        private void GetOrders()
        {
            SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();

            SvcKitchen.ServiceResult result;
   
            gvOrders.DataSource = svc.GetOrders(out result);

            gvOrders.DataBind();

            if (result.ErrorCode == 0)
                lblInfo.ForeColor = System.Drawing.Color.DarkGreen;
            else
                lblInfo.ForeColor = System.Drawing.Color.DarkGreen;

            lblInfo.Text = result.Message;
        }
        #endregion init

        #region events
        protected void gvOrders_SelectedIndexChanged(object sender, EventArgs e)
        {
            SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();

            SvcKitchen.ServiceResult result;

            try
            {
                Session["Order"] = svc.GetOrderByID(int.Parse(gvOrders.SelectedRow.Cells[2].Text), out result);

                if (result.ErrorCode == 0)
                    Response.Redirect("~/OrderEdit");

                lblInfo.ForeColor = System.Drawing.Color.DarkRed;
                lblInfo.Text = result.Message;
            }
            
            catch
            {
                lblInfo.ForeColor = System.Drawing.Color.DarkRed;
                lblInfo.Text = "There was an error attempting to select the specified order.";
            }

        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();
            SvcKitchen.ServiceResult result;

            try
            {
                SvcKitchen.Order order = new SvcKitchen.Order();
                order.OrderDate = DateTime.Now;

                result = svc.addOrder(order);

                if (result.ErrorCode == 0)
                {
                    GetOrders();
                    lblInfo.ForeColor = System.Drawing.Color.DarkGreen;
                }
                else
                {
                    lblInfo.ForeColor = System.Drawing.Color.DarkGreen;
                }
                    

                lblInfo.Text = result.Message;
            }
            catch
            {
                lblInfo.ForeColor = System.Drawing.Color.DarkRed;
                lblInfo.Text = "There was an error attempting to create a new order.";
            }
        }

        protected void gvOrders_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();

            try
            {
                SvcKitchen.ServiceResult result = svc.DeleteOrder(int.Parse(gvOrders.Rows[e.RowIndex].Cells[2].Text));

                if (result.ErrorCode == 0)
                {
                    GetOrders();
                    lblInfo.ForeColor = System.Drawing.Color.DarkGreen;
                }
                else
                {
                    lblInfo.ForeColor = System.Drawing.Color.DarkRed;
                }
                lblInfo.Text = result.Message;
            }
            catch
            {
                lblInfo.ForeColor = System.Drawing.Color.DarkRed;
                lblInfo.Text = "There was an error attempting to delete the order.";
            }
        }

        #endregion events
    }
}