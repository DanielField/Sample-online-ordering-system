using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace CKService
{
    public class UserRole
    {
        public int UserRoleID { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public int AccessLevel { get; set; }
        public string RoleName { get; set; }

        public UserRole()
        {
        }

        public UserRole(OleDbDataReader reader)
        {
            UserRoleID = Convert.ToInt32(reader["UserRoleID"]);
            UserID = Convert.ToInt32(reader["UserID"]);
            RoleID = Convert.ToInt32(reader["RoleID"]);
            AccessLevel = Convert.ToInt32(reader["AccessLevel"]);
            RoleName = reader["RoleDescription"].ToString();
        }
    }
}