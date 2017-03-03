using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CharityKitchenWebDatabase
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            var svc = new SvcKitchen.SvcKitchenSoapClient();

            SvcKitchen.ServiceResult result;
            var user = svc.UserLogin(txtUsername.Text, txtPassword.Text, out result);

            lblInfo.Text = result.Message;

            if (user.ID > 0) // If the user is valid
            {
                Session["user"] = user;
                Response.Redirect("~/Default");
            }
            else // if the user is invalid
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Invalid credentials";
            }
        }
    }
}