using VokzFinancy.DTOs;

namespace VokzFinancy.Services {

    public interface IEmailService {

        Task<bool> SendEmailConfirmation(EmailDTO email);
        Task<bool> SendEmailResetPassword(EmailDTO email);

    }

}