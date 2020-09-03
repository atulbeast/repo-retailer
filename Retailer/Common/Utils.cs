using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json;
namespace Retailer.Common
{
    public static class Utils
    {
        public static string getErrors(ModelStateDictionary mState)
        {
            if (mState != null)
                return JsonConvert.SerializeObject(mState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage));
            else
                return "";
        }

        public static async Task SendSMS(string mobileNo, string sms)
        {
            string SMSSubmitString = "";

            string uriSMS = "https://api.msg91.com/api/sendhttp.php?route=4&sender=Retailer&message=" + sms + WebConfigurationManager.AppSettings["WebsiteLink"] + "&country=91&mobiles=" + mobileNo + "&authkey=" + WebConfigurationManager.AppSettings["SMS_AUTH"];
            try
            {
                HttpWebRequest request = WebRequest.Create(uriSMS) as HttpWebRequest;
                HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                SMSSubmitString = await readStream.ReadToEndAsync();
                response.Close();
                readStream.Close();

            }
            catch (Exception ex)
            {

            }

        }


        #region Send Email

        /// <summary>  
        ///  Send Email method.  
        /// </summary>  
        /// <param name="email">Email parameter</param>  
        /// <param name="msg">Message parameter</param>  
        /// <param name="subject">Subject parameter</param>  
        /// <returns>Return await task</returns>  
        public static async Task<bool> SendEmailAsync(string email, string msg, string fromUser, string subject = "")
        {
            // Initialization.  
            bool isSend = false;

            try
            {
                // Initialization.  
                var body = msg;
                var message = new MailMessage();

                // Settings.  
                message.To.Add(new MailAddress(email));
                message.From = new MailAddress(fromUser ?? WebConfigurationManager.AppSettings["SMTP_USERNAME"]);
                message.Subject = !string.IsNullOrEmpty(subject) ? subject : WebConfigurationManager.AppSettings["SMTP_SUB"];
                message.Body = body;
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    // Settings.  
                    var credential = new NetworkCredential
                    {
                        UserName = WebConfigurationManager.AppSettings["SMTP_USERNAME"],
                        Password = WebConfigurationManager.AppSettings["SMTP_PASSWORD"]
                    };

                    // Settings.  
                    smtp.Credentials = credential;
                    smtp.Host = WebConfigurationManager.AppSettings["SMTP"];
                    smtp.Port = Convert.ToInt32(WebConfigurationManager.AppSettings["SMTP_PORT"]);
                    // smtp.EnableSsl = true;

                    await smtp.SendMailAsync(message);

                    // Settings.  
                    isSend = true;
                }
            }
            catch (Exception ex)
            {
                // Info  
                throw ex;
            }

            // info.  
            return isSend;
        }

        #endregion

        /// <summary>
        /// Create Password Asp.Net Identity based encryption
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }




        public static string AddImage(string folderName)
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollection files = httpRequest.Files;
                    var imgUrl = new List<string>();
                    for (int i = 0; i < files.Count; i++)
                    {

                        HttpPostedFile file = files[i];
                        string fname = file.FileName;

                        string subPath = @"/Images/" + folderName;

                        bool exists = System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(subPath));

                        if (!exists)
                            System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(subPath));

                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(subPath), DateTime.Now.ToFileTime() + "-" + fname);
                        file.SaveAs(fname);
                        int pos = fname.IndexOf("\\Images");
                        imgUrl.Add(fname.Substring(pos));
                    }
                    return string.Join("", imgUrl);
                    //return Json("Something went wrong in upload");
                    // Returns message that successfully uploaded  

                }
                catch (Exception ex)
                {
                    return "Error occurred. Error details: " + ex.Message;
                }
            }
            else
            {
                return "No files selected.";
            }

        }


        public static string GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max).ToString();
        }
    }
}