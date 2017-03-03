using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace CKService
{
    public class Ingredient
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public Ingredient()
        {
            ID = 0;
            Name = "";
        }

        public Ingredient(OleDbDataReader reader)
        {
            ID = (int)reader[INGREDIENT_ID];
            Name = reader[INGREDIENT_NAME].ToString();
        }

        public const string INGREDIENT_ID = "IngredientID";
        public const string INGREDIENT_NAME = "IngredientName";
    }
}