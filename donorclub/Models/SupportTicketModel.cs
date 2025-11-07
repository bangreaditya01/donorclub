using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace donorclub.Models
{
    public class SupportTicketModel
    {
        public int Ticket_ID { get; set; }
        public string Ticket_IDEncrypt { get; set; }
        public string Ticket_Type { get; set; }
        public int Reply_ID { get; set; }
        public string memb_code { get; set; }
        public string Ticket_To { get; set; }
        public string Ticket_From { get; set; }
        public string Ticket_Subject { get; set; }
        public string Ticket_Desc { get; set; }
        public string Ticket_Image { get; set; }
        public string Ticket_Status { get; set; }
        public string Ticket_Date { get; set; }
        public string Modifies_Date { get; set; }

        public string Admin_Reply { get; set; }
        public string Reply_Date { get; set; }

    }
}