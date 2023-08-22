using System;
namespace judo_univ_rennes.Components.Posts
{
	public partial class PostUpdateComponent
	{

        #region DI


        [Inject] IConfiguration _config { get; set; }
        [Inject] IPostRepo _postRepo { get; set; }
        [Inject] ILocalStorageService _localStorage { get; set; }
        [Inject] AuthenticationStateProvider _authProvider { get; set; }
        #endregion
        #region Properties
        protected RichTextEdit richTextEditRef;
        protected bool readOnly;
        protected string contentAsHtml = " <p>come On baby</p>";
        protected MarkupString DisplayContent;
        protected string contentAsDeltaJson;
        protected string contentAsText;
        protected string savedContent;
        public Post PostToAdd = new();
        #endregion

        #region Parameters
        [Parameter]  public int PostId { get; set; }
        #endregion



        #region Methods



        protected override async Task OnInitializedAsync()
        {
            AuthenticationState state = await _authProvider.GetAuthenticationStateAsync();
            var user = state.User;
            PostToAdd.ApiUserId = user.Claims.FirstOrDefault(s => s.Type == "uid").Value;

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

            StateHasChanged();
        }

        public async Task OnSave()
        {
            savedContent = await richTextEditRef.GetHtmlAsync();

            PostToAdd.Content = savedContent;
    
            PostToAdd.UpdatedOn = DateTime.UtcNow;
        

            var ok = await _postRepo.Create(PostToAdd);


            if (ok)
            {

                await richTextEditRef.ClearAsync();
            }
        }
        #endregion
    }
}

