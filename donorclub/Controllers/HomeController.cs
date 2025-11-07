using donorclub.DBEntity;
using donorclub.Models;
using LudoFoundation_app.CommanFunction;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace donorclub.Controllers
{
    public class HomeController : BasePage
    {
        string emailStatus = string.Empty;
        string messagecontent = string.Empty;
        string smsStatus = string.Empty;
        string Body = string.Empty;
        string projectName = string.Empty;
        DataTable dtResult = new DataTable();


        public ActionResult Index()
        {
            List<RewardModel> dList = new List<RewardModel>();
            try
            {
                dtResult = UserManager.USER_REWARDREPORT("GETREWARDSACHIVERS");
                if (dtResult.Rows.Count > 0)
                {
                    string JSONString = JsonConvert.SerializeObject(dtResult);
                    dList = JsonConvert.DeserializeObject<List<RewardModel>>(JSONString);
                }
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            return View(dList.ToList());
            //return View();
        }

        public ActionResult Plan()
        {
            return View();
        }

        public static List<string> CountryList()
        {
            List<string> Culturelist = new List<string>();
            CultureInfo[] getCultureinfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo item in getCultureinfo)
            {
                RegionInfo region = new RegionInfo(item.LCID);
                if (!Culturelist.Contains(region.EnglishName))
                {
                    Culturelist.Add(region.EnglishName);
                }
            }
            Culturelist.Sort();
            return Culturelist;
        }

        public static List<string> CountryCodeList()
        {
            List<string> Culturelist = new List<string>();
            string codes = @"+93,+355,+213,+376,+244,+1-268,+54,+374,+297,+1-684,+61,+43,+994,+1-242,+880,+1-246,+257,+32,+	229,+1-441,+975,+387,+501,+375,
                            +591,+267,+55,+973,+673,+359,+226,236,+855,+1,+1-345,+242,+235,+56,+86,Cote d'Ivoire,+237,+243,+682,+57,+269,+238,+506,+385,+53,
                            +357,+420,+45,+253,+1 767,+1 809,+593,+20,+291,+503,+34,+372,+251,+679,+358,+33,+691,+241,+220,+44,+245,+995,+240,+49,+233,+30,
                            +1 473,+502,+224,+1 671,+592,+509,+852,+504,+36,+62,+91,+98,+353,+964,+354,+972,+00 1,+39,+1 284,+1 876,+962,+81,+7 6,+254,+996,
                            +686,+82,+383,+966,+965,+856,+371,+218,+231,+1 758,+266,+961,+423,+370,+352,+261,+212,+60,+265,+373,+960,+52,+976,+692,+389,+223,
                            +356,+382,+377,+258,+230,+222,+95,+264,+505,+31,+977,+234,+227,+47,+674,+64,+968,+92,+507,+595,+51,+63,+970,+680,+675,+48,+351,
                            +850,+1 787,+974,+40,+27,+7,+250,+685,+221,+248,+65,+1 869,+232,+386,+378,+677,+252,+381,+94,+211,+239,+249,+41,+597,+421,+46,
                            +268,+963,+255,+676,+66,+992,+993,+670,+228,+886,+1 868,+216,+90,+688,+971,+256,+380,+598,+1,+998,+678,+58,+84,+1 784,+967,+260,
                            +255 24,+263";
            string[] codelist = codes.Split(',');
            for (int i = 0; i < codelist.Length; i++)
            {
                if (!Culturelist.Contains(codelist[i]))
                {
                    Culturelist.Add(codelist[i]);
                }
            }
            Culturelist.Sort();
            return Culturelist;
        }
        public void getCountries()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                foreach (var item in CountryList())
                {
                    items.Add(new SelectListItem
                    {
                        Text = item,
                        Value = item
                    });
                }
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            ViewBag.AllCountries = items;
        }
        public void getCountryCodes()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                foreach (var item in CountryCodeList())
                {
                    items.Add(new SelectListItem
                    {
                        Text = item,
                        Value = item
                    });
                }
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            ViewBag.AllCountryCode = items;
        }

        public JsonResult getFirstUser(string sp_user)
        {
            UserModel user = new UserModel();
            if (string.IsNullOrEmpty(sp_user))
                dtResult = UserManager.GetFirstUser();
            else
                dtResult = UserManager.GetUserDetailsByUsername(sp_user);
            if (dtResult.Rows.Count > 0)
            {
                user.sp_user = dtResult.Rows[0]["Memb_Name"].ToString();
                user.Memb_Name = dtResult.Rows[0]["username"].ToString();
            }
            else
            {
                user.sp_user = "";
                user.Memb_Name = "";
            }

            return Json(user, JsonRequestBehavior.AllowGet);
        }

        public class CaptchaResponse
        {
            [JsonProperty("success")]
            public string Success { get; set; }

            [JsonProperty("error-codes")]
            public List<string> ErrorCodes { get; set; }
        }

        public ActionResult SignIn(string email, string password)
        {
            try
            {
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                    TempData["LoginAlert"] = null;
                    //  password = Crypto.Decrypt(password, System.Text.Encoding.Unicode);
                    UserInfo userInfo = new UserInfo(email, password, "1", "U");

                    if (String.IsNullOrEmpty(userInfo.UserID))
                    {
                        TempData["LoginAlert"] = "Invalid UserId or Password";
                    }
                    else
                    {
                        if (string.Equals(userInfo.UserID, "B"))
                        {
                            TempData["LoginAlert"] = "YOUR ID IS BLOCKED PLEASE CONTACT AT -" + WebConfigurationManager.AppSettings["senderEmlID"].ToString();
                        }
                        else if (string.Equals(userInfo.UserID, "E"))
                        {
                            TempData["LoginAlert"] = "Please First Verify Your Email ID.";
                        }
                        else if (!userInfo.IsAuthenticated)
                        {
                            TempData["LoginAlert"] = "Invalid Password";
                        }
                        else if (userInfo.IsAuthenticated)
                        {
                            Common.CurrentUserInfo = userInfo;
                            // Common.CookieUserID = userInfo.EmailID;
                            // Common.CookieUserType = userInfo.userType;

                            return RedirectToAction("Index", "Dashboard");
                        }
                    }
                }

                if (Common.CurrentUserInfo == null)
                {
                    Common.CurrentUserInfo = null;
                    Common.CookieUserID = null;
                    Common.CookieUserType = null;
                    //if (TempData["LoginAlert"] != null)
                    //    TempData["LoginAlert"] = TempData["LoginAlert"].ToString();
                }
                else
                {
                    UserInfo userInfo = new UserInfo(email, password, "1", "U");
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            catch (Exception ex)
            {
                TempData["LoginAlert"] = "Login failed! Error:" + ex.Message;
            }
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(SignInModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["LoginAlert"] = "Please enter valid data.";
                    return RedirectToAction("SignIn");
                }
                string teettt = Crypto.Encrypt("Lado@74558", System.Text.Encoding.Unicode);
                var response = Request["g-recaptcha-response"];

                const string secret = "6Lc6bpMUAAAAAJfV0P0puF3LjkXrlZI7XzkVXYOW";

                //const string secret = "6LeQbpMUAAAAAI7ol7QJTXd_BiWuihhK0IlIvFhu"; localhost
                var client = new WebClient();

                var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

                var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

                // when response is false check for the error message
                //if (captchaResponse.Success != null)
                //{
                //    if (captchaResponse.ErrorCodes == null)
                //    {
                string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                UserInfo userInfo = new UserInfo(user.username, Crypto.Encrypt(user.mpwd, System.Text.Encoding.Unicode), "1", "U");
                if (String.IsNullOrEmpty(userInfo.UserID))
                {
                    TempData["LoginAlert"] = "Invalid UserId or Password";
                }
                else
                {
                    if (string.Equals(userInfo.UserID, "B"))
                    {
                        TempData["LoginAlert"] = "YOUR ID IS BLOCKED PLEASE CONTACT AT - " + WebConfigurationManager.AppSettings["senderEmlID"].ToString();
                    }
                    else if (string.Equals(userInfo.UserID, "E"))
                    {
                        TempData["LoginAlert"] = "Please First Verify Your Email ID.";
                    }
                    else if (!userInfo.IsAuthenticated)
                    {
                        TempData["LoginAlert"] = "Invalid Password";
                    }
                    else if (userInfo.IsAuthenticated)
                    {
                        Common.CurrentUserInfo = userInfo;

                        return RedirectToAction("Index", "Dashboard");
                    }
                }
                //    }
                //    else
                //    {
                //        TempData["LoginAlert"] = "Invalid Captch";
                //    }
                //}
                //else
                //{
                //    TempData["LoginAlert"] = "Invalid Captch";
                //}

                return RedirectToAction("SignIn", "Home");
            }
            catch (Exception ex)
            {
                TempData["LoginAlert"] = "Login failed! Error:" + ex.Message;
                return RedirectToAction("SignIn", "Home");
            }
        }

        public ActionResult SignUp(string refe)
        {
            //getCountryCodes();
            getCountries();
            SignUpModel user = new SignUpModel();
            try
            {
                if (userInfo != null)
                {
                    user.sp_user = userInfo.username;
                    user.MembName_L = userInfo.memb_name;
                }
                else
                {
                    if (!string.IsNullOrEmpty(refe))
                    {
                        //string membCode = Crypto.Decrypt(mb, System.Text.Encoding.Unicode);
                        dtResult = UserManager.GetUserDetailsByUsername(refe);
                        if (dtResult.Rows.Count > 0)
                        {
                            user.sp_user = dtResult.Rows[0]["username"].ToString();
                            user.MembName_L = dtResult.Rows[0]["Memb_Name"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult SignUp(SignUpModel user)
        {
            try
            {
                SignUpModel objSignup = new SignUpModel();


                objSignup.Mode = "REGISTRATION";
                string otp = getRandomPassword();
                messagecontent = "YOUR REGISTRATION CONFIRMATION " + WebConfigurationManager.AppSettings["ProjectName"].ToString() + " OTP - " + otp + ".";

                Session["SignUPOTP"] = otp;




                DataTable dt1 = UserManager.CheackMobile_No(user.Mobile_No);
                if (dt1.Rows.Count >= 1)
                {
                    TempData["SignUpAlert"] = "Mobile No. Already used. Please try another..";
                    return RedirectToAction("SignUp", "Home");
                }
                DataTable dt2 = UserManager.CheackEmail(user.EMail);
                if (dt2.Rows.Count != 0)
                {
                    TempData["SignUpAlert"] = "Email-ID Already Exist. Please try another..";
                    return RedirectToAction("SignUp", "Home");
                }

                if (user.mpwd.Any(Char.IsWhiteSpace))
                {
                    TempData["SignUpAlert"] = "Spaces not allow in Password.";
                    return RedirectToAction("SignUp", "Home");
                }
                objSignup.client_ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(objSignup.client_ip))
                {
                    objSignup.client_ip = Request.ServerVariables["REMOTE_ADDR"];
                }
                #region Pin validation
                //if (string.IsNullOrEmpty(user.pin.Trim()))
                //{
                //    TempData["SignUpAlert"] = "Please enter Pin No.";
                //    return RedirectToAction("SignUp", "Home");
                //}

                #endregion
                #region Random Userid pwd
                int sta = 0;
                while (sta < 1)
                {
                    //user.username = "rh" + getrandompassword();
                    user.username = "rh" + otp;
                    string usern = user.username;
                    DataTable dtuser = UserManager.USER_REPORT_DETAILS("checkusername", usern);
                    if (dtuser.Rows.Count == 0)
                    {
                        sta = 1;
                    }
                }

                #endregion


                objSignup.sp_user = user.sp_user;
                if (string.IsNullOrEmpty(objSignup.sp_user))
                {
                    objSignup.sp_user = "LADOFOUNDATION";
                }
                objSignup.place = "L";
                objSignup.plan_type = "HELP";
                objSignup.username = user.username;
                objSignup.pwd = user.mpwd;
              // objSignup.mpwd = user.mpwd;
                objSignup.mpwd = Crypto.Encrypt(user.mpwd, System.Text.Encoding.Unicode);

                objSignup.Memb_Name = user.Memb_Name;
                objSignup.Mobile_No = user.Mobile_No;
                objSignup.EMail = user.EMail;
                objSignup.M_COUNTRY = "India";
                Session["objSignup"] = objSignup;

                #region Top up
                //DataTable dt = UserManager.Pindetails("CHECKPINADDNOTUSED", "0", null, user.pin.Trim(), null, null, null);
                //if (dt.Rows.Count > 0)
                //{
                //    if (dt.Rows[0]["tf_flag"].ToString() == "Y")
                //    {
                //        TempData["TopUpAlert"] = "This Pin Already transfer.";
                //    }
                //    else if (dt.Rows[0]["u_flag"].ToString() == "Y")
                //    {
                //        TempData["TopUpAlert"] = "This Pin Already Used.";
                //    }
                //    else
                //    {
                //        string amount = dt.Rows[0]["amount"].ToString();
                //        string pinid = dt.Rows[0]["PinID"].ToString();

                #endregion

                //if (bool.Parse(WebConfigurationManager.AppSettings["IsSignUpOTP"]))
                //{
                //    smsStatus = SmsHelper.SendSMS(objSignup.Mobile_No, messagecontent);
                //    return RedirectToAction("verifyUser", "Home");
                //}
                dtResult = UserManager.REGISTRATION(objSignup);
                if (dtResult.Rows.Count > 0)
                {
                    if (string.Equals(dtResult.Rows[0]["SP_STATUS"].ToString(), "SUCCESS"))
                    {
                        #region Top up
                        //int result = UserManager.User_TopUp("ADDTOPUP", dtResult.Rows[0]["MEMB_CODE"].ToString(), amount, null, "0", "PIN", amount, "1", pinid);
                        //if (result > 0)
                        //{
                        //    TempData["SignUpAlert"] = "1";
                        //}
                        //else
                        //{
                        //    TempData["SignUpAlert"] = "Topup failed.";
                        //    return RedirectToAction("SignUpAlert", "Home");
                        //}

                        #endregion

                        //     SendEmail(objSignup);
                        TempData["SignUpEmail"] = user.EMail;
                        TempData["SignUpUsername"] = user.username;
                        TempData["SignUpPassword"] = user.mpwd;
                        TempData["SignUpName"] = user.Memb_Name;
                        TempData["SignUpAlert"] = "Registration Successfull.";
                        TempData["Type"] = "Registration";
                        Session["username"] = user.username;
                        //return RedirectToAction("pattern", "Home");
                        return RedirectToAction("success", "Home");
                    }
                    else
                    {
                        TempData["SignUpAlert"] = dtResult.Rows[0]["SP_STATUS"].ToString();
                    }
                }

                // }
                //}
                //else
                //{
                //    TempData["TopUpAlert"] = "This Pin is invalid";
                //}
            }
            catch (Exception ex)
            {
                TempData["SignUpAlert"] = "Registration Failed. Error:" + ex.Message;
            }
            return RedirectToAction("SignUp", "Home");
        }

        public ActionResult verifyUser()
        {
            VerifyUserModel user = new VerifyUserModel();
            try
            {
                if (Session["SignUPOTP"] == null)
                {
                    return RedirectToAction("SignUp");
                }
                user.Request_Code = "";
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult verifyUser(VerifyUserModel user)
        {
            try
            {
                SignUpModel objSignup = new SignUpModel();
                objSignup = (SignUpModel)Session["objSignup"];

                if (!ModelState.IsValid)
                {
                    TempData["VerifySignAlert"] = "Please enter valid data.";
                    return RedirectToAction("verifyUser");
                }

                if (string.IsNullOrEmpty(user.Request_Code))
                {
                    TempData["VerifySignAlert"] = "Please enter otp.";
                    return RedirectToAction("verifyUser");
                }

                if (Session["SignUPOTP"] == null)
                {
                    TempData["VerifySignAlert"] = "Your otp is expired.";
                    return RedirectToAction("verifyUser");
                }

                if (!string.Equals(user.Request_Code, Session["SignUPOTP"].ToString()))
                {
                    TempData["VerifySignAlert"] = "Your otp is Wrong.";
                    return RedirectToAction("verifyUser");
                }


                dtResult = UserManager.REGISTRATION(objSignup);
                if (dtResult.Rows.Count > 0)
                {
                    if (string.Equals(dtResult.Rows[0]["SP_STATUS"].ToString(), "SUCCESS"))
                    {
                        string encryptEmail = Crypto.Encrypt(objSignup.username, System.Text.Encoding.Unicode);

                        SendEmail(objSignup);

                        return RedirectToAction("success", "Home");

                    }
                    else
                    {
                        TempData["VerifySignAlert"] = dtResult.Rows[0]["SP_STATUS"].ToString();
                    }
                }
                else
                {
                    TempData["VerifySignAlert"] = "Registration Failed.";
                }
            }
            catch (Exception ex)
            {
                TempData["VerifySignAlert"] = "Your Session is Expired. Error:" + ex.Message;
            }
            return RedirectToAction("verifyUser", "Home");
        }
        public ActionResult success()
        {
            UserModel user = new UserModel();
            try
            {
                if (Session["VerifyEMAILID"] != null)
                    user.EMail = Session["VerifyEMAILID"].ToString();

                if (Session["VerifyEmail"] != null)
                    user.status = Session["VerifyEmail"].ToString();
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            return View(user);
        }

        public ActionResult verifyEmail(string id)
        {
            try
            {
                string Email = Crypto.Decrypt(id, System.Text.Encoding.Unicode);
                dtResult = UserManager.UserLogin(Email);
                if (dtResult.Rows.Count > 0)
                {
                    if (string.Equals("N", dtResult.Rows[0]["M_Status"].ToString().Trim()))
                    {
                        int result = UserManager.verifyEmail(Email);
                        if (result > 0)
                        {
                            Session["VerifyEmail"] = "S";
                            if (!string.IsNullOrEmpty(dtResult.Rows[0]["Mobile_No"].ToString().Trim()))
                            {
                                messagecontent = "Your account verified successfully with " + WebConfigurationManager.AppSettings["ProjectName"].ToString() + "\r\n" + "USER ID : " + dtResult.Rows[0]["username"].ToString().Trim() + "\r\n" + "Password : " + dtResult.Rows[0]["mpwd"].ToString().Trim();
                                smsStatus = SmsHelper.SendSMS(dtResult.Rows[0]["Mobile_No"].ToString().Trim(), messagecontent);
                            }
                        }
                        else
                            Session["VerifyEmail"] = "F";
                    }
                    else
                    {
                        Session["VerifyEmail"] = "A";
                    }
                }
                else
                {
                    Session["VerifyEmail"] = "F";
                }
                Session["VerifyEMAILID"] = Email;
            }
            catch
            {
                Session["VerifyEmail"] = "F";
            }
            return RedirectToAction("success", "Home");
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgetPasswordModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ForgetPassAlert"] = "Please enter valid data.";
                    return RedirectToAction("ForgotPassword");
                }

                dtResult = UserManager.USER_REPORT_DETAILS("CHECKUSERNAME", user.username);
                if (dtResult.Rows.Count > 0)
                {

                    projectName = WebConfigurationManager.AppSettings["ProjectName"].ToString();
                    string mpwd = Crypto.Decrypt(dtResult.Rows[0]["mpwd"].ToString(), System.Text.Encoding.Unicode);
                    string emailEncrypt = Crypto.Encrypt(dtResult.Rows[0]["USERNAME"].ToString(), System.Text.Encoding.Unicode);
                    string CCMail, FromMail, Subject, Enquirer;

                    Enquirer = dtResult.Rows[0]["EMail"].ToString();

                    CCMail = "";
                    FromMail = "";

                    TempData["SignUpEmail"] = dtResult.Rows[0]["EMail"].ToString();
                    TempData["SignUpUsername"] = dtResult.Rows[0]["USERNAME"].ToString();
                    TempData["SignUpPassword"] = mpwd;

                    Subject = "RESET PASSWORD -" + WebConfigurationManager.AppSettings["ProjectName"].ToString();

                    using (var sr = new StreamReader(Server.MapPath(@"\App_Data\Templates\ResetPassword1.txt")))
                    {
                        Body = sr.ReadToEnd();
                    }
                    string messageBody = string.Format(Body, projectName, projectName, dtResult.Rows[0]["USERNAME"].ToString(), mpwd, WebConfigurationManager.AppSettings["domainName"]);
                    emailStatus = SendMail.SendMails(Enquirer, CCMail, FromMail, Subject, Body, "", true);

                    messagecontent = "Your valid User Id and Password is given below with Connectkare " + "\r\n" + "USER ID : " + dtResult.Rows[0]["USERNAME"].ToString() + "\r\n" + "Password : " + mpwd;
                    smsStatus = SmsHelper.SendSMS(dtResult.Rows[0]["Mobile_No"].ToString(), messagecontent);

                    TempData["SignUpEmail"] = Enquirer;
                    TempData["ForgetPassAlert"] = "Password Reset Successfully. Please Check Your Email or Mobileno.";
                    TempData["Type"] = "ForgotPassword";
                    return RedirectToAction("success", "Home");
                }
                else
                {
                    TempData["ForgetPassAlert"] = "Please enter valid User ID.";
                }
            }
            catch (Exception ex)
            {
                TempData["ForgetPassAlert"] = "Password Reset Failed. Error:" + ex.Message;
            }
            return RedirectToAction("ForgotPassword", "Home");
        }

        public ActionResult ContactPartial()
        {
            return PartialView("ContactPartial");
        }

        [HttpPost]
        public ActionResult Contact(ContactEnquiry contact)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ContactAlert"] = "Please enter valid data.";
                    return RedirectToAction("Index", "Home");
                }

                if (!string.IsNullOrEmpty(contact.username.Trim()) && !string.IsNullOrEmpty(contact.email.Trim()) && !string.IsNullOrEmpty(contact.message.Trim()))
                {
                    contact.message = contact.message.Trim().Replace("\r\n", "<br/>");
                    int result = UserManager.AddContactEnquiry(contact.username.Trim(), contact.email.Trim(), contact.phone_no, contact.subject, contact.message.Trim());
                    if (result > 0)
                    {
                        TempData["ContactAlert"] = "Enquiry Submitted Successfully.";
                    }
                    else
                    {
                        TempData["ContactAlert"] = "Enquiry Submitted Failed.";
                    }
                }
                else
                {
                    TempData["ContactAlert"] = "Enquiry Submitted Failed.";
                }
            }
            catch (Exception ex)
            {
                TempData["ContactAlert"] = "Enquiry Submitted Failed. Error:" + ex.Message;
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Common.CurrentUserInfo = null;
            Common.CookieUserID = null;
            Common.CookieUserType = null;

            return RedirectToAction("Index", "Home");
            //return Json("True", JsonRequestBehavior.AllowGet);
        }

        public ActionResult pattern()
        {
            return View();
        }

        public ActionResult setpassword(string username, string password)
        {
            dtResult = UserManager.USER_PATTERN_UPDATE("UPDATEPATTERN", username);
            if (dtResult.Rows.Count > 0)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        public void SendEmail(SignUpModel objSignup)
        {
            string encryptEmail = Crypto.Encrypt(objSignup.username, System.Text.Encoding.Unicode);
            projectName = WebConfigurationManager.AppSettings["ProjectName"].ToString();
            string CCMail, FromMail, Subject, Enquirer;

            Enquirer = objSignup.EMail;
            CCMail = "";
            FromMail = "";
            Subject = objSignup.Memb_Name + " - WELCOME TO " + projectName;


            using (var sr = new StreamReader(Server.MapPath(@"\App_Data\Templates\Signup1.txt")))
            {
                Body = sr.ReadToEnd();
            }
            string messageBody = string.Format(Body, projectName, projectName, objSignup.Memb_Name.ToUpper(), projectName, objSignup.EMail,
                objSignup.Mobile_No, objSignup.username, Crypto.Decrypt(objSignup.mpwd, System.Text.Encoding.Unicode), projectName, projectName, projectName);

            emailStatus = SendMail.SendMails(Enquirer, CCMail, FromMail, Subject, Body, "", true);
            //messagecontent = @"You have Successfully Registered with  " + projectName + "\r\n" +
            //                      "USER ID : " + objSignup.username + "\r\n" + "Password : " + Crypto.Decrypt(objSignup.mpwd, System.Text.Encoding.Unicode);
            //smsStatus = SmsHelper.SendSMS(objSignup.Mobile_No.Trim(), messagecontent);

            TempData["SignUpEmail"] = objSignup.EMail;
            TempData["VerifySignAlert"] = "Registration Successfull.";
            TempData["Type"] = "Registration";
            TempData["SignUpUsername"] = objSignup.username;
            TempData["SignUpPassword"] = Crypto.Decrypt(objSignup.mpwd, System.Text.Encoding.Unicode);
            TempData["SignUpName"] = objSignup.Memb_Name;
            TempData["SignUpAlert"] = "Registration Successfull.";
            TempData["Type"] = "Registration";

        }
    }
}