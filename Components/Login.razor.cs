    using System;
    using System.Reflection;
    using judo_univ_rennes.Contracts;
    using judo_univ_rennes.Data;
    using Microsoft.AspNetCore.Components;
    using MudBlazor;
using judo_univ_rennes.Dtos.User;

using judo_univ_rennes.Statics;

namespace judo_univ_rennes.Components
{


        public partial class Login
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

            [Inject]
            public IConfiguration configuration { get; set; }
        #endregion

        #region Properties

        /// <summary>
        /// LoginModel
        /// </summary>
        public UserLoginDto loginModel { get; set; } = new UserLoginDto();
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

            MudForm form;
        /// <summary>
        /// Creadential google status
        /// </summary>

        public CredentialWithPhoto usercredential { get; set; } = new CredentialWithPhoto();



        public string Password { get; set; } = "";

            bool isShow;
            InputType PasswordInput = InputType.Password;
            string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
        public string YourClientId { get; set; }

        #endregion

        #region Methods

        protected override async Task OnInitializedAsync()
        {
            YourClientId = configuration["Authentication:Google:ClientId"];
            Console.WriteLine(YourClientId);
            await base.OnInitializedAsync();
        }
        protected async Task HandleLogin()
        {
            busy = true;
            var response = await _authRepo.Login(loginModel);

            if (response)
            {
                _navManager.NavigateTo("index");
                InvokeAsync(StateHasChanged);


            }
            else
            {
                Snackbar.Add("échec d'authentification", Severity.Error);
            }
            busy = false;
        }
        void ChangePwdVisibility()
        {
            if (isShow)
            {
                isShow = false;
                PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInput = InputType.Password;
            }
            else
            {
                isShow = true;
                PasswordInputIcon = Icons.Material.Filled.Visibility;
                PasswordInput = InputType.Text;
            }
        }
        protected async Task ContinuerWithGoole()
        {
            busy = true;
            var result = await _authRepo.GoogleAuth(usercredential);
            if (result)
            {

                _navManager.NavigateTo("index");
            }
            else
            {
                busy = false;
                InvokeAsync(StateHasChanged);
            }

        }
        #endregion
    }
    
}
