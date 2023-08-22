

namespace judo_univ_rennes.Services
{
    public class AuthService : IAuthRepo
    {
    private readonly ILogger<AuthService> logger;
    private readonly IMapper mapper;
    private readonly UserManager<ApiUser> userManager;
    private readonly IConfiguration configuration;
    private readonly IEmailSender emailSender;
    private readonly ITokenRepo tokenRepo;
        private readonly IMemoryCache memoryCache;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
        public AuthService(
        ILogger<AuthService> logger,
        UserManager<ApiUser> userManager,
        IMapper mapper,
        IConfiguration configuration,
        IMemoryCache memoryCache,
        IEmailSender emailSender,
        ITokenRepo tokenRepo,
                    ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider
            )
    {
        this.logger = logger;
        this.mapper = mapper;
        this.userManager = userManager;
        this.configuration = configuration;
        this.emailSender = emailSender;
        this.tokenRepo = tokenRepo;
            this.memoryCache = memoryCache;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
        }
   
        public async Task<bool> CheckMail(string email)
        {
            logger.LogInformation($"Check if exisits Attempt for {email} ");
            try
            {
                var isExist = await userManager.FindByEmailAsync(email);
                if (isExist != null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Something Went Wrong in the {nameof(CheckMail)}");
                var message = new Message(new string[] { "y.yang@iia-formation.fr" }, $"{nameof(CheckMail)} Exception ", $"Something went wrong in the {nameof(CheckMail)}" + ex.ToString());
                await emailSender.SendEmailAsync(message);
                return false ;
            }
        }



        public async Task<bool> ForgotPassword(UserEmailDto userDto)
        {
            logger.LogInformation($"Forgot password reset process Attempt for {userDto.Email} ");
            try
            {
                var user = await userManager.FindByEmailAsync(userDto.Email);
                if (user == null)
                {
                    logger.LogInformation($" {userDto.Email} not found ");

                    return false;
                }
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var resetPasswordLink=$"https://judo-univ-rennes.duckdns.org/account/passwordrecovery/{userDto.Email}/{token}";
                var message = new Message(new string[] { user.Email }, "Réinitialiser votre mot de passe SKALI", resetPasswordLink);
                logger.LogInformation($"Email with reset password token sent to {userDto.Email} ");
                //var message = new Message(new string[] {"y.yang@iia-formation.fr" }, "Your token to reset password",$"Please paste this token to the 'reset forgotten password' page : {token} " );
                await emailSender.SendEmailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"reset password for {userDto.Email} exception, {ex} ");

                return false;
            }
        }

        public async Task<bool> Login(UserLoginDto userDto)
        {
            logger.LogInformation($"Login Attempt for {userDto.Email} ");
            try
            {
                var user = await userManager.FindByEmailAsync(userDto.Email);
                var passwordValid = await userManager.CheckPasswordAsync(user, userDto.Password);
                if (user == null || passwordValid == false)
                {
                    logger.LogInformation($" {userDto.Email} password invalid or user not found");
                    return false;
                }
                string tokenString = await tokenRepo.GenerateToken(user, false, "");
                await _localStorage.SetItemAsync("authToken", tokenString);
                //Change auth state of app
                await ((ApiAuthenticationStateProvider)_authenticationStateProvider)
                    .LoggedIn();
                //_client.DefaultRequestHeaders.Authorization =
                //    new AuthenticationHeaderValue("bearer", tokenString);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError($"exception when {userDto.Email} attempts to login, {ex}");

                return false;
            }
            
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider)
                .LoggedOut();
        }

