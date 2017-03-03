using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CharityKitchenWebDatabase.Extensions;

namespace CharityKitchenWebDatabase.Users
{
    public partial class Users : System.Web.UI.Page
    {
        #region vars

        static int userID;
        public static SvcKitchen.User userDeleting; // stores the user being deleted.

        #endregion vars

        #region init

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            switch (this.GetAccessLevel("Users"))
            {
                case AccessLevel.Deny:
                    Response.Redirect("~/Login");
                    break;
                case AccessLevel.Read:
                    btnNewUser.Enabled = false;
                    btnShowNew.Enabled = false;
                    pnlNew.Enabled = false;
                    gvUsers.Enabled = false;
                    break;
            }

            try
            {
                LoadData();
                pnlDelete.BackColor = System.Drawing.Color.LightGray;
            }
            catch
            {
                lblInfo.ForeColor = System.Drawing.Color.DarkRed;
                lblInfo.Text = "Error loading the data on the page.";
            }
            
        }

        private void LoadData()
        {
            SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();
            SvcKitchen.ServiceResult result = new SvcKitchen.ServiceResult();

            var users = svc.GetAllUsers(out result);
            DataTable dtUsers = new DataTable();
            dtUsers.Columns.Add();
            dtUsers.Columns.Add();
            dtUsers.Columns.Add();

            foreach (SvcKitchen.User u in users)
                dtUsers.Rows.Add(new object[] { u.ID, u.Username, u.Password });

            if (result.ErrorCode == 0)
            {

                gvUsers.DataSource = users;
                gvUsers.DataBind();

                for (int i = 0; i < users.Length; i++)
                {
                    if ((string)dtUsers.Rows[i][2] == "password")
                    {
                        gvUsers.Rows[i].Cells[2].Text = "*" + gvUsers.Rows[i].Cells[2].Text;
                    }
                }

                lblInfo.ForeColor = System.Drawing.Color.DarkGreen;
                lblInfo.Text = result.Message;
            }
            else
            {
                lblInfo.ForeColor = System.Drawing.Color.DarkRed;
                lblInfo.Text = result.Message;
            }
        }

        #endregion init

        #region events

        protected void btnNewUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNewUser.Text != "*" && !string.IsNullOrEmpty(txtNewUser.Text))
                {
                    SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();

                    var result = svc.AddNewUser(txtNewUser.Text);

                    if (result.ErrorCode == 0)
                    {
                        LoadData();
                    }
                    else
                    {
                        lblInfo.ForeColor = System.Drawing.Color.DarkRed;
                        lblInfo.Text = result.Message;
                    }
                }
            }
            catch
            {
                lblInfo.Text = "There was an error attempting to create a new user.";
                lblInfo.ForeColor = System.Drawing.Color.DarkRed;
            }
        }

        protected void btnShowNew_Click(object sender, EventArgs e)
        {
            pnlNew.Visible = true;
        }

        protected void gvUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                userID = int.Parse(gvUsers.SelectedRow.Cells[1].Text);
                Session["UserID"] = userID;
                Response.Redirect("~/Users/UserEdit");
            }
            catch
            {
                lblInfo.Text = "Error parsing the user ID. Please ensure the table is valid and up to date.";
                lblInfo.ForeColor = System.Drawing.Color.DarkRed;
            }

        }

        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                pnlDelete.Visible = true;
                btnNo.Enabled = true;
                btnYes.Enabled = true;

                userDeleting = new SvcKitchen.User();
                userDeleting.ID = int.Parse(gvUsers.Rows[e.RowIndex].Cells[1].Text);
                userDeleting.Username = gvUsers.Rows[e.RowIndex].Cells[2].Text;

                if (userDeleting.Username.StartsWith("*"))
                {
                    userDeleting.Username = userDeleting.Username.Substring(1);
                }
            }
            catch
            {
                lblInfo.Text = "Error parsing the user ID. Please ensure the table is valid and up to date.";
                lblInfo.ForeColor = System.Drawing.Color.DarkRed;
            }
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            try
            {
                SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();
                SvcKitchen.ServiceResult result = new SvcKitchen.ServiceResult();

                result = svc.DeleteUser(userDeleting.ID);

                if (result.ErrorCode == 0)
                    lblInfo.ForeColor = System.Drawing.Color.DarkGreen;
                else
                    lblInfo.ForeColor = System.Drawing.Color.DarkRed;
                lblInfo.Text = result.Message;
                
                LoadData();
            }
            catch
            {
                lblInfo.ForeColor = System.Drawing.Color.DarkRed;
                lblInfo.Text = "There was an error trying to delete the user.";
            }
            finally
            {
                pnlDelete.Visible = false;
                btnNo.Enabled = false;
                btnYes.Enabled = false;
            }
        }

        protected void btnNo_Click(object sender, EventArgs e)
        {
            pnlDelete.Visible = false;
            btnNo.Enabled = false;
            btnYes.Enabled = false;

            userDeleting = null;
        }

        #endregion events
    }
}