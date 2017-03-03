using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;

namespace CKService
{
    /// <summary>
    /// This class contains all of the methods that interact with the database.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SvcKitchen : System.Web.Services.WebService
    {
        private const string CONNECTION_STRING = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|kitchen.accdb;Persist Security Info=True";

        #region Orders

        /// <summary>
        /// Get all of the orders in an array.
        /// </summary>
        /// <param name="result">returns a ServiceResult, containing error information.</param>
        /// <returns>List of Orders.</returns>
        [WebMethod]
        public Order[] GetOrders(out ServiceResult result)
        {
            List<Order> orders = new List<Order>();

            try
            {
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand("SELECT * FROM tblOrders", dbConn);
                dbConn.Open();
                OleDbDataReader reader = dbCmd.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(new Order(reader));
                }
                dbConn.Close();

                result = new ServiceResult();
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
                orders = new List<Order>();
            }

            return orders.ToArray();
        }

        /// <summary>
        /// Gets an order with the specified id.
        /// </summary>
        /// <param name="id">primary key of the order.</param>
        /// <param name="result">returns a ServiceResult, containing error information.</param>
        /// <returns>Order object.</returns>
        [WebMethod]
        public Order GetOrderByID(int id, out ServiceResult result)
        {
            Order order = new Order();

            try
            {
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand("SELECT * FROM tblOrders WHERE OrderID = " + id, dbConn);
                dbConn.Open();
                OleDbDataReader reader = dbCmd.ExecuteReader();
         
                reader.Read();
                order.ID = (int)reader["OrderID"];
                order.OrderDate = (DateTime)reader["OrderDate"];
                order.DeliveryAddress = (string)reader["DeliveryAddress"];
                order.PhoneNumber = (string)reader["PhoneNumber"];

                dbConn.Close();
                result = new ServiceResult();
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
                order = new Order();
            }

            return order;
        }

        /// <summary>
        /// Inserts a new order into the database if the id is zero, else it will update.
        /// </summary>
        /// <param name="record">Order to insert/update.</param>
        /// <param name="result">returns a ServiceResult, containing error information.</param>
        /// <returns>Order object</returns>
        [WebMethod]
        public void addOrder(Order record, out ServiceResult result)
        {
            string query = "";

            if (record.ID == 0)
            {
                // insert
                query = "INSERT INTO tblOrders (OrderDate, DeliveryAddress, PhoneNumber) ";
                query += "VALUES (\"{0}\", \"{1}\", \"{2}\")";
                query = string.Format(query, record.OrderDate, record.DeliveryAddress, record.PhoneNumber);
            }
            else
            {
                // update
                query = "UPDATE tblOrders SET DeliveryAddress=\"{0}\", PhoneNumber=\"{1}\" "
                      + "WHERE OrderID={2}";
                query = string.Format(query, record.DeliveryAddress, record.PhoneNumber, record.ID);
            }
            try
            {
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand(query, dbConn);
                dbConn.Open();
                dbCmd.ExecuteNonQuery();
                dbConn.Close();
                result = new ServiceResult();
            }
            catch (OleDbException ole)
            {
                // database error
                result = new ServiceResult(1, "There was an issue with the database", ole);
            }
            catch (FormatException fmt)
            {
                // bad data
                result = new ServiceResult(2, "Submitted data was incorrect", fmt);
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
            }
        }

        /// <summary>
        /// Deletes an order from the database.
        /// This method will also delete all order lines associated with the specified order.
        /// </summary>
        /// <param name="id">Primary key of the order being deleted.</param>
        /// <param name="result">returns a ServiceResult, containing error information.</param>
        [WebMethod]
        public void DeleteOrder(int id, out ServiceResult result)
        {
            try
            {
                DeleteOrderLineByOrderID(id, out result);

                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand("DELETE FROM tblOrders WHERE OrderID=" + id, dbConn);
                dbConn.Open();
                dbCmd.ExecuteNonQuery();
                dbConn.Close();

                result = new ServiceResult();
            }
            catch (OleDbException ex)
            {
                result = new ServiceResult(1, "There was an issue with the database", ex);
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
            }
        }

        #endregion Orders

