using donorclub.Models;
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
    public class UserManager
    {
        public static DataTable REGISTRATION(SignUpModel ObjSignUp)
        {
            SqlParameter[] sqlpara = new SqlParameter[35];
            sqlpara[0] = new SqlParameter("@MODE", ObjSignUp.Mode);
            sqlpara[1] = new SqlParameter("@USERNAME", ObjSignUp.username);
            sqlpara[2] = new SqlParameter("@MPWD", ObjSignUp.mpwd);
            sqlpara[3] = new SqlParameter("@SPID", ObjSignUp.sp_user);
            sqlpara[4] = new SqlParameter("@PLACE", ObjSignUp.place);
            sqlpara[5] = new SqlParameter("@PLAN_TYPE", ObjSignUp.place);
            sqlpara[6] = new SqlParameter("@MembName_F", ObjSignUp.MembName_F);
            sqlpara[7] = new SqlParameter("@MembName_M", ObjSignUp.MembName_M);
            sqlpara[8] = new SqlParameter("@MembName_L", ObjSignUp.MembName_L);
            sqlpara[9] = new SqlParameter("@Memb_Name", ObjSignUp.Memb_Name);
            sqlpara[10] = new SqlParameter("@Gender", ObjSignUp.gender);
            sqlpara[11] = new SqlParameter("@Mobile_No", ObjSignUp.Mobile_No);
            sqlpara[12] = new SqlParameter("@Phone_No", ObjSignUp.Phone_No);
            sqlpara[13] = new SqlParameter("@EMail", ObjSignUp.EMail);
            sqlpara[14] = new SqlParameter("@Address1", ObjSignUp.Address1);
            sqlpara[15] = new SqlParameter("@Address2", ObjSignUp.Address2);
            sqlpara[16] = new SqlParameter("@M_COUNTRY", ObjSignUp.M_COUNTRY);
            sqlpara[17] = new SqlParameter("@State", ObjSignUp.State);
            sqlpara[18] = new SqlParameter("@District", ObjSignUp.District);
            sqlpara[19] = new SqlParameter("@City", ObjSignUp.City);
            sqlpara[20] = new SqlParameter("@Pin_Code", ObjSignUp.Pin_Code);
            sqlpara[21] = new SqlParameter("@Reg_Amt", string.IsNullOrEmpty(ObjSignUp.Reg_Amt) ? "0" : ObjSignUp.Reg_Amt);
            sqlpara[22] = new SqlParameter("@RV_Code", ObjSignUp.RV_Code);
            sqlpara[23] = new SqlParameter("@PIN_ID", ObjSignUp.PIN_ID);
            sqlpara[24] = new SqlParameter("@pin_no", ObjSignUp.pin_no);
            sqlpara[25] = new SqlParameter("@REMARK", ObjSignUp.REMARK);
            sqlpara[26] = new SqlParameter("@client_ip", ObjSignUp.client_ip);
            sqlpara[27] = new SqlParameter("@ac_name", ObjSignUp.ac_name);
            sqlpara[28] = new SqlParameter("@ac_no", ObjSignUp.ac_no);
            sqlpara[29] = new SqlParameter("@bk_name", ObjSignUp.bk_name);
            sqlpara[30] = new SqlParameter("@bk_branch", ObjSignUp.bk_branch);
            sqlpara[31] = new SqlParameter("@bk_ifsc", ObjSignUp.bk_ifsc);
            sqlpara[32] = new SqlParameter("@bk_card_no", ObjSignUp.bk_card_no);
            sqlpara[33] = new SqlParameter("@btc_ac", ObjSignUp.btc_ac);
            sqlpara[34] = new SqlParameter("@pwd", ObjSignUp.pwd);


            return DBHelper.GetDataTable("SP_REGISTRATION", sqlpara);
        }

        //public static DataTable GetUserLogin(string username)
        //{
        //    return DBHelper.GetDataTable("select *,(Select Top 1 username from Entry order by memb_code asc) as Admin_username from Entry where tempf='Y' and M_Status='Y' and username='" + username + "'");
        //}

        public static DataTable UserLogin(string USERNAME)
        {
            SqlParameter[] sqlpara = new SqlParameter[3];
            sqlpara[0] = new SqlParameter("@MODE", "Login");
            sqlpara[1] = new SqlParameter("@USERNAME", USERNAME);
            sqlpara[2] = new SqlParameter("@MPWD", null);

            return DBHelper.GetDataTable("SP_LOGIN", sqlpara);
        }


        //public static DataTable GetUserLevel(string memb_code)
        //{
        //    return DBHelper.GetDataTable("select Top 1 level_no from pairdtl where memb_code=" + memb_code + " order by level_no desc");
        //}
        //public static DataTable GetUserDetailsByMemb_Code(string memb_code)
        //{
        //    return DBHelper.GetDataTable("select * from Entry where tempf='Y' and M_Status='Y' and memb_code='" + memb_code + "'");
        //}

        public static DataTable GetUserDetailsByIP(string client_ip)
        {
            return DBHelper.GetDataTable("select * from Entry where tempf='Y' and M_Status='Y' and client_ip='" + client_ip + "'");
        }

        public static DataTable GetUserDetailsByEmail(string EMAIL)
        {
            return DBHelper.GetDataTable("select * from Entry where EMAIL='" + EMAIL + "'");
        }

        public static DataTable GetUserDetailsByUsername(string USERNAME)
        {
            return DBHelper.GetDataTable("select Memb_Name,username from Entry where username='" + USERNAME + "'");
        }

        public static DataTable GetUserDetailsByMobileNo(string Mobile_No)
        {
            return DBHelper.GetDataTable("select * from Entry where Mobile_No='" + Mobile_No + "'");
        }

        public static DataTable ChkExistsUsername(string USERNAME)
        {
            return DBHelper.GetDataTable("select * from Entry where username='" + USERNAME + "'");
        }

        public static DataTable GetFirstUser()
        {
            return DBHelper.GetDataTable("select Top 1 Memb_Name,EMAIL,username from Entry order by memb_code asc");
        }

        public static DataTable GetUserVerify(string username)
        {
            return DBHelper.GetDataTable("select *,(Select Top 1 username from Entry order by memb_code asc) as Admin_username from Entry where tempf='Y' and username='" + username + "'");
        }

        public static int verifyEmail(string username)
        {
            return DBHelper.ExecuteNonQuery("Update Entry set M_Status='Y' where username='" + username + "'");
        }

        //public static int EditProfile(string memb_name, string m_country, string dbo, string mobile_no, string Gender, string memb_code, string MembName_L, string EmailID)
        //{
        //    return DBHelper.ExecuteNonQuery("update entry set memb_name='" + memb_name.Trim() + "', m_country='"+ m_country.Trim() + "',dbo='" + dbo + "',mobile_no='" + mobile_no + "',Gender='" + Gender + "',MembName_L='" + MembName_L + "',EMail='" + EmailID + "' where  memb_code=" + memb_code + "");
        //}

        public static DataTable GetAccountDetails(string memb_code)
        {
            return DBHelper.GetDataTable("select * from bankdtl where memb_code='" + memb_code + "' order by ttime desc");
        }

        public static DataTable GetSponserDetails(string spon_code)
        {
            return DBHelper.GetDataTable(@"select memb_name as sp_name,email as sp_Email,m_country as sp_country,mobile_no as sp_mobile_no,
                                            username as spon_id from Entry where memb_code='" + spon_code + "'");
        }

        public static int AddAccountDetails(string accStatus, string memb_code, string btc_ac, string eth_ac, string ltc_ac)
        {
            //if (accStatus == "0")
            //{
            return DBHelper.ExecuteNonQuery("Insert into bankdtl ( memb_code , ttime , btc_ac , eth_ac , ltc_ac ) values ('" +
                memb_code + "','" + DateTime.Now.ToString() + "','" + btc_ac + "','" + eth_ac + "','" + ltc_ac + "')");
            //}
            //else
            //{
            //    return DBHelper.ExecuteNonQuery("Update bankdtl set ttime='"+ DateTime.Now.ToString() 
            //        + "',btc_ac='"+ btc_ac + "',eth_ac='" + eth_ac + "',ltc_ac='" + ltc_ac + "' where memb_code='"+ memb_code + "'");
            //}
        }


        public static DataTable GetReferral(string memb_code)
        {
            return DBHelper.GetDataTable(@"select (Select Top 1 level_no from pairdtl where memb_code = a.memb_code order by srno desc) as level_no,
                                            Convert(varchar(11),a.reg_date,106) as reg_date , a.memb_name , a.m_country , a.email ,a.username,
                                            (select Sum(amount) from DONATIONDTL x where x.memb_code = a.memb_code and COMMIT_STATUS='Y') as confirm_ph   ,
                                            (select Sum(amount) from DONATIONDTL x where x.memb_code = a.memb_code and COMMIT_STATUS='N') as unconfirm_ph from Entry a
                                            where a.spon_code=" + memb_code + "  order by a.reg_date");
        }


        public static DataTable GetAllTeam(string memb_code)
        {
            return DBHelper.GetDataTable(@"select Convert(varchar(11),a.reg_date,106) as reg_date, a.memb_name,a.username , a.m_country , a.email , b.level_no,
                                        (select x.email from entry x where x.memb_code = a.spon_code) as sp_user   ,(select x.Memb_Name from entry x where x.memb_code = a.spon_code) as sp_name   ,
                                        (select Sum(amount) from DONATIONDTL x where x.memb_code = a.memb_code and COMMIT_STATUS='Y') as confirm_ph   ,
                                        (select Sum(amount) from DONATIONDTL x where x.memb_code = a.memb_code and COMMIT_STATUS='N') as unconfirm_ph   
                                        from entry a , pairdtl b where a.memb_code = b.memb_code and b.rcode =" + memb_code + " order by a.reg_date");
        }


        public static DataTable GetGrowthWallet(string memb_code)
        {
            return DBHelper.GetDataTable(@"Select ttime as created_at,COMMIT_NO as provide_id,(Case when CR=0 then dr when dr=0 then cr end) as amount,b.flag 
                            from BRTRANSACTION b where TRANS_TYPE=1 and MEMB_CODE=" + memb_code + " and ttime <= getDate() and (flag='Y' or flag='N')");
        }


        public static DataTable GetBonusWallet(string memb_code)
        {
            return DBHelper.GetDataTable(@"Select b.ttime as created_at,b.COMMIT_NO as provide_id,b.flag,Sum(dr) as amount,e.username as provide_by 
                                           from BRTRANSACTION_BINARY b,entry e where b.RCODE=e.memb_code and b.MEMB_CODE=" + memb_code +
                                           " group by b.ttime,b.COMMIT_NO,b.flag,e.username");
        }


        public static DataTable GetLevelBonus(string memb_code)
        {
            return DBHelper.GetDataTable(@"Select created_at,provide_id,flag,Sum(amount) as amount,provide_by,level_no from 
                                        (Select b.ttime as created_at,b.commit_no as provide_id,b.flag,b.dr as amount ,e.username as provide_by,
                                        (Select Top 1 level_no from pairdtl where memb_code = b.memb_code order by srno desc) as level_no 
                                        from BRTRANSACTION_BINARY b,entry e where b.RCODE=e.memb_code and b.TRANS_TYPE=6 
                                        and b.MEMB_CODE=" + memb_code + ")Q group by created_at,provide_id,flag,provide_by,level_no");
        }


        public static DataTable GetReferralBonus(string memb_code)
        {
            return DBHelper.GetDataTable(@"Select created_at,provide_id,flag,Sum(amount) as amount,provide_by from 
                                            (Select b.ttime as created_at,b.COMMIT_NO as provide_id,b.flag,b.dr as amount,e.username as provide_by 
                                            from BRTRANSACTION_BINARY b ,entry e where b.RCODE=e.memb_code and b.TRANS_TYPE=7 and b.MEMB_CODE=" + memb_code +
                                            ") Q group by created_at,provide_id,flag,provide_by");
        }
        public static DataTable GetReport(string memb_code)
        {
            return DBHelper.GetDataTable(@"select a.remark, a.tdate, a.commit_date as credit_date ,
                                            (select case when a.flag = 'Y' then 'Confirm' else 'Pending' end) as status ,
                                            (select case when a.dr > 0 then a.dr else a.cr end) as amount ,
                                            (Select Top 1 username from entry where memb_code=a.Rcode) as username from BRTRANSACTION_BINARY a
                                            where a.memb_code=" + memb_code + " and  TRANS_TYPE=7 order by a.tdate desc");
            // return DBHelper.GetDataTable("select a.remark, a.tdate, a.credit_date, (select case when a.flagstatus = 'Y' and a.QUICK='Y' then 'Confirm' else 'Pending' end) as status, (select case when a.dr > 0 then a.dr else a.cr end) as amount ,commit_no,(Select Top 1 username from entry where memb_code=(Select Top 1 rmemb_code from helpdata where commit_no=a.COMMIT_NO)) as username from brtransaction a where a.memb_code=" + memb_code + " order by a.tdate desc");
            // return DBHelper.GetDataTable("select q.* from (select a.remark, a.tdate, a.credit_date, (select case when a.flag = 'Y' then 'Confirm' else 'Pending' end) as status, (select case when a.dr > 0 then a.dr else a.cr end) as amount from brtransaction a where a.memb_code=" + memb_code + " union all select a.remark , a.tdate , a.credit_date, (select case when a.flag = 'Y' then 'Confirm' else 'Pending' end ) as status, (select case when a.dr > 0 then a.dr else a.cr end ) as amount from brtransaction a where a.memb_code=" + memb_code + " ) q order by q.tdate");
            //return DBHelper.GetDataTable("Select * from (select 'Get From' as remark, e.username, h.tdate, h.flag_date as credit_date,(Case  when h.flag='N' and h.cancelflag='Y' Then 'Reject' when h.flag='Y' Then 'Confirmed' Else 'Pending' End) as status ,amount from helpdata h ,entry e where h.rmemb_code=" + memb_code + " and h.bymemb_code=e.memb_code Union All select 'Provide to' as remark, e.username, h.tdate, h.flag_date as credit_date ,(Case  when h.flag='N' and h.cancelflag='Y' Then 'Reject' when h.flag='Y' Then 'Confirmed' Else 'Pending' End) as status ,amount from helpdata h ,entry e where h.bymemb_code=" + memb_code + " and h.rmemb_code=e.memb_code) Q order by credit_date desc");
        }

        public static int UserWithdrawals(string srno, string memb_code, string amount, string remark
            , string req_type, string cardno, string account_name, string bankac, string bankname
            , string bankbranch, string micr_code, string ifs_code, string pan_no, string Wallet)
        {
            SqlParameter[] sqlpara = new SqlParameter[14];
            sqlpara[0] = new SqlParameter("@srno", srno);
            sqlpara[1] = new SqlParameter("@memb_code", memb_code);
            sqlpara[2] = new SqlParameter("@amount", amount);
            sqlpara[3] = new SqlParameter("@remark", remark);
            sqlpara[4] = new SqlParameter("@req_type", req_type);
            sqlpara[5] = new SqlParameter("@cardno", cardno);
            sqlpara[6] = new SqlParameter("@account_name", account_name);
            sqlpara[7] = new SqlParameter("@bankac", bankac);
            sqlpara[8] = new SqlParameter("@bankname", bankname);
            sqlpara[9] = new SqlParameter("@bankbranch", bankbranch);
            sqlpara[10] = new SqlParameter("@micr_code", micr_code);
            sqlpara[11] = new SqlParameter("@ifs_code", ifs_code);
            sqlpara[12] = new SqlParameter("@pan_no", pan_no);
            sqlpara[13] = new SqlParameter("@Wallet", Wallet);

            return DBHelper.ExecuteNonQuery("SP_Withdrawals", sqlpara);
        }

        public static DataTable GetTotalBalance(string memb_code)
        {
            return DBHelper.GetDataTable(@"Select isnull((Select (Sum(dr) - Sum(cr)) from BRTRANSACTION where flag='Y' and 
                            (TRANS_TYPE in(1,2,12) and memb_code=" + memb_code + " and TTIME <=getDate()),0) as totalGrowthBal,isnull((Select (Sum(dr) - Sum(cr)) from BRTRANSACTION_BINARY where memb_code= " + memb_code + " and flag='Y' ),0) as totalBonusBal");
        }


        public static int UserInvestment(string SRNO, string memb_code, string amount)
        {
            SqlParameter[] sqlpara = new SqlParameter[3];
            sqlpara[0] = new SqlParameter("@SRNO", SRNO);
            sqlpara[1] = new SqlParameter("@memb_code", memb_code);
            sqlpara[2] = new SqlParameter("@amount", amount);

            return DBHelper.ExecuteNonQuery("SP_Investment", sqlpara);
        }

        public static DataTable GetProvideDetails(string memb_code)
        {
            SqlParameter[] sqlpara = new SqlParameter[3];
            sqlpara[0] = new SqlParameter("@SRNO", "1");
            sqlpara[1] = new SqlParameter("@memb_code", memb_code);
            sqlpara[2] = new SqlParameter("@amount", "0");

            return DBHelper.GetDataTable("SP_Investment", sqlpara);
        }

        public static DataTable GetTotalProvide(string memb_code)
        {
            return DBHelper.GetDataTable("Select Count(*) as TotalPH from DONATIONDTL where MEMB_CODE=" + memb_code + " and isnull(COMMIT_STATUS,'N') !='D'");
        }

        public static DataTable GetLastProvide(string memb_code)
        {
            return DBHelper.GetDataTable("Select Top 1 Amount from DONATIONDTL  where MEMB_CODE=" + memb_code + " and isnull(COMMIT_STATUS,'N') !='D' order by srno desc");
        }

        public static DataTable GetGetHelpDetails(string memb_code, string Wallet)
        {
            SqlParameter[] sqlpara = new SqlParameter[14];
            sqlpara[0] = new SqlParameter("@srno", "1");
            sqlpara[1] = new SqlParameter("@memb_code", memb_code);
            sqlpara[2] = new SqlParameter("@amount", "0");
            sqlpara[3] = new SqlParameter("@remark", null);
            sqlpara[4] = new SqlParameter("@req_type", null);
            sqlpara[5] = new SqlParameter("@cardno", null);
            sqlpara[6] = new SqlParameter("@account_name", null);
            sqlpara[7] = new SqlParameter("@bankac", null);
            sqlpara[8] = new SqlParameter("@bankname", null);
            sqlpara[9] = new SqlParameter("@bankbranch", null);
            sqlpara[10] = new SqlParameter("@micr_code", null);
            sqlpara[11] = new SqlParameter("@ifs_code", null);
            sqlpara[12] = new SqlParameter("@pan_no", null);
            sqlpara[13] = new SqlParameter("@Wallet", Wallet);

            return DBHelper.GetDataTable("SP_Withdrawals", sqlpara);
        }

        public static DataTable GetToDayGetHelp(string MEMB_CODE)
        {
            return DBHelper.GetDataTable(@"Select (Select Sum(cr) from BRTRANSACTION_BINARY where TRANS_TYPE=14 and TDATE=Cast(getDate() as date)
                and MEMB_CODE=" + MEMB_CODE + " ) as BONUSTotal ,(Select Sum(cr) from BRTRANSACTION where TRANS_TYPE=12 and TDATE=Cast(getDate() as date) and MEMB_CODE=" + MEMB_CODE + ") as GROWTHTotal");
        }

        public static int AddContactEnquiry(string username, string email, string phone_no, string address, string message)
        {
            SqlParameter[] sqlpara = new SqlParameter[6];
            sqlpara[0] = new SqlParameter("@MODE", "CONTACT");
            sqlpara[1] = new SqlParameter("@username", username);
            sqlpara[2] = new SqlParameter("@email", email);
            sqlpara[3] = new SqlParameter("@phone_no", phone_no);
            sqlpara[4] = new SqlParameter("@message", message);
            sqlpara[5] = new SqlParameter("@subject", address);

            return DBHelper.ExecuteNonQuery("SP_Contact_Outer", sqlpara);

            //  return DBHelper.ExecuteNonQuery("Insert into contact_outer(username,email,phone_no,message,tdate,subject) Values('" + username + "','" + email + "', '" + phone_no + "','" + message + "','" + DateTime.Now.ToString() + "','" + address + "')");
        }



        public static DataTable GetDashboardData(string MODE, string MCODE)
        {
            SqlParameter[] sqlpara = new SqlParameter[2];
            sqlpara[0] = new SqlParameter("@MODE", MODE);
            sqlpara[1] = new SqlParameter("@MCODE", MCODE);

            return DBHelper.GetDataTable("SP_DASHBOARD", sqlpara);
        }

        public static DataTable GetDashboardDataLevel(string MODE, string MCODE, string level_No, string status)
        {
            SqlParameter[] sqlpara = new SqlParameter[4];
            sqlpara[0] = new SqlParameter("@MODE", MODE);
            sqlpara[1] = new SqlParameter("@MCODE", MCODE);
            sqlpara[2] = new SqlParameter("@level_No", level_No);
            sqlpara[3] = new SqlParameter("@status", status);

            return DBHelper.GetDataTable("SP_DASHBOARDLEVEL", sqlpara);
        }


        public static int LINK_ACCEPT(string MEMB_CODE, string SRNO, string TDATE, string FDATE)
        {
            SqlParameter[] sqlpara = new SqlParameter[4];
            sqlpara[0] = new SqlParameter("@MEMB_CODE", MEMB_CODE);
            sqlpara[1] = new SqlParameter("@SRNO", SRNO);
            sqlpara[2] = new SqlParameter("@TDATE", TDATE);
            sqlpara[3] = new SqlParameter("@FDATE", FDATE);

            return DBHelper.ExecuteNonQuery("BT_LINK_ACCEPT", sqlpara);
        }

        public static int RejectLink(string RMEMB_CODE, string SRNO)
        {
            SqlParameter[] sqlpara = new SqlParameter[5];
            sqlpara[0] = new SqlParameter("@MODE", "R");
            sqlpara[1] = new SqlParameter("@rmemb_code", RMEMB_CODE);
            sqlpara[2] = new SqlParameter("@hsrno", SRNO);
            sqlpara[3] = new SqlParameter("@extendHR", "24");
            sqlpara[4] = new SqlParameter("@extend_by", "USER");

            return DBHelper.ExecuteNonQuery("SP_EXTEND_TIME", sqlpara);
        }

        public static DataTable ExtendTime(string RMEMB_CODE, string SRNO)
        {
            SqlParameter[] sqlpara = new SqlParameter[5];
            sqlpara[0] = new SqlParameter("@MODE", "E");
            sqlpara[1] = new SqlParameter("@rmemb_code", RMEMB_CODE);
            sqlpara[2] = new SqlParameter("@hsrno", SRNO);
            sqlpara[3] = new SqlParameter("@extendHR", "12");
            sqlpara[4] = new SqlParameter("@extend_by", "USER");

            return DBHelper.GetDataTable("SP_EXTEND_TIME", sqlpara);
        }

        public static DataTable Assignment_MSG(string Mode, string hsrno, string smemb_code
            , string rmemb_code, string message, string attachment)
        {
            SqlParameter[] sqlpara = new SqlParameter[6];
            sqlpara[0] = new SqlParameter("@MODE", Mode);
            sqlpara[1] = new SqlParameter("@hsrno", hsrno);
            sqlpara[2] = new SqlParameter("@smemb_code", smemb_code);
            sqlpara[3] = new SqlParameter("@rmemb_code", rmemb_code);
            sqlpara[4] = new SqlParameter("@message", message);
            sqlpara[5] = new SqlParameter("@attachment", attachment);

            return DBHelper.GetDataTable("SP_Assignment_MSG", sqlpara);
        }



        public static DataTable FAKE_SLIP(string srno, string hsrno, string memb_code)
        {
            SqlParameter[] sqlpara = new SqlParameter[3];
            sqlpara[0] = new SqlParameter("@srno", srno);
            sqlpara[1] = new SqlParameter("@hsrno", hsrno);
            sqlpara[2] = new SqlParameter("@memb_code", memb_code);

            return DBHelper.GetDataTable("SP_FAKE_SLIP", sqlpara);
        }

        public static DataTable GetAddresses(string memb_code)
        {
            return DBHelper.GetDataTable("Select Top 1 btc_ac,eth_ac,ltc_ac from bankdtl where memb_code=" + memb_code + " order by srno desc");
        }


        public static DataTable TRANSACTION_LINK(string MODE, string hsrno, string smemb_code, string rmemb_code, string message, string transFor)
        {
            SqlParameter[] sqlpara = new SqlParameter[6];
            sqlpara[0] = new SqlParameter("@MODE", MODE);
            sqlpara[1] = new SqlParameter("@hsrno", hsrno);
            sqlpara[2] = new SqlParameter("@smemb_code", smemb_code);
            sqlpara[3] = new SqlParameter("@rmemb_code", rmemb_code);
            sqlpara[4] = new SqlParameter("@message", message);
            sqlpara[5] = new SqlParameter("@transFor", transFor);

            return DBHelper.GetDataTable("SP_TRANSACTION_LINK", sqlpara);
        }



        public static DataTable ORDERDETAILS(string MODE, string MCODE, string Commit_No)
        {
            SqlParameter[] sqlpara = new SqlParameter[3];
            sqlpara[0] = new SqlParameter("@MODE", MODE);
            sqlpara[1] = new SqlParameter("@MCODE", MCODE);
            sqlpara[2] = new SqlParameter("@Commit_No", Commit_No);

            return DBHelper.GetDataTable("SP_ORDERDETAILS", sqlpara);
        }



        public static DataTable GetLatestNews()
        {
            return DBHelper.GetDataTable("select * from news where status='A' order by ttime desc");
        }


        public static int AddLetterDetails(string MODE, string srno, string memb_code, string type, string title, string message, string flag)
        {
            SqlParameter[] sqlpara = new SqlParameter[7];
            sqlpara[0] = new SqlParameter("@MODE", MODE);
            sqlpara[1] = new SqlParameter("@srno", srno);
            sqlpara[2] = new SqlParameter("@memb_code", memb_code);
            sqlpara[3] = new SqlParameter("@type", type);
            sqlpara[4] = new SqlParameter("@title", title);
            sqlpara[5] = new SqlParameter("@message", message);
            sqlpara[6] = new SqlParameter("@flag", flag);

            return DBHelper.ExecuteNonQuery("SP_LETTERDTL", sqlpara);
        }

        public static DataTable GetLetterDetail(string memb_code, string type)
        {
            SqlParameter[] sqlpara = new SqlParameter[7];
            sqlpara[0] = new SqlParameter("@MODE", "GETUSER");
            sqlpara[1] = new SqlParameter("@srno", null);
            sqlpara[2] = new SqlParameter("@memb_code", memb_code);
            sqlpara[3] = new SqlParameter("@type", type);
            sqlpara[4] = new SqlParameter("@title", null);
            sqlpara[5] = new SqlParameter("@message", null);
            sqlpara[6] = new SqlParameter("@flag", null);

            return DBHelper.GetDataTable("SP_LETTERDTL", sqlpara);
        }

        public static DataTable GetUserAlert()
        {
            SqlParameter[] sqlpara = new SqlParameter[7];
            sqlpara[0] = new SqlParameter("@MODE", "GETALERT");
            sqlpara[1] = new SqlParameter("@srno", null);
            sqlpara[2] = new SqlParameter("@memb_code", null);
            sqlpara[3] = new SqlParameter("@type", "A");
            sqlpara[4] = new SqlParameter("@title", null);
            sqlpara[5] = new SqlParameter("@message", null);
            sqlpara[6] = new SqlParameter("@flag", "Y");

            return DBHelper.GetDataTable("SP_LETTERDTL", sqlpara);
        }


        public static DataTable CheckGetHelp(string memb_code)
        {
            return DBHelper.GetDataTable("Select * from helpdata where flag='N' and rmemb_code=" + memb_code + "");
        }

        public static DataTable CheckProvideHelp(string memb_code)
        {
            return DBHelper.GetDataTable("Select * from helpdata where (cintper=10 or cintper=20) and flag='N' and cancelflag='N' and bymemb_code=" + memb_code + "");
        }

        public static DataTable CheckGetProvideHelp(string memb_code)
        {
            return DBHelper.GetDataTable("Select * from request_amount where flag='N' and isNull(req_type,'A') !='AW_ROI' and memb_code=" + memb_code + "");
        }

        public static DataTable CheckProvideGetHelp(string memb_code)
        {
            return DBHelper.GetDataTable("Select * from DONATIONDTL where COMMIT_STATUS='N' and memb_code=" + memb_code + "");
        }

        public static DataTable CheckPinAmt(string memb_code)
        {
            return DBHelper.GetDataTable("Select amount from pin_dtl where flag='Y'");
        }


        public static DataTable CheckWithdrawals(string memb_code)
        {
            SqlParameter[] sqlpara = new SqlParameter[2];
            sqlpara[0] = new SqlParameter("@memb_code", memb_code);
            sqlpara[1] = new SqlParameter("@Mode", "CHECK");

            return DBHelper.GetDataTable("SP_CheckWithdrawals", sqlpara);
        }

        public static int AddUpdateContactUS(UserModel _contact)
        {
            SqlParameter[] sqlpara = new SqlParameter[4];
            //sqlpara[0] = new SqlParameter("@Subject", _contact.Subject);
            sqlpara[0] = new SqlParameter("@Name", _contact.Memb_Name);
            sqlpara[1] = new SqlParameter("@Email_ID", _contact.EMail);
            //sqlpara[3] = new SqlParameter("@Phone", MobileNo);
            sqlpara[2] = new SqlParameter("@Message", _contact.Message);
            sqlpara[3] = new SqlParameter("@mode", "ADDCONTACT");
            return DBHelper.ExecuteNonQuery("SP_ContactUs", sqlpara);

        }

        public static int AddEmail(/*string Name, */string Email, string IP)
        {
            SqlParameter[] sqlpara = new SqlParameter[2];
            sqlpara[0] = new SqlParameter("@Email_ID", Email);
            sqlpara[1] = new SqlParameter("@IP", IP);
            //sqlpara[2] = new SqlParameter("@Name", Name);
            return DBHelper.ExecuteNonQuery("SP_Add_EmailSub", sqlpara);
        }

        public static DataTable CheackEmail(string Email)
        {
            SqlParameter[] sqlpara = new SqlParameter[2];
            sqlpara[0] = new SqlParameter("@EMail", Email);
            sqlpara[1] = new SqlParameter("@mode", "CEEXISTS");
            return DBHelper.GetDataTable("SP_User_Details", sqlpara);
        }
        public static DataTable CheackMobile_No(string Mobile_No)
        {
            SqlParameter[] sqlpara = new SqlParameter[2];
            sqlpara[0] = new SqlParameter("@Mobile_No", Mobile_No);
            sqlpara[1] = new SqlParameter("@mode", "CMEXISTS");
            return DBHelper.GetDataTable("SP_User_Details", sqlpara);
        }
        public static DataTable USER_REPORT(string MODE, string Memb_Code)
        {
            SqlParameter[] sqlpara = new SqlParameter[2];
            sqlpara[0] = new SqlParameter("@MODE", MODE);
            sqlpara[1] = new SqlParameter("@Memb_Code", Memb_Code);

            return DBHelper.GetDataTable("SP_REPORT", sqlpara);
        }

        public static DataTable USER_REWARDREPORT(string MODE)
        {
            SqlParameter[] sqlpara = new SqlParameter[1];
            sqlpara[0] = new SqlParameter("@MODE", MODE);
            return DBHelper.GetDataTable("SP_REPORT", sqlpara);
        }

        public static DataTable USER_REPORT_DETAILS(string MODE, string username)
        {
            SqlParameter[] sqlpara = new SqlParameter[2];
            sqlpara[0] = new SqlParameter("@MODE", MODE);
            sqlpara[1] = new SqlParameter("@username", username);

            return DBHelper.GetDataTable("SP_REPORT", sqlpara);
        }


        public static DataTable Change_Password(string MEMB_CODE, string mpwd)
        {
            SqlParameter[] sqlpara = new SqlParameter[3];
            sqlpara[0] = new SqlParameter("@MODE", "CHANGEPASSWORD");
            sqlpara[1] = new SqlParameter("@MEMB_CODE", MEMB_CODE);
            sqlpara[2] = new SqlParameter("@mpwd", mpwd);

            return DBHelper.GetDataTable("SP_USER_DETAILS", sqlpara);
        }

        public static DataTable ADD_ACCOUNT_DETAILS(string MEMB_CODE, BankDetailsModel objBank)
        {
            SqlParameter[] sqlpara = new SqlParameter[13];
            sqlpara[0] = new SqlParameter("@MODE", "ADDACCOUNT");
            sqlpara[1] = new SqlParameter("@MEMB_CODE", MEMB_CODE);
            sqlpara[2] = new SqlParameter("@ac_name", objBank.ac_name);
            sqlpara[3] = new SqlParameter("@ac_no", objBank.ac_no);
            sqlpara[4] = new SqlParameter("@bk_name", objBank.bk_name);
            sqlpara[5] = new SqlParameter("@bk_branch", objBank.bk_branch);
            sqlpara[6] = new SqlParameter("@bk_ifsc", objBank.bk_ifsc);
            sqlpara[7] = new SqlParameter("@debit_card_no", objBank.debit_card_no);
            sqlpara[8] = new SqlParameter("@transit_no", objBank.transit_no);
            sqlpara[9] = new SqlParameter("@ac_type", objBank.ac_type);
            sqlpara[10] = new SqlParameter("@gpay_no", objBank.gpay_no);
            sqlpara[11] = new SqlParameter("@phonepay_no", objBank.phonepay_no);
            sqlpara[12] = new SqlParameter("@paytm_no", objBank.paytm_no);

            return DBHelper.GetDataTable("SP_USER_DETAILS", sqlpara);
        }

        //  public static DataTable USER_DETAILS(string MODE, string memb_code, string Memb_Name, string MemberName_L, string Gender
        //, string dbo, string Mobile_No, string EMail, string Address1
        //, string Address2, string M_COUNTRY, string City, string username
        //, string mpwd, string RV_Code, string client_ip, string coo_ac
        //, string btc_ac, string eth_ac, string ltc_ac)
        public static DataTable USER_DETAILS(string MODE, UserModel user)
        {
            SqlParameter[] sqlpara = new SqlParameter[20];
            sqlpara[0] = new SqlParameter("@MODE", MODE);
            sqlpara[1] = new SqlParameter("@memb_code", user.memb_code);
            sqlpara[2] = new SqlParameter("@Memb_Name", user.Memb_Name);
            sqlpara[3] = new SqlParameter("@MemberName_L", user.MembName_L);
            sqlpara[4] = new SqlParameter("@Gender", user.Gender);
            sqlpara[5] = new SqlParameter("@dbo", user.dbo);
            sqlpara[6] = new SqlParameter("@Mobile_No", user.Mobile_No);
            sqlpara[7] = new SqlParameter("@EMail", user.EMail);
            sqlpara[8] = new SqlParameter("@Address1", user.Address1);
            sqlpara[9] = new SqlParameter("@Address2", user.Address2);
            sqlpara[10] = new SqlParameter("@M_COUNTRY", user.M_COUNTRY);
            sqlpara[11] = new SqlParameter("@City", user.City);
            sqlpara[12] = new SqlParameter("@username", user.username);
            sqlpara[13] = new SqlParameter("@mpwd", user.mpwd);
            sqlpara[14] = new SqlParameter("@RV_Code", user.RV_Code);
            sqlpara[15] = new SqlParameter("@client_ip", user.client_ip);
            sqlpara[16] = new SqlParameter("@coo_ac", user.coo_ac);
            sqlpara[17] = new SqlParameter("@btc_ac", user.btc_ac);
            sqlpara[18] = new SqlParameter("@eth_ac", user.eth_ac);
            sqlpara[19] = new SqlParameter("@ltc_ac", user.ltc_ac);

            return DBHelper.GetDataTable("SP_USER_DETAILS", sqlpara);
        }


        public static int User_Details(string MODE, string memb_code, string Status)
        {
            SqlParameter[] sqlpara = new SqlParameter[3];
            sqlpara[0] = new SqlParameter("@MODE", MODE);
            sqlpara[1] = new SqlParameter("@memb_code", memb_code);
            sqlpara[2] = new SqlParameter("@Status", Status);

            return DBHelper.ExecuteNonQuery("SP_User_Details", sqlpara);
        }


        public static DataTable Pindetails(string Mode, string PinID, string Memb_Code, string Pin, string Commit_No, string from_Id, string amount)
        {
            SqlParameter[] sqlpara = new SqlParameter[7];
            sqlpara[0] = new SqlParameter("@Mode", Mode);
            sqlpara[1] = new SqlParameter("@PinID", PinID);
            sqlpara[2] = new SqlParameter("@Memb_Code", Memb_Code);
            sqlpara[3] = new SqlParameter("@Pin", Pin);
            sqlpara[4] = new SqlParameter("@Commit_No", Commit_No);
            sqlpara[5] = new SqlParameter("@from_Id", from_Id);
            sqlpara[6] = new SqlParameter("@amount", amount);

            return DBHelper.GetDataTable("SP_Pindetails", sqlpara);
        }


        public static DataTable PinRequestdetails(string Mode, string PinID, string Memb_Code, string Pin
            , string Commit_No, string from_Id, string amount, string Pin_Quantity)
        {
            SqlParameter[] sqlpara = new SqlParameter[8];
            sqlpara[0] = new SqlParameter("@Mode", Mode);
            sqlpara[1] = new SqlParameter("@PinID", PinID);
            sqlpara[2] = new SqlParameter("@Memb_Code", Memb_Code);
            sqlpara[3] = new SqlParameter("@Pin", Pin);
            sqlpara[4] = new SqlParameter("@Commit_No", Commit_No);
            sqlpara[5] = new SqlParameter("@from_Id", from_Id);
            sqlpara[6] = new SqlParameter("@amount", amount);
            sqlpara[7] = new SqlParameter("@Pin_Quantity", Pin_Quantity);

            return DBHelper.GetDataTable("SP_Pindetails", sqlpara);
        }
        public static int User_TopUp(string MODE, string memb_code, string amount, string address, string btsAmount
            , string BTC_Type, string plan_type, string From_ID, string PinID)
        {
            SqlParameter[] sqlpara = new SqlParameter[9];
            sqlpara[0] = new SqlParameter("@MODE", MODE);
            sqlpara[1] = new SqlParameter("@memb_code", memb_code);
            sqlpara[2] = new SqlParameter("@amount", amount);
            sqlpara[3] = new SqlParameter("@address", address);
            sqlpara[4] = new SqlParameter("@btsAmount", btsAmount);
            sqlpara[5] = new SqlParameter("@BTC_Type", BTC_Type);
            sqlpara[6] = new SqlParameter("@plan_type", plan_type);
            sqlpara[7] = new SqlParameter("@From_ID", From_ID);
            sqlpara[8] = new SqlParameter("@PInID", PinID);

            return DBHelper.ExecuteNonQuery("SP_TopUpUser", sqlpara);
        }
        public static DataTable CheckUsername(string username)
        {
            return DBHelper.GetDataTable("select * from Entry where username='" + username + "'");
        }

        public static DataTable USER_PATTERN_UPDATE(string MODE, string username)
        {
            SqlParameter[] sqlpara = new SqlParameter[2];
            sqlpara[0] = new SqlParameter("@MODE", MODE);
            sqlpara[1] = new SqlParameter("@username", username);

            return DBHelper.GetDataTable("SP_REPORT", sqlpara);
        }
    }
}
