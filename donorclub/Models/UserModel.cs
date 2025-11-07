using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace donorclub.Models
{
    public class UserModel
    {
        public string provider_mobile_no { get; set; }
        public string memb_code { get; set; }
        public string Spon_Code { get; set; }
        public string Plac_Code { get; set; }
        public string Place { get; set; }
        public string Reg_Date { get; set; }
        public string Reg_Time { get; set; }
        public string MembName_F { get; set; }
        public string MembName_M { get; set; }
        public string MembName_L { get; set; }

        [Required(ErrorMessage = "Please Enter Member Name", AllowEmptyStrings = false)]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be between 3 to 30 alphabets")]
        public string Memb_Name { get; set; }
        public string Gender { get; set; }

        [MaxLength(16, ErrorMessage = "Mobile No is Not Valid")]
        [MinLength(8, ErrorMessage = "Mobile No is Not Valid")]
        [Required(ErrorMessage = "Mobile Number is required.")]
        public string Mobile_No { get; set; }
        public string Phone_No { get; set; }

        [Required(ErrorMessage = "Please Enter Email ID", AllowEmptyStrings = false)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email ID is not valid")]
        [MinLength(5)]
        [EmailAddress(ErrorMessage = "Email ID must be greater than 5 char")]
        [RegularExpression("^([a-zA-Z0-9_\\.\\-])+\\@(([a-zA-Z\\-])+\\.)+([a-zA-Z]{2,6})$", ErrorMessage = "Email ID is not valid")]
        public string EMail { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Required(ErrorMessage = "Please Select Country", AllowEmptyStrings = false)]
        public string M_COUNTRY { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Pin_Code { get; set; }
        public string Reg_Amt { get; set; }

        [Required(ErrorMessage = "Please Enter User-ID", AllowEmptyStrings = false)]
        public string username { get; set; }

        //[Display(Name = "Password", Prompt = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password should not be less than 6 characters")]
        [MaxLength(15, ErrorMessage = "Password should not be greater than 20 characters")]
        //[RegularExpression("[a-zA-Z0-9.^;<>?|=%*#$@!+&_]*$", ErrorMessage = "Invalid Password")]
        public string mpwd { get; set; }
        public string RV_Code { get; set; }
        public string PIN_ID { get; set; }
        public string pin_no { get; set; }
        public string authrised { get; set; }
        public string auth_date { get; set; }
        public string M_Status { get; set; }
        public string MPOSITION { get; set; }
        public string FLAG { get; set; }
        public string TEMPF { get; set; }
        // public string REMARK { get; set; }
        public string client_ip { get; set; }
        public string last_log_in { get; set; }
        public string dbo { get; set; }


        [Required(ErrorMessage = "Current or Old Password is required")]
        public string OldPass { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string NewPass { get; set; }

        [Required(ErrorMessage = "Confirm Password required")]
        [Compare("NewPass", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmNewPass { get; set; }

        //[Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm Password required")]
        [Compare("mpwd", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPass { get; set; }


        //------------ Account ------------//

        [RegularExpression("^[A-Z0-9a-z]+$", ErrorMessage = "Enter Valid Bitcoin Address")]
        [StringLength(40, MinimumLength = 30, ErrorMessage = "Enter Valid Bitcoin Address")]
        public string btc_ac { get; set; }
        public string eth_ac { get; set; }
        public string ltc_ac { get; set; }
        public string accStatus { get; set; }
        public string level_no { get; set; }

        [Required(ErrorMessage = "Please Enter Sponsor ID", AllowEmptyStrings = false)]
        public string sp_user { get; set; }


        public string remark { get; set; }
        public string Message { get; set; }
        public string tdate { get; set; }
        public string credit_date { get; set; }
        public string amount { get; set; }
        public string status { get; set; }

        public string hsrno { get; set; }

        [Required(ErrorMessage = "Please Enter Account Number", AllowEmptyStrings = false)]
        public string AccountNo { get; set; }

        [Required(ErrorMessage = "Please Enter Paytm Number", AllowEmptyStrings = false)]
        public string Paytm_No { get; set; }

        [Required(ErrorMessage = "Please Enter Account Holder Name", AllowEmptyStrings = false)]
        public string account_name { get; set; }

        [Required(ErrorMessage = "Please Enter Bank Name", AllowEmptyStrings = false)]
        public string bank_name { get; set; }

        [Required(ErrorMessage = "Please Enter Branch Name", AllowEmptyStrings = false)]
        public string branch_name { get; set; }

        [Required(ErrorMessage = "Please Enter IFSC Code", AllowEmptyStrings = false)]
        public string bk_ifsc { get; set; }

        public string gpay_no { get; set; }

        //[Required(ErrorMessage = "Please Enter Phone pay no", AllowEmptyStrings = false)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Enter Valid Phonepe No")]
        public string phonepay_no { get; set; }

        // [Required(ErrorMessage = "Please Enter Paytm no", AllowEmptyStrings = false)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Enter Valid Paytm No")]
        public string paytm_no { get; set; }




        public string ac_no { get; set; }
        public string ac_name { get; set; }
        public string bk_name { get; set; }
        public string bk_branch { get; set; }
        public string ac_type { get; set; }
        public string coo_ac { get; set; }
        public List<Transactions> transactionList { get; set; }
    }

    public class ForgetPasswordModel
    {
        [Required(ErrorMessage = "Please Enter User ID", AllowEmptyStrings = false)]
        public string username { get; set; }
    }

    public class SignInModel
    {
        [Required(ErrorMessage = "Please Enter User ID", AllowEmptyStrings = false)]
        public string username { get; set; }

        [Required(ErrorMessage = "Please Enter Password", AllowEmptyStrings = false)]
        public string mpwd { get; set; }
    }


    public class SignUpModel
    {
        [Required(ErrorMessage = "Please Enter Sponsor ID", AllowEmptyStrings = false)]
        public string sp_user { get; set; }

        [Required(ErrorMessage = "Please Enter Name", AllowEmptyStrings = false)]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be between 3 to 30 alphabets")]
        public string Memb_Name { get; set; }


        [Required(ErrorMessage = "Please Enter Mobile No", AllowEmptyStrings = false)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Enter Valid Mobile Number.")]
        public string Mobile_No { get; set; }


        [Required(ErrorMessage = "Please Enter Email ID", AllowEmptyStrings = false)]
        [RegularExpression("^([a-zA-Z0-9_\\.\\-])+\\@(([a-zA-Z\\-])+\\.)+([a-zA-Z]{2,6})$", ErrorMessage = "Enter Valid Email ID")]
        public string EMail { get; set; }


        [Required(ErrorMessage = "Password is required")]
        public string mpwd { get; set; }


        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("mpwd", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPass { get; set; }


        [Required(ErrorMessage = "User ID is required")]
        //[RegularExpression("^[A-Z0-9a-z]+$", ErrorMessage = "Enter Valid User ID")]
        public string username { get; set; }

        public string MembName_L { get; set; }

        //[Required(ErrorMessage = "Pin is required")]        
        public string pin { get; set; }
        public string pattern { get; set; }
        public string MembName_F { get; set; }
        public string MembName_M { get; set; }

        //[Required(ErrorMessage = "Pin is required")]   
        public string Sponcer_mobile { get; set; }
        public string place { get; set; }
        public string plan_type { get; set; }
        public string gender { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string M_COUNTRY { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Pin_Code { get; set; }
        public string Reg_Amt { get; set; }
        public string RV_Code { get; set; }
        public string PIN_ID { get; set; }
        public string pin_no { get; set; }
        public string REMARK { get; set; }
        public string client_ip { get; set; }
        public string ac_name { get; set; }
        public string ac_no { get; set; }
        public string bk_name { get; set; }
        public string bk_branch { get; set; }
        public string bk_ifsc { get; set; }
        public string bk_card_no { get; set; }
        public string btc_ac { get; set; }
        public string Phone_No { get; set; }
        public string Mode { get; set; }

        public string pwd { get; set; }


    }

    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Current Password is required")]
        public string OldPass { get; set; }

        [Required(ErrorMessage = "New Password is required")]
        public string NewPass { get; set; }

        [Required(ErrorMessage = "Re-type New Password required")]
        [Compare("NewPass", ErrorMessage = "The password and Re-type password not match.")]
        public string ConfirmNewPass { get; set; }
    }

    public class BankDetailsModel
    {
        [Required(ErrorMessage = "Please Enter Account Number", AllowEmptyStrings = false)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Enter Valid Account No")]
        public string ac_no { get; set; }

        [Required(ErrorMessage = "Please Enter Account Holder Name", AllowEmptyStrings = false)]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "Enter Valid Account Name")]
        public string ac_name { get; set; }

        [Required(ErrorMessage = "Please Enter Bank Name", AllowEmptyStrings = false)]
        public string bk_name { get; set; }

        [Required(ErrorMessage = "Please Enter Branch Name", AllowEmptyStrings = false)]
        public string bk_branch { get; set; }

        [Required(ErrorMessage = "Please Enter IFSC Code", AllowEmptyStrings = false)]
        [RegularExpression("^[a-z0-9A-Z]+$", ErrorMessage = "Enter Valid IFSC Code")]
        public string bk_ifsc { get; set; }

        // [Required(ErrorMessage = "Please Select Account Type", AllowEmptyStrings = false)]
        public string ac_type { get; set; }

        // [Required(ErrorMessage = "Please Enter Google pay no", AllowEmptyStrings = false)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Enter Valid Google pay")]
        public string gpay_no { get; set; }

        //[Required(ErrorMessage = "Please Enter Phone pay no", AllowEmptyStrings = false)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Enter Valid Phonepe No")]
        public string phonepay_no { get; set; }

        // [Required(ErrorMessage = "Please Enter Paytm no", AllowEmptyStrings = false)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Enter Valid Paytm No")]
        public string paytm_no { get; set; }
        public string debit_card_no { get; set; }
        public string transit_no { get; set; }

        //[Required(ErrorMessage = "Enter OTP Details", AllowEmptyStrings = false)]
        //public string request_code { get; set; }
    }

    public class EditProfileModel
    {
        public string MembName_L { get; set; }

        [Required(ErrorMessage = "Please Enter Member Name", AllowEmptyStrings = false)]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be between 3 to 30 alphabets")]
        public string Memb_Name { get; set; }

        // [Required(ErrorMessage = "Please Select Gender", AllowEmptyStrings = false)]
        public string Gender { get; set; }


        [RegularExpression("^[A-Z0-9a-z]+$", ErrorMessage = "Enter Valid Bitcoin Address")]
        [StringLength(40, MinimumLength = 30, ErrorMessage = "Enter Valid Bitcoin Address")]
        public string btc_ac { get; set; }

        public string Mobile_No { get; set; }
        public string EMail { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public string username { get; set; }
    }


    public class ContactEnquiry
    {
        [Required(ErrorMessage = "Name is Required", AllowEmptyStrings = false)]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "Enter Valid Name")]
        public string username { get; set; }

        //[EmailAddress(ErrorMessage = "Enter Valid Email Id")]
        [Required(ErrorMessage = "Email Id is Required", AllowEmptyStrings = false)]
        [RegularExpression("^([a-zA-Z0-9_\\.\\-])+\\@(([a-zA-Z\\-])+\\.)+([a-zA-Z]{2,6})$", ErrorMessage = "Enter Valid Email Id")]
        public string email { get; set; }

        [Required(ErrorMessage = "Subject is Required", AllowEmptyStrings = false)]
        [RegularExpression("^[a-z0-9A-Z\\s]+$", ErrorMessage = "Enter Valid Subject")]
        public string subject { get; set; }

        [Required(ErrorMessage = "Message is Required", AllowEmptyStrings = false)]
        [RegularExpression("^[a-z0-9A-Z\\s]+$", ErrorMessage = "Enter Valid Message")]
        public string message { get; set; }

        [Required(ErrorMessage = "Mobile No is Required", AllowEmptyStrings = false)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Enter Valid Mobile No")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Enter Valid Mobile No")]
        public string phone_no { get; set; }
    }
    public class VerifyUserModel
    {
        [Required(ErrorMessage = "Otp is Required", AllowEmptyStrings = false)]
        public string Request_Code { get; set; }
    }
}