        #region Order lines

        /// <summary>
        /// Inserts/Updates an order line.
        /// </summary>
        /// <param name="record">Order line object</param>
        /// <param name="result">returns a ServiceResult, containing error information.</param>
        [WebMethod]
        public void AddOrderLine(OrderLine record, out ServiceResult result)
        {
            string query = "";

            if (record.OrderLineID == 0)
            {
                // insert
                query = "INSERT INTO tblOrderLines (MealID, Quantity, OrderID) ";
                query += "VALUES ({0}, {1}, {2})";
                query = string.Format(query, record.MealID, record.Quantity, record.OrderID);
            }
            else
            {
                // update
                query = "UPDATE tblOrderLines SET MealID={0}, Quantity={1}, OrderID={2} "
                      + "WHERE OrderLineID=" + record.OrderLineID;
                query = string.Format(query, record.MealID, record.Quantity, record.OrderID);
            }
            try
            {
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand(query, dbConn);
                dbConn.Open();
                dbCmd.ExecuteNonQuery();
                dbConn.Close();
                result = new ServiceResult();
            }
            catch (OleDbException ole)
            {
                // database error
                result = new ServiceResult(1, "There was an issue with the database", ole);
            }
            catch (FormatException fmt)
            {
                // bad data
                result = new ServiceResult(2, "Submitted data was incorrect", fmt);
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
            }
        }

        /// <summary>
        /// Gets an array of order lines where the order id equals the specified integer.
        /// </summary>
        /// <param name="id">Foreign key of the Order.</param>
        /// <param name="result">returns a ServiceResult, containing error information.</param>
        /// <returns>Returns an array of OrderLine.</returns>
        [WebMethod]
        public OrderLine[] GetOrderLines(int id, out ServiceResult result)
        {
            List<OrderLine> orderLines = new List<OrderLine>();

            try
            {
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand("SELECT * FROM qryOrderLines WHERE OrderID = " + id, dbConn);
                dbConn.Open();
                OleDbDataReader reader = dbCmd.ExecuteReader();
                while (reader.Read())
                {
                    orderLines.Add(new OrderLine(reader));
                }
                dbConn.Close();

                result = new ServiceResult();
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
                orderLines = new List<OrderLine>();
            }

            return orderLines.ToArray();
        }

        /// <summary>
        /// Gets the specified order line.
        /// </summary>
        /// <param name="id">Primary key of the order line.</param>
        /// <param name="result">returns a ServiceResult, containing error information.</param>
        /// <returns>OrderLine object</returns>
        [WebMethod]
        public OrderLine GetByOrderLineId(int id, out ServiceResult result)
        {
            OrderLine record;

            try
            {
                string query = "SELECT * FROM qryOrderLines WHERE OrderLineID = " + id;
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand(query, dbConn);
                dbConn.Open();
                OleDbDataReader reader = dbCmd.ExecuteReader();
                reader.Read();
                record = new OrderLine(reader);
                dbConn.Close();

                result = new ServiceResult();
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
                record = new OrderLine();
            }

            return record;
        }

        /// <summary>
        /// Deletes the specified order line.
        /// </summary>
        /// <param name="id">Primary key of the order line</param>
        /// <param name="result">returns a ServiceResult, containing error information.</param>
        [WebMethod]
        public void DeleteOrderLine(int id, out ServiceResult result)
        {
            try
            {
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand("DELETE FROM tblOrderLines WHERE OrderLineID=" + id, dbConn);
                dbConn.Open();
                dbCmd.ExecuteNonQuery();
                dbConn.Close();

                result = new ServiceResult();
            }
            catch (OleDbException ex)
            {
                result = new ServiceResult(1, "There was an issue with the database", ex);
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
            }
        }

        /// <summary>
        /// Deletes the specified order line by the Order id, instead of the OrderLine id.
        /// </summary>
        /// <param name="id">Primary key of the order line</param>
        /// <param name="result">returns a ServiceResult, containing error information.</param>
        [WebMethod]
        public void DeleteOrderLineByOrderID(int id, out ServiceResult result)
        {
            try
            {
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand("DELETE FROM tblOrderLines WHERE OrderID=" + id, dbConn);
                dbConn.Open();
                dbCmd.ExecuteNonQuery();
                dbConn.Close();

                result = new ServiceResult();
            }
            catch (OleDbException ex)
            {
                result = new ServiceResult(1, "There was an issue with the database", ex);
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
            }
        }

