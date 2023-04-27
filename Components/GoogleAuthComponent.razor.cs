using System;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Hosting;
using Microsoft.JSInterop;
using judo_univ_rennes.Statics;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace judo_univ_rennes.Components
{
    public partial class GoogleAuthComponent
    {
        [Inject] IJSRuntime JS { get; set; }
        [Parameter]
        public String ClientId { get; set; }
        [Parameter]
        public bool Hide { get; set; } = false;
        [Parameter]
        public CredentialWithPhoto UserCredential { get; set; }
        private DotNetObjectReference<GoogleAuthComponent> objRef;
        private IJSObjectReference jsModule { get; set; }
        protected override async Task OnParametersSetAsync()
        {
            jsModule = await JS.InvokeAsync<IJSObjectReference>(
                "import", "./Components/GoogleAuthComponent.razor.js");
            objRef = DotNetObjectReference.Create(this);
            await jsModule.InvokeVoidAsync("initGoogle", new object[] { objRef, ClientId });
            await base.OnParametersSetAsync();
        }

        [JSInvokable]
        public async Task SaveCredentials(string mycredential)
        {

            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken decodedValue = handler.ReadJwtToken(mycredential);
            UserCredential.Email = (string)decodedValue.Payload["email"];
            UserCredential.UserId = (string)decodedValue.Payload["sub"];
            UserCredential.Name = (string)decodedValue.Payload["given_name"];
            UserCredential.Photo = (string)decodedValue.Payload["picture"];
            UserCredential.IsLogged = true;
            await UserCredentialChanged.InvokeAsync(UserCredential);
            StateHasChanged();
        }
        //Automatically binded by @bind
        //https://www.syncfusion.com/faq/blazor/components/how-do-you-pass-values-from-child-to-parent-using-eventcallback-in-blazor
        [Parameter]
        public EventCallback<CredentialWithPhoto> UserCredentialChanged { get; set; }
        public void Dispose()
        {
            objRef?.Dispose();
        }
    }
}

