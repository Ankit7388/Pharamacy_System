using WebApi.Models;

namespace WebApi.server
{
    public interface IEmailService
    {
        void SendEmail(EmailDTO request);
    }
}
