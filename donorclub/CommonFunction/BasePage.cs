using System;
using System.Data;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace LudoFoundation_app.CommanFunction
{
    public class BasePage : Controller
    {
        protected UserInfo userInfo = null;

        public void Page_PreInit(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(userInfo.UserID))
                RedirectToAction("Index", "Home");
        }

        public BasePage()
        {
            userInfo = Common.CurrentUserInfo;

            if (Common.CurrentPageName != "changeculture" && userInfo!=null)
            {
                userInfo.actioname = Common.CurrentPageName;
                userInfo.controllerName = Common.CurrentControllerName;
            }

            if (userInfo == null)
            {
                return;
            }

        }

        public string GetSmallDateValue(string strDt)
        {
            if (String.IsNullOrEmpty(strDt))
                return "";
            DateTime userSelectedDate = Convert.ToDateTime(strDt);
            return userSelectedDate.Date.ToString("dd/MM/yyyy");
        }

        public string GetSmallDateValue2(string strDt)
        {
            if (String.IsNullOrEmpty(strDt))
                return "";
            DateTime userSelectedDate = Convert.ToDateTime(strDt);
            return userSelectedDate.Date.ToString("MM/dd/yyyy");
        }

        public string GetSmallDateValue3(string strDt)
        {
            if (String.IsNullOrEmpty(strDt))
                return "";
            DateTime userSelectedDate = Convert.ToDateTime(strDt);
            return userSelectedDate.Date.ToString("yyyy-MM-dd");
        }

        public string getRandomPassword()
        {

            string allowedChars = "";

           // allowedChars = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";

           // allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";

            allowedChars += "1,2,3,4,5,6,7,8,9,0";

            char[] sep = { ',' };

            string[] arr = allowedChars.Split(sep);

            string passwordString = "";

            string temp = "";

            Random rand = new Random();

            for (int i = 0; i < 6; i++)
            {

                temp = arr[rand.Next(0, arr.Length)];

                passwordString += temp;

            }

            return passwordString;

        }

        public string converttimetoint(string inputtime)
        {
            string outputtime = "10.30 am";

            string[] mystring = inputtime.Split(' ');

            if (String.Equals(mystring[1].ToString(), "PM") || String.Equals(mystring[1].ToString(), "pm"))
            {
                string[] substring = mystring[0].Split(':');
                outputtime = (12 + int.Parse(substring[0])).ToString() + substring[1];
            }
            else
            {
                string[] substring = mystring[0].Split(':');
                outputtime = substring[0] + substring[1];
            }

            return outputtime;
        }

        public string CreateRandomPassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public string CreateRandomPin(int length)
        {
            const string valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}