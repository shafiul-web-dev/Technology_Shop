namespace Technology_Shop.Interfaces.EmailInterface
{
	public interface IEmailService
	{
		Task SendAsync(string toEmail, string subject, string htmlMessage);
	}
}
