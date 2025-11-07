using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Razor.Tokenizer;
using donorclub.DataLayer;

namespace donorclub.DBEntity
{
    public class SupportTicketManager
    {
        public static int AddUpdate_SupportTicket(string Ticket_ID, string memb_code, string Ticket_Type, string Reply_ID
        , string Ticket_To, string Ticket_From, string Ticket_Subject, string Ticket_Desc, string Ticket_Image, string Ticket_Status)
        {
            SqlParameter[] sqlpara = new SqlParameter[10];
            sqlpara[0] = new SqlParameter("@Ticket_ID", Ticket_ID);
            sqlpara[1] = new SqlParameter("@memb_code", memb_code);
            sqlpara[2] = new SqlParameter("@Ticket_Type", Ticket_Type);
            sqlpara[3] = new SqlParameter("@Reply_ID", Reply_ID);
            sqlpara[4] = new SqlParameter("@Ticket_To", Ticket_To);
            sqlpara[5] = new SqlParameter("@Ticket_From", Ticket_From);
            sqlpara[6] = new SqlParameter("@Ticket_Desc", Ticket_Desc);
            sqlpara[7] = new SqlParameter("@Ticket_Image", Ticket_Image);
            sqlpara[8] = new SqlParameter("@Ticket_Status", Ticket_Status);
            sqlpara[9] = new SqlParameter("@Ticket_Subject", Ticket_Subject);

            return DBHelper.ExecuteNonQuery("usp_AddUpdate_SupportTicket", sqlpara);
        }

        public static DataTable GetSupportTicket(string Ticket_ID, string memb_code, string Mode)
        {
            SqlParameter[] sqlpara = new SqlParameter[3];
            sqlpara[0] = new SqlParameter("@Ticket_ID", Ticket_ID);
            sqlpara[1] = new SqlParameter("@memb_code", memb_code);
            sqlpara[2] = new SqlParameter("@Mode", Mode);

            return DBHelper.GetDataTable("usp_GetSupportTicket", sqlpara);
        }
    }
}