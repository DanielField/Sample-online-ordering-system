using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace CKService
{
    public class Meal
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public Meal()
        {
            ID = 0;
            Name = "";
        }

        public Meal(OleDbDataReader reader)
        {
            ID = (int)reader[MEAL_ID];
            Name = reader[MEAL_NAME].ToString();
        }

        public const string MEAL_ID = "MealID";
        public const string MEAL_NAME = "MealName";
        public const string MEAL_TABLE = "tblMeals";
    }
}