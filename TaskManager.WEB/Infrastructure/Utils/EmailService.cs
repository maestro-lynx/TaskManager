using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace TaskManager.WEB.Infrastructure.Utils
{
	public static class EmailService
	{

		public static bool SendEmail(string Email, string receiverName,string subject, string message)
		{
			string senderAddress = "TaskManagerEmail@yandex.kz";
			string senderAddressPassword = "TaskManager123";
			string senderName = "TaskManager";

			var senderEmail = new MailAddress(senderAddress, senderName);
			var receiverEmail = new MailAddress(Email, receiverName);
			var password = senderAddressPassword;
			var sub = subject;
			var body = message;
			var smtp = new SmtpClient
			{
				Host = "smtp.yandex.ru",
				Port = 25,
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(senderEmail.Address, password)
			};
			try
			{
				using (var mess = new MailMessage(senderEmail, receiverEmail)
				{

					Subject = subject,
					Body = body
				})
				{
					mess.IsBodyHtml = true;
					smtp.Send(mess);
				}
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}