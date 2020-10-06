using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace TryNetCore.Utils
{
    public class SendMail
    {
        


        public static async  Task emailSend(string senderEmail, string receiverEmail, string senderName, string subject, string content, string senderEmailPassword)
        {

            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = true;
            mail.To.Add(receiverEmail);

            mail.From = new MailAddress(senderEmail, senderName);
            mail.Subject = subject;

            // mail.Body = $"<a href=\"{content}\"  > selam </a>";
            // mail.Body = $"<div style=\"font-weight:bold;\">  {content}  </div>";

            mail.Body = $"{content}";
            mail.IsBodyHtml = true;

            SmtpClient smp = new SmtpClient();

            smp.UseDefaultCredentials = false;

            smp.Credentials = new NetworkCredential(senderEmail, senderEmailPassword);

            smp.Port = 587;
            smp.Host = "smtp.yandex.com.tr";

            smp.EnableSsl = true;
            smp.DeliveryMethod = SmtpDeliveryMethod.Network;

            await smp.SendMailAsync(mail);
            
        }

    }

}