        public async Task<bool> Register(UserRegisterDto userDto)
        {
            logger.LogInformation($"Registration Attempt for {userDto.Email} ");
            try
            {
                var user = mapper.Map<ApiUser>(userDto);
                user.UserName = userDto.Email;
                user.Email = userDto.Email;
                var result = await userManager.CreateAsync(user, userDto.Password);
                if (result.Succeeded == false)
                {
                    foreach (var error in result.Errors)
                    {
                        logger.LogWarning($"{error.ToString()} when Registration Attempt for {userDto.Email} ");
                    }
                    return false;
                }
                await userManager.AddToRoleAsync(user, "Client");
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                //var confirmationLink = Url.Action("ConfirmEmail", "Identity", new { token, email = user.Email }, Request.Scheme);
                var confirmationLink =  $"https://judo-univ-rennes.duckdns.org/account/confirmemail/{userDto.Email}/{token}";
                //var confirmationLink =  $"https://localhost:8081/account/confirmemail/{userDto.Email}/{token}";
                var unlockTime = await userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow);
                //logger.LogInformation($"confirmationLink : {confirmationLink}");
                //var message = new Message
                //                (
                //                    new string[] { user.Email },
                //                    $"Confirmer votre Compte Skali",
                //                    $"{MailContent.ComfirmEmailPrefix}{confirmationLink}{MailContent.ComfirmEmailSuffix}"
                //                );
                //await emailSender.SendEmailAsync(message);
                logger.LogInformation($"email : {confirmationLink} send to {user.Email}");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError($"Registration Attempt for {userDto.Email} exception, {ex} ");

                return false;

            }
            //throw new NotImplementedException();
        }
        public async Task<bool> ConfirmEmail(string email, string token)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }
            var result = await userManager.ConfirmEmailAsync(user, token);
            var unlock = await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now);
            if (!result.Succeeded)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> ResetForgottenPassword(UserResetForgottenPasswordDto userDto)
        {
            logger.LogInformation($"Forgot password reset process Attempt for {userDto.Email} ");
            try
            {
                var user = await userManager.FindByEmailAsync(userDto.Email);
                if (user == null || userDto.Token == null)
                {
                    return false;
                }
                else if (userDto.Password != userDto.PasswordConfirm)
                {
                    return false;
                }

                var resetPassResult = await userManager.ResetPasswordAsync(user, userDto.Token, userDto.Password);
                if (!resetPassResult.Succeeded)
                {
                    return false;
                }
                logger.LogInformation($" password recovery process ok for {userDto.Email} ");

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Something Went Wrong in the {nameof(ResetForgottenPassword)}");
                return false;
            }
        }

        public async Task<bool> ResetPassword(UserResetPasswordDto userDto)
        {
            logger.LogInformation($"Logged user password reset process Attempt for {userDto.Email} ");
            try
            {
                var user = await userManager.FindByEmailAsync(userDto.Email);
               
                var passwordValid = await userManager.CheckPasswordAsync(user, userDto.Password);
                if (user == null || passwordValid == false)
                {
                    return false;
                }
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var resetPassResult = await userManager.ResetPasswordAsync(user, token, userDto.NewPassword);
                if (!resetPassResult.Succeeded)
                {
                    return false;
                }
                logger.LogInformation($"{userDto.Email} password reset");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError($"Logged user password reset process exception for {userDto.Email}, {ex} ");

                return false;
            }
        }

        public async Task<bool> GoogleAuth(CredentialWithPhoto usercredential)
        {
            //var canCreateAccount = await CheckMail(usercredential.Email);
            var userResult = await userManager.FindByEmailAsync(usercredential.Email);
            //if (canCreateAccount == true)
            if (userResult == null)
            {
                ApiUser user = new();
                user.UserName = usercredential.Name;
                user.Email = usercredential.Email;
                var result = await userManager.CreateAsync(user);
                if (result.Succeeded == false)
                {
                    foreach (var error in result.Errors)
                    {
                        logger.LogWarning($"{error} when Registration Attempt for {usercredential.Email} ");
                    }
                    return false;
                }
                await userManager.AddToRoleAsync(user, "Client");
            }
            var login = await userManager.FindByEmailAsync(usercredential.Email);

            string tokenString = await tokenRepo.GenerateToken(login, true, usercredential.Photo);
            await _localStorage.SetItemAsync("authToken", tokenString);
            //Change auth state of app
            await ((ApiAuthenticationStateProvider)_authenticationStateProvider)
                .LoggedIn();
            //_client.DefaultRequestHeaders.Authorization =
            //    new AuthenticationHeaderValue("bearer", tokenString);
            return true;
        }

        public async Task<bool> RemoveAccount(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            bool remove = false;
            if (user!=null)
            {

               IdentityResult result= await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    remove = true;               
                }
                else
                {
                    remove = false;
                }
            }
            else
            {
                remove = false;
            }
            return remove;
        }
    }
}
