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
        public Message(string clientEmailAdress, EmailTypes emailtype)
        {
            // To Do Create Object for Emaiaddress
            To = to.ToList();
            switch (emailtype)
            {
                case EmailTypes.ConfirmationEmail:
                    createConfirmationMail();
                    break;
                case EmailTypes.OrderFinished:
                    createOrderFinishedMail();
                    break;
            }

        }
        private void createConfirmationMail()
        {
            string filename = "ConfirmationMail.txt";
            string subfolder = "EmailSender/Emailtxt";
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, subfolder, filename);
            // BfW specific
            Subject = "Bestätigungsemail ihres Kundenauftrages beim Service Point des BfW Nürnberg";
            Content = createContentString(filepath);
        }

        private void createOrderFinishedMail()
        {
            string filename = "OrderFinished.txt";
            string subfolder = "EmailSender/Emailtxt";
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, subfolder, filename);
            // BfW specific
            Subject = "Ihr Kundenauftrag beim Service Point des BfW Nürnberg ist fertiggestellt";
            Content = createContentString(filepath);
        }

        private string createContentString(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string fileContent = reader.ReadToEnd();
                string concatenatedString = string.Join(Environment.NewLine, fileContent.Split(
                    new[] { Environment.NewLine },
                    StringSplitOptions.None
                ));

                return concatenatedString;
            }
        }
    }
}
