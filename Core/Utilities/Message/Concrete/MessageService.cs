using Core.Utilities.Message.Abstract;
using System.Net.Mail;

namespace Core.Utilities.Message.Concrete
{
    public class MessageService : IMessageService
    {
        public async Task SendMessage(string to, string subject, string message)
        {
            MailMessage mailMessage = new()
            {
                IsBodyHtml = true,
            };
            mailMessage.To.Add(to);
            mailMessage.Subject = subject;
            mailMessage.From = new("eltech@gmail.com", "Elgiz Elgizli", System.Text.Encoding.UTF8);

            SmtpClient smtpClient = new()
            {
                Port = 587,
                EnableSsl = true,
            };
    }
    }
}
