using System;
using Blazored.LocalStorage;
using Google.Apis.Drive.v3.Data;
using judo_univ_rennes.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace judo_univ_rennes.Components.Posts
{
	public partial class PostListComponent
	{

        #region DI


        [Inject] IConfiguration _config { get; set; }
        [Inject] IPostRepo _postRepo { get; set; }
        [Inject] ILocalStorageService _localStorage { get; set; }
        [Inject] IMapper _mapper { get; set; }
        [Inject] AuthenticationStateProvider _authProvider { get; set; }
        [Inject] NavigationManager _nav { get; set; }
        #endregion
        #region Properties
        private HubConnection? hubConnection;

        public List<PostViewModel> Posts ;
        public PostViewModel RecentPost;
        #endregion

        #region Parameters

        #endregion



        #region Methods

        protected override async Task OnInitializedAsync()
        {

            AuthenticationState state = await _authProvider.GetAuthenticationStateAsync();
            var user = state.User;

            var posts = await _postRepo.GetAll();
            Posts = _mapper.Map<List<PostViewModel>>(posts);
            base.OnParametersSetAsync();

        }


        protected override async Task OnParametersSetAsync()
        {
            hubConnection = new HubConnectionBuilder()
            .WithUrl(_nav.ToAbsoluteUri("/notifhub")
            , options =>
            {
                options.AccessTokenProvider = async () =>
                {
                    return await _localStorage.GetItemAsync<string>("authToken");
                };
            }
            )
            .Build();

            hubConnection.On<FullPostNotification>("refreshPost", (notifcation) =>
            {
                if (notifcation.action== "INSERT")
                {

                    RecentPost = _mapper.Map<PostViewModel>(notifcation.data);
                }
                InvokeAsync(StateHasChanged);
            });

            base.OnParametersSetAsync();
        }
        public bool IsConnected =>
        hubConnection?.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }
        #endregion
    }
}

