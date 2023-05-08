using MovieTicketBookingApp.Interfaces;
using System.Net.Mail;
using System.Net;

namespace MovieTicketBookingApp.InterfaceImplementation
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config) {
            _config = config;
        }

        public Task SendEmailAsync( string toEmail, string subject, string message)
        {
            // The below data is sensitive so its fetched from the secrets.json created by Manage User secrets
            string fromEmail = _config["MailSettings:email"];
            string emailPassword = _config["MailSettings:password"];
            string host = _config["MailSettings:host"];
            int port = Convert.ToInt32(_config["MailSettings:port"]);

            Console.WriteLine($"Here: {fromEmail}, {emailPassword}, {host}, {port}, {toEmail}, {subject}, {message}");
            var client = new SmtpClient(host, port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail, emailPassword)
            };

            return client.SendMailAsync(
                new MailMessage(from: fromEmail,
                                to: toEmail,
                                subject,
                                message
                                ));
        }
    }
}
