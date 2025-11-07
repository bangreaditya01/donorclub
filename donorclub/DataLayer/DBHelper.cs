using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace donorclub.DataLayer
{
    public class DBHelper
    {
        private static string GetConnectionString()
        {
            return "Data Source=.;Initial Catalog=DonorsClub;Integrated Security=True;";
           // return "Data Source=5.9.251.85;Initial Catalog=DonorsClub;User ID=donorsclub;Password=donD$@8or98%#$%sclub";
            //return "Data Source=5.9.251.85;Initial Catalog=DonorsClub;Integrated Security=True;";
            //return "Data Source=.;Initial Catalog=Uniquetrade_ROI;Integrated Security=True;";
        }
        private static SqlConnection GetConnection()
        {
            return new SqlConnection(GetConnectionString());

        }
        private static SqlCommand GetCommand(string cmdTxt, SqlConnection connection, params SqlParameter[] commandParameters)
        {
            SqlCommand command = new SqlCommand(cmdTxt, connection);
            if (cmdTxt.ToLower().StartsWith("usp_") || cmdTxt.ToLower().StartsWith("pr") || cmdTxt.ToLower().StartsWith("sp_") || cmdTxt.ToLower().StartsWith("bt"))
                command.CommandType = CommandType.StoredProcedure;
            if (commandParameters != null)
                AttachParameters(command, commandParameters);
            return command;
        }

        public static DataTable GetDataTable(string cmdTxt, params SqlParameter[] commandParameters)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand command = GetCommand(cmdTxt, connection, commandParameters);
                command.CommandTimeout = 100;
                SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                DataTable dt = new DataTable();
                dt.Load(dr);
                return dt;
            }
        }

        public static int GetScaler(string cmdTxt, params SqlParameter[] commandParameters)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand command = GetCommand(cmdTxt, connection, commandParameters);
                int var = Convert.ToInt16(command.ExecuteScalar());
                connection.Close();
                return var;
            }
        }

        public static bool GetScaler_Boolean(string cmdTxt, params SqlParameter[] commandParameters)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand command = GetCommand(cmdTxt, connection, commandParameters);
                bool var = Convert.ToBoolean(command.ExecuteScalar());
                connection.Close();
                return var;
            }
        }

        public static string GetScaler_String(string cmdTxt, params SqlParameter[] commandParameters)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand command = GetCommand(cmdTxt, connection, commandParameters);
                string var = command.ExecuteScalar().ToString();
                connection.Close();
                return var;
            }
        }

        public static SqlDataReader GetDataReader(string cmdTxt, params SqlParameter[] commandParameters)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand command = GetCommand(cmdTxt, connection, commandParameters);
                SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                return dr;
            }
        }

        public static DataSet GetDataSet(string cmdTxt, params SqlParameter[] commandParameters)
        {
            SqlConnection connection = GetConnection();
            SqlCommand command = GetCommand(cmdTxt, connection, commandParameters);
            command.CommandTimeout = 180;
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet ds = new DataSet(command.CommandText);
            da.Fill(ds);
            return ds;
        }
        public static int ExecuteNonQuery(string cmdTxt, params SqlParameter[] commandParameters)
        {
            SqlConnection connection = GetConnection();
            connection.Open();
            SqlCommand command = GetCommand(cmdTxt, connection, commandParameters);
            int rVal = command.ExecuteNonQuery();
            connection.Close();
            return rVal;
        }



        public static bool IsDataAvailable(string cmdTxt)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand command = GetCommand(cmdTxt, connection, null);
                SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                DataTable dt = new DataTable();
                dt.Load(dr);
                return dt.Rows.Count > 0;
            }
        }

        //public static SelectList ToSelectList(string SQL, string valueField, string textField)
        //{
        //    List<SelectListItem> list = new List<SelectListItem>();
        //    DataTable dt = GetDataTable(SQL, null);
        //    if(dt.Rows.Count > 0)
        //    { 
        //            list.Add(new SelectListItem()
        //            {
        //                Text = R.Read<string>(textField).ToString(), //row[textField].ToString(),
        //                Value = R.Read<string>(valueField).ToString()
        //            });
        //        }
        //    }
        //    return new SelectList(list, "Value", "Text");
        //}

        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (SqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }
    }
}

