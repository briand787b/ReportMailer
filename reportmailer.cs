using System;
using System.Net;
using System.IO;
using System.Net.Mail;

public class Mailer
{	
	public static void Main(string[] args)
	{
		string msgBody = String.Empty;
		msgBody += GatherMessage("/home/brian/servers/gaatl-envuat-01/drivespace.txt");
		msgBody += GatherMessage("/home/brian/servers/gaatl-sqluat-01/drivespace.txt");	
		SendEmail(msgBody);
	}

	private static string GatherMessage(string path)
	{
		string content = String.Empty;

		try
		{
			content = File.ReadAllText(path);			
			Console.WriteLine(content);
		}
		catch (Exception e)
		{
			Console.WriteLine("An exception was thrown");
			Console.WriteLine(e.Message);
		}
		
		return content;
	}

	private static void SendEmail(string body)
	{
		MailMessage msg = new MailMessage();

		msg.From = new MailAddress(Secrets.Username);
		msg.To.Add("development@mdsiinc.com");
		msg.Subject = "Server Status Report";
		msg.Body = body;

		using (SmtpClient client = new SmtpClient())
		{
			client.EnableSsl = true;
			client.UseDefaultCredentials = false;
			client.Credentials = new NetworkCredential(Secrets.Username, Secrets.Password);
			client.Host = "smtp.gmail.com";
			client.Port = 587;
			client.DeliveryMethod = SmtpDeliveryMethod.Network;

			client.Send(msg);
		}

		Console.WriteLine("Email was sent successfully");
	}
}
