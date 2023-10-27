using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace IntraNet.Domain.Entities
{
    public class EmailService
    {
        // GET: EmailService
        public async Task<bool> SendEmailAsync(string emailTo, string mailbody, string subject)
        {
            var from = new MailAddress("admin@admin.it");
            var to = new MailAddress(emailTo);

            var useDefaultCredentials = false;
            var enableSsl = true;
            var replyto = "admin@admin.it"; // set here your email; 
            var userName = string.Empty;
            var password = string.Empty;
            var port = 587;
            var host = "localhost";

            userName = "admin@admin.it"; // setup here the username; 
            password = "x"; // setup here the password; 
            bool.TryParse("true", out useDefaultCredentials); //setup here if it uses defaault credentials 
            bool.TryParse("true", out enableSsl); //setup here if it uses ssl 
            int.TryParse("587", out port); //setup here the port 
            host = "smtp.office365.com"; //setup here the host 

            using (MailMessage mail = new MailMessage(from, to))
            {
                mail.Subject = subject;
                AlternateView htmlView =
                    AlternateView.CreateAlternateViewFromString(mailbody, Encoding.UTF8, "text/html");
                mail.AlternateViews.Add(htmlView); // And a html attachment to make sure.
                mail.Body = mailbody;  // But the basis is the html body
                mail.IsBodyHtml = true; // But the basis is the html body
                mail.ReplyToList.Add(new MailAddress(replyto, "pedro.henrique"));
                mail.ReplyToList.Add(from);
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.Delay |
                                                   DeliveryNotificationOptions.OnFailure |
                                                   DeliveryNotificationOptions.OnSuccess;

                using (var client = new SmtpClient())
                {
                    client.Host = host;
                    client.EnableSsl = enableSsl;
                    client.Port = port;
                    client.UseDefaultCredentials = false;

                    if (!client.UseDefaultCredentials && !string.IsNullOrEmpty(userName) &&
                        !string.IsNullOrEmpty(password))
                    {
                        //confirmação do disparo!
                        client.Credentials = new NetworkCredential(userName, password);
                    }

                    await client.SendMailAsync(mail);
                }
            }

            return true;
        }
    }
}