using System.Net;
using System.Net.Mail;

namespace SmKPlugins
{
    public class EmailService
    {
        private readonly string _smtpServer;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;

        public EmailService(string smtpServer, int port, string username, string password)
        {
            _smtpServer = smtpServer;
            _port = port;
            _username = username;
            _password = password;
        }

        public void SendEmail(string from, string to, string subject, string body)
        {
            using (var client = new SmtpClient(_smtpServer, _port))
            {
                client.Credentials = new NetworkCredential(_username, _password);
                client.EnableSsl = true;

                var mailMessage = new MailMessage(from, to)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                client.Send(mailMessage);
            }
        }
    }
}