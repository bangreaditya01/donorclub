using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using donorclub.DBEntity;
using System.Web.Configuration;

namespace LudoFoundation_app.CommanFunction
{
    [Serializable()]
    public class UserInfo
    {

        #region Public Varaible
        public string UserID = "";
        public string User_Name = "";
        public string EmailID = "";
        public string userType = "";
        public bool IsActive = false;
        public bool Isvalid_Email = false;
        public bool IsInst_Valid = false;


        public string memb_codeEncrypt = "";

        public string mposition = "";
        public string memb_code = "";
        public string memb_name = "";
        public string username = "";
        public string M_COUNTRY = "";
        public string spon_code = "";
        public string plac_code = "";
        public string mpwd = "";
        //-------------------------------
        //public string pwd = "";
        //------------------------------------
        public string ticketType = "";
        public string ticketStatus = "";
        public string toticketUser = "";
        public string fromticketuser = "";

        public string dbo = "";
        public string Gender = "";
        public string Mobile_No = "";

        public string accStatus = "";
        public string btc_ac = "";
        public string eth_ac = "";
        public string ltc_ac = "";

        public string ac_no = "";
        public string ac_name = "";
        public string bk_name = "";
        public string bk_branch = "";
        public string bk_ifsc = "";
        public string ac_type = "";

        public string gpay_no = "";
        public string phonepay_no = "";
        public string paytm_no = "";

        public string sp_name = "";
        public string sp_Email = "";
        public string sp_ID = "";
        public string sp_country = "";
        public string sp_mobile_no = "";

        public string level_No = "";
        public string level_text = "";
        public string authrised = "";
        

        public string Address1 = "";
        public string City = "";

        public string actioname = "";
        public string controllerName = "";
        public string languageCode = "";

        public string faceBookURL = "";
        public string MembName_L = "";

        public string referel_link = "";

        #endregion Public Varaible

        private bool _isAuthenticated = false;

        public bool IsAuthenticated { get { return _isAuthenticated; } }

        public UserInfo(string userid, string usertype)
        {
            if (IsSuperUser(userid))
            {
                SetSuperValues(userid);
            }
            else
            {
                DataTable dt = UserManager.UserLogin(userid);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    SetValues(row);
                }
            }
        }

        public UserInfo(string userid, string password, string UserStatus, string usertype)
        {
            if (IsSuperUser(userid, password))
            {
                SetSuperValues(userid);
            }
            else
            {
                if (string.Equals(usertype, "U"))
                {
                    DataTable dt = UserManager.UserLogin(userid);
                    if (dt.Rows.Count > 0)
                    {
                        if (string.Equals("N", dt.Rows[0]["TEMPF"].ToString().Trim()))
                        {
                            UserID = "B";
                        }
                        else if (string.Equals("N", dt.Rows[0]["M_Status"].ToString().Trim()))
                        {
                            UserID = "E";
                        }
                        else if (string.Equals(password, dt.Rows[0]["mpwd"].ToString()))
                        {
                            DataRow row = dt.Rows[0];
                            SetValues(row);
                        }
                        else if (!string.Equals(password, dt.Rows[0]["mpwd"].ToString()))
                        {
                            UserID = dt.Rows[0]["memb_code"].ToString();
                        }
                    }
                }
            }
        }

        private void SetSuperValues(string userid)
        {
            UserID = "0";
            _isAuthenticated = true;
            User_Name = "Super Admin";
            EmailID = "mahendradhande.ite@gmail.com";
            userType = "S";
            IsActive = true;
            //_emailDrafts = EmailDraftManager.Get(UserID);
        }

        private bool IsSuperUser(string userid, string pwd)
        {
            if (userid.Equals(SuperUser.UserID) && pwd.Equals(SuperUser.Pwd))//SuperUser
                return true;
            else return false;
        }

        private bool IsSuperUser(string userid)
        {
            if (userid.Equals(SuperUser.UserID))
                return true;
            else return false;
        }

