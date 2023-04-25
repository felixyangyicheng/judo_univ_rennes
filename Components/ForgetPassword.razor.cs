using Microsoft.AspNetCore.Components;
using MudBlazor;
using judo_univ_rennes.Contracts;
using judo_univ_rennes.Dtos.User;

namespace judo_univ_rennes.Components
{
    public partial class ForgetPassword
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
        /// vérification de login form, si c'est bon success = true;
        /// </summary>
        private bool success;
        /// <summary>
        /// processus de login, qui permet de lancer animation loading
        /// </summary>
        private bool busy { get; set; } = false;
        /// <summary>
        /// 
        /// </summary>
        string[] errors = { };
        UserEmailDto userDto { get; set; }=new UserEmailDto();
        MudForm form;

        #endregion

        #region Methods

        private async Task HandleForget()
        {
            var result=await _authRepo.ForgotPassword(userDto);
            StateHasChanged();
           
            Snackbar.Add("Un lien de récupération de mot de passe a été envoyé par mail");

        }

        #endregion
    }
}