        #endregion Order lines

        #region meals

        /// <summary>
        /// Gets all meals from the database.
        /// </summary>
        /// <param name="result">returns a ServiceResult, containing error information.</param>
        /// <returns>Array of meals.</returns>
        [WebMethod]
        public Meal[] GetMeals(out ServiceResult result)
        {
            List<Meal> meal = new List<Meal>();

            try
            {
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand("SELECT * FROM tblMeals", dbConn);
                dbConn.Open();
                OleDbDataReader reader = dbCmd.ExecuteReader();
                while (reader.Read())
                {
                    meal.Add(new Meal(reader));
                }
                dbConn.Close();

                result = new ServiceResult();
            }
            catch (OleDbException ex)
            {
                result = new ServiceResult(1, "There was an issue with the database", ex);
                meal = new List<Meal>();
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
                meal = new List<Meal>();
            }

            return meal.ToArray();
        }

        /// <summary>
        /// Updates the specified meal with the specified data.
        /// </summary>
        /// <param name="m">Meal that will replace the existing meal in the database.</param>
        /// <param name="result">returns a ServiceResult, containing error information.</param>
        [WebMethod]
        public void UpdateMeal(Meal m, out ServiceResult result)
        {
            try
            {
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand("UPDATE tblMeals SET MealName=\"" + m.Name + "\" WHERE MealID=" + m.ID, dbConn);
                dbConn.Open();
                dbCmd.ExecuteNonQuery();
                dbConn.Close();

                result = new ServiceResult();
            }
            catch (OleDbException ex)
            {
                result = new ServiceResult(1, "There was an issue with the database", ex);
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
            }
        }

        /// <summary>
        /// Deletes the specified meal from the database.
        /// </summary>
        /// <param name="id">Primary key of the meal being deleted.</param>
        /// <param name="result">returns a ServiceResult, containing error information.</param>
        [WebMethod]
        public void DeleteMeal(int id, out ServiceResult result)
        {
            try
            {
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand("DELETE FROM tblMeals WHERE MealID=" + id, dbConn);
                dbConn.Open();
                dbCmd.ExecuteNonQuery();
                dbConn.Close();

                result = new ServiceResult();
            }
            catch (OleDbException ex)
            {
                result = new ServiceResult(1, "There was an issue with the database", ex);
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
            }
        }

        /// <summary>
        /// Inserts a meal into the meals table with the specified name.
        /// </summary>
        /// <param name="mealName">Name of meal.</param>
        /// <param name="result">returns a ServiceResult, containing error information.</param>
        [WebMethod]
        public void AddMeal(string mealName, out ServiceResult result)
        {
            try
            {
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand("INSERT INTO tblMeals (MealName) VALUES (\"" + mealName + "\")", dbConn);
                dbConn.Open();
                dbCmd.ExecuteNonQuery();
                dbConn.Close();

                result = new ServiceResult();
            }
            catch (OleDbException ex)
            {
                result = new ServiceResult(1, "There was an issue with the database", ex);
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
            }
        }

        #endregion meals

        #region mealLines

        /// <summary>
        /// Gets all of the meal lines from the database where the meal id equals the specified id
        /// </summary>
        /// <param name="result">returns a ServiceResult, containing error information.</param>
        /// <returns>Array of MealLine objects.</returns>
        [WebMethod]
        public MealLine[] GetMealLines(int mealID, out ServiceResult result)
        {
            List<MealLine> mealLine = new List<MealLine>();

            try
            {
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand("SELECT * FROM qryMealLines WHERE MealID=" + mealID, dbConn);
                dbConn.Open();
                OleDbDataReader reader = dbCmd.ExecuteReader();
                while (reader.Read())
                {
                    mealLine.Add(new MealLine(reader));
                }
                dbConn.Close();

                result = new ServiceResult();
            }
            catch (OleDbException ex)
            {
                result = new ServiceResult(1, "There was an issue with the database", ex);
                mealLine = new List<MealLine>();
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
                mealLine = new List<MealLine>();
            }

            return mealLine.ToArray();
        }

