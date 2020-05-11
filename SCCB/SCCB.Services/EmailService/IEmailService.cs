using SCCB.Core.DTO;

namespace SCCB.Services.EmailService
{
    /// <summary>
    /// Interface for email service.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Composes email with information on changing password and sends it using specified email.
        /// </summary>
        /// <param name="email">Email and change password token.</param>
        void SendChangePasswordEmail(EmailWithToken email);
    }
}
