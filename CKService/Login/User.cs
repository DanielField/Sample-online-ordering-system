using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace CKService
{
    public class User
    {
        #region vars

        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public RoleCombo[] Roles { get; set; }

        #endregion vars

        #region init

        public User() { }

        public User(OleDbDataReader reader)
        {
            List<RoleCombo> roles = new List<RoleCombo>();

            while (reader.Read())
            {
                ID = (int)reader["UserID"];
                Username = (string)reader["Username"];
                Password = (string)reader["Password"];
                roles.Add(new RoleCombo((string)reader["RoleDescription"], (int)reader["AccessLevel"]));
            }

            Roles = roles.ToArray();
        }

        #endregion init
    }
}