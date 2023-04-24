namespace Ticketsystem
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
