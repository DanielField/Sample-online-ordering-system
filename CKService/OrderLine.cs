using System.Data.OleDb;

namespace CKService
{
    public class OrderLine
    {
        public int OrderLineID { get; set; }
        public int MealID { get; set; }
        public string MealName { get; set; }
        public int Quantity { get; set; }
        public int OrderID { get; set; }

        public OrderLine()
        {
            OrderLineID = 0;
            MealID = 0;
            Quantity = 0;
            OrderID = 0;
            MealName = "";
        }

        public OrderLine(OleDbDataReader reader)
        {
            OrderLineID = (int)reader[ORDER_LINE_ID];
            MealID = (int)reader[Meal.MEAL_ID];
            Quantity = (int)reader[QUANTITY];
            OrderID = (int)reader[Order.ORDER_ID];
            MealName = (string)reader[Meal.MEAL_NAME];
        }

        public const string ORDER_LINE_ID = "OrderLineID";
        public const string QUANTITY = "Quantity";
        public const string ORDERLINE_TABLE = "tblOrderLines";
        public const string ORDERLINE_TABLE_QRY = "qryOrderLines";
    }
}