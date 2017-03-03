using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CharityKitchenWebDatabase
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var user = Session["user"] as SvcKitchen.User;
            if (user.ID == 0)
            {
                Response.Redirect("~/Login");
            }

            btnLogout.BorderStyle = BorderStyle.None;
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session["user"] = new SvcKitchen.User();
            Response.Redirect("~/Login");
        }
    }
}