        /// <summary>
        /// Deletes the specified meal line.
        /// </summary>
        /// <param name="id">Primary key of the meal line</param>
        /// <param name = "result" > returns a ServiceResult, containing error information.</param>
        [WebMethod]
        public void DeleteMealLine(int id, out ServiceResult result)
        {
            try
            {
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand("DELETE FROM tblMealLines WHERE MealLineID=" + id, dbConn);
                dbConn.Open();
                dbCmd.ExecuteNonQuery();
                dbConn.Close();

                result = new ServiceResult();
            }
            catch (OleDbException ex)
            {
                result = new ServiceResult(1, "There was an issue with the database", ex);
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
            }
        }

        /// <summary>
        /// Deletes the meal lines that have the specified MealID.
        /// </summary>
        /// <param name="id">Primary key of the meal.</param>
        /// <param name = "result" > returns a ServiceResult, containing error information.</param>
        [WebMethod]
        public void DeleteMealLineByMealID(int id, out ServiceResult result)
        {
            try
            {
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand("DELETE FROM tblMealLines WHERE MealID=" + id, dbConn);
                dbConn.Open();
                dbCmd.ExecuteNonQuery();
                dbConn.Close();

                result = new ServiceResult();
            }
            catch (OleDbException ex)
            {
                result = new ServiceResult(1, "There was an issue with the database", ex);
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
            }
        }

        /// <summary>
        /// Adds a meal line to the database.
        /// </summary>
        /// <param name="mealID">The id of the meal.</param>
        /// <param name="ingredientID">the id of the ingredient.</param>
        /// <param name="quantity">the ingredient quantity.</param>
        /// <param name="result">returns a ServiceResult, containing error information.</param>
        [WebMethod]
        public void AddMealLine(MealLine record, out ServiceResult result)
        {
            string query = "";

            if (record.ID == 0)
            {
                // insert
                query = "INSERT INTO tblMealLines (MealID, IngredientID, IngredientQuantity) ";
                query += "VALUES ({0}, {1}, {2})";
                query = string.Format(query, record.MealID, record.IngredientID, record.IngredientQuantity);
            }
            else
            {
                // update
                query = "UPDATE tblMealLines SET MealID={0}, IngredientID={1}, IngredientQuantity={2} "
                      + "WHERE MealLineID=" + record.ID;
                query = string.Format(query, record.MealID, record.IngredientID, record.IngredientQuantity);
            }

            try
            {
                // scrap: string.Format("INSERT INTO tblMealLines (MealID, IngredientID, IngredientQuantity) VALUES ({0},{1},{2})", mealID, ingredientID, quantity)
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand(query, dbConn);
                dbConn.Open();
                dbCmd.ExecuteNonQuery();
                dbConn.Close();

                result = new ServiceResult();
            }
            catch (OleDbException ex)
            {
                result = new ServiceResult(1, "There was an issue with the database", ex);
            }
            catch (FormatException ex)
            {
                result = new ServiceResult(2, "Submitted data was incorrect", ex);
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
            }
        }

        #endregion mealLines

        #region users

        /// <summary>
        /// Gets a user from the users table where the username and password match the specified data.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="result">returns a ServiceResult, containing error information.</param>
        /// <returns>Returns the user that matches the credentials. If there is an exception, return a new User object.</returns>
        [WebMethod]
        public User UserLogin(string username, string password, out ServiceResult result)
        {
            try
            {
                string query = "SELECT * FROM qryUserLogin WHERE Username=\"{0}\" AND Password=\"{1}\"";
                query = string.Format(query, username, password);
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand(query, dbConn);
                dbConn.Open();
                OleDbDataReader reader = dbCmd.ExecuteReader();
                User u = new User(reader);
                dbConn.Close();

                result = new ServiceResult();
                return u;
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
                return new User();
            }
        }

