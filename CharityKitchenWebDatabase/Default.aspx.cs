using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CharityKitchenWebDatabase.Extensions;

namespace CharityKitchenWebDatabase
{
    public partial class _Default : Page
    {
        // For use on the aspx file.
        public static string username;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            // Disable the buttons where the return value of GetAccessLevel is AccessLevel.Deny
            btnOrders.Enabled = this.GetAccessLevel("Orders") == AccessLevel.Deny ? false : true;
            btnMeals.Enabled = this.GetAccessLevel("Meals") == AccessLevel.Deny ? false : true;
            btnUsers.Enabled = this.GetAccessLevel("Users") == AccessLevel.Deny ? false : true;
            btnIngredients.Enabled = this.GetAccessLevel("Ingredients") == AccessLevel.Deny ? false : true;

            SvcKitchen.User user = Session["user"] as SvcKitchen.User;
            username = user.Username;
        }

        protected void btnOrders_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Orders");
        }

        protected void btnUsers_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Users/Users");
        }

        protected void btnIngredients_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Ingredients");
        }

        protected void btnMeals_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Meals");
        }
    }
}