using MailKit.Net.Smtp;
using MimeKit;
using Ticketsystem.Enums;

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
        /// <summary>
        /// Choice of enumeration determines the type of the Message.
        /// Content of the message can be change by changing the .txt files
        /// in the Emailtxt folder.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="emailtype">Currently EmailTypes.ConfirmationEmail or EmailTypes.OrderFinished</param>
        public Message(IEnumerable<MailboxAddress> to, EmailTypes emailtype)
        {
            // To Do Create Object for email address
            To = to.ToList();
            switch (emailtype)
            {
                case EmailTypes.ConfirmationEmail:
                    CreateConfirmationMail();
                    break;
                case EmailTypes.OrderFinished:
                    CreateOrderFinishedMail();
                    break;
            }

        }
        private void CreateConfirmationMail()
        {
            string filename = "ConfirmationMail.txt";
            string subfolder = "EmailSender/Emailtxt";
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, subfolder, filename);
            // BfW specific
            Subject = "Bestätigungsemail ihres Kundenauftrages beim Service Point des BfW Nürnberg";
            Content = CreateContentString(filepath);
        }

        private void CreateOrderFinishedMail()
        {
            string filename = "OrderFinished.txt";
            string subfolder = "EmailSender/Emailtxt";
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, subfolder, filename);
            // BfW specific
            Subject = "Ihr Kundenauftrag beim Service Point des BfW Nürnberg ist fertiggestellt";
            Content = CreateContentString(filepath);
        }

        private string CreateContentString(string filePath)
        {
            using StreamReader reader = new StreamReader(filePath);
            string fileContent = reader.ReadToEnd();
            string concatenatedString = string.Join(Environment.NewLine, fileContent.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None
            ));

            return concatenatedString;
        }
    }
}
