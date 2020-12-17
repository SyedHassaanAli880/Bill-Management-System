using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MainClass;
using System.Data;

namespace Authentication
{
    public class LoginCodeClass
    {
        private static bool logged = false;

        public static bool get_logged() { return logged; }

        public static void set_logged(bool x) { logged = x; }

        private static int userID, roleID;

        private static string name, role;

        public static int ROLEID //setter and getter for role ID
        {
            get
            {return roleID;}

            private set
            {roleID = value;}
        }

        public static int USERID //setter and getter for user ID
        {
            get
            {return userID;}

            private set
            {userID = value;}
        }

        public static string NAME //setter and getter for name
        {
            get
            {return name;}

            set
            {
                name = value;
            }
        }

        public static string ROLE //setter and getter for name
        {
            get
            { return role;}

            set
            {role = value;}
        }

        public static bool getlogindetails(string proc, Hashtable ht)
        {
            bool check = false;

            try
            {
                SqlConnection sql_con = new SqlConnection(MainClass.CodingSourceClass.connection());

                SqlCommand cmd = new SqlCommand(proc, sql_con);

                cmd.CommandType = CommandType.StoredProcedure;

                foreach (DictionaryEntry item in ht)
                {
                    cmd.Parameters.AddWithValue(item.Key.ToString(), item.Value);
                }

                sql_con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    check = true;

                    while (dr.Read())
                    {
                        USERID = Convert.ToInt32(dr[0].ToString());
                        NAME = dr[1].ToString();

                        ROLEID = Convert.ToInt32(dr[2].ToString());
                        ROLE = dr[3].ToString();
                    }
                }
                else
                {
                    check = false;

                   CodingSourceClass.ShowMsg("Invalid username or password", "Error");
                }

                sql_con.Close();
            }
            catch (Exception ex)
            {
                CodingSourceClass.ShowMsg(ex.Message,"Error");
            }

            return check;
        }
    }
}
