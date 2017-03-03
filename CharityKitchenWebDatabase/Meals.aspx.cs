using CharityKitchenWebDatabase.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CharityKitchenWebDatabase
{
    /// <summary>
    /// This page contains a list of meals (recipes). Select a meal, and it will display a list of meal lines (recipe ingredients). 
    /// You can edit the meals and the meal lines by following the instructions given on the help section.
    /// </summary>
    public partial class Meals : System.Web.UI.Page
    {
        #region vars
        static SvcKitchen.Meal meal;
        static SvcKitchen.MealLine mealLine;

        static bool isNewMeal, isNewMealLine;
        #endregion vars

        #region init
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            switch (this.GetAccessLevel("Meals"))
            {
                case AccessLevel.Deny:
                    Response.Redirect("~/Default");
                    break;
                case AccessLevel.Read:
                    btnNew.Enabled = false;
                    gvMeals.Enabled = false;
                    break;
            }

            // Panel styling
            pnlEdit.BackColor = System.Drawing.Color.LightGray;
            pnlEditMealLine.BackColor = System.Drawing.Color.LightGray;
            pnlEdit.Width = 350;
            pnlEditMealLine.Width = 350;

            // attempt to load the meals into the gvMeals GridView
            try 
            {
                LoadMeals();
            }
            catch 
            {
                lblInfo.ForeColor = System.Drawing.Color.DarkRed;
                lblInfo.Text = "Looks like there was an error attempting to load the data.";
            }
        }
        #endregion init

        #region meal methods
        /// <summary>
        /// Loads the meals and displays them.
        /// </summary>
        private void LoadMeals()
        {
            SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();
            SvcKitchen.ServiceResult result = new SvcKitchen.ServiceResult();

            gvMeals.DataSource = svc.GetMeals(out result);
            gvMeals.DataBind();
            lblInfo.DisplayResult(result);
        }

        /// <summary>
        /// Instantiate a new meal, set the id and name of the meal to the data from within the grid view, at the selected row.
        /// show the edit panel, and set isNew to false.
        /// Display the meal in the editing panel.
        /// MEAL LINE SHIT GOES HERE.
        /// </summary>
        protected void gvMeals_SelectedIndexChanged(object sender, EventArgs e)
        {
            // meals
            meal = new SvcKitchen.Meal();
            meal.ID = int.Parse(gvMeals.SelectedRow.Cells[2].Text);
            meal.Name = gvMeals.SelectedRow.Cells[3].Text;

            pnlEdit.Visible = true;

            isNewMeal = false;

            lblID.Text = meal.ID.ToString();
            txtMeal.Text = meal.Name;

            // meal lines
            try
            {
                LoadMealLines();
                btnNewLine.Visible = true;
                PopulateIngredients();
            }
            catch
            {
                lblInfo.Text = "There was an error attempting to load the recipe data.";
                lblInfo.ForeColor = System.Drawing.Color.DarkRed;
            }
        }

        /// <summary>
        /// Show the editing panel and clear the form, set the isNew boolean to true, and instantiate a new meal.
        /// </summary>
        protected void btnNew_Click(object sender, EventArgs e)
        {
            pnlEdit.Visible = true;
            
            meal = new SvcKitchen.Meal();
            isNewMeal = true;

            lblID.Text = "";
            txtMeal.Text = "";
        }

        /// <summary>
        /// Delete the meal, and the meal lines that are related to the meal.
        /// </summary>
        protected void gvMeals_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();
                SvcKitchen.ServiceResult result = new SvcKitchen.ServiceResult();

                // Delete meal lines to prevent the meal lines from having no corresponding meal.
                result = svc.DeleteMealLineByMealID(int.Parse(gvMeals.Rows[e.RowIndex].Cells[2].Text));
                // If there is no error deleting the meal lines, delete the meal.
                if(result.ErrorCode == 0)
                    result = svc.DeleteMeal(int.Parse(gvMeals.Rows[e.RowIndex].Cells[2].Text));

                // set the colour of the label to red if there is an error, else green; and then display the result's message.
                lblInfo.DisplayResult(result);

                pnlEdit.Visible = false;

                try
                {
                    LoadMeals();
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
                lblInfo.Text = "There was an error attempting to delete the meal/recipe.";
            }
            
        }

        /// <summary>
        /// Get the meal name and ID from the form. If the meal is new, set the ID to zero.
        /// </summary>
        /// <param name="_isNew">Boolean to determine whether the meal is new or if the meal already exists.</param>
        private void GetMealFromForm(bool _isNew)
        {
            if (_isNew)
            {
                meal.Name = txtMeal.Text;
                meal.ID = 0;
            }
            else
            {
                meal.Name = txtMeal.Text;
                meal.ID = int.Parse(lblID.Text);
            }
        }

        /// <summary>
        /// If the recipe is new, insert it. If the recipe is an existing recipe, update it.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event arguments</param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();
                SvcKitchen.ServiceResult result = new SvcKitchen.ServiceResult();

                GetMealFromForm(isNewMeal);

                if (meal.ID == 0)
                    result = svc.AddMeal(meal.Name);
                else
                    result = svc.UpdateMeal(meal);

                lblInfo.DisplayResult(result);

                pnlEdit.Visible = false;
                lblID.Text = "";
                txtMeal.Text = "";

                try
                {
                    LoadMeals();
                }
                catch
                {
                    lblInfo.ForeColor = System.Drawing.Color.DarkRed;
                    lblInfo.Text = "Looks like there was an error attempting to load the data.";
                }
            }
            catch
            {
                lblInfo.Text = "There was an error attempting to submit the recipe.";
                lblInfo.ForeColor = System.Drawing.Color.DarkRed;
            }
        }
        #endregion meal methods

        #region meal line methods
        /// <summary>
        /// Loads the meal lines and displays them.
        /// </summary>
        private void LoadMealLines()
        {
            SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();
            SvcKitchen.ServiceResult result = new SvcKitchen.ServiceResult();

            gvMealLines.DataSource = svc.GetMealLines(meal.ID, out result);
            gvMealLines.DataBind();

            lblLineInfo.DisplayResult(result);
        }

        /// <summary>
        /// Loads the meal lines and displays them.
        /// </summary>
        private void PopulateIngredients()
        {
            SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();
            SvcKitchen.ServiceResult result = new SvcKitchen.ServiceResult();

            cboIngredients.DataSource = svc.GetIngredients(out result);
            cboIngredients.DataValueField = "ID";
            cboIngredients.DataTextField = "Name";
            cboIngredients.DataBind();

            lblLineInfo.DisplayResult(result);
        }

        /// <summary>
        /// Clear the meal line (recipe ingredient) form.
        /// </summary>
        private void ClearMealLineForm()
        {
            lblMealLineID.Text = "";
            txtQuantity.Text = "";
            cboIngredients.SelectedIndex = 0;
        }

        /// <summary>
        /// Get the meal line data from the form. If the meal line is new, set the id to zero.
        /// </summary>
        /// <param name="_isNew">Boolean to determine whether the meal is new or if the meal already exists.</param>
        private void GetMealLineFromForm(bool _isNew)
        {
            if (_isNew)
            {
                mealLine.ID = 0;
                mealLine.IngredientID = int.Parse(cboIngredients.SelectedItem.Value);
                mealLine.IngredientQuantity = int.Parse(txtQuantity.Text);
                mealLine.MealID = meal.ID;
            }
            else
            {
                mealLine.ID = int.Parse(lblMealLineID.Text);
                mealLine.IngredientID = int.Parse(cboIngredients.SelectedItem.Value);
                mealLine.IngredientQuantity = int.Parse(txtQuantity.Text);
                mealLine.MealID = meal.ID;
            }
        }

        /// <summary>
        /// Show the editing panel, set the isNewMealLine boolean to true, instantiate a new MealLine, and clear the form.
        /// </summary>
        protected void btnNewLine_Click(object sender, EventArgs e)
        {
            pnlEditMealLine.Visible = true;

            isNewMealLine = true;
            mealLine = new SvcKitchen.MealLine();

            ClearMealLineForm();
        }

        protected void gvMealLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            // set mealLine to contain the data of the selected row
            mealLine = new SvcKitchen.MealLine();
            mealLine.ID = int.Parse(gvMealLines.SelectedRow.Cells[2].Text);
            mealLine.MealID = int.Parse(gvMealLines.SelectedRow.Cells[3].Text);
            mealLine.IngredientID = int.Parse(gvMealLines.SelectedRow.Cells[4].Text);
            mealLine.IngredientQuantity = int.Parse(gvMealLines.SelectedRow.Cells[5].Text);

            pnlEditMealLine.Visible = true;

            // Not a new line, so set it to false
            isNewMealLine = false;

            // Display data
            lblMealLineID.Text = mealLine.ID.ToString();
            cboIngredients.SelectedValue = mealLine.IngredientID.ToString();
            txtQuantity.Text = mealLine.IngredientQuantity.ToString();
        }

        protected void gvMealLines_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();
                SvcKitchen.ServiceResult result = new SvcKitchen.ServiceResult();

                // delete by meal line id
                result = svc.DeleteMealLine(int.Parse(gvMealLines.Rows[e.RowIndex].Cells[2].Text));

                // set the colour of the label to red if there is an error, else green; and then display the result's message.
                lblLineInfo.DisplayResult(result);

                pnlEditMealLine.Visible = false;

                // reload the meal lines
                try
                {
                    LoadMealLines();
                }
                catch
                {
                    lblLineInfo.Text = "There was an error attempting to load the Data.";
                    lblLineInfo.ForeColor = System.Drawing.Color.DarkRed;
                }
            }
            catch
            {
                lblLineInfo.ForeColor = System.Drawing.Color.DarkRed;
                lblLineInfo.Text = "There was an error attempting to delete the meal/recipe.";
            }
        }

        protected void btnSubmitLine_Click(object sender, EventArgs e)
        {
            try
            {
                SvcKitchen.SvcKitchenSoapClient svc = new SvcKitchen.SvcKitchenSoapClient();
                SvcKitchen.ServiceResult result = new SvcKitchen.ServiceResult();

                // get the meal line
                GetMealLineFromForm(isNewMealLine);

                // add/update it
                result = svc.AddMealLine(mealLine);
                
                lblLineInfo.DisplayResult(result);

                // reset the form
                pnlEditMealLine.Visible = false;
                ClearMealLineForm();

                // reload
                try
                {
                    LoadMealLines();
                }
                catch
                {
                    lblInfo.ForeColor = System.Drawing.Color.DarkRed;
                    lblInfo.Text = "Looks like there was an error attempting to load the data.";
                }
            }
            catch
            {
                lblInfo.Text = "There was an error attempting to submit the recipe.";
                lblInfo.ForeColor = System.Drawing.Color.DarkRed;
            }
        }
        #endregion meal line methods
    }
}