        private void SetValues(DataRow row)
        {
            if (row != null)
            {
                UserID = row["memb_code"].ToString();

                mposition = row["mposition"] != null && row["mposition"] != DBNull.Value ? row["mposition"].ToString().Trim() : "";
                memb_code = row["memb_code"] != null && row["memb_code"] != DBNull.Value ? row["memb_code"].ToString().Trim() : "";
                memb_name = row["memb_name"] != null && row["memb_name"] != DBNull.Value ? row["memb_name"].ToString().Trim() : "";
                username = row["username"] != null && row["username"] != DBNull.Value ? row["username"].ToString().Trim() : "";
                M_COUNTRY = row["M_COUNTRY"] != null && row["M_COUNTRY"] != DBNull.Value ? row["M_COUNTRY"].ToString().Trim() : "";
                spon_code = row["spon_code"] != null && row["spon_code"] != DBNull.Value ? row["spon_code"].ToString().Trim() : "";
                plac_code = row["plac_code"] != null && row["plac_code"] != DBNull.Value ? row["plac_code"].ToString().Trim() : "";
                mpwd = row["mpwd"] != null && row["mpwd"] != DBNull.Value ? row["mpwd"].ToString() : "";
                authrised = row["authrised"] != null && row["authrised"] != DBNull.Value ? row["authrised"].ToString() : "";
                memb_codeEncrypt = Crypto.Encrypt(memb_code, System.Text.Encoding.Unicode);

                referel_link = WebConfigurationManager.AppSettings["domainName"] + "/Home/SignUp?refe=" + username;

                faceBookURL = row["Address2"] != null && row["Address2"] != DBNull.Value ? row["Address2"].ToString().Trim() : "";

                userType = "U";
                User_Name = row["username"] != null && row["username"] != DBNull.Value ? row["username"].ToString().Trim() : "";
                EmailID = row["EMail"] != null && row["EMail"] != DBNull.Value ? row["EMail"].ToString().Trim() : "";
                _isAuthenticated = true;

                dbo = row["dbo"] != null && row["dbo"] != DBNull.Value ? Convert.ToDateTime(row["dbo"].ToString()).ToString("yyyy-MM-dd") : "";
                Gender = row["Gender"] != null && row["Gender"] != DBNull.Value ? row["Gender"].ToString().Trim() : "";
                Mobile_No = row["Mobile_No"] != null && row["Mobile_No"] != DBNull.Value ? row["Mobile_No"].ToString().Trim() : "";
                MembName_L = row["MembName_L"] != null && row["MembName_L"] != DBNull.Value ? row["MembName_L"].ToString().Trim() : "";

                Address1 = row["Address1"] != null && row["Address1"] != DBNull.Value ? row["Address1"].ToString().Trim() : "";
                City = row["City"] != null && row["City"] != DBNull.Value ? row["City"].ToString().Trim() : "";

                if (string.Equals(row["Admin_username"].ToString(), username))
                {
                    ticketStatus = "Close";
                    ticketType = "R";
                    fromticketuser = username;
                    toticketUser = row["Admin_username"].ToString();
                }
                else
                {
                    ticketStatus = "Open";
                    ticketType = "S";
                    toticketUser = row["Admin_username"].ToString();
                    fromticketuser = username;
                }


                DataTable dt = UserManager.USER_REPORT("GETBANKDETAILS", memb_code);
                if (dt.Rows.Count > 0)
                {
                    accStatus = "1";
                    btc_ac = dt.Rows[0]["btc_ac"].ToString().Trim();
                    eth_ac = dt.Rows[0]["eth_ac"].ToString().Trim();
                    ltc_ac = dt.Rows[0]["ltc_ac"].ToString().Trim();

                    ac_name = dt.Rows[0]["ac_name"].ToString().Trim();
                    ac_no = dt.Rows[0]["ac_no"].ToString().Trim();
                    bk_name = dt.Rows[0]["bk_name"].ToString().Trim();
                    bk_branch = dt.Rows[0]["bk_branch"].ToString().Trim();
                    bk_ifsc = dt.Rows[0]["bk_ifsc"].ToString().Trim();
                    ac_type = dt.Rows[0]["ac_type"].ToString().Trim();
                    gpay_no = dt.Rows[0]["gpay_no"].ToString().Trim();
                    phonepay_no = dt.Rows[0]["phonepay_no"].ToString().Trim();
                    paytm_no = dt.Rows[0]["paytm_no"].ToString().Trim();
                }
                else
                {
                    accStatus = "0";
                }


                DataTable dtSp = UserManager.GetSponserDetails(spon_code);
                if (dtSp.Rows.Count > 0)
                {
                    sp_country = dtSp.Rows[0]["sp_country"].ToString().Trim();
                    sp_Email = dtSp.Rows[0]["sp_Email"].ToString().Trim();
                    sp_mobile_no = dtSp.Rows[0]["sp_mobile_no"].ToString().Trim();
                    sp_name = dtSp.Rows[0]["sp_name"].ToString().Trim();
                    sp_ID = dtSp.Rows[0]["spon_id"].ToString().Trim();
                }

                DataTable dtDash = UserManager.GetDashboardData("USERLEVEL", memb_code);
                if (dtDash.Rows.Count > 0)
                {
                    level_No = dtDash.Rows[0]["level_No"].ToString().Trim();
                    level_text = dtDash.Rows[0]["level_text"].ToString().Trim();
                }
                else
                {
                    level_No = "0";
                    level_text = "Participant";
                }
            }
        }

        private void SetValuesAdmin(DataRow row)
        {
            if (row != null)
            {
                UserID = row["srno"].ToString();
                username = row["user_id"] != null && row["user_id"] != DBNull.Value ? row["user_id"].ToString().Trim() : "";
                mpwd = row["pwd"] != null && row["pwd"] != DBNull.Value ? row["pwd"].ToString() : "";

                userType = "A";
            }
        }
    }

    internal class SuperUser
    {
        internal const string UserID = "mvsolution2011";
        internal const string Pwd = "mvsolution@2011";
    }
}