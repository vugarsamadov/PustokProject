using Microsoft.CodeAnalysis.CSharp.Syntax;
using PustokProject.Services;

namespace PustokProject.Extensions
{
	public static class EmailExtensions
	{


		public static void SendWelcomeEmail(this IEmailService emailService,string email)
		{
			var currentdir = Directory.GetCurrentDirectory();
			var htmlPath = "C:\\Users\\Birinci Novbe\\Desktop\\PustokProject\\PustokProject/EmailTemplates/welcometemplate.txt";
			
			StreamReader sr = new StreamReader(htmlPath);
			var body = sr.ReadToEnd();

			emailService.SendEmailAsync(email, "Welcome", body);
		}


	}
}