        /// <summary>
        /// Get a list of all users.
        /// </summary>
        /// <param name="result">returns a ServiceResult, containing error information.</param>
        /// <returns>List of user objects</returns>
        [WebMethod]
        public List<User> GetAllUsers(out ServiceResult result)
        {
            List<User> users = new List<User>();
            try
            {
                string query = "SELECT * FROM tblUsers";
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand(query, dbConn);
                dbConn.Open();
                OleDbDataReader reader = dbCmd.ExecuteReader();
                while (reader.Read())
                {
                    User u = new User();
                    u.ID = (int)reader["UserID"];
                    u.Username = reader["Username"].ToString();
                    u.Password = reader["Password"].ToString();
                    users.Add(u);
                }
                dbConn.Close();

                result = new ServiceResult();
            }
            catch (OleDbException ole)
            {
                // database error
                result = new ServiceResult(1, "There was an issue with the database", ole);
            }
            catch (FormatException fmt)
            {
                // bad data
                result = new ServiceResult(2, "Submitted data was incorrect", fmt);
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
            }
            return users;
        }

        /// <summary>
        /// Gets the user that matches the id specified.
        /// </summary>
        /// <param name="id">primary key of the user.</param>
        /// <param name="result">returns a ServiceResult, containing error information.</param>
        /// <returns>User object.</returns>
        [WebMethod]
        public User GetUserByID(int id, out ServiceResult result)
        {
            User user = new User();
            try
            {
                string query = "SELECT * FROM tblUsers WHERE UserID=" + id;
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand(query, dbConn);
                dbConn.Open();
                OleDbDataReader reader = dbCmd.ExecuteReader();
                reader.Read();
                user.ID = (int)reader["UserID"];
                user.Username = reader["Username"].ToString();
                user.Password = reader["Password"].ToString();
                dbConn.Close();

                result = new ServiceResult();
            }
            catch (OleDbException ole)
            {
                // database error
                result = new ServiceResult(1, "There was an issue with the database", ole);
            }
            catch (FormatException fmt)
            {
                // bad data
                result = new ServiceResult(2, "Submitted data was incorrect", fmt);
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
            }
            return user;
        }

