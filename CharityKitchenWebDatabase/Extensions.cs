using CharityKitchenWebDatabase.SvcKitchen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CharityKitchenWebDatabase.Extensions
{
    public enum AccessLevel { Deny, Read, Write }

    public static class ExtensionObject
    {
        /// <summary>
        /// Gets the access level for the specified role
        /// </summary>
        /// <param name="p">The page</param>
        /// <param name="role">The role that gets specified</param>
        /// <returns></returns>
        public static AccessLevel GetAccessLevel(this Page p, string role)
        {
            try
            {
                // Moved these two statements into the try-catch. 
                // This prevents the bug where if i logout and try to access a webpage while logged out, it would have a null value.
                var u = (SvcKitchen.User)p.Session["user"];
                RoleCombo userRole = (from r in u.Roles
                                      where r.Role.Contains(role)
                                      select r).FirstOrDefault();
                switch (userRole.Level)
                {
                    case 1:
                        return AccessLevel.Read;
                    case 2:
                        return AccessLevel.Write;
                    default:
                        return AccessLevel.Deny;
                }
            }
            catch
            {
                return AccessLevel.Deny;
            }
        }

        /// <summary>
        /// Set data source and then bind the data.
        /// </summary>
        /// <param name="ddl">drop down list</param>
        /// <param name="dataSource">data source</param>
        public static void DataSourceAndBind(this DropDownList ddl, object dataSource)
        {
            ddl.DataSource = dataSource;
            ddl.DataBind();
        }

        /// <summary>
        /// Sets the label ForeColour to dark red if there is an error, dark green if there is no error.
        /// Sets the text to the result's message.
        /// </summary>
        /// <param name="lbl">Label to modify.</param>
        /// <param name="result">ServiceResult object.</param>
        public static void DisplayResult(this Label lbl, SvcKitchen.ServiceResult result)
        {
            if (result.ErrorCode == 0)
                lbl.ForeColor = System.Drawing.Color.DarkGreen;
            else
                lbl.ForeColor = System.Drawing.Color.DarkRed;
            lbl.Text = result.Message;
        }

        /// <summary>
        /// Set data source, set DataValueField and DataTextField and then bind the data.
        /// </summary>
        /// <param name="ddl">drop down list</param>
        /// <param name="valueMember">DataValueField</param>
        /// <param name="displayMember">DataTextField</param>
        /// <param name="dataSource">data source</param>
        public static void DataSourceAndBind(this DropDownList ddl, string valueMember, string displayMember, object dataSource)
        {
            ddl.DataSource = dataSource;
            ddl.DataValueField = valueMember;
            ddl.DataTextField = displayMember;
            ddl.DataBind();
        }
    }
}