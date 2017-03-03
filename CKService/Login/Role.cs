using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace CKService
{
    public class Role
    {
        public int ID { get; set; }
        public string Description { get; set; }

        public Role() { }

        public Role(OleDbDataReader reader)
        {
            ID = (int)reader["RoleID"];
            Description = (string)reader["RoleDescription"];
        }

    }
}