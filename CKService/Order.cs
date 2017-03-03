using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace CKService
{
    public class Order
    {
        public int ID { get; set; }
        public DateTime OrderDate { get; set; }
        public string DeliveryAddress { get; set; }
        public string PhoneNumber { get; set; }

        public Order()
        {
            ID = 0;
            OrderDate = DateTime.Now;
            DeliveryAddress = "";
            PhoneNumber = "";
        }

        public Order(OleDbDataReader reader)
        {
            ID = (int)reader[ORDER_ID];
            OrderDate = (DateTime)reader[ORDER_DATE];
            DeliveryAddress = reader[DELIVERY_ADDRESS].ToString();
            PhoneNumber = reader[PHONE_NUMBER].ToString();
        }

        public const string ORDER_ID = "OrderID";
        public const string ORDER_DATE = "OrderDate";
        public const string DELIVERY_ADDRESS = "DeliveryAddress";
        public const string PHONE_NUMBER = "PhoneNumber";
        public const string ORDER_TABLE = "tblOrders";
    }
}