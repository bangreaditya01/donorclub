using donorclub.DBEntity;
using donorclub.Models;
using LudoFoundation_app.CommanFunction;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace donorclub.Controllers
{
    public class DashboardController : BasePage
    {

        string messagecontent = string.Empty;
        string smsStatus = string.Empty;
        DataTable dtResult = new DataTable();

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

        public string GetClientIP()
        {
            string client_ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (client_ip == "" || client_ip == null)
            {
                client_ip = Request.ServerVariables["REMOTE_ADDR"];
            }
            return client_ip;
        }
        public ActionResult Index(string level, string status)
        {

            DashboardModel dash = new DashboardModel();

            List<Transactions> dList = new List<Transactions>();
            List<NewsModel> newList = new List<NewsModel>();
            List<LetterModel> alertList = new List<LetterModel>();

            List<UserLevelModel> teammatesList = new List<UserLevelModel>();
            try
            {

                dash.Current_IP = GetClientIP();

                dtResult = UserManager.GetDashboardData("Dashboard_Total", userInfo.memb_code);
                if (dtResult.Rows.Count > 0)
                {
                    string JSONString = JsonConvert.SerializeObject(dtResult);
                    var obj = JsonConvert.DeserializeObject<List<DashboardModel>>(JSONString);
                    dash = obj.First();
                    //dash.ac_status = dtTotal.Rows[0]["ac_status"].ToString();
                    //dash.Total_Team = dtTotal.Rows[0]["Total_Team"].ToString();
                    //dash.Total_Bonus = dtTotal.Rows[0]["Total_Bonus"].ToString();
                    //dash.Total_Get = dtTotal.Rows[0]["Total_Get"].ToString();
                    //dash.Total_Provide = dtTotal.Rows[0]["Total_Provide"].ToString();
                    //dash.Total_PH = dtTotal.Rows[0]["Total_PH"].ToString();

                    //dash.Leader_Income = dtTotal.Rows[0]["Leader_Income"].ToString();
                    //dash.Leader_Wallet = dtTotal.Rows[0]["Leader_Wallet"].ToString();
                }


                dtResult = UserManager.GetDashboardData("TRANSACTION", userInfo.memb_code);
                if (!string.IsNullOrEmpty(level) && !string.IsNullOrEmpty(status))
                {
                    dash.level_Status = "True";
                    dtResult = UserManager.GetDashboardDataLevel("TRANSACTION", userInfo.memb_code, level, status);
                }
                if (dtResult.Rows.Count > 0)
                {
                    string JSONString = JsonConvert.SerializeObject(dtResult);
                    dList = JsonConvert.DeserializeObject<List<Transactions>>(JSONString);
                }
                dash.transList = dList;

                dList = new List<Transactions>();
                dtResult = UserManager.GetDashboardData("ORDER", userInfo.memb_code);
                if (dtResult.Rows.Count > 0)
                {
                    string JSONString = JsonConvert.SerializeObject(dtResult);
                    dList = JsonConvert.DeserializeObject<List<Transactions>>(JSONString);
                }
                dash.orderList = dList;


                dtResult = UserManager.USER_REPORT("CHECKUSERTEMPF", userInfo.memb_code);
                if (dtResult.Rows.Count > 0)
                {
                    TempData["LoginAlert"] = "YOUR ID IS BLOCKED PLEASE CONTACT AT -" + WebConfigurationManager.AppSettings["senderEmlID"].ToString();
                    userInfo = null;
                    Common.CurrentUserInfo = null;
                    Common.CookieUserID = null;
                    Common.CookieUserType = null;
                    return RedirectToAction("Signin", "Home");
                }

                dtResult = UserManager.GetLatestNews();
                if (dtResult.Rows.Count > 0)
                {
                    string JSONString = JsonConvert.SerializeObject(dtResult);
                    newList = JsonConvert.DeserializeObject<List<NewsModel>>(JSONString);
                }
                dash.Newslist = newList;


                dtResult = UserManager.GetUserAlert();
                if (dtResult.Rows.Count > 0)
                {
                    string jsonString = JsonConvert.SerializeObject(dtResult);
                    alertList = JsonConvert.DeserializeObject<List<LetterModel>>(jsonString);
                }
                dash.AlertList = alertList;

                dtResult = UserManager.GetAllTeam(userInfo.memb_code);
                if (dtResult.Rows.Count > 0)
                {
                    string jsonString = JsonConvert.SerializeObject(dtResult);
                    teammatesList = JsonConvert.DeserializeObject<List<UserLevelModel>>(jsonString);
                }
                dash.teamamtesList = teammatesList;


            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
                dash.transList = dList;
                dash.orderList = dList;
                dash.Newslist = newList;
                dash.AlertList = alertList;
                dash.teamamtesList = teammatesList;
            }
            return View(dash);
        }
        public ActionResult confirmTrans(string membCode, string trans)
        {
            try
            {
                //if (!string.IsNullOrEmpty(otp_code))
                //{
                //    if (Session["RequestCode"] != null)
                //    {
                //        if (string.Equals(otp_code, Session["RequestCode"].ToString()))
                //        {
                if (Convert.ToInt32(trans) > 0)
                {
                    int result = UserManager.LINK_ACCEPT(userInfo.memb_code, trans, DateTime.Now.ToString(), DateTime.Now.ToString());
                    if (result > 0)
                    {
                        dtResult = UserManager.USER_REPORT("GETMOBILENOBYTRANSACTION", trans);
                        if (dtResult.Rows.Count > 0)
                        {
                            messagecontent = WebConfigurationManager.AppSettings["ProjectName"].ToString() + "Your Provide Help link is confirm for Order No " + "PH" + dtResult.Rows[0]["Commit_No"].ToString() + " of amount ₹ " + dtResult.Rows[0]["AMOUNT"].ToString().Trim() + "";
                            smsStatus = SmsHelper.SendSMS(dtResult.Rows[0]["Mobile_No"].ToString().Trim(), messagecontent);

                            messagecontent = WebConfigurationManager.AppSettings["ProjectName"].ToString() + "Your Get Help link is confirm for Order No " + "PH" + dtResult.Rows[0]["Commit_No"].ToString() + " of amount ₹ " + dtResult.Rows[0]["AMOUNT"].ToString().Trim() + "";
                            smsStatus = SmsHelper.SendSMS(userInfo.Mobile_No, messagecontent);

                        }

                        TempData["ConfirmLinkAlert"] = "Transaction no " + trans + " is confirmed successfully.";
                    }
                    else
                    {
                        TempData["ConfirmLinkAlert"] = "Something to wrong to Confirm.";
                    }
                }
                //        }
                //        else
                //        {
                //            TempData["ConfirmLinkAlert"] = "Your OTP is invalid.";
                //        }
                //    }
                //    else
                //    {
                //        TempData["ConfirmLinkAlert"] = "Your OTP is expired.";
                //    }
                //}
                //else
                //{
                //    TempData["ConfirmLinkAlert"] = "Please enter OTP.";
                //}
            }
            catch (Exception ex)
            {
                TempData["ConfirmLinkAlert"] = "Something to wrong to Confirm. Error:" + ex.Message;
            }
            return RedirectToAction("Index", "Dashboard");
        }
        public ActionResult rejectlink(string trans)
        {
            try
            {
                if (Convert.ToInt32(trans) > 0)
                {
                    int result = UserManager.RejectLink(userInfo.memb_code, trans);
                    if (result > 0)
                    {
                        dtResult = UserManager.USER_REPORT("GETMOBILENOBYTRANSACTION", trans);
                        if (dtResult.Rows.Count > 0)
                        {
                            messagecontent = WebConfigurationManager.AppSettings["ProjectName"].ToString() + " Your Provide Help link is Reject for Order No " + "PH" + dtResult.Rows[0]["Commit_No"].ToString() + " of amount ₹ " + dtResult.Rows[0]["AMOUNT"].ToString().Trim() + ",\n if you already amount paid by get help person so please contact your sponser or admin. ";
                            smsStatus = SmsHelper.SendSMS(dtResult.Rows[0]["Mobile_No"].ToString().Trim(), messagecontent);

                        }
                        TempData["RejectLinkAlert"] = "Transaction no " + trans + " is rejected successfully.";
                    }
                    else
                    {
                        TempData["RejectLinkAlert"] = "Something to wrong to reject.";
                    }
                }
            }
            catch
            {
                TempData["RejectLinkAlert"] = "Something to wrong to reject.";
            }
            return RedirectToAction("Index", "Dashboard");
        }
        public ActionResult extentTime(string membCode, string trans)
        {
            try
            {
                if (Convert.ToInt32(trans) > 0)
                {
                    dtResult = UserManager.ExtendTime(userInfo.memb_code, trans);
                    if (dtResult.Rows.Count > 0)
                    {
                        TempData["ExtendTimeAlert"] = dtResult.Rows[0]["Status_Text"].ToString();
                    }
                    else
                    {
                        TempData["ExtendTimeAlert"] = "Something to wrong to Extend.";
                    }
                }
            }
            catch
            {
                TempData["ExtendTimeAlert"] = "Something to wrong to Extend.";
            }
            return RedirectToAction("Index", "Dashboard");
        }

        public ActionResult addFakeSlip(string trans)
        {
            try
            {
                if (Convert.ToInt32(trans) > 0)
                {
                    dtResult = UserManager.FAKE_SLIP("0", trans, userInfo.memb_code);
                    if (dtResult.Rows.Count > 0)
                    {
                        TempData["FakeSlipAlert"] = dtResult.Rows[0]["Status_MSG"].ToString();
                    }
                    else
                    {
                        TempData["FakeSlipAlert"] = "Something to wrong to generate fake Hash.";
                    }
                }
            }
            catch
            {
                TempData["FakeSlipAlert"] = "Something to wrong to generate fake Hash.";
            }
            return RedirectToAction("Index", "Dashboard");
        }
        public ActionResult getMakePayment(string memb_code, string commitNo, string hsrno)
        {
            UserModel user = new UserModel();
            List<Transactions> dList = new List<Transactions>();
            try
            {
                commitNo = commitNo.Replace("PH", "");
                commitNo = commitNo.Replace("GH", "");
                if (Convert.ToInt32(commitNo) > 0)
                {
                    DataTable dtTrans = UserManager.ORDERDETAILS("DETAILS", userInfo.memb_code, commitNo);
                    if (dtTrans.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtTrans.Rows.Count; i++)
                        {
                            if (hsrno == dtTrans.Rows[i]["srno"].ToString())
                            {
                                dtResult = UserManager.USER_REPORT("GETBANKDETAILS", dtTrans.Rows[i]["recievermembCode"].ToString());
                                if (dtResult.Rows.Count > 0)
                                {
                                    user.ac_name = dtResult.Rows[0]["ac_name"].ToString();
                                    user.ac_no = dtResult.Rows[0]["ac_no"].ToString();
                                    user.ac_type = dtResult.Rows[0]["ac_type"].ToString();
                                    user.bk_branch = dtResult.Rows[0]["bk_branch"].ToString();
                                    user.bk_ifsc = dtResult.Rows[0]["bk_ifsc"].ToString();
                                    user.bk_name = dtResult.Rows[0]["bk_name"].ToString();
                                    user.btc_ac = dtResult.Rows[0]["btc_ac"].ToString();
                                    user.paytm_no = dtResult.Rows[0]["paytm_no"].ToString();
                                    user.gpay_no = dtResult.Rows[0]["gpay_no"].ToString();
                                    user.phonepay_no = dtResult.Rows[0]["phonepay_no"].ToString();
                                    user.amount = dtTrans.Rows[i]["usdamt"].ToString();
                                    user.Mobile_No = dtResult.Rows[0]["Mobile_No"].ToString();
                                }

                                DataTable dtP = UserManager.USER_REPORT("GETBANKDETAILS", dtTrans.Rows[i]["sendermembCode"].ToString());
                                if (dtP.Rows.Count > 0)
                                {
                                    user.provider_mobile_no = dtP.Rows[0]["Mobile_No"].ToString();
                                }
                            }
                        }
                        string JSONString = JsonConvert.SerializeObject(dtTrans);
                        dList = JsonConvert.DeserializeObject<List<Transactions>>(JSONString);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            user.hsrno = hsrno;
            user.transactionList = dList;
            return PartialView("getMakePayment", user);
        }
        public ActionResult getConfirmPayment(string memb_code, string commitNo, string hsrno)
        {
            UserModel user = new UserModel();
            List<Transactions> dList = new List<Transactions>();
            try
            {
                commitNo = commitNo.Replace("GH", "");
                if (Convert.ToInt32(commitNo) > 0)
                {
                    DataTable dtTrans = UserManager.ORDERDETAILS("DETAILS", userInfo.memb_code, commitNo);
                    if (dtTrans.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtTrans.Rows.Count; i++)
                        {
                            if (hsrno == dtTrans.Rows[i]["srno"].ToString())
                            {
                                DataTable dtResult = UserManager.USER_REPORT("GETBANKDETAILS", dtTrans.Rows[i]["recievermembCode"].ToString());
                                if (dtResult.Rows.Count > 0)
                                {
                                    user.ac_name = dtResult.Rows[0]["ac_name"].ToString();
                                    user.ac_no = dtResult.Rows[0]["ac_no"].ToString();
                                    user.ac_type = dtResult.Rows[0]["ac_type"].ToString();
                                    user.bk_branch = dtResult.Rows[0]["bk_branch"].ToString();
                                    user.bk_ifsc = dtResult.Rows[0]["bk_ifsc"].ToString();
                                    user.bk_name = dtResult.Rows[0]["bk_name"].ToString();
                                    user.paytm_no = dtResult.Rows[0]["paytm_no"].ToString();
                                    user.gpay_no = dtResult.Rows[0]["gpay_no"].ToString();
                                    user.phonepay_no = dtResult.Rows[0]["phonepay_no"].ToString();
                                    user.btc_ac = dtResult.Rows[0]["btc_ac"].ToString();
                                    user.amount = dtTrans.Rows[i]["usdamt"].ToString();
                                    user.Mobile_No = dtResult.Rows[0]["Mobile_No"].ToString();
                                }

                                DataTable dtP = UserManager.USER_REPORT("GETBANKDETAILS", dtTrans.Rows[i]["sendermembCode"].ToString());
                                if (dtP.Rows.Count > 0)
                                {
                                    user.provider_mobile_no = dtP.Rows[0]["Mobile_No"].ToString();
                                }
                            }
                        }
                        //DataTable dtR = UserManager.USER_REPORT("GETBANKDETAILS", dtTrans.Rows[0]["recievermembCode"].ToString());
                        //if (dtR.Rows.Count > 0)
                        //{
                        //    user.ac_name = dtR.Rows[0]["ac_name"].ToString();
                        //    user.ac_no = dtR.Rows[0]["ac_no"].ToString();
                        //    user.ac_type = dtR.Rows[0]["ac_type"].ToString();
                        //    user.bk_branch = dtR.Rows[0]["bk_branch"].ToString();
                        //    user.bk_ifsc = dtR.Rows[0]["bk_ifsc"].ToString();
                        //    user.bk_name = dtR.Rows[0]["bk_name"].ToString();
                        //}

                        string JSONString = JsonConvert.SerializeObject(dtTrans);
                        dList = JsonConvert.DeserializeObject<List<Transactions>>(JSONString);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            user.hsrno = hsrno;
            user.transactionList = dList;
            return PartialView("getConfirmPayment", user);
        }
        public ActionResult addTransactionLink(string trans, string rmemb_code, string message, string transFor)
        {
            try
            {
                if (Convert.ToInt32(trans) > 0)
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        dtResult = UserManager.TRANSACTION_LINK("ADD", trans, userInfo.memb_code, rmemb_code, message, transFor);
                        if (dtResult.Rows.Count > 0)
                        {
                            TempData["TransactionLinkAlert"] = dtResult.Rows[0]["Status_Text"].ToString();
                        }
                        else
                        {
                            TempData["TransactionLinkAlert"] = "Something to wrong to added hash code.";
                        }
                    }
                    else
                        TempData["TransactionLinkAlert"] = "Please enter hash code.";
                }
            }
            catch (Exception ex)
            {
                TempData["TransactionLinkAlert"] = "Something to wrong to added hash code.Error:" + ex.Message;
            }
            return RedirectToAction("Index", "Dashboard");
            // return Json("True", JsonRequestBehavior.AllowGet);
        }
                                                                                                                
        [HttpPost]
        public ActionResult UploadSlip(string phmsrno, string message, HttpPostedFileBase phFile)
        {
            try
            {
                string srno = phmsrno;
                string attachment = null;
                int imageStatus = 0;
                if (phFile != null)
                {
                    string fileName = phFile.FileName;
                    string sextension = Path.GetExtension(phFile.FileName);
                    if (string.Equals(sextension.ToLower(), ".jpg") || string.Equals(sextension.ToLower(), ".jpeg") || string.Equals(sextension.ToLower(), ".png"))
                    {
                        string wdate = System.DateTime.Now.ToString();
                        wdate = wdate.Replace("/", "");
                        wdate = wdate.Replace("_", "");
                        wdate = wdate.Replace(":", "");
                        wdate = wdate.Replace("-", "");
                        wdate = wdate.Replace(" ", "");

                        string fileAttn = "SLIP" + wdate + sextension;
                        string path = Server.MapPath("~/Content/assignAttachment/" + fileAttn);

                        //To save file, use SaveAs method
                        phFile.SaveAs(path); //File will be saved in application root

                        attachment = WebConfigurationManager.AppSettings["domainName"] + "/Content/assignAttachment/" + fileAttn; ;
                    }
                    imageStatus = 1;
                }

                if (imageStatus == 1)
                {
                    if (!string.IsNullOrEmpty(attachment))
                    {
                        if (!string.IsNullOrEmpty(message))
                            message = message.Replace("\r\n", "<br/>");

                        dtResult = UserManager.Assignment_MSG("ADD", srno, userInfo.memb_code, "0", message, attachment);
                        TempData["SlipUploadAlert"] = "Slip uploaded successfully.";
                    }
                    else
                    {
                        TempData["SlipUploadAlert"] = "Please Attachment Select Only PNG , JPEG , JPG Image.";
                    }
                }
                else
                {
                    TempData["SlipUploadAlert"] = "Please select slip.";
                }
            }
            catch (Exception ex)
            {
                TempData["SlipUploadAlert"] = "Slip upload failed. Error:" + ex.Message;
            }
            return RedirectToAction("Index");
        }
        public ActionResult getTransactionLink(string trans)
        {
            List<TransactionLinkModel> dList = new List<TransactionLinkModel>();
            try
            {
                if (Convert.ToInt32(trans) > 0)
                {
                    dtResult = UserManager.TRANSACTION_LINK("GET", trans, null, null, null, null);
                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        TransactionLinkModel transT = new TransactionLinkModel();
                        transT.message = dtResult.Rows[i]["message"].ToString();
                        transT.trans_type = "GET";
                        transT.transFor = dtResult.Rows[i]["transFor"].ToString();

                        dList.Add(transT);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            return PartialView("getTransactionLink", dList.ToList());
        }
        public ActionResult getOrderDetails(string commitNo)
        {
            List<Transactions> dList = new List<Transactions>();
            try
            {
                commitNo = commitNo.Replace("GH", "");
                commitNo = commitNo.Replace("PH", "");
                if (Convert.ToInt32(commitNo) > 0)
                {
                    dtResult = UserManager.ORDERDETAILS("ORDERDETAILS", userInfo.memb_code, commitNo);
                    if (dtResult.Rows.Count > 0)
                    {
                        string JSONString = JsonConvert.SerializeObject(dtResult);
                        dList = JsonConvert.DeserializeObject<List<Transactions>>(JSONString);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            return PartialView("getOrderDetails", dList.ToList());
        }
        public ActionResult deleteOrderDetails(string commitNo, string odrType)
        {
            try
            {
                commitNo = commitNo.Replace("GH", "");
                commitNo = commitNo.Replace("PH", "");
                if (Convert.ToInt32(commitNo) > 0)
                {
                    dtResult = UserManager.ORDERDETAILS(odrType, userInfo.memb_code, commitNo);
                    if (dtResult.Rows.Count > 0)
                    {
                        TempData["DeleteOrderAlert"] = dtResult.Rows[0]["Status_Text"].ToString();
                    }
                    else
                    {
                        TempData["DeleteOrderAlert"] = "Something to wrong to delete Order.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["DeleteOrderAlert"] = "Something to wrong to delete Order.Error:" + ex.Message;
            }
            return RedirectToAction("Index", "Dashboard");
        }

        //-------- Profile -------//
        #region Profile
        public ActionResult MyProfile()
        {
            getCountries();
            UserModel user = new UserModel();
            try
            {
                user.Memb_Name = userInfo.memb_name;
                user.M_COUNTRY = userInfo.M_COUNTRY;
                user.Mobile_No = userInfo.Mobile_No;
                user.EMail = userInfo.EmailID;
                user.username = userInfo.username;
                user.OldPass = userInfo.mpwd;
                user.Spon_Code = userInfo.sp_ID;
                user.sp_user = userInfo.sp_name;
                user.MembName_L = userInfo.MembName_L;
                user.btc_ac = userInfo.btc_ac;
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            return View(user);
        }

        public ActionResult editProfile()
        {
            getCountries();
            UserModel user = new UserModel();
            try
            {
                user.Memb_Name = userInfo.memb_name;
                user.M_COUNTRY = userInfo.M_COUNTRY;
                user.Mobile_No = userInfo.Mobile_No;
                user.EMail = userInfo.EmailID;
                user.username = userInfo.username;
                user.OldPass = userInfo.mpwd;
                user.Spon_Code = userInfo.sp_ID;
                user.sp_user = userInfo.sp_name;
                user.MembName_L = userInfo.MembName_L;
                user.btc_ac = userInfo.btc_ac;
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            return View(user);
        }
        [HttpPost]
        public ActionResult editProfile(UserModel user, HttpPostedFileBase userFile)
        {
            try
            {
                if ((userFile != null) && (userFile.ContentLength > 0))
                {
                    string sextension = Path.GetExtension(userFile.FileName);
                    if (sextension.ToLower() == ".png" || sextension.ToLower() == ".jpg" || sextension.ToLower() == ".jpeg")
                    {
                        string wdate = DateTime.Now.ToString("yyyymmddhhMMss");
                        string fileAttn = "USER" + wdate + sextension;
                        string path = Server.MapPath("~/Content/userAttachment/" + fileAttn);
                        Stream strm = userFile.InputStream;
                        var targetFile = path;
                        GenerateThumbnails(0.5, strm, targetFile);
                        user.MembName_L = fileAttn;
                    }
                }

                user.memb_code = userInfo.memb_code;
                user.Mobile_No = userInfo.Mobile_No;
                user.EMail = userInfo.EmailID;
                user.username = userInfo.username;
                user.mpwd = userInfo.mpwd;

                dtResult = UserManager.USER_DETAILS("EDITPROFILE", user);
                if (dtResult.Rows.Count > 0)
                {
                    if (string.Equals(dtResult.Rows[0]["SP_STATUS"].ToString(), "SUCCESS"))
                    {
                        TempData["ProfileAlert"] = "Profile Update Successfully.";

                        userInfo.memb_name = dtResult.Rows[0]["memb_name"].ToString();
                        userInfo.M_COUNTRY = dtResult.Rows[0]["M_COUNTRY"].ToString();
                        userInfo.Mobile_No = dtResult.Rows[0]["Mobile_No"].ToString();
                        userInfo.dbo = dtResult.Rows[0]["dboS"].ToString();
                        userInfo.Gender = dtResult.Rows[0]["Gender"].ToString();
                        userInfo.EmailID = dtResult.Rows[0]["EMail"].ToString();
                        userInfo.Address1 = dtResult.Rows[0]["Address1"].ToString();
                        userInfo.City = dtResult.Rows[0]["City"].ToString();
                        userInfo.btc_ac = dtResult.Rows[0]["btc_ac"].ToString();
                        userInfo.MembName_L = dtResult.Rows[0]["MembName_L"].ToString();
                    }
                    else
                        TempData["ProfileAlert"] = "Profile Update Failed.";
                }
                else
                {
                    TempData["ProfileAlert"] = "Profile Update Failed.";
                }
            }
            catch (Exception ex)
            {
                TempData["ProfileAlert"] = "Error in Edit Profile.Error:" + ex.Message;
            }
            return RedirectToAction("editProfile", "Dashboard");
        }

        public ActionResult changepwd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult changepwd(ChangePasswordModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ProfileAlert"] = "Please Enter Valid Data.";
                    return RedirectToAction("changepwd", "Home");
                }
                if (user.NewPass.Any(Char.IsWhiteSpace))
                {
                    TempData["ProfileAlert"] = "Spaces not allow in Password.";
                    return RedirectToAction("changepwd", "Home");
                }

                if (string.Equals(user.OldPass, userInfo.mpwd))
                {
                    dtResult = UserManager.Change_Password(userInfo.memb_code, user.NewPass.Trim());
                    if (dtResult.Rows.Count > 0)
                    {
                        if (string.Equals(dtResult.Rows[0]["SP_STATUS"].ToString(), "SUCCESS"))
                        {
                            TempData["ProfileAlert"] = "Password Change Successfully.";
                            userInfo.mpwd = dtResult.Rows[0]["mpwd"].ToString();
                        }
                        else
                            TempData["ProfileAlert"] = "Password Change Failed.";
                    }
                    else
                    {
                        TempData["ProfileAlert"] = "Password Change Failed.";
                    }
                }
                else
                {
                    TempData["ProfileAlert"] = "Current Password is incorrect";
                }
            }
            catch
            {
                TempData["ProfileAlert"] = "Error in Change Password.";
            }
            return RedirectToAction("changepwd", "Dashboard");
        }

        #endregion

        #region Bank detail
        public ActionResult addBankDetails()
        {
            BankDetailsModel user = new BankDetailsModel();
            try
            {
                user.ac_name = userInfo.ac_name;
                user.ac_no = userInfo.ac_no;
                user.ac_type = userInfo.ac_type;
                user.bk_branch = userInfo.bk_branch;
                user.bk_ifsc = userInfo.bk_ifsc;
                user.bk_name = userInfo.bk_name;
                user.gpay_no = userInfo.gpay_no;
                user.phonepay_no = userInfo.phonepay_no;
                user.paytm_no = userInfo.paytm_no;
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult addBankDetails(BankDetailsModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ProfileAlert"] = "Please Enter Valid Data.";
                    return RedirectToAction("addBankDetails", "Dashboard");
                }
                //if (!string.Equals(Session["Bank"],user.request_code))
                //{
                //    TempData["ProfileAlert"] = "Please Enter Valid OTP Details.";
                //    return RedirectToAction("addBankDetails", "Home");
                //}

                dtResult = UserManager.ADD_ACCOUNT_DETAILS(userInfo.memb_code, user);
                if (dtResult.Rows.Count > 0)
                {
                    if (string.Equals(dtResult.Rows[0]["SP_STATUS"].ToString(), "SUCCESS"))
                    {
                        TempData["ProfileAlert"] = "Bank Details Updated Successfully.";
                        userInfo.ac_name = dtResult.Rows[0]["ac_name"].ToString();
                        userInfo.ac_no = dtResult.Rows[0]["ac_no"].ToString();
                        userInfo.bk_name = dtResult.Rows[0]["bk_name"].ToString();
                        userInfo.bk_branch = dtResult.Rows[0]["bk_branch"].ToString();
                        userInfo.bk_ifsc = dtResult.Rows[0]["bk_ifsc"].ToString();
                        userInfo.ac_type = dtResult.Rows[0]["ac_type"].ToString();
                        userInfo.gpay_no = dtResult.Rows[0]["gpay_no"].ToString();
                        userInfo.phonepay_no = dtResult.Rows[0]["phonepay_no"].ToString();
                        userInfo.paytm_no = dtResult.Rows[0]["paytm_no"].ToString();
                    }
                }
                else
                {
                    TempData["ProfileAlert"] = "Update Bank Details Failed.";
                }
            }
            catch (Exception ex)
            {
                TempData["ProfileAlert"] = "Error Bank Details Failed." + ex;
            }
            return RedirectToAction("addBankDetails", "Dashboard");
        }
        #endregion
        public JsonResult getRequestCode(string codeType, string mobileno)
        {
            string returnMSG = string.Empty;
            string requestCode = string.Empty;
            try
            {
                requestCode = SmsHelper.GenerateRandomOTP(6);
                //string email = userInfo.EmailID;

                string sub = string.Empty;
                if (codeType == "GET")
                {
                    sub = "Withdrawal";
                    Session["RequestCode"] = requestCode;


                    string messagecontent = "Your OTP Code for Confirm Get help link is " + requestCode;
                    if (!string.IsNullOrEmpty(userInfo.Mobile_No))
                    {
                        smsStatus = SmsHelper.SendSMS(userInfo.Mobile_No.Trim(), messagecontent);
                    }

                    GetRequest(requestCode);
                }
                else if (codeType == "W")
                {
                    sub = "Withdrawal";
                    Session["WithdrawlRequestCode"] = requestCode;
                    messagecontent = "Your Request Code for Withdrawal is " + requestCode;
                    if (!string.IsNullOrEmpty(userInfo.Mobile_No))
                    {
                        smsStatus = SmsHelper.SendSMS(userInfo.Mobile_No.Trim(), messagecontent);
                    }
                }
                else if (codeType == "PU")
                {
                    sub = "Profile Update";
                    Session["ProfileRequestCode"] = requestCode;
                }
                else if (codeType == "CP")
                {
                    sub = "Change Password";
                    Session["CPasswordRequestCode"] = requestCode;
                }
                else if (codeType == "TP")
                {
                    sub = "Top Up";
                    Session["TopUpRequestCode"] = requestCode;
                    messagecontent = "Your Request Code for Topup is " + requestCode;
                    if (!string.IsNullOrEmpty(userInfo.Mobile_No))
                    {
                        smsStatus = SmsHelper.SendSMS(userInfo.Mobile_No.Trim(), messagecontent);
                    }
                }
                else if (codeType == "B")
                {
                    sub = "Bank Details";
                    Session["Bank"] = requestCode;

                    messagecontent = "Your Bank Details Change OTP Code for : " + requestCode;
                    if (!string.IsNullOrEmpty(userInfo.Mobile_No))
                    {
                        smsStatus = SmsHelper.SendSMS(userInfo.Mobile_No, messagecontent);
                    }
                }
                else if (codeType == "FT")
                {
                    sub = "Fund Transfer";
                    Session["FundTransferRequestCode"] = requestCode;
                }

                //string result = OTPMail(email, sub, requestCode);
                returnMSG = "1";
            }
            catch (Exception ex)
            {
                returnMSG = "Request code sending failed.Error:" + ex.Message;
            }
            return Json(returnMSG, JsonRequestBehavior.AllowGet);
        }
        private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
        {
            try
            {
                using (var image = System.Drawing.Image.FromStream(sourcePath))
                {
                    var newWidth = (int)(image.Width * scaleFactor);
                    var newHeight = (int)(image.Height * scaleFactor);
                    var thumbnailImg = new Bitmap(newWidth, newHeight);
                    var thumbGraph = Graphics.FromImage(thumbnailImg);
                    thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                    thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                    thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                    thumbGraph.DrawImage(image, imageRectangle);
                    thumbnailImg.Save(targetPath, image.RawFormat);
                }
            }
            catch
            {

            }
        }

        public ActionResult GetRequest(string requestCode)
        {
            // string requestCode = string.Empty;
            try
            {
                // requestCode = GenerateRandomOTP(6);
                // Session["RequestCode"] = requestCode;
                string CCMail, FromMail, Subject, Enquirer;
                string Body = string.Empty;
                Enquirer = userInfo.EmailID;
                CCMail = "";
                FromMail = "";

                Subject = "Request Code for Get Help";

                Body = "<!DOCTYPE html>"
                        + "<html>"
                        + "<head>"
                        + "    <title>Request Code for Get Help</title>"
                        + "</head>"
                        + "<body>"
                        + "    <div style='width:500px; margin:auto; background-color:#fff6;margin-top:50px;padding: 10px 10px 10px 10px;border-radius: 6px 7px;box-shadow: 0px 1px 11px 4px #2a220733;background:url('https//www.planwallpaper.com/static/images/light_textured_backround.jpg');background-position: 50% 50%;'>"
                        + "    <h2 style='text-align:center;width: 500px;'>Request Code for Get Help </h2>" + WebConfigurationManager.AppSettings["ProjectName"].ToString()
                        + "    <div style='width:500px;'>"
                        + "        <h4 style='margin-left: 26px;text-align:center;'>Hi, Sir / Madam</h4>"
                        + "        <hr>"
                        + "        <div style='width:500px;'>"
                        + "            <br>"
                        + "            <p style='text-align:center;'>Your Request Code is : <strong>" + requestCode + "</strong></p>"
                        + "            <br>"
                        + "        </div>"
                        + "        <center></center>"
                        + "        <p style='text-align:center;'>Thank You..<p>"
                        + "    </div>"
                        + "    </div>"
                        + "</body>"
                        + "</html>";


                string Status = SendMail.SendMails(Enquirer, CCMail, FromMail, Subject, Body, "", true);


            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            return Json("True", JsonRequestBehavior.AllowGet);
        }
        public ActionResult activeBlockedMember(string membCode, string status)
        {
            try
            {
                dtResult = UserManager.USER_REPORT("CHECKBLOCKSTATUS", membCode);
                if (dtResult.Rows.Count > 0)
                {
                    if (dtResult.Rows[0]["StautsB"].ToString() == "Y")
                    {
                        int result = UserManager.User_Details("BLOCKED", membCode, status);
                        if (result > 0)
                        {
                            if (status == "Y")
                            {
                                TempData["BlockedAlert"] = "Member Unblock Successfully...";
                            }
                            else
                            {
                                TempData["BlockedAlert"] = "Member Block Successfully...";
                            }
                        }
                        else
                            TempData["BlockedAlert"] = "Member Activation Failed.";
                    }
                    else
                    {
                        TempData["BlockedAlert"] = "This user dont have any pending link.";
                    }
                }
                else
                {
                    TempData["BlockedAlert"] = "Member Block Failed.";
                }
            }
            catch
            {
                TempData["BlockedAlert"] = "Member Block Failed.";
            }
            return RedirectToAction("Direct_team", "Dashboard");
        }

        //-------------------------- Provide Help ---------------------//

        #region Provide Help
        public ActionResult ProvideHelp()
        {
            ProvideHelpModel provide = new ProvideHelpModel();
            try
            {
                provide.Packages = PopulatePackages();
                dtResult = UserManager.USER_REPORT("GETLASTPROVIDEHELP", userInfo.memb_code);
                if (dtResult.Rows.Count > 0)
                {
                    provide.LastProvide = dtResult.Rows[0]["amount"].ToString();

                    //provide.amount = ((Convert.ToInt32(provide.LastProvide) * 80) / 100).ToString();
                }


                #region Pin
                //DataTable dtCHECKPIN = UserManager.USER_REPORT("GETPINAMOUNT", userInfo.memb_code);
                //if (dtCHECKPIN.Rows.Count > 0)
                //{
                //    string amountpin = dtCHECKPIN.Rows[0]["amount"].ToString();

                //    if (amountpin == "500")
                //    {
                //        provide.amount = "2500";
                //    }
                //    else if (amountpin == "1000")
                //    {
                //        provide.amount = "5000";
                //    }
                //    else if (amountpin == "2000")
                //    {
                //        provide.amount = "10000";
                //    }

                //}

                //DataTable CHECKPINRETOPUP = UserManager.USER_REPORT("GETPINPHCOUNT", userInfo.memb_code);
                //if (CHECKPINRETOPUP.Rows.Count > 0)
                //{
                //    if (Convert.ToInt32(CHECKPINRETOPUP.Rows[0]["pincount"].ToString()) <= Convert.ToInt32(CHECKPINRETOPUP.Rows[0]["phcount"].ToString()))
                //    {
                //        TempData["InvestmentAlert"] = "Your ID is not Re-Top by Pin please first activate by PIN then provide help will be open";
                //    }
                //}


                //DataTable dtRecommitCHECK = UserManager.USER_REPORT("GETCOMMITMENTRECOMMITMENT", userInfo.memb_code);
                //if (dtRecommitCHECK.Rows.Count > 0)
                //{
                //    if (dtRecommitCHECK.Rows[0]["COMMITMENTRECOMMITMENT"].ToString()=="0")
                //    {
                //        provide.Recommitment = dtRecommitCHECK.Rows[0]["COMMITMENTRECOMMITMENT"].ToString();
                //    }
                //    else
                //    {
                //        provide.Recommitment = dtRecommitCHECK.Rows[0]["COMMITMENTRECOMMITMENT"].ToString();
                //    }

                //}
                #endregion

            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            return View(provide);
        }


        private static List<SelectListItem> PopulatePackages()
        {
            List<SelectListItem> Packages = new List<SelectListItem>();

            ProvideHelpModel provide = new ProvideHelpModel();
            string allPackages = WebConfigurationManager.AppSettings["Packages"].ToString();
            string[] pkg = allPackages.Split(',');

            for (int i = 0; i < pkg.Length; i++)
            {
                Packages.Add(new SelectListItem
                {
                    Text = "₹" + pkg[i].ToString(),
                    Value = pkg[i].ToString()
                });


            }
            return Packages;
        }


        [HttpPost]
        public ActionResult ProvideHelp(ProvideHelpModel provide)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["InvestmentAlert"] = "Please enter valid data.";
                    return RedirectToAction("ProvideHelp");
                }

                if (string.IsNullOrEmpty(provide.pin.Trim()))
                {
                    TempData["InvestmentAlert"] = "Please enter Pin No.";
                    return RedirectToAction("ProvideHelp");
                }

                //DataTable CHECKAUTHRISED = UserManager.USER_REPORT("CHECKAUTHRISED", userInfo.memb_code);
                //if (CHECKAUTHRISED.Rows.Count > 0)
                //{
                //    if (CHECKAUTHRISED.Rows[0]["authrised"].ToString() != "G")
                //    {
                //        TempData["InvestmentAlert"] = "Your ID is not activate please activate first then provide help is open";
                //        return RedirectToAction("ProvideHelp");
                //    }
                //}
                if (string.IsNullOrEmpty(userInfo.ac_name) || string.IsNullOrEmpty(userInfo.ac_no)
                   || string.IsNullOrEmpty(userInfo.bk_name) || string.IsNullOrEmpty(userInfo.bk_branch)
                   || string.IsNullOrEmpty(userInfo.bk_ifsc))
                {
                    TempData["InvestmentAlert"] = "Please first update your bank details.";
                    return RedirectToAction("ProvideHelp");
                }

                #region PIN
                //DataTable dtCHECKPIN = UserManager.USER_REPORT("GETPINAMOUNT", userInfo.memb_code);
                //if (dtCHECKPIN.Rows.Count > 0)
                //{
                //    string amountpin = dtCHECKPIN.Rows[0]["amount"].ToString();

                //    if (amountpin == "500")
                //    {
                //        provide.amount = "2500";
                //    }
                //    else if (amountpin == "1000")
                //    {
                //        provide.amount = "5000";
                //    }
                //    else if (amountpin == "2000")
                //    {
                //        provide.amount = "10000";
                //    }
                //}
                //else
                //{
                //    TempData["InvestmentAlert"] = "Your ID is not activate please activate first then provide help is open";
                //    return RedirectToAction("ProvideHelp");
                //}

                //DataTable CHECKAUTHRISED = UserManager.USER_REPORT("CHECKAUTHRISED", userInfo.memb_code);
                //if (CHECKAUTHRISED.Rows.Count > 0)
                //{
                //    if (CHECKAUTHRISED.Rows[0]["authrised"].ToString() != "G")
                //    {
                //        TempData["InvestmentAlert"] = "Your ID is not activate please activate first then provide help is open";
                //        return RedirectToAction("ProvideHelp");
                //    }
                //}

                //DataTable CHECKPINRETOPUP = UserManager.USER_REPORT("GETPINPHCOUNT", userInfo.memb_code);
                //if (CHECKPINRETOPUP.Rows.Count > 0)
                //{
                //    if (Convert.ToInt32(CHECKPINRETOPUP.Rows[0]["pincount"].ToString()) <= Convert.ToInt32(CHECKPINRETOPUP.Rows[0]["phcount"].ToString()))
                //    {
                //        TempData["InvestmentAlert"] = "Your ID is not Re-Top by Pin please first activate by PIN then provide help will be open";
                //        return RedirectToAction("ProvideHelp");
                //    }
                //}
                //if (Convert.ToDecimal(provide.amount) < 100000)
                //{
                //    if (string.IsNullOrEmpty(userInfo.ac_name) || string.IsNullOrEmpty(userInfo.ac_no)
                //    || string.IsNullOrEmpty(userInfo.bk_name) || string.IsNullOrEmpty(userInfo.bk_branch)
                //    || string.IsNullOrEmpty(userInfo.bk_ifsc) || string.IsNullOrEmpty(userInfo.ac_type))
                //    {
                //        TempData["InvestmentAlert"] = "Please first update your bank details.";
                //        return RedirectToAction("ProvideHelp");
                //    }
                //}
                //else
                //{
                //    if (string.IsNullOrEmpty(userInfo.btc_ac))
                //    {
                //        TempData["InvestmentAlert"] = "Please first update your Bitcoin Address.";
                //        return RedirectToAction("ProvideHelp");
                //    }
                //}

                //DataTable dtCCC = UserManager.USER_REPORT("CHECKCURRENTPH", userInfo.memb_code);
                //if (dtCCC.Rows.Count > 0)
                //{
                //    TempData["InvestmentAlert"] = "Your current PH is going on.";
                //    return RedirectToAction("ProvideHelp");
                //}

                //DataTable dtCHECK = UserManager.USER_REPORT("GETLASTPROVIDEHELP", userInfo.memb_code);
                //if (dtCHECK.Rows.Count > 0)
                //{
                //    if (Convert.ToDecimal(dtCHECK.Rows[0]["amount"].ToString()) >= Convert.ToDecimal(provide.amount))
                //    {
                //        TempData["InvestmentAlert"] = "Recommitment should be greater then previous commitment.";
                //        return RedirectToAction("ProvideHelp");
                //    }
                //}


                #endregion

                //int depositeStatus = 0;
                //string allPackages = string.Empty;
                //provide.Packages = PopulatePackages();
                //var selectedItem = provide.Packages.Find(p => p.Value == provide.amount.ToString());
                //if (selectedItem != null)
                //{
                //    allPackages = WebConfigurationManager.AppSettings["Packages"].ToString();
                //    string[] pkg = allPackages.Split(',');

                //    for (int i = 0; i < pkg.Length; i++)
                //    {
                //        if (string.Equals(selectedItem.Value, pkg[i]))
                //        {
                //            depositeStatus = 1;
                //            break;
                //        }
                //    }
                //}
                //if (depositeStatus == 1)
                //{
                //    DataTable dPH = UserManager.USER_REPORT("CHECKPROVIDEPENDING", userInfo.memb_code);
                //    DataTable dtGH = UserManager.USER_REPORT("CHECKGETPENDING", userInfo.memb_code);
                //    if (dPH.Rows.Count > 0)
                //    {
                //        TempData["InvestmentAlert"] = "Your previous provide help is pending.";
                //        return RedirectToAction("ProvideHelp");
                //    }





                    //else if (dtGH.Rows.Count > 0)
                    //{
                    //    TempData["InvestmentAlert"] = "Your get help is pending.";
                    //}
                    else
                    {

                        DataTable dt = UserManager.Pindetails("CHECKPINADDNOTUSED", "0", null, provide.pin.Trim(), null, null, null);
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["tf_flag"].ToString() == "Y")
                            {
                                TempData["InvestmentAlert"] = "This Pin Already transfer.";
                            }
                            else if (dt.Rows[0]["u_flag"].ToString() == "Y")
                            {
                                TempData["InvestmentAlert"] = "This Pin Already Used.";
                            }
                            else
                            {
                                string amount = dt.Rows[0]["amount"].ToString();
                                string pinid = dt.Rows[0]["PinID"].ToString();

                                int result2 = UserManager.User_TopUp("ADDTOPUP", userInfo.memb_code, amount, null, "0", "PIN", amount, "1", pinid);
                                if (result2 > 0)
                                {

                                    int result = UserManager.UserInvestment("0", userInfo.memb_code, string.IsNullOrEmpty(provide.amount) ? "0" : provide.amount);
                                    if (result > 0)
                                    {
                                        TempData["InvestmentAlert"] = "Provide Help Submitted Successfull.";
                                        Session["SuccessTitle"] = "Provide Help Success";

                                    }
                                    else
                                        TempData["InvestmentAlert"] = "Provide Help Submitted Failed.";
                                }
                                else
                                {
                                    TempData["InvestmentAlert"] = "ProvideHelp failed.";
                                    return RedirectToAction("ProvideHelp");

                                }




                            }
                        }
                        else
                        {
                            TempData["TopUpAlert"] = "This Pin is invalid";
                        }
                    }
                //-----
                //else
                //{
                //    TempData["InvestmentAlert"] = "Please enter valid amount.";
                //}
            }
            catch (Exception ex)
            {
                TempData["InvestmentAlert"] = "Provide Help Submitted Failed.";
            }
            return RedirectToAction("providehelp", "Dashboard");
        }

        #endregion


        //------------------------ Get Help -----------------//
        #region  Get Help 
        public ActionResult GetHelp()
        {
            GetHelpModel trans = new GetHelpModel();
            try
            {
                dtResult = UserManager.USER_REPORT("WALLETBALANCE", userInfo.memb_code);
                if (dtResult.Rows.Count > 0)
                {
                    trans.Total_Balance = dtResult.Rows[0]["Total_Balance"].ToString();
                }
                else
                {
                    trans.Total_Balance = "0";
                }

                dtResult = UserManager.USER_REPORT("GETMAXCOMMITMENT", userInfo.memb_code);
                if (dtResult.Rows.Count > 0)
                {
                    trans.Max_Commitment = dtResult.Rows[0]["Max_Amount"].ToString();
                }
            }
            catch
            {
                trans.Total_Balance = "0";
            }
            return View(trans);
        }


        public Boolean ValidateGetHelp(GetHelpModel get)
        {
            if (!ModelState.IsValid)
            {
                TempData["WithdrawalsAlert"] = "Please enter valid data.";
                return false;
            }

            if (string.IsNullOrEmpty(userInfo.ac_name) || string.IsNullOrEmpty(userInfo.ac_no)
                   || string.IsNullOrEmpty(userInfo.bk_name) || string.IsNullOrEmpty(userInfo.bk_branch)
                   || string.IsNullOrEmpty(userInfo.bk_ifsc))
            {
                TempData["WithdrawalsAlert"] = "Please first update your bank details.";
                return false;
            }
            if (Convert.ToDecimal(get.amount) % Convert.ToDecimal(WebConfigurationManager.AppSettings["MultipleGrowthWithdrawal"].ToString()) != 0
                    || Convert.ToDecimal(get.amount) < Convert.ToDecimal(WebConfigurationManager.AppSettings["MinGrowthWithdrawal"].ToString()))
            {
                TempData["WithdrawalsAlert"] = @"Minimum  withdrawal is ₹ " + WebConfigurationManager.AppSettings["MinGrowthWithdrawal"].ToString() +
                    ",Maximum withdrawal is ₹ " + WebConfigurationManager.AppSettings["MaxGrowthWithdrawal"].ToString() + " and multiple of ₹  " +
                    WebConfigurationManager.AppSettings["MultipleGrowthWithdrawal"].ToString();
                return false;
            }
            //dtResult = UserManager.USER_REPORT("GETCURRENDAY", userInfo.memb_code);
            //if (dtResult.Rows.Count > 0)
            //{
            //    if (dtResult.Rows[0]["CurrentDayName"].ToString().ToLower() == WebConfigurationManager.AppSettings["StopGrowthWithdrawal"].ToString())
            //    {
            //        TempData["WithdrawalsAlert"] = "Withdrawal only on Monday to saturday at 7:00 AM to 9:00 AM.";
            //        return false;
            //    }
            //}

            //dtResult = UserManager.USER_REPORT("GETCURRENTTIME", userInfo.memb_code);
            //if (dtResult.Rows.Count == 0)
            //{
            //    TempData["WithdrawalsAlert"] = "Withdrawal daily at 7:00 AM to 09:00 AM. ";
            //    return false;
            //}




            dtResult = UserManager.USER_REPORT("CHECKPROVIDEPENDING", userInfo.memb_code);
            if (dtResult.Rows.Count > 0)
            {
                TempData["WithdrawalsAlert"] = "Your previous provide help is pending.";
                return false;
            }

            dtResult = UserManager.USER_REPORT("CHECKGETPENDING", userInfo.memb_code);
            if (dtResult.Rows.Count > 0)
            {
                TempData["WithdrawalsAlert"] = "Your get help is pending.";
                return false;
            }

            dtResult = UserManager.USER_REPORT("GETWITHDRAWFLAG", userInfo.memb_code);
            if (dtResult.Rows.Count > 0)
            {
                if (dtResult.Rows[0]["WITHDRAWFLAG"].ToString().ToUpper() == "N")
                {
                    TempData["WithdrawalsAlert"] = "From Second Withdrawal one direct compulsory.";
                    return false;
                }
            }


            #region temp
            //DataTable dtmax = UserManager.USER_REPORT("GETMAXCOMMITMENT", userInfo.memb_code);
            //if (dtmax.Rows.Count > 0)
            //{
            //    string maxwith = dtmax.Rows[0]["Max_Amount"].ToString();
            //    if (Convert.ToDecimal(maxwith) > 50000)
            //    {
            //        if (Convert.ToDecimal(get.amount) % 1000 != 0 || Convert.ToDecimal(get.amount) > 20000)
            //        {
            //            TempData["WithdrawalsAlert"] = "Minimum withdrawal is ₹ 1000 to ₹ 20000 and multiple of ₹ 1000";
            //            return RedirectToAction("GetHelp");
            //        }
            //    }
            //    else if (Convert.ToDecimal(maxwith) == 50000)
            //    {
            //        if (Convert.ToDecimal(get.amount) % 1000 != 0 || Convert.ToDecimal(get.amount) > 10000)
            //        {
            //            TempData["WithdrawalsAlert"] = "Minimum withdrawal is ₹ 1000 to ₹ 10000 and multiple of ₹ 1000";
            //            return RedirectToAction("GetHelp");
            //        }
            //    }
            //    else
            //    {
            //        if (Convert.ToDecimal(get.amount) % 1000 != 0 || Convert.ToDecimal(get.amount) > 5000)
            //        {
            //            TempData["WithdrawalsAlert"] = "Minimum withdrawal is ₹ 1000 to ₹ 5000 and multiple of ₹ 1000";
            //            return RedirectToAction("GetHelp");
            //        }
            //    }
            //}

            //DataTable dtTimeW = UserManager.USER_REPORT("GETWITHDRAWALTIME", userInfo.memb_code);
            //if (dtTimeW.Rows.Count > 0)
            //{
            //    TempData["WithdrawalsAlert"] = "Withdrawal should be done after confirmation of provide help 240 hrs ";
            //    return RedirectToAction("GetHelp");
            //}

            #endregion

            return true;
        }

        [HttpPost]
        public ActionResult GetHelp(GetHelpModel get)
        {
            try
            {
                if (!ValidateGetHelp(get))
                {
                    return RedirectToAction("gethelp", "Dashboard");
                }

                dtResult = UserManager.USER_REPORT("WALLETBALANCE", userInfo.memb_code);
                if (dtResult.Rows.Count > 0)
                {
                    string Total_Balance = dtResult.Rows[0]["Total_Balance"].ToString();

                    if (Convert.ToDecimal(get.amount) <= Convert.ToDecimal(Total_Balance))
                    {
                        int result = UserManager.UserWithdrawals("0", userInfo.memb_code, string.IsNullOrEmpty(get.amount) ? "0" : get.amount
                            , null, null, null, userInfo.ac_name, userInfo.ac_no, userInfo.bk_name, userInfo.bk_branch, null, userInfo.bk_ifsc,
                            null, "GROWTHBONUS");
                        if (result > 0)
                        {
                            //TempData["WithdrawalsAlert"] = "Get Help Submitted Successfully.";
                            Session["SuccessTitle"] = "Withdrawal Success";
                            return RedirectToAction("success", "Dashboard");
                        }
                        else
                            TempData["WithdrawalsAlert"] = "Get Help Submitted Failed.";
                    }
                    else
                    {
                        TempData["WithdrawalsAlert"] = "Your balance amount is less than request amount.";
                    }
                }
                else
                {
                    TempData["WithdrawalsAlert"] = "Get Help Submitted Failed.";
                }
            }
            catch
            {
                TempData["WithdrawalsAlert"] = "Get Help Submitted Failed.";
            }
            return RedirectToAction("gethelp", "Dashboard");
        }
        #endregion

        //------------------------ Get Help Leader Bonus -----------------//

        public Boolean ValidatewithdrawalRewardBonus(GetHelpModel get)
        {
            if (!ModelState.IsValid)
            {
                TempData["WithdrawalsAlert"] = "Please enter valid data.";
                return false;
            }

            if (string.IsNullOrEmpty(userInfo.ac_name) || string.IsNullOrEmpty(userInfo.ac_no)
               || string.IsNullOrEmpty(userInfo.bk_name) || string.IsNullOrEmpty(userInfo.bk_branch)
               || string.IsNullOrEmpty(userInfo.bk_ifsc))
            {
                TempData["WithdrawalsAlert"] = "Please first update your bank details.";
                return false;
            }

            if (Convert.ToDecimal(get.amount) % Convert.ToDecimal(WebConfigurationManager.AppSettings["MultipleWorkingWithdrawal"].ToString()) != 0
                || Convert.ToDecimal(get.amount) < Convert.ToDecimal(WebConfigurationManager.AppSettings["MinWorkingWithdrawal"].ToString()))
            {
                TempData["WithdrawalsAlert"] = @"Minimum  withdrawal is ₹ " + WebConfigurationManager.AppSettings["MinWorkingWithdrawal"].ToString() +
                    ",Maximum withdrawal is ₹ " + WebConfigurationManager.AppSettings["MaxWorkingWithdrawal"].ToString() + " and multiple of ₹  " +
                    WebConfigurationManager.AppSettings["MultipleWorkingWithdrawal"].ToString();

                return false;
            }

            //if (Convert.ToDecimal(get.amount) > Convert.ToDecimal(WebConfigurationManager.AppSettings["MinWorkingWithdrawal"].ToString()))
            //{
            //    TempData["WithdrawalsAlert"] = @"Minimum  withdrawal is ₹ " + WebConfigurationManager.AppSettings["MinWorkingWithdrawal"].ToString() +
            //        ",Maximum withdrawal is ₹ " + WebConfigurationManager.AppSettings["MaxWorkingWithdrawal"].ToString() + " and multiple of ₹  " +
            //        WebConfigurationManager.AppSettings["MultipleWorkingWithdrawal"].ToString();
            //    return false;
            //}

            //dtResult = UserManager.USER_REPORT("GETCURRENDAY", userInfo.memb_code);
            //if (dtResult.Rows.Count > 0)
            //{
            //    if (dtResult.Rows[0]["CurrentDayName"].ToString().ToLower() != "sunday")
            //    {
            //        TempData["WithdrawalsAlert"] = "Withdrawal only on sunday at 8:00 AM to 9:00 AM.";
            //        return false;
            //    }
            //}

            //dtResult = UserManager.USER_REPORT("GETCURRENTTIME", userInfo.memb_code);
            //if (dtResult.Rows.Count == 0)
            //{
            //    TempData["WithdrawalsAlert"] = "Withdrawal only at 8:00 AM to 9:00 AM.";
            //    return false;
            //}


            dtResult = UserManager.USER_REPORT("CHECKPROVIDEPENDING", userInfo.memb_code);
            if (dtResult.Rows.Count > 0)
            {
                TempData["WithdrawalsAlert"] = "Your previous provide help is pending.";
                return false;
            }

            dtResult = UserManager.USER_REPORT("CHECKGETPENDING", userInfo.memb_code);
            if (dtResult.Rows.Count > 0)
            {
                TempData["WithdrawalsAlert"] = "Your get help is pending.";
                return false;
            }


            //DataTable dtmax = UserManager.USER_REPORT("GETMAXREWARDSWITHDRAWAL", userInfo.memb_code);
            //if (dtmax.Rows.Count > 0)
            //{
            //    string maxwith = dtmax.Rows[0]["Max_Amount"].ToString();
            //    if (Convert.ToDecimal(get.amount) > Convert.ToDecimal(maxwith))
            //    {
            //        TempData["WithdrawalsAlert"] = "Your maximum withdrawal limit is ₹ " + maxwith + ".";
            //        return RedirectToAction("withdrawalRewardBonus");
            //    }
            //}
            return true;
        }
        public ActionResult withdrawalRewardBonus()
        {
            GetHelpModel trans = new GetHelpModel();
            try
            {
                dtResult = UserManager.USER_REPORT("WORKINGWALLETBALANCE", userInfo.memb_code);
                if (dtResult.Rows.Count > 0)
                {
                    trans.Total_Balance = dtResult.Rows[0]["Total_Balance"].ToString();
                }
                else
                {
                    trans.Total_Balance = "0";
                }

                dtResult = UserManager.USER_REPORT("GETMAXREWARDSWITHDRAWAL", userInfo.memb_code);
                if (dtResult.Rows.Count > 0)
                {
                    trans.Max_Commitment = dtResult.Rows[0]["Max_Amount"].ToString();
                }
            }
            catch (Exception ex)
            {
                trans.Total_Balance = "0";
            }
            return View(trans);
        }

        [HttpPost]
        public ActionResult withdrawalRewardBonus(GetHelpModel get)
        {
            try
            {
                if (!ValidatewithdrawalRewardBonus(get))
                {
                    return RedirectToAction("withdrawalRewardBonus");
                }

                dtResult = UserManager.USER_REPORT("LEADERWALLETBALANCE", userInfo.memb_code);
                if (dtResult.Rows.Count > 0)
                {
                    string Total_Balance = dtResult.Rows[0]["Total_Balance"].ToString();

                    if (Convert.ToDecimal(get.amount) <= Convert.ToDecimal(Total_Balance))
                    {
                        int result = UserManager.UserWithdrawals("0", userInfo.memb_code, string.IsNullOrEmpty(get.amount) ? "0" : get.amount
                            , "LEVEL", null, null, userInfo.ac_name, userInfo.ac_no, userInfo.bk_name, userInfo.bk_branch, null, userInfo.bk_ifsc, null, "LEADERBONUS");
                        if (result > 0)
                        {
                            //  TempData["WithdrawalsAlert"] = "Withdrawal Submitted Successfully.";
                            Session["SuccessTitle"] = "Withdrawal Success";
                            return RedirectToAction("success", "Dashboard");
                        }
                        else
                            TempData["WithdrawalsAlert"] = "Withdrawal Submitted Failed.";
                    }
                    else
                    {
                        TempData["WithdrawalsAlert"] = "Your balance amount is less than request amount.";
                    }
                }
                else
                {
                    TempData["WithdrawalsAlert"] = "Withdrawal Submitted Failed.";
                }
            }
            catch (Exception ex)
            {
                TempData["WithdrawalsAlert"] = "Withdrawal Submitted Failed.";
            }
            return RedirectToAction("withdrawalRewardBonus", "Dashboard");
        }
        public ActionResult withdrawalHistory()
        {
            List<GetHelpModel> dList = new List<GetHelpModel>();
            try
            {
                dtResult = UserManager.GetGetHelpDetails(userInfo.memb_code, "LEADERBONUS");
                if (dtResult.Rows.Count > 0)
                {
                    string JSONString = JsonConvert.SerializeObject(dtResult);
                    dList = JsonConvert.DeserializeObject<List<GetHelpModel>>(JSONString);
                }
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            return View(dList.ToList());
        }
        public ActionResult success()
        {
            try
            {
                ViewBag.SuccessMSG = "Your request has been received. Wait patiently to be matched with another participant.";
                ViewBag.SuccessTitle = Session["SuccessTitle"].ToString();
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            return View();
        }

        //-------------------- Report ----------------//
        #region Report

        public ActionResult GetHelpHistory()
        {
            List<GetHelpModel> lstGetHelpHistory = new List<GetHelpModel>();
            try
            {
                dtResult = UserManager.USER_REPORT("GHHISTORY", userInfo.memb_code);
                if (dtResult.Rows.Count > 0)
                {
                    string JSONString = JsonConvert.SerializeObject(dtResult);
                    lstGetHelpHistory = JsonConvert.DeserializeObject<List<GetHelpModel>>(JSONString);
                }
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            return View(lstGetHelpHistory.ToList());
        }
        public ActionResult ProvideHistory()
        {
            List<ProvideHelpModel> lstProvideHistory = new List<ProvideHelpModel>();
            try
            {
                dtResult = UserManager.USER_REPORT("PHHISTORY", userInfo.memb_code);
                if (dtResult.Rows.Count > 0)
                {
                    string JSONString = JsonConvert.SerializeObject(dtResult);
                    lstProvideHistory = JsonConvert.DeserializeObject<List<ProvideHelpModel>>(JSONString);
                }
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            return View(lstProvideHistory.ToList());
        }
        public ActionResult roi_income()
        {
            List<ReportModel> lstROIDetails = new List<ReportModel>();
            try
            {
                dtResult = UserManager.USER_REPORT("ROIINCOME", userInfo.memb_code);
                if (dtResult.Rows.Count > 0)
                {
                    string JSONString = JsonConvert.SerializeObject(dtResult);
                    lstROIDetails = JsonConvert.DeserializeObject<List<ReportModel>>(JSONString);
                }
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            return View(lstROIDetails.ToList());
        }
        public ActionResult direct_income()
        {
            List<ReportModel> lstDirectIncome = new List<ReportModel>();
            try
            {
                dtResult = UserManager.USER_REPORT("DIRECTINCOME", userInfo.memb_code);
                if (dtResult.Rows.Count > 0)
                {
                    string JSONString = JsonConvert.SerializeObject(dtResult);
                    lstDirectIncome = JsonConvert.DeserializeObject<List<ReportModel>>(JSONString);
                }
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            return View(lstDirectIncome.ToList());
        }
        public ActionResult level_income()
        {
            List<ReportModel> lstLevelIncome = new List<ReportModel>();
            try
            {
                dtResult = UserManager.USER_REPORT("LEVELINCOME", userInfo.memb_code);
                if (dtResult.Rows.Count > 0)
                {
                    string JSONString = JsonConvert.SerializeObject(dtResult);
                    lstLevelIncome = JsonConvert.DeserializeObject<List<ReportModel>>(JSONString);
                }
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            return View(lstLevelIncome.ToList());
        }
        public ActionResult Speed_Income()
        {

            List<ReportModel> lstSpeedIncome = new List<ReportModel>();
            try
            {
                dtResult = UserManager.USER_REPORT("SPEEDINCOME", userInfo.memb_code);
                if (dtResult.Rows.Count > 0)
                {
                    string JSONString = JsonConvert.SerializeObject(dtResult);
                    lstSpeedIncome = JsonConvert.DeserializeObject<List<ReportModel>>(JSONString);
                }
            }
            catch
            {

            }
            return View(lstSpeedIncome.ToList());
        }
        public ActionResult Direct_team()
        {
            List<UserLevelModel> dList = new List<UserLevelModel>();
            try
            {
                dtResult = UserManager.USER_REPORT("DIRECTTEAM", userInfo.memb_code);
                if (dtResult.Rows.Count > 0)
                {
                    string JSONString = JsonConvert.SerializeObject(dtResult);
                    dList = JsonConvert.DeserializeObject<List<UserLevelModel>>(JSONString);
                }
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            return View(dList.ToList());
        }
        public ActionResult Downline_Team()
        {
            List<UserLevelModel> dList = new List<UserLevelModel>();
            try
            {
                dtResult = UserManager.USER_REPORT("DOWNLINETEAM", userInfo.memb_code);
                if (dtResult.Rows.Count > 0)
                {
                    string JSONString = JsonConvert.SerializeObject(dtResult);
                    dList = JsonConvert.DeserializeObject<List<UserLevelModel>>(JSONString);
                }
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            return View(dList.ToList());
        }


        #endregion

        //-------------------- Support ---------------//
        #region Support

        public ActionResult GenerateTicket()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GenerateTicket(string Ticket_Desc, string Subject)
        {
            try
            {
                if (string.IsNullOrEmpty(Ticket_Desc) || string.IsNullOrWhiteSpace(Ticket_Desc))
                {
                    TempData["SupportAlert"] = "Please Enter Ticket Description";
                }
                else
                {
                    Subject = Subject.Replace("\r\n", "<br/>");
                    string Ticket_To = "Support Team";
                    string Ticket_From = userInfo.fromticketuser;
                    string Ticket_Status = "Open";

                    string Ticket_Type = "S";


                    int result = SupportTicketManager.AddUpdate_SupportTicket("0", userInfo.memb_code, Ticket_Type, "0", Ticket_To, Ticket_From, Subject, Ticket_Desc, null, Ticket_Status);
                    if (result > 0)
                    {
                        TempData["SupportAlert"] = "Ticket created Successfully.";
                    }
                    else
                    {
                        TempData["SupportAlert"] = "Ticket creation Failed.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["SupportAlert"] = "Ticket creation Failed.Error:" + ex.Message;
            }
            return RedirectToAction("GenerateTicket", "Dashboard");
        }
        public ActionResult ReceiveTicket()
        {
            List<SupportTicketModel> dList = new List<SupportTicketModel>();
            try
            {
                dtResult = SupportTicketManager.GetSupportTicket("0", userInfo.memb_code, "RECIEVE");
                if (dtResult.Rows.Count > 0)
                {
                    string jsonString = JsonConvert.SerializeObject(dtResult);
                    dList = JsonConvert.DeserializeObject<List<SupportTicketModel>>(jsonString);
                }
            }
            catch
            {

            }
            return View(dList.ToList());
        }
        public ActionResult SendTicket()
        {
            List<SupportTicketModel> dList = new List<SupportTicketModel>();
            try
            {
                dtResult = SupportTicketManager.GetSupportTicket("0", userInfo.memb_code, "SEND");
                if (dtResult.Rows.Count > 0)
                {
                    string jsonString = JsonConvert.SerializeObject(dtResult);
                    dList = JsonConvert.DeserializeObject<List<SupportTicketModel>>(jsonString);
                }
            }
            catch
            {

            }
            return View(dList.ToList());
        }
        #endregion
        //----------------------------- PIN Pages --------------------//

        #region PIN Details

        public ActionResult Pin_request()
        {
            PINDETAILSMODEL trans = new PINDETAILSMODEL();
            try
            {
                trans.amount = "300";
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
                trans.amount = "600";
            }
            return View(trans);
        }

        [HttpPost]
        public ActionResult Pin_request(PINDETAILSMODEL fund)
        {
            try
            {
                // fund.amount = "400";
                if (string.IsNullOrEmpty(fund.amount))
                {
                    TempData["CreatePinAlert"] = "Please select amount.";
                    return RedirectToAction("Pin_request");
                }
                if (string.IsNullOrEmpty(fund.Quantiy))
                {
                    TempData["CreatePinAlert"] = "Please enter quantity.";
                    return RedirectToAction("Pin_request");
                }

                if (Convert.ToDecimal(fund.Quantiy) <= 0)
                {
                    TempData["CreatePinAlert"] = "Please enter quantity greater than 0.";
                    return RedirectToAction("Pin_request");
                }

                dtResult = UserManager.PinRequestdetails("ADDPINREQUEST", "0", userInfo.memb_code, null, null
                    , userInfo.memb_code, fund.amount, fund.Quantiy);
                if (dtResult.Rows.Count > 0)
                {
                    TempData["CreatePinAlert"] = dtResult.Rows[0]["Success"].ToString();
                }
                else
                {
                    TempData["CreatePinAlert"] = "Pin Request Added failed.";
                }
            }
            catch (Exception ex)
            {
                TempData["CreatePinAlert"] = "Pin Request Added failed.Error:" + ex.Message;
            }
            return RedirectToAction("Pin_request");
        }

        public ActionResult Pin_request_history()
        {
            List<PINDETAILSMODEL> dList = new List<PINDETAILSMODEL>();
            try
            {
                dtResult = UserManager.Pindetails("MYREQUESTPINHISTORY", "0", userInfo.memb_code, null, null, userInfo.memb_code, null);
                if (dtResult.Rows.Count > 0)
                {
                    string jsonString = JsonConvert.SerializeObject(dtResult);
                    dList = JsonConvert.DeserializeObject<List<PINDETAILSMODEL>>(jsonString);
                }
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            return View(dList);
        }

        public ActionResult Pin_history()
        {
            List<PINDETAILSMODEL> dList = new List<PINDETAILSMODEL>();
            try
            {
                dtResult = UserManager.Pindetails("MYPINHISTORY", "0", userInfo.memb_code, null, null, userInfo.memb_code, null);
                if (dtResult.Rows.Count > 0)
                {
                    string jsonString = JsonConvert.SerializeObject(dtResult);
                    dList = JsonConvert.DeserializeObject<List<PINDETAILSMODEL>>(jsonString);
                }
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;

            }
            return View(dList);
        }

        public ActionResult Pin_create()
        {
            PINDETAILSMODEL trans = new PINDETAILSMODEL();
            try
            {
                trans.amount = "300";
                dtResult = UserManager.USER_REPORT("WALLETBALANCE", userInfo.memb_code);
                if (dtResult.Rows.Count > 0)
                {
                    trans.Main_Wallet = dtResult.Rows[0]["Total_Balance"].ToString();
                }
                else
                {
                    trans.Main_Wallet = "0";
                }
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
                trans.Main_Wallet = "0";
            }
            return View(trans);
        }

        [HttpPost]
        public ActionResult Pin_create(PINDETAILSMODEL fund)
        {
            try
            {
                //fund.amount = "200";
                if (string.IsNullOrEmpty(fund.amount))
                {
                    TempData["CreatePinAlert"] = "Please select amount.";
                    return RedirectToAction("createPin");
                }

                if (string.IsNullOrEmpty(fund.Quantiy))
                {
                    TempData["CreatePinAlert"] = "Please enter quantity.";
                    return RedirectToAction("createPin");
                }

                if (Convert.ToDecimal(fund.Quantiy) <= 0)
                {
                    TempData["CreatePinAlert"] = "Please enter quantity greater than 0.";
                    return RedirectToAction("createPin");
                }

                dtResult = UserManager.USER_REPORT("WALLETBALANCE", userInfo.memb_code);
                if (dtResult.Rows.Count > 0)
                {
                    string totalBalance = dtResult.Rows[0]["Total_Balance"].ToString();
                    decimal pinAmount = Convert.ToDecimal(fund.Quantiy) * Convert.ToDecimal(fund.amount);
                    if (pinAmount <= Convert.ToDecimal(totalBalance))
                    {
                        for (int i = 1; i <= Convert.ToDecimal(fund.Quantiy); i++)
                        {
                            string Pin = CreateRandomPin(15);
                            int sta = 0;
                            while (sta < 1)
                            {
                                DataTable dtUser = UserManager.Pindetails("CHECKPINADD", "0", userInfo.memb_code, Pin, null, userInfo.memb_code, fund.amount);
                                if (dtUser.Rows.Count == 0)
                                {
                                    sta = 1;
                                }
                            }
                            DataTable dtAdd = UserManager.Pindetails("ADDPIN", "0", userInfo.memb_code, Pin, null, userInfo.memb_code, fund.amount);
                        }
                        TempData["CreatePinAlert"] = "1";
                    }
                    else
                    {
                        TempData["CreatePinAlert"] = "You have insufficient balance to create pin.";
                    }
                }
                else
                {
                    TempData["CreatePinAlert"] = "You have insufficient balance to create pin.";
                }
            }
            catch (Exception ex)
            {
                TempData["CreatePinAlert"] = "Pin Create failed.Error:" + ex.Message;
            }
            return RedirectToAction("createPin");
        }

        public ActionResult Pin_createHistory()
        {
            List<PINDETAILSMODEL> dList = new List<PINDETAILSMODEL>();
            try
            {
                dtResult = UserManager.Pindetails("CreatePinHistory", "0", userInfo.memb_code, null, null, userInfo.memb_code, null);
                if (dtResult.Rows.Count > 0)
                {
                    string jsonString = JsonConvert.SerializeObject(dtResult);
                    dList = JsonConvert.DeserializeObject<List<PINDETAILSMODEL>>(jsonString);
                }
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            return View(dList);
        }

        public ActionResult Pin_transfer()
        {
            PINDETAILSMODEL trans = new PINDETAILSMODEL();
            try
            {

                dtResult = UserManager.Pindetails("GETAVAILABLEPIN", "0", userInfo.memb_code, null, null, null, null);
                if (dtResult.Rows.Count > 0)
                {
                    trans.Available_Pin = dtResult.Rows[0]["Available_Pin"].ToString();
                }
                else
                {
                    trans.Available_Pin = "0";
                }
            }
            catch (Exception ex)
            {
                trans.Available_Pin = "0";
                TempData["Error : "] = ex.Message;
            }
            return View(trans);
        }

        [HttpPost]
        public ActionResult Pin_transfer(PINDETAILSMODEL fund)
        {
            try
            {
                if (string.IsNullOrEmpty(fund.username))
                {
                    TempData["TransferPinAlert"] = "Please enter User ID.";
                    return RedirectToAction("Pin_transfer");
                }

                if (string.IsNullOrEmpty(fund.amount))
                {
                    TempData["TransferPinAlert"] = "Please select amount.";
                    return RedirectToAction("Pin_transfer");
                }

                if (string.IsNullOrEmpty(fund.Quantiy))
                {
                    TempData["TransferPinAlert"] = "Please enter Pin Quantity.";
                    return RedirectToAction("Pin_transfer");
                }

                if (Convert.ToInt32(fund.Quantiy) <= 0)
                {
                    TempData["TransferPinAlert"] = "Please enter Pin Quantity greater than 0.";
                    return RedirectToAction("Pin_transfer");
                }

                dtResult = UserManager.CheckUsername(fund.username.Trim());
                if (dtResult.Rows.Count > 0)
                {
                    string memb_code = dtResult.Rows[0]["memb_code"].ToString();
                    string Available_Pin = "0";
                    DataTable dtAvail = UserManager.Pindetails("GETAVAILABLEPIN", "0", userInfo.memb_code, null, null, null, fund.amount);
                    if (dtAvail.Rows.Count > 0)
                    {
                        Available_Pin = dtAvail.Rows[0]["Available_Pin"].ToString();
                    }

                    if (Convert.ToInt32(fund.Quantiy) <= Convert.ToInt32(Available_Pin))
                    {
                        DataTable dt = UserManager.Pindetails("MYPINHISTORYNOTUSED", "0", userInfo.memb_code, fund.Pin, null, userInfo.memb_code, fund.amount);
                        for (int i = 0; i < Convert.ToInt32(fund.Quantiy); i++)
                        {
                            string amount = dt.Rows[i]["amount"].ToString();
                            string pinid = dt.Rows[i]["PinID"].ToString();
                            string Pin = dt.Rows[i]["Pin"].ToString();

                            DataTable dtAdd = UserManager.Pindetails("TRANSFERPIN", pinid, memb_code, Pin, null, userInfo.memb_code, amount);
                            TempData["TransferPinAlert"] = "Pin Transfer Successfully.";
                        }

                        //DataTable dt = UserManager.Pindetails("CHECKPINTRANSFER", "0", userInfo.memb_code, fund.Pin, null, userInfo.memb_code, null);
                        //if (dt.Rows.Count > 0)
                        //{
                        //    if (dt.Rows[0]["tf_flag"].ToString() == "Y")
                        //    {
                        //        TempData["TransferPinAlert"] = "This Pin Already transfer.";
                        //    }
                        //    else if (dt.Rows[0]["u_flag"].ToString() == "Y")
                        //    {
                        //        TempData["TransferPinAlert"] = "This Pin Already Used.";
                        //    }
                        //    else
                        //    {
                        //        string amount = dt.Rows[0]["amount"].ToString();
                        //        string pinid = dt.Rows[0]["PinID"].ToString();

                        //        DataTable dtAdd = UserManager.Pindetails("TRANSFERPIN", pinid, memb_code, fund.Pin, null, userInfo.memb_code, amount);
                        //        TempData["TransferPinAlert"] = "Pin Transfer Successfully.";
                        //    }
                        //}
                        //else
                        //{
                        //    TempData["TransferPinAlert"] = "This Pin is invalid";
                        //}
                    }
                    else
                    {
                        TempData["TransferPinAlert"] = "You have only " + Available_Pin + " pin quantity to transfer.";
                    }
                }
                else
                {
                    TempData["TransferPinAlert"] = "User ID is invalid";
                }
            }
            catch (Exception ex)
            {
                TempData["TransferPinAlert"] = "Pin Transfer failed.Error:" + ex.Message;
            }
            return RedirectToAction("Pin_transfer");
        }

        public ActionResult Pin_TOPUP()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Pin_TOPUP(PINDETAILSMODEL trans)
        {
            try
            {
                if (string.IsNullOrEmpty(trans.username.Trim()))
                {
                    TempData["TopUpAlert"] = "Please enter User ID.";
                    return RedirectToAction("Pin_TOPUP");
                }
                if (string.IsNullOrEmpty(userInfo.ac_name) || string.IsNullOrEmpty(userInfo.ac_no)
                 || string.IsNullOrEmpty(userInfo.bk_name) || string.IsNullOrEmpty(userInfo.bk_branch)
                 || string.IsNullOrEmpty(userInfo.bk_ifsc))
                {
                    TempData["TopUpAlert"] = "Please first update your bank details.";
                    return RedirectToAction("Pin_TOPUP");
                }

                if (string.IsNullOrEmpty(trans.Pin.Trim()))
                {
                    TempData["TopUpAlert"] = "Please enter Pin No.";
                    return RedirectToAction("Pin_TOPUP");
                }

                dtResult = UserManager.Pindetails("CHECKPINTRANSFER", "0", userInfo.memb_code, trans.Pin.Trim(), null, userInfo.memb_code, null);
                if (dtResult.Rows.Count > 0)
                {
                    if (dtResult.Rows[0]["tf_flag"].ToString() == "Y")
                    {
                        TempData["TopUpAlert"] = "This Pin Already transfer.";
                    }
                    else if (dtResult.Rows[0]["u_flag"].ToString() == "Y")
                    {
                        TempData["TopUpAlert"] = "This Pin Already Used.";
                    }
                    else
                    {
                        string amount = dtResult.Rows[0]["amount"].ToString();
                        string pinid = dtResult.Rows[0]["PinID"].ToString();

                        DataTable dtUser = UserManager.CheckUsername(trans.username);
                        if (dtUser.Rows.Count > 0)
                        {
                            string membCode = dtUser.Rows[0]["memb_code"].ToString();
                            string s_mobileno = dtUser.Rows[0]["memb_code"].ToString();

                            //if (!string.Equals(userInfo.memb_code, dtUser.Rows[0]["Spon_code"].ToString()))
                            //{
                            //    TempData["TopUpAlert"] = "Pin Top up only Direct Downline Memebers";
                            //    return RedirectToAction("topuppin");
                            //}


                            //DataTable CHECKAUTHRISED = UserManager.USER_REPORT("CHECKAUTHRISED", membCode);
                            //if (CHECKAUTHRISED.Rows.Count > 0)
                            //{
                            //    if (CHECKAUTHRISED.Rows[0]["authrised"].ToString() == "G")
                            //    {
                            //        TempData["TopUpAlert"] = "Member ID is already activate please enter another id";
                            //        return RedirectToAction("Pin_TOPUP");
                            //    }
                            //}

                            int result = UserManager.User_TopUp("ADDTOPUP", membCode, amount, null, "0", "PIN", amount, userInfo.memb_code, pinid);
                            if (result > 0)
                            {
                                TempData["TopUpAlert"] = "1";
                            }
                            else
                            {
                                TempData["TopUpAlert"] = "Topup failed.";
                            }
                        }
                        else
                        {
                            TempData["TopUpAlert"] = "Please enter valid User id.";
                        }
                    }
                }
                else
                {
                    TempData["TopUpAlert"] = "This Pin is invalid";
                }
            }
            catch (Exception ex)
            {
                TempData["TopUpAlert"] = "Topup failed.Error:" + ex.Message;
            }
            return RedirectToAction("Pin_TOPUP");
        }

        public ActionResult Pin_transferHistory()
        {
            PinTransferModel pin = new PinTransferModel();
            List<PINDETAILSMODEL> tdList = new List<PINDETAILSMODEL>();
            List<PINDETAILSMODEL> rdList = new List<PINDETAILSMODEL>();
            try
            {
                dtResult = UserManager.Pindetails("TransferPinHistory", "0", userInfo.memb_code, null, null, userInfo.memb_code, null);
                if (dtResult.Rows.Count > 0)
                {
                    string jsonString = JsonConvert.SerializeObject(dtResult);
                    tdList = JsonConvert.DeserializeObject<List<PINDETAILSMODEL>>(jsonString);
                }

                dtResult = UserManager.Pindetails("ReceivePinHistory", "0", userInfo.memb_code, null, null, userInfo.memb_code, null);
                if (dtResult.Rows.Count > 0)
                {
                    string jsonString = JsonConvert.SerializeObject(dtResult);
                    rdList = JsonConvert.DeserializeObject<List<PINDETAILSMODEL>>(jsonString);
                }
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            pin.trnsferHistory = tdList;
            pin.receiveHistory = rdList;
            return View(pin);
        }


        public ActionResult Pin_receivedHistory()
        {
            PinTransferModel pin = new PinTransferModel();
            List<PINDETAILSMODEL> tdList = new List<PINDETAILSMODEL>();
            List<PINDETAILSMODEL> rdList = new List<PINDETAILSMODEL>();
            try
            {
                dtResult = UserManager.Pindetails("TransferPinHistory", "0", userInfo.memb_code, null, null, userInfo.memb_code, null);
                if (dtResult.Rows.Count > 0)
                {
                    string jsonString = JsonConvert.SerializeObject(dtResult);
                    tdList = JsonConvert.DeserializeObject<List<PINDETAILSMODEL>>(jsonString);
                }

                dtResult = UserManager.Pindetails("ReceivePinHistory", "0", userInfo.memb_code, null, null, userInfo.memb_code, null);
                if (dtResult.Rows.Count > 0)
                {
                    string jsonString = JsonConvert.SerializeObject(dtResult);
                    rdList = JsonConvert.DeserializeObject<List<PINDETAILSMODEL>>(jsonString);
                }
            }
            catch (Exception ex)
            {
                TempData["Error : "] = ex.Message;
            }
            pin.trnsferHistory = tdList;
            pin.receiveHistory = rdList;
            return View(pin);
        }

        public JsonResult checkPinDetails(string pin)
        {
            string msg = "";
            try
            {
                dtResult = UserManager.Pindetails("CHECKPINTRANSFER", "0", userInfo.memb_code, pin, null, userInfo.memb_code, null);
                if (dtResult.Rows.Count > 0)
                {
                    if (dtResult.Rows[0]["tf_flag"].ToString() == "Y")
                    {
                        msg = "This Pin Already transfer.";
                    }
                    else if (dtResult.Rows[0]["u_flag"].ToString() == "Y")
                    {
                        msg = "This Pin Already Used.";
                    }
                    else
                    {
                        msg = "";
                    }
                }
                else
                {
                    msg = "This Pin is invalid";
                }
            }
            catch
            {
                msg = "This Pin is invalid";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Getpincounts(string amount)
        {
            PINDETAILSMODEL trans = new PINDETAILSMODEL();
            try
            {

                dtResult = UserManager.Pindetails("GETAVAILABLEPIN", "0", userInfo.memb_code, null, null, null, amount);
                if (dtResult.Rows.Count > 0)
                {
                    trans.Available_Pin = dtResult.Rows[0]["Available_Pin"].ToString();
                }
                else
                {
                    trans.Available_Pin = "0";
                }
            }
            catch (Exception ex)
            {
                trans.Available_Pin = "0";
                TempData["Error : "] = ex.Message;
            }

            return Json(trans, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getUserDetails(string email)
        {
            SponsorDetailsModel user = new SponsorDetailsModel();
            try
            {
                dtResult = UserManager.CheckUsername(email);
                if (dtResult.Rows.Count > 0)
                {
                    //DataTable dtD = UserManager.GetDownlineUser(userInfo.memb_code, dt.Rows[0]["memb_code"].ToString());
                    //if (dtD.Rows.Count > 0)
                    //{
                    user.Memb_Name = dtResult.Rows[0]["Memb_Name"].ToString();
                    //}
                    //else
                    //{
                    //    user.Memb_Name = "This user id not in your downline.";
                    //}
                }
                else
                {
                    user.Memb_Name = "";
                }
            }
            catch
            {
                user.Memb_Name = "";
            }
            return Json(user, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}