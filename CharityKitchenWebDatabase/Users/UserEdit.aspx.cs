using System;
using CharityKitchenWebDatabase.Extensions;
using CharityKitchenWebDatabase.SvcKitchen;
using System.Web.UI.WebControls;

namespace CharityKitchenWebDatabase.Users
{
    /// <summary>
    /// This page is for editing the user details.
    /// </summary>
    public partial class UserEdit : System.Web.UI.Page
    {
        #region vars

        static int userID;

        static SvcKitchen.User u;

        static SvcKitchen.UserRole ur;

        #endregion vars

        #region init

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            switch (this.GetAccessLevel("Users"))
            {
                case AccessLevel.Deny:
                    Response.Redirect("~/Users/Users.aspx");
                    break;
                case AccessLevel.Read:
                    btnSaveUserCredentials.Enabled = false;
                    btnSaveUserRole.Enabled = false;
                    txtConfirm.Enabled = false;
                    txtPassword.Enabled = false;
                    txtUsername.Enabled = false;
                    txtAccessLevel.Enabled = false;
                    gvRoles.Enabled = false;
                    break;
            }

            userID = (int)Session["UserID"];
            
            SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();
            SvcKitchen.ServiceResult result = new SvcKitchen.ServiceResult();

            u = svc.GetUserByID(userID, out result);

            DisplayUser();

            GetUserRoles();

            lblInfo.Text = result.Message;

            lblRole.Enabled = false;
            txtAccessLevel.Enabled = false;
            btnSaveUserRole.Enabled = false;
        }

        #endregion init

        #region private methods

        /// <summary>
        /// Fills gvRoles with all of the user roles.
        /// </summary>
        private void GetUserRoles()
        {
            SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();
            SvcKitchen.ServiceResult result = new SvcKitchen.ServiceResult();

            gvRoles.DataSource = svc.GetUserRolesById(userID, out result);
            gvRoles.DataBind();

            lblInfo.Text = result.Message;
        }

        /// <summary>
        /// Displays the user's details in the user field.
        /// </summary>
        private void DisplayUser()
        {
            lblID.Text = userID.ToString();
            txtUsername.Text = u.Username;
        }

        /// <summary>
        /// Displays the specified user role in the role edit section of the page.
        /// </summary>
        /// <param name="_ur">UserRole object</param>
        private void DisplayUserRole(UserRole _ur)
        {
            lblRole.Text = _ur.RoleName;
            lblUserRoleID.Text = "Role ID: " + ur.RoleID;
            txtAccessLevel.Text = ur.AccessLevel.ToString();
        }

        #endregion

        #region events

        protected void gvRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();
            SvcKitchen.ServiceResult result = new SvcKitchen.ServiceResult();

            ur = new UserRole();
            ur.UserRoleID = Convert.ToInt32(gvRoles.SelectedRow.Cells[1].Text);
            ur.UserID = Convert.ToInt32(gvRoles.SelectedRow.Cells[2].Text);
            ur.RoleID = Convert.ToInt32(gvRoles.SelectedRow.Cells[3].Text);
            ur.AccessLevel = Convert.ToInt32(gvRoles.SelectedRow.Cells[4].Text);
            ur.RoleName = gvRoles.SelectedRow.Cells[5].Text;

            DisplayUserRole(ur);

            lblRole.Enabled = true;
            txtAccessLevel.Enabled = true;
            btnSaveUserRole.Enabled = true;
            pnlUserRoles.Visible = true;
        }

        /// <summary>
        /// Updates the user role with the data specified on the page, then reloads the data from the database.
        /// </summary>
        protected void btnSaveUserRole_Click(object sender, EventArgs e)
        {
            SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();

            ServiceResult result = new ServiceResult();
            result = svc.UpdateUserRole(ur.UserRoleID, Convert.ToInt32(txtAccessLevel.Text));

            lblInfo.Text = result.Message;

            GetUserRoles();
        }

        /// <summary>
        /// Updates the username and password with the data specified on the page.
        /// If the password does not match the confirmation password, it will display an error message in red.
        /// </summary>
        protected void btnSaveUserCredentials_Click(object sender, EventArgs e)
        {
            SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();
            ServiceResult result = new ServiceResult();

            if (txtPassword.Text == txtConfirm.Text)
            {
                result = svc.UpdateUser(txtUsername.Text, txtPassword.Text, userID);

                lblInfo.ForeColor = System.Drawing.Color.Black;
                lblInfo.Text = "Operation successful.";
            }
            else
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Error. Please ensure that the passwords match.";
            }
        }

        #endregion events
    }
}