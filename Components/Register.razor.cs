
using System.Net.Mail;
using System.Text.RegularExpressions;


namespace judo_univ_rennes.Components
{
    public partial class Register
    {
        #region DI

        /// <summary>
        /// injection dépendance IAuth
        /// </summary>
        [Inject]
        public IAuthRepo _authRepo { get; set; }
        /// <summary>
        /// injection dépendance navigationMananger
        /// </summary>
        [Inject]
        public NavigationManager _navManager { get; set; }
        /// <summary>
        /// injection dépendance mud snackbar
        /// </summary>
        [Inject]
        public ISnackbar Snackbar { get; set; }
        #endregion

        #region Properties
        /// <summary>
        /// modèle d'inscription
        /// </summary>
        public UserRegisterDto registerModel { get; set; } = new UserRegisterDto();
        private bool success;
        private bool busy;
        private string[] errors = { };
        /// <summary>
        /// formulaire POST
        /// </summary>
        private MudForm form;

        public string PasswordSample { get; set; } = "Minimum 8 caractères, dont 1 chiffre, 1 majuscule et 1 caractère spécial";


        private bool isPwdShow;
        private bool isConfirmShow;
        private InputType PasswordInput = InputType.Password;
        private InputType ConfirmInput = InputType.Password;
        private string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
        private string PasswordConfirmInputIcon = Icons.Material.Filled.VisibilityOff;
        private string CheckEmailIcon = Icons.Material.Filled.FactCheck;
        /// <summary>
        /// Date de naissance
        /// </summary>
        private DateTime? dob = DateTime.Today;
        #endregion

        #region Methods

        /// <summary>
        /// Initialisation de conposant
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Verify email
        /// error message:
        /// -when email format is incorrect
        /// -when email has been used
        /// </summary>
        /// <returns></returns>

        protected async Task VerifyEmail()
        {
            try
            {
                MailAddress m = new MailAddress(registerModel.Email);
                var response = await _authRepo.CheckMail(registerModel.Email);
                if (!response)
                {
                    Snackbar.Add("Cette adresse a déjà été utilisée", Severity.Error);
                }
                else
                {
                    Snackbar.Add("Adresse mail valide", Severity.Success);
                }
            }
            catch (FormatException)
            {
                Snackbar.Add("Format d'adresse invalide", Severity.Error);
            }
        }


        /// <summary>
        /// Exécution lors que l'utilisateur click sur inscription
        /// </summary>
        /// <returns></returns>

        protected async Task HandleRegister()
        {
            busy = true;
            if (registerModel.Birthday != null)
            {
                registerModel.Birthday = ((DateTime)registerModel.Birthday).ToUniversalTime();
            }
            var response = await _authRepo.Register(registerModel);

            if (response)
            {
                Snackbar.Add("Inscription réussie", Severity.Success);
                Snackbar.Add("Un mail de confirmation vous a été envoyé, veuillez confirmer votre adresse mail", Severity.Info);
            }
            else
            {
                Snackbar.Add("Echec d'authentification", Severity.Error);
            }
            busy = false;
        }
        /// <summary>
        /// Change visibility of password input
        /// </summary>
        private void ChangePwdVisibility()
        {
            if (isPwdShow)
            {
                isPwdShow = false;
                PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInput = InputType.Password;
            }
            else
            {
                isPwdShow = true;
                PasswordInputIcon = Icons.Material.Filled.Visibility;
                PasswordInput = InputType.Text;
            }
        }
        /// <summary>
        /// Change visibility of password confirm input
        /// </summary>
        private void ChangeConfirmVisibility()
        {
            if (isConfirmShow)
            {
                isConfirmShow = false;
                PasswordConfirmInputIcon = Icons.Material.Filled.VisibilityOff;
                ConfirmInput = InputType.Password;
            }
            else
            {
                isConfirmShow = true;
                PasswordConfirmInputIcon = Icons.Material.Filled.Visibility;
                ConfirmInput = InputType.Text;
            }
        }
        /// <summary>
        /// verify password strength
        /// </summary>
        /// <param name="pw"></param>
        /// <returns></returns>
        private IEnumerable<string> PasswordStrength(string pw)
        {
            if (string.IsNullOrWhiteSpace(pw))
            {
                yield return "Mot de passe requis!";
                yield break;
            }
            if (pw.Length < 8)
                yield return "Le mot de passe doit avoir minimum 8";
            if (!Regex.IsMatch(pw, @"[A-Z]"))
                yield return "Le mot de passe doit avoir au moins 1 lettre majuscule";
            if (!Regex.IsMatch(pw, @"[a-z]"))
                yield return "Le mot de passe doit avoir au moins 1 lettre miniscule";
            if (!Regex.IsMatch(pw, @"[0-9]"))
                yield return "Le mot de passe doit avoir au moins 1 numéro";
            if (!Regex.IsMatch(pw, @"[\*@!#%&\(\)\^~{}\\.]"))
                yield return "le mot de passe doit contenir un caractère spécial,ex: *@!#%&\\()^~{}./";
        }
        /// <summary>
        /// verify if password matches with password confirm
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private string PasswordMatch(string arg)
        {
            if (registerModel.Password != arg)
                return "les deux saisies de mot de passe ne sont pas identiques";
            return null;
        }
        #endregion

    }
}
