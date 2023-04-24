
using MimeKit;
namespace judo_univ_rennes.MailHelper
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public Message(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            string r = "receiver";
            To.AddRange(to.Select(x => new MailboxAddress(r,x)));
            Subject = subject;
            Content = content;
        }
    }
}
