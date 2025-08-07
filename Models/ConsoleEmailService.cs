using Technology_Shop.Interfaces.EmailInterface;

namespace Technology_Shop.Models
{
	public class ConsoleEmailService : IEmailService
	{
		public Task SendAsync(string toEmail, string subject, string htmlMessage)
		{
			Console.WriteLine("----------EMail Message------");
			Console.WriteLine($"To: {toEmail}");
			Console.WriteLine($"Subject: {subject}");
			Console.WriteLine(htmlMessage);
			Console.WriteLine("-----------------------------");
			return Task.CompletedTask;
		}

	}
}
