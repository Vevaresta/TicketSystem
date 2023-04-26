namespace Ticketsystem
{
    public interface IEmailSender
    {
        Task<string> SendEmail(Message message);
    }
}
