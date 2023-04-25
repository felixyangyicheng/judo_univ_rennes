using judo_univ_rennes.Dtos.User;
using judo_univ_rennes.Statics;

namespace judo_univ_rennes.Contracts
{
    public interface IAuthRepo
    {
        public Task<bool> Login(UserLoginDto userDto);
        public Task<bool> CheckMail(string email);
        public Task Logout();

        public Task<bool> Register(UserRegisterDto userDto);
        public Task<bool> ConfirmEmail(string email, string token);

        public Task<bool> ResetPassword(UserResetPasswordDto userDto);
        public Task<bool> ForgotPassword(UserEmailDto userDto);
        public Task<bool> ResetForgottenPassword(UserResetForgottenPasswordDto ususerDtoer);
        //public Task<bool> GoogleAuth(CredentialWithPhoto usercredential);
        public Task<bool> RemoveAccount(string email);
    }
}
