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
        public Post PostToUpdate = new();
        #endregion

        #region Parameters
        [Parameter]  public int PostId { get; set; }
        #endregion



        #region Methods


        protected override async Task OnParametersSetAsync()
        {
            AuthenticationState state = await _authProvider.GetAuthenticationStateAsync();
            var user = state.User;
            PostToUpdate.ApiUserId = user.Claims.FirstOrDefault(s => s.Type == "uid").Value;

            PostToUpdate = await _postRepo.FindById(PostId);

            await richTextEditRef.SetHtmlAsync(PostToUpdate.Content);

            DisplayContent = (MarkupString)contentAsHtml;


            base.OnParametersSetAsync();
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

            PostToUpdate.Content = savedContent;
    
            PostToUpdate.UpdatedOn = DateTime.UtcNow;
        

            var ok = await _postRepo.Update(PostToUpdate);


            if (ok)
            {

                await richTextEditRef.ClearAsync();
            }
        }
        #endregion
    }
}

