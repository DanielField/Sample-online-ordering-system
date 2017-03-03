using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CharityKitchenWebDatabase.Extensions;

namespace CharityKitchenWebDatabase.Ingredients
{
    /// <summary>
    /// The purpose of this page is to display all of the ingredients in the system, and to allow the user to delete/add/edit ingredients.
    /// </summary>
    public partial class Ingredients : System.Web.UI.Page
    {
        #region vars
        static SvcKitchen.Ingredient ingredient;

        // Boolean that determines whether the ingredient is to be added, or if it is to be updated.
        static bool isNewIngredient;
        #endregion vars

        #region init
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            switch (this.GetAccessLevel("Ingredients"))
            {
                case AccessLevel.Deny:
                    Response.Redirect("~/Default");
                    break;
                case AccessLevel.Read:
                    btnNew.Enabled = false;
                    gvIngredients.Enabled = false;
                    break;
            }

            // Panel styling
            pnlEdit.BackColor = System.Drawing.Color.LightGray;
            pnlEdit.Width = 350;

            // attempt to load the meals into the gvMeals GridView
            try
            {
                LoadIngredients();
            }
            catch
            {
                lblInfo.ForeColor = System.Drawing.Color.DarkRed;
                lblInfo.Text = "Looks like there was an error attempting to load the data.";
            }
        }

        /// <summary>
        /// Loads the ingredients and displays them.
        /// </summary>
        private void LoadIngredients()
        {
            SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();
            SvcKitchen.ServiceResult result = new SvcKitchen.ServiceResult();

            gvIngredients.DataSource = svc.GetIngredients(out result);
            gvIngredients.DataBind();
            lblInfo.DisplayResult(result);
        }

        #endregion init

        #region GridView events

        protected void gvIngredients_SelectedIndexChanged(object sender, EventArgs e)
        {
            ingredient = new SvcKitchen.Ingredient();
            ingredient.ID = int.Parse(gvIngredients.SelectedRow.Cells[2].Text);
            ingredient.Name = gvIngredients.SelectedRow.Cells[3].Text;

            pnlEdit.Visible = true;

            isNewIngredient = false;

            lblID.Text = ingredient.ID.ToString();
            txtIngredientName.Text = ingredient.Name;
        }

        protected void gvIngredients_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();
                SvcKitchen.ServiceResult result = new SvcKitchen.ServiceResult();

                result = svc.DeleteIngredient(int.Parse(gvIngredients.Rows[e.RowIndex].Cells[2].Text));

                lblInfo.DisplayResult(result);

                pnlEdit.Visible = false;

                try
                {
                    LoadIngredients();
                }
                catch
                {
                    lblInfo.Text = "There was an error attempting to load the Data.";
                    lblInfo.ForeColor = System.Drawing.Color.DarkRed;
                }
            }
            catch
            {
                lblInfo.ForeColor = System.Drawing.Color.DarkRed;
                lblInfo.Text = "There was an error attempting to delete the ingredient.";
            }
        }

        #endregion GridView events

        #region button events

        protected void btnNew_Click(object sender, EventArgs e)
        {
            pnlEdit.Visible = true;

            ingredient = new SvcKitchen.Ingredient();
            isNewIngredient = true;

            lblID.Text = "";
            txtIngredientName.Text = "";
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();
                SvcKitchen.ServiceResult result = new SvcKitchen.ServiceResult();

                GetIngredientFromForm(isNewIngredient);

                if (ingredient.ID == 0)
                    result = svc.AddIngredient(ingredient.Name);
                else
                    result = svc.UpdateIngredient(ingredient.Name, ingredient.ID);

                lblInfo.DisplayResult(result);

                pnlEdit.Visible = false;
                lblID.Text = "";
                txtIngredientName.Text = "";

                try
                {
                    LoadIngredients();
                }
                catch
                {
                    lblInfo.ForeColor = System.Drawing.Color.DarkRed;
                    lblInfo.Text = "Looks like there was an error attempting to load the data.";
                }
            }
            catch
            {
                lblInfo.Text = "There was an error attempting to submit the ingredient.";
                lblInfo.ForeColor = System.Drawing.Color.DarkRed;
            }
        }

        #endregion button events

        #region methods

        /// <summary>
        /// Get the name and ID from the form. If the ingredient is new, set the ID to zero.
        /// </summary>
        /// <param name="_isNew">Boolean to determine whether the ingredient is new or if the ingredient already exists.</param>
        private void GetIngredientFromForm(bool _isNew)
        {
            if (_isNew)
            {
                ingredient.Name = txtIngredientName.Text;
                ingredient.ID = 0;
            }
            else
            {
                ingredient.Name = txtIngredientName.Text;
                ingredient.ID = int.Parse(lblID.Text);
            }
        }

        #endregion methods
    }
}