        /// <summary>
        /// Inserts a new user into the database.
        /// </summary>
        /// <param name="username">Username of the user being inserted.</param>
        /// <param name="password">Password of the user being inserted.</param>
        /// <param name="dbConn">database connection object.</param>
        /// <param name="dbTrans">database transaction object.</param>
        private static void InsertNewUser(string username, string password, OleDbConnection dbConn, OleDbTransaction dbTrans)
        {
            string query = "INSERT INTO tblUsers (Username, [Password]) VALUES (\"{0}\",\"{1}\")";
            query = string.Format(query, username, password);
            OleDbCommand dbCmd = new OleDbCommand(query, dbConn, dbTrans);
            dbCmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Get the id of the most recently generated user, before finishing the transaction.
        /// </summary>
        /// <param name="dbConn">Database connection object.</param>
        /// <param name="transaction">Database transaction object.</param>
        /// <returns>User id.</returns>
        private static int GetNewUserID(OleDbConnection dbConn, OleDbTransaction transaction)
        {
            OleDbCommand dbCmd = new OleDbCommand("SELECT @@IDENTITY", dbConn, transaction);
            return (int)dbCmd.ExecuteScalar();
        }

        /// <summary>
        /// Insert the default roles for the user specified.
        /// </summary>
        /// <param name="dbConn">Database connection object.</param>
        /// <param name="dbTrans">Database transaction object.</param>
        /// <param name="userID">User id.</param>
        /// <param name="roles">Roles to be added.</param>
        private static void AddDefaultRolesToUser(OleDbConnection dbConn, OleDbTransaction dbTrans, int userID, List<Role> roles)
        {
            // for each role
            // add role line for user
            // each line will default to deny

            foreach (Role role in roles)
            {
                string query = "INSERT INTO tblUserRoles (UserID, RoleID, AccessLevel) ";
                query += "VALUES ({0}, {1}, {2})";
                query = string.Format(query, userID, role.ID, 0);
                OleDbCommand dbCmd = new OleDbCommand(query, dbConn, dbTrans);
                dbCmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Adds a user to the database with the default roles and password.
        /// </summary>
        /// <param name="uname">Username</param>
        /// <param name="result">returns a ServiceResult, containing error information.</param>
        [WebMethod]
        public void AddNewUser(string uname, out ServiceResult result)
        {
            result = new ServiceResult();
            try
            {
                //string query = "INSERT INTO tblUsers (Username, Password) ";
                //query += $"VALUES (\"{uname}\", \"password\")";

                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                //OleDbCommand dbCmd = new OleDbCommand(query, dbConn);

                dbConn.Open();
                OleDbTransaction trans = dbConn.BeginTransaction();

                try
                {
                    if (isUsernameValid(uname) == false)
                    {
                        result = new ServiceResult(5, "Invalid character(s) in username.", null);
                        return;
                    }

                    List<User> users = GetAllUsers(out result);

                    foreach (User u in users)
                    {
                        if (u.Username == uname)
                        {
                            result = new ServiceResult(4, "The username already exists.", null);
                            return;
                        }
                    }
                    InsertNewUser(uname, "password", dbConn, trans);
                    var roles = RoleGetAll(out result);
                    var newUser = GetNewUserID(dbConn, trans);
                    AddDefaultRolesToUser(dbConn, trans, newUser, roles);

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                }
                finally
                {
                    dbConn.Close();
                }
                // attach all roles with level 0 (deny) to user
            }
            catch (OleDbException ole)
            {
                // database error
                result = new ServiceResult(1, "There was an issue with the database", ole);
            }
            catch (FormatException fmt)
            {
                // bad data
                result = new ServiceResult(2, "Submitted data was incorrect", fmt);
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
            }
        }

        /// <summary>
        /// Checks whether the username contains only valid characters
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>true if it's valid, else false</returns>
        private bool isUsernameValid(string username)
        {
            char[] allowedChars = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                                    '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '-', '=',
                                    '!', '@', '#', '$', '%', '^', '&',      '(', ')', '_', '+',
                                    ',', '<', '.', '>', '/', '?', ':', '[', '{', '}', ']', '\\', '`', '~',  };

            foreach (char c in username)
                if (!allowedChars.Contains(c.ToString().ToLower().ToCharArray()[0])) // Converts it to lower case for non-case sensitive comparison
                {
                    return false;
                }
            return true;
        }

        /// <summary>
        /// Deletes the specified user and all associated user roles.
        /// </summary>
        /// <param name="id">Primary key of the user being deleted.</param>
        /// <param name="result">returns a ServiceResult, containing error information.</param>
        [WebMethod]
        public void DeleteUser(int id, out ServiceResult result)
        {
            try
            {
                DeleteUserRoleByUserID(id, out result);

                string query = "DELETE FROM tblUsers WHERE UserID=" + id;
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand(query, dbConn);
                dbConn.Open();
                dbCmd.ExecuteNonQuery();
                dbConn.Close();
                result = new ServiceResult();
            }
            catch (OleDbException ole)
            {
                // database error
                result = new ServiceResult(1, "There was an issue with the database", ole);
            }
            catch (FormatException fmt)
            {
                // bad data
                result = new ServiceResult(2, "Submitted data was incorrect", fmt);
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
            }
        }

        /// <summary>
        /// Updates the user credentials with the specified data.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="id">Primary key of the user.</param>
        /// <param name="result">returns a ServiceResult, containing error information.</param>
        [WebMethod]
        public void UpdateUser(string username, string password, int id, out ServiceResult result)
        {
            try
            {
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand(string.Format("UPDATE tblUsers SET Username=\"{0}\", [Password]=\"{1}\" WHERE UserID={2}", username, password, id), dbConn);
                dbConn.Open();
                dbCmd.ExecuteNonQuery();
                dbConn.Close();

                result = new ServiceResult();
            }
            catch (OleDbException ex)
            {
                result = new ServiceResult(1, "There was an issue with the database", ex);
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
            }
        }

        #endregion users

        #region roles/userRoles

        [WebMethod]
        public List<Role> RoleGetAll(out ServiceResult result)
        {
            List<Role> roles = new List<Role>();

            try
            {
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand("SELECT * FROM tblRoles", dbConn);
                dbConn.Open();
                OleDbDataReader reader = dbCmd.ExecuteReader();
                while (reader.Read())
                {
                    roles.Add(new Role(reader));
                }
                dbConn.Close();

                result = new ServiceResult();
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
                roles = new List<Role>();
            }

            return roles;
        }

        [WebMethod]
        public void RoleSave(Role record, out ServiceResult result)
        {
            string query = "";

            if (record.ID == 0)
            {
                query = "INSERT INTO tblRoles (RoleDescription) VALUES(\"{0}\")";
                query = string.Format(query, record.Description);
            }
            else
            {
                query = "UPDATE tblRoles SET RoleDescription = \"{0}\" WHERE RoleID = {1}";
                query = string.Format(query, record.Description, record.ID);
            }

            try
            {
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand(query, dbConn);
                dbConn.Open();
                dbCmd.ExecuteNonQuery();
                dbConn.Close();
                result = new ServiceResult();
            }
            catch (OleDbException ole)
            {
                // database error
                result = new ServiceResult(1, "There was an issue with the database", ole);
            }
            catch (FormatException fmt)
            {
                // bad data
                result = new ServiceResult(2, "Submitted data was incorrect", fmt);
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
            }
        }

        [WebMethod]
        public void RoleDelete(Role record, out ServiceResult result)
        {
            try
            {
                string query = "DELETE FROM tblRoles WHERE RoleID = " + record.ID;
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand(query, dbConn);
                dbConn.Open();
                dbCmd.ExecuteNonQuery();
                dbConn.Close();
                result = new ServiceResult();
            }
            catch (OleDbException ole)
            {
                // database error
                result = new ServiceResult(1, "There was an issue with the database", ole);
            }
            catch (FormatException fmt)
            {
                // bad data
                result = new ServiceResult(2, "Submitted data was incorrect", fmt);
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
            }
        }

        [WebMethod]
        public Role RoleGetById(int id, out ServiceResult result)
        {
            Role record;

            try
            {
                string query = "SELECT * FROM tblRoles WHERE RoleID = " + id;
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand(query, dbConn);
                dbConn.Open();
                OleDbDataReader reader = dbCmd.ExecuteReader();
                reader.Read();
                record = new Role(reader);
                dbConn.Close();

                result = new ServiceResult();
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
                record = new Role();
            }

            return record;
        }

        [WebMethod]
        public void UpdateUserRole(int userRoleID, int level, out ServiceResult result)
        {
            try
            {
                string query =
                    string.Format("UPDATE tblUserRoles SET AccessLevel={1} WHERE UserRoleID={0}", userRoleID, level);
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand(query, dbConn);
                dbConn.Open();
                dbCmd.ExecuteNonQuery();
                dbConn.Close();
                result = new ServiceResult();
            }
            catch (OleDbException ole)
            {
                // database error
                result = new ServiceResult(1, "There was an issue with the database", ole);
            }
            catch (FormatException fmt)
            {
                // bad data
                result = new ServiceResult(2, "Submitted data was incorrect", fmt);
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
            }
        }

        [WebMethod]
        public List<UserRole> GetUserRolesById(int id, out ServiceResult result)
        {
            List<UserRole> userRoles = new List<UserRole>();
            try
            {
                string query = "SELECT * FROM qryUserRoles WHERE UserID=" + id;
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand(query, dbConn);
                dbConn.Open();
                OleDbDataReader reader = dbCmd.ExecuteReader();
                while (reader.Read())
                {
                    UserRole userRole = new UserRole(reader);
                    userRoles.Add(userRole);
                }
                dbConn.Close();
                result = new ServiceResult();
            }
            catch (OleDbException ole)
            {
                // database error
                result = new ServiceResult(1, "There was an issue with the database", ole);
            }
            catch (FormatException fmt)
            {
                // bad data
                result = new ServiceResult(2, "Submitted data was incorrect", fmt);
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
            }

            return userRoles;
        }

        [WebMethod]
        public void DeleteUserRole(int id, out ServiceResult result)
        {
            try
            {
                string query = "DELETE FROM tblUserRoles WHERE UserRoleID=" + id;
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand(query, dbConn);
                dbConn.Open();
                dbCmd.ExecuteNonQuery();
                dbConn.Close();
                result = new ServiceResult();
            }
            catch (OleDbException ole)
            {
                // database error
                result = new ServiceResult(1, "There was an issue with the database", ole);
            }
            catch (FormatException fmt)
            {
                // bad data
                result = new ServiceResult(2, "Submitted data was incorrect", fmt);
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
            }
        }

        [WebMethod]
        public void DeleteUserRoleByUserID(int id, out ServiceResult result)
        {
            try
            {
                string query = "DELETE FROM tblUserRoles WHERE UserID=" + id;
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand(query, dbConn);
                dbConn.Open();
                dbCmd.ExecuteNonQuery();
                dbConn.Close();
                result = new ServiceResult();
            }
            catch (OleDbException ole)
            {
                // database error
                result = new ServiceResult(1, "There was an issue with the database", ole);
            }
            catch (FormatException fmt)
            {
                // bad data
                result = new ServiceResult(2, "Submitted data was incorrect", fmt);
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
            }
        }

        [WebMethod]
        public void DeleteRole(int id, out ServiceResult result)
        {
            try
            {
                string query = "DELETE FROM tblRoles WHERE RoleID=" + id;
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand(query, dbConn);
                dbConn.Open();
                dbCmd.ExecuteNonQuery();
                dbConn.Close();
                result = new ServiceResult();
            }
            catch (OleDbException ole)
            {
                // database error
                result = new ServiceResult(1, "There was an issue with the database", ole);
            }
            catch (FormatException fmt)
            {
                // bad data
                result = new ServiceResult(2, "Submitted data was incorrect", fmt);
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
            }
        }



        #endregion roles/userRoles

        #region ingredients

        [WebMethod]
        public Ingredient[] GetIngredients(out ServiceResult result)
        {
            List<Ingredient> ing = new List<Ingredient>();

            try
            {
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand("SELECT * FROM tblIngredients", dbConn);
                dbConn.Open();
                OleDbDataReader reader = dbCmd.ExecuteReader();
                while (reader.Read())
                {
                    ing.Add(new Ingredient(reader));
                }
                dbConn.Close();

                result = new ServiceResult();
            }
            catch (OleDbException ex)
            {
                result = new ServiceResult(1, "There was an issue with the database", ex);
                ing = new List<Ingredient>();
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
                ing = new List<Ingredient>();
            }

            return ing.ToArray();
        }

        [WebMethod]
        public void UpdateIngredient(string name, int id, out ServiceResult result)
        {
            try
            {
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand("UPDATE tblIngredients SET ingredientName=\"" + name + "\" WHERE IngredientID=" + id, dbConn);
                dbConn.Open();
                dbCmd.ExecuteNonQuery();
                dbConn.Close();

                result = new ServiceResult();
            }
            catch (OleDbException ex)
            {
                result = new ServiceResult(1, "There was an issue with the database", ex);
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
            }
        }

        [WebMethod]
        public void DeleteIngredient(int id, out ServiceResult result)
        {
            try
            {
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand("DELETE FROM tblIngredients WHERE IngredientID=" + id, dbConn);
                dbConn.Open();
                dbCmd.ExecuteNonQuery();
                dbConn.Close();

                result = new ServiceResult();
            }
            catch (OleDbException ex)
            {
                result = new ServiceResult(1, "There was an issue with the database", ex);
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
            }
        }

        [WebMethod]
        public void AddIngredient(string ingredientName, out ServiceResult result)
        {
            try
            {
                OleDbConnection dbConn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand dbCmd = new OleDbCommand("INSERT INTO tblIngredients (IngredientName) VALUES (\"" + ingredientName + "\")", dbConn);
                dbConn.Open();
                dbCmd.ExecuteNonQuery();
                dbConn.Close();

                result = new ServiceResult();
            }
            catch (OleDbException ex)
            {
                result = new ServiceResult(1, "There was an issue with the database", ex);
            }
            catch (Exception ex)
            {
                result = new ServiceResult(3, "Unspecified error", ex);
            }
        }

        #endregion ingredients
    }
}
