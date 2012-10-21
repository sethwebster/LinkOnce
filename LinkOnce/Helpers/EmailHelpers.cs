using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace LinkOnce.Helpers
{
    public class EmailHelper
    {
        public static bool SendEmail(MailAddress from, IEnumerable<MailAddress> to, string Subject, string Body, bool isHtml)
        {

            SmtpClient cli = new SmtpClient(
                ConfigurationManager.AppSettings["MAILGUN_SMTP_SERVER"],
                int.Parse(ConfigurationManager.AppSettings["MAILGUN_SMTP_PORT"])
                );
            cli.Credentials = new NetworkCredential(
                ConfigurationManager.AppSettings["MAILGUN_SMTP_LOGIN"],
                ConfigurationManager.AppSettings["MAILGUN_SMTP_PASSWORD"]
                );
            MailMessage message = new MailMessage();
            message.From = from;
            foreach (var t in to)
            {
                message.To.Add(t);
            }

            message.Subject = Subject;
            message.Body = Body;
            message.IsBodyHtml = isHtml;
            cli.Send(message);
            return true;
        }

        public static Task SendEmailAsync(MailAddress from, IEnumerable<MailAddress> to, string Subject, string Body, bool isHtml)
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                var res = SendEmail(from, to, Subject, Body, isHtml);
                return res;
            });
        }

        public static bool SendEmail(MailAddress from, MailAddress to, string subject, string body, bool isHtml)
        {
            return SendEmail(from, new MailAddress[] { to }, subject, body, isHtml);
        }

        public static Task SendEmailAsync(MailAddress from, MailAddress to, string subject, string body, bool isHtml)
        {
            return SendEmailAsync(from, new MailAddress[] { to }, subject, body, isHtml);
        }
        public static bool SendEmail(string from, string to, string subject, string body, bool isHtml)
        {
            return SendEmail(new MailAddress(from), new MailAddress[] { new MailAddress(to) }, subject, body, isHtml);
        }

        public static Task SendEmailAsync(string from, string to, string subject, string body, bool isHtml)
        {
            return SendEmailAsync(new MailAddress(from), new MailAddress[] { new MailAddress(to) }, subject, body, isHtml);
        }
    }

}