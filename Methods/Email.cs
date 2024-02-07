using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Xml;
using System.Xml.Linq;

namespace CarbonFootprint1.Methods
{
    public class Email : Controller
    {
        //Send Emails Method
        public async Task<Boolean> SendEmailAsync(string email, string subject, string message)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("cbgnotifications@cbg.com.gh"); 
                mail.To.Add(new MailAddress(email));
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = message;
             


                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
              
                using (SmtpClient smtp = new SmtpClient("smtp.office365.com", 587))
                //using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("cbgnotifications@cbg.com.gh", "!Compound4ME");
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception e)
                    {
                        var t = e.Message;
                        //errorLog.LogExceptions(e, "Send Email Async");
                        return false;
                    }
                }
            }
            //return true;
        }
    }
}