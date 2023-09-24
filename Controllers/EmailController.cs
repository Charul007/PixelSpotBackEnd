using MimeKit;
using PixelSpot1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using MailKit.Net.Smtp;
using Newtonsoft.Json.Linq;
using System.Web.ModelBinding;

namespace PixelSpot1.Controllers
{
    [EnableCors("*","*","*")]
    public class EmailController : ApiController
    {
        PixelSpotEntities db = new PixelSpotEntities();
        //send email
        public static string otp = null;

        #region sendOTP

        [HttpGet]
        [Route("api/Email/email")]
        public int email(string getEmail)
        {


            var usr = db.Users.Where(u => u.u_email == getEmail).FirstOrDefault();

            if (usr != null)
            {

                Random rnd = new Random();
                otp = rnd.Next(1000, 9999).ToString();

                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress("OTP-Validation", "charulpatidar2000@gmail.com"));
                message.To.Add(MailboxAddress.Parse(getEmail));
                message.Subject = "Verification code from PIXELSOPT Support Services!!";
                message.Body = new TextPart("plain")
                {
                    Text = String.Format(@"Please use the verification code below to sign in.
                    {0}
                    If you didn’t request this, you can ignore this email.
                    Thanks,
                    The Printer Support Service team", otp)

                };

                string emailAddress = "charulpatidar2000@gmail.com";
                string password = "obfgmkmkixfrgcfy";

                SmtpClient client = new SmtpClient();
                try
                {
                    client.Connect("smtp.gmail.com", 465, true);
                    client.Authenticate(emailAddress, password);
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();

                }
            }//end if

            return 1;
        }

        #endregion


        #region verifyOTP

       

        [HttpPost]
        [Route("api/Email/opt")]
        public int opt()
        {
            String requestBody = Request.Content.ReadAsStringAsync().Result;

            

            JObject o = JObject.Parse(requestBody);

            String getotp = o["OTP"].Value<String>();



            if (otp.Equals(getotp))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        #endregion


        #region setNewPassword

        [HttpPost]
        [Route("api/Email/setNewPassword")]
        public int setNewPassword([FromBody] User user)
        {

            if(user == null)
            { return 0; }

            User ur = db.Users.Where( u => u.u_email == user.u_email).FirstOrDefault();
            if (ur != null)
            {
                ur.u_password = user.u_password;
                db.SaveChanges();
                return 1;
            }

            return 0;

        }
        
        #endregion
    }
}
