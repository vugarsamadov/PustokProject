using System.Net;
using System.Net.Http;
using System.Net.Mail;

namespace PustokProject.Services
{

	public interface IEmailService
	{
		public void SendEmailAsync(string to, string subject, string body);
	}
	public class EmailService : IEmailService
	{
        public string ReceiverEmail { get; set; }


        public void SendEmailAsync(string to, string subject, string body)
		{
			string from = "mi779pf7t@code.edu.az";
			MailMessage message = new MailMessage(from, to, subject, body);
			message.IsBodyHtml = true;
			SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
			client.UseDefaultCredentials = false;
			client.EnableSsl = true;

			client.Credentials = new NetworkCredential("mi779pf7t@code.edu.az", "vkli bzxi gtss cuos");
			
			client.Send(message);
		}
	}
}
