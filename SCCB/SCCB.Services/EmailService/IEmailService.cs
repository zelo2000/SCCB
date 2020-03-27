using SCCB.Core.DTO;

namespace SCCB.Services.EmailService
{
    public interface IEmailService
    {
        void SendChangePasswordEmail(EmailWithToken email);
    }
}
