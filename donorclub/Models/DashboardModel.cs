using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace donorclub.Models
{
    public class DashboardModel
    {
        public string ac_status { get; set; }
        public string Total_Team { get; set; }
        public string Total_Bonus { get; set; }
        public string Total_Get { get; set; }
        public string Total_Provide { get; set; }
        public string Current_IP { get; set; }
        public string Total_PH { get; set; }
        public string Total_Direct_team { get; set; }
        public string Total_Direct_Income { get; set; }
        public string Total_ROI { get; set; }
        public string Total_Level_Income { get; set; }

        public string Total_Balance { get; set; }
        public string Total_Earning { get; set; }

        public string Leader_Income { get; set; }
        public string Leader_Wallet { get; set; }

        public string Working_Wallet { get; set; }
        public string isactivestatus { get; set; }
        public string hsrno { get; set; }
        public string remaining_hour { get; set; }

        public string level_Status { get; set; }

        public string ac_no { get; set; }
        public string ac_name { get; set; }
        public string bk_name { get; set; }
        public string bk_branch { get; set; }
        public string ac_type { get; set; }
        public string paytm_no { get; set; }
        public string gpay_no { get; set; }
        public string bk_ifsc { get; set; }
        public string Mobile_No { get; set; }
        public string provider_mobile_no { get; set; }
        public string phonepay_no { get; set; }

        public List<Transactions> transList { get; set; }

        public List<Transactions> orderList { get; set; }

        public List<NewsModel> Newslist { get; set; }

        public List<LetterModel> AlertList { get; set; }

        public List<UserLevelModel> teamamtesList { get; set; }

        public List<Transactions> transactionList { get; set; }
    }

    public class Transactions
    {
        public string trans_type { get; set; }
        public string trans_date { get; set; }
        public string trans_time { get; set; }
        public string order_no { get; set; }
        public string sender_name { get; set; }
        public string reciever_name { get; set; }
        public string sender_Mobile_No { get; set; }
        public string reciever_Mobile_No { get; set; }
        public string usdamt { get; set; }
        public string btcamt { get; set; }
        public string ethamt { get; set; }
        public string flag { get; set; }
        public string trans_no { get; set; }
        public string Total_Provide { get; set; }
        public string srno { get; set; }
        public string r_e_status { get; set; }
        public string rmemb_code { get; set; }
        public string total_assign { get; set; }

        public string class_color { get; set; }
        public string transStatus_text { get; set; }
        public string classThumb_color { get; set; }

        public string transactionStatus { get; set; }
        public string transactionFor { get; set; }
        public string transactionHash { get; set; }

        public string fakeSlipStatus { get; set; }
    }

    public class AssignmentMSGModel
    {
        public string hsrno { get; set; }
        public string smemb_code { get; set; }
        public string rmemb_code { get; set; }
        public string message { get; set; }
        public string attachment { get; set; }
        public string flag { get; set; }
        public string tdate { get; set; }
        public string ttime { get; set; }
        public string position { get; set; }
    }

    public class TransactionLinkModel
    {
        public string message { get; set; }
        public string trans_type { get; set; }
        public string transFor { get; set; }
    }

    public class NewsModel
    {
        public string srno { get; set; }
        public string sub { get; set; }
        public string heading { get; set; }
        public string msg { get; set; }
        public string ttime { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public string news_image { get; set; }
    }

    public class ProvideHelpModel
    {
        public string commit_no { get; set; }
        public string ttime { get; set; }

        //[Required(ErrorMessage = "Amount is Required", AllowEmptyStrings = false)]
        //[RegularExpression("^[0-9]*$", ErrorMessage = "Enter Valid Amount")]
        public string amount { get; set; }
        public string LinkType { get; set; }
        public string pin { get; set; }
        public string amountpin { get; set; }
        public string Provide_Status { get; set; }
        public string minLimit { get; set; }
        public string maxLimit { get; set; }
        public string LastProvide { get; set; }
        public string Recommitment { get; set; }
        public List<SelectListItem> Packages { get; set; }
    }


    public class GetHelpModel
    {
        public string OrderID { get; set; }
        public string OrderNo { get; set; }
        public string tdate { get; set; }

        [Required(ErrorMessage = "Amount is Required", AllowEmptyStrings = false)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Enter Valid Amount")]
        public string amount { get; set; }

        public string amountx { get; set; }
        public string Get_Status { get; set; }

        public string Total_Balance { get; set; }
        public string Max_Commitment { get; set; }
    }

    public class UserLevelModel
    {
        public string memb_code { get; set; }
        public string reg_date { get; set; }
        public string memb_name { get; set; }
        public string username { get; set; }
        public string m_country { get; set; }
        public string email { get; set; }
        public string level_no { get; set; }
        public string Mobile_No { get; set; }
        public string sp_user { get; set; }
        public string sp_name { get; set; }
        public string confirm_ph { get; set; }
        public string unconfirm_ph { get; set; }
        public string TotalPh_Amount { get; set; }
        public string PhPaid_Amount { get; set; }
        public string Total_Ph { get; set; }
        public string TotalConfirm_Ph { get; set; }
        public string TotalUnconfirm_Ph { get; set; }
        public string tempf { get; set; }
    }


    public class LetterModel
    {
        public string srno { get; set; }
        public string memb_code { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public string flag { get; set; }
        public string flag_date { get; set; }
        public string tdate { get; set; }
        public string ttime { get; set; }
    }

    public class ReportModel
    {
        public string amount { get; set; }
        public string ph_amount { get; set; }
        public string ph_date { get; set; }
        public string tdate { get; set; }
        public string from_name { get; set; }
        public string from_userid { get; set; }
        public string order_no { get; set; }
        public string level_no { get; set; }
        public string levelPer { get; set; }
    }

    public class RewardModel
    {
        public string memb_name { get; set; }
        public string username { get; set; }
        public string Mobile_No { get; set; }
        public string Amount { get; set; }

    }

    public class PINDETAILSMODEL
    {
        public string PinID { get; set; }
        public string Memb_Code { get; set; }

        [Required(ErrorMessage = "Pin No is Required", AllowEmptyStrings = false)]
        public string Pin { get; set; }


        public string u_date { get; set; }

        public string Flag { get; set; }
        public string Commit_No { get; set; }
        public string TF_Date { get; set; }
        public string from_ID { get; set; }
        public string Tdate { get; set; }
        public string tf_flag { get; set; }

        [Required(ErrorMessage = "Amount is Required", AllowEmptyStrings = false)]
        public string amount { get; set; }


        public string u_flag { get; set; }

        [Required(ErrorMessage = "User ID is Required", AllowEmptyStrings = false)]
        public string username { get; set; }


        public string Memb_Name { get; set; }
        public string EMail { get; set; }
        public string Main_Wallet { get; set; }


        public string Available_Pin { get; set; }



        [Required(ErrorMessage = "Quantity is Required", AllowEmptyStrings = false)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Enter Valid Quantity")]
        public string Quantiy { get; set; }

    }

    public class PinTransferModel
    {
        public List<PINDETAILSMODEL> trnsferHistory { get; set; }
        public List<PINDETAILSMODEL> receiveHistory { get; set; }
    }

    public class SponsorDetailsModel
    {
        public string Memb_Name { get; set; }
    }

    public class PinAmountModel
    {
        public string Pin { get; set; }
        public string amount { get; set; }
    }
}