using Microsoft.AspNetCore.Components;
using MudBlazor;
using judo_univ_rennes.Contracts;
using judo_univ_rennes.Dtos.User;
using System.Text.RegularExpressions;

namespace judo_univ_rennes.Components
{
    public partial class ResetPassword
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
        public UserResetPasswordDto resetPasswordDto { get; set; } = new UserResetPasswordDto();
        bool success;
        string[] errors = { };

        [Parameter]
        public string Email { get; set; }


        MudForm form;

        public string PasswordSample { get; set; } = "Minimum 8 caractères, dont 1 chiffre, 1 majuscule et 1 caractère spécial";
        //public string Password { get; set; } = "";
        private bool busy;

        bool isPwdShow;
        bool isNewPwdShow;
        bool isConfirmShow;
        InputType PasswordInput = InputType.Password;
        InputType NewPasswordInput = InputType.Password;
        InputType ConfirmInput = InputType.Password;
        string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
        string NewPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
        string ConfirmInputIcon = Icons.Material.Filled.VisibilityOff;


        protected override async Task OnInitializedAsync()
        {
            
            resetPasswordDto.Email = Email;
            await base.OnInitializedAsync();
        }

        protected async Task HandleResetPwd()
        {
            busy = true;

            var response = await _authRepo.ResetPassword(resetPasswordDto);


            if (response)
            {
                Snackbar.Add("réinitialisation de mdp réussie", Severity.Success);
                _navManager.NavigateTo("index");
            }
            else
            {
                Snackbar.Add("échec de réinitilisation mdp", Severity.Error);
            }
            busy = false;
        }

        void ChangePwdVisibility()
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
        void ChangeNewPwdVisibility()
        {
            if (isNewPwdShow)
            {
                isNewPwdShow = false;
                NewPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                NewPasswordInput = InputType.Password;
            }
            else
            {
                isNewPwdShow = true;
                NewPasswordInputIcon = Icons.Material.Filled.Visibility;
                NewPasswordInput = InputType.Text;
            }
        }
        void ChangeConfirmVisibility()
        {
            if (isConfirmShow)
            {
                isConfirmShow = false;
                ConfirmInputIcon = Icons.Material.Filled.VisibilityOff;
                ConfirmInput = InputType.Password;
            }
            else
            {
                isConfirmShow = true;
                ConfirmInputIcon = Icons.Material.Filled.Visibility;
                ConfirmInput = InputType.Text;
            }
        }
        private IEnumerable<string> PasswordStrength(string pw)
        {
            if (string.IsNullOrWhiteSpace(pw))
            {
                yield return "Il faut un mot de passe!";
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
        }

        private string PasswordMatch(string arg)
        {
            if (resetPasswordDto.NewPassword != arg)
                return "les deux saisies de mdp ne sont pas identiques";
            return null;
        }
    }
}
