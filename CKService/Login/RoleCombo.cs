using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CKService
{
    public class RoleCombo
    {
        public string Role { get; set; }
        public int Level { get; set; }

        public RoleCombo() { }

        public RoleCombo(string _role, int _level)
        {
            Role = _role;
            Level = _level;
        }
    }
}