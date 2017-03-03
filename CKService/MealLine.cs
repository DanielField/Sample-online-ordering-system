using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace CKService
{
    public class MealLine
    {
        public int ID { get; set; }
        public int MealID { get; set; }
        public int IngredientID { get; set; }
        public int IngredientQuantity { get; set; }
        public string IngredientName { get; set; }

        public MealLine()
        {
            ID = 0;
            MealID = 0;
            IngredientID = 0;
            IngredientQuantity = 0;
            IngredientName = "";
        }

        public MealLine(OleDbDataReader reader)
        {
            ID = (int)reader[MEAL_LINE_ID];
            MealID = (int)reader[Meal.MEAL_ID];
            IngredientID = (int)reader[Ingredient.INGREDIENT_ID];
            IngredientQuantity = (int)reader[INGREDIENT_QUANTITY];
            IngredientName = (string)reader[INGREDIENT_NAME];
        }

        public const string MEAL_LINE_ID = "MealLineID";
        public const string INGREDIENT_QUANTITY = "IngredientQuantity";
        public const string INGREDIENT_NAME = "IngredientName";
    }
}