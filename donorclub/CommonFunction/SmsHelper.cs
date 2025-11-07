using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Web.Configuration;

namespace LudoFoundation_app.CommanFunction
{
    public class SmsHelper
    {
        public static string SendSMS(string Tos, string msg)
        {
            try
            {
                bool smsisactive = Convert.ToBoolean(WebConfigurationManager.AppSettings["IsSMSAllow"].ToString());

                if (smsisactive == true)
                {

                    //string strUrl = WebConfigurationManager.AppSettings["smsurl"].ToString();

                    //strUrl = strUrl.Replace("*", "&");
                    //strUrl = strUrl.Replace("{{U}}", WebConfigurationManager.AppSettings["smsusername"].ToString());
                    //strUrl = strUrl.Replace("{{P}}", WebConfigurationManager.AppSettings["smspassword"].ToString());
                    //strUrl = strUrl.Replace("{{APN}}", WebConfigurationManager.AppSettings["smsapn"].ToString());
                    //strUrl = strUrl.Replace("{{Mobile}}", Tos);
                    //strUrl = strUrl.Replace("{{MSG}}", msg);

                     string strUrl = "http://www.smsjust.com/sms/user/urlsms.php?username=LADO FOUNDATION05&pass=LADO FOUNDATION@05&senderid=MIRACL&dest_mobileno=" + Tos.Trim() + "&message=" + msg + "&response=Y";

                    // Create a request object  
                    WebRequest request = HttpWebRequest.Create(strUrl);
                    // Get the response back  
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream s = (Stream)response.GetResponseStream();
                    StreamReader readStream = new StreamReader(s);
                    string dataString = readStream.ReadToEnd();
                    response.Close();
                    s.Close();
                    readStream.Close();
                    return "Message Send Successfully.";
                }
                else
                {
                    return "SMS Not Activet";
                }
            }
            catch (Exception ex)
            {
                return "Message Sending Failed Error: "+ex.Message;
            }
        }
        public static string GenerateRandomOTP(int iOTPLength)
        {
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string sOTP = String.Empty;
            string sTempChars = String.Empty;
            Random rand = new Random();
            for (int i = 0; i < iOTPLength; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                sOTP += sTempChars;
            }
            return sOTP;
        }

    }
}