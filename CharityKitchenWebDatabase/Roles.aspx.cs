using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CharityKitchenWebDatabase.Extensions;

namespace CharityKitchenWebDatabase.Roles
{
    /// <summary>
    /// The purpose of this page is to display all of the roles in the system, and to allow the user to delete/add/edit roles.
    /// </summary>
    public partial class Roles : System.Web.UI.Page
    {
        #region vars

        static SvcKitchen.Role role;
        static bool isNew;

        #endregion vars

        #region init

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            switch (this.GetAccessLevel("Users"))
            {
                case AccessLevel.Deny:
                    Response.Redirect("~/Default");
                    break;
                case AccessLevel.Read:
                    pnlEdit.Enabled = false;
                    btnNewRole.Enabled = false;
                    gvRoles.Enabled = false;
                    break;
            }

            try
            {
                LoadData();
            }
            catch
            {
                lblInfo.Text = "There was an error attempting to load the data.";
                lblInfo.ForeColor = System.Drawing.Color.DarkRed;
            }
        }

        /// <summary>
        /// Load all of the data.
        /// </summary>
        private void LoadData()
        {
            SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();
            SvcKitchen.ServiceResult result = new SvcKitchen.ServiceResult();

            gvRoles.DataSource = svc.RoleGetAll(out result);
            gvRoles.DataBind();

            lblInfo.DisplayResult(result);
        }

        #endregion init

        #region methods

        /// <summary>
        /// Gets the role from the form and stores it in the static role object.
        /// </summary>
        /// <param name="_isNew"></param>
        private void GetRoleFromForm(bool _isNew) 
        {
            if (_isNew) 
            {
                role.Description = txtDescription.Text;
                role.ID = 0;
            }
            else 
            {
                role.Description = txtDescription.Text;
                role.ID = int.Parse(lblID.Text);
            }
        }

        #endregion methods

        #region events

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();
                SvcKitchen.ServiceResult result = new SvcKitchen.ServiceResult();

                GetRoleFromForm(isNew);

                result = svc.RoleSave(role);

                lblInfo.DisplayResult(result);

                pnlEdit.Visible = false;
                lblID.Text = "";
                txtDescription.Text = "";

                LoadData();
            }
            catch
            {
                lblInfo.Text = "There was an error attempting to submit the role.";
                lblInfo.ForeColor = System.Drawing.Color.DarkRed;
            }
        }

        protected void btnNewRole_Click(object sender, EventArgs e)
        {
            pnlEdit.Visible = true;

            role = new SvcKitchen.Role();
            isNew = true;
        }

        protected void gvRoles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();
                SvcKitchen.ServiceResult result = new SvcKitchen.ServiceResult();

                role = new SvcKitchen.Role();
                role.ID = int.Parse(gvRoles.Rows[e.RowIndex].Cells[2].Text);
                role.Description = gvRoles.Rows[e.RowIndex].Cells[3].Text;

                result = svc.RoleDelete(role);

                lblInfo.DisplayResult(result);

                pnlEdit.Visible = false;
                lblID.Text = "";
                txtDescription.Text = "";

                LoadData();
            }
            catch
            {
                lblInfo.ForeColor = System.Drawing.Color.DarkRed;
                lblInfo.Text = "There was an error attempting to delete the role.";
            }
            
        }

        protected void gvRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            role = new SvcKitchen.Role();
            role.ID = int.Parse(gvRoles.SelectedRow.Cells[2].Text);
            role.Description = gvRoles.SelectedRow.Cells[3].Text;

            lblID.Text = role.ID.ToString();
            txtDescription.Text = role.Description;

            pnlEdit.Visible = true;

            isNew = false;
        }
        #endregion events
    }
}