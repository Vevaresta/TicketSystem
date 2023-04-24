using MailKit.Net.Smtp;
using MimeKit;

namespace Ticketsystem
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public Message(IEnumerable<MailboxAddress> to, string subject, string content)
        {
            To = to.ToList();
            Subject = subject;
            Content = content;
        }
        
    }
}
