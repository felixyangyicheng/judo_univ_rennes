using judo_univ_rennes.MailHelper;
namespace judo_univ_rennes.Contracts
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
        Task SendEmailAsync(Message message);
    }
}
