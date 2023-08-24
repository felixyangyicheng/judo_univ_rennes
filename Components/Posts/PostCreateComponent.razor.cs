using System;
using MailKit.Search;

namespace judo_univ_rennes.Components.Posts
{
	public partial class PostCreateComponent
	{

        #region DI


        [Inject] IConfiguration _config { get; set; }
        [Inject] IPostRepo _postRepo { get; set; }
        [Inject] ILocalStorageService _localStorage { get; set; }
        [Inject] ISnackbar snackbar { get; set; }
        [Inject] AuthenticationStateProvider _authProvider { get; set; }
        #endregion
        #region Properties
        protected RichTextEdit richTextEditRef;
        protected ClaimsPrincipal user { get; set; }
        protected string uid { get; set; }
        protected bool readOnly;
        protected string contentAsHtml = " ";
        protected MarkupString DisplayContent;
        protected string contentAsDeltaJson;
        protected string contentAsText;
        protected string savedContent;
        public Post PostToAdd = new();
        #endregion

        #region Parameters
        [Parameter]
        public EventCallback<bool> OnPostAdded { get; set; }
        #endregion

        #region Methods



        protected override async Task OnInitializedAsync()
        {
            AuthenticationState state = await _authProvider.GetAuthenticationStateAsync();
            user = state.User;
            uid = user.Claims.FirstOrDefault(s => s.Type == "uid").Value;

            await richTextEditRef.SetHtmlAsync(contentAsHtml);

            DisplayContent = (MarkupString)contentAsHtml;


             base.OnInitializedAsync();
        }
        public async Task OnContentChanged()
        {
            contentAsHtml = await richTextEditRef.GetHtmlAsync();
            DisplayContent = (MarkupString)contentAsHtml;
            contentAsDeltaJson = await richTextEditRef.GetDeltaAsync();
            contentAsText = await richTextEditRef.GetTextAsync();
            savedContent = contentAsHtml;
            StateHasChanged();
        }

        public async Task OnSave()
        {
            savedContent = await richTextEditRef.GetHtmlAsync();
            PostToAdd.ApiUserId = uid;
            PostToAdd.Content = savedContent;
            PostToAdd.CreatedOn = DateTime.UtcNow;
            PostToAdd.UpdatedOn = DateTime.UtcNow;
 

            var ok = await _postRepo.Create(PostToAdd);


            if (ok)
            {

                await richTextEditRef.ClearAsync();
                PostToAdd = new();
                await OnPostAdded.InvokeAsync(ok);
            }
            else
            {
                snackbar.Add("KO", Severity.Error);
            }
        }
        #endregion
    }
}

