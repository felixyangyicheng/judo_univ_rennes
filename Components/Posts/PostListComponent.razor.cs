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

        public List<PostDto> Posts ;
        public PostDto RecentPost = new();
        public PostDto ModifiedPost = new();
        #endregion

        #region Parameters

        #endregion



        #region Methods

        protected override async Task OnInitializedAsync()
        {

            AuthenticationState state = await _authProvider.GetAuthenticationStateAsync();
            var user = state.User;

            var posts = await _postRepo.GetAll();
            Posts = _mapper.Map<List<PostDto>>(posts);
            base.OnInitializedAsync();

        }
        private async Task StartHubConnection()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(_nav.ToAbsoluteUri("/notifhub"), options =>
                    {
                        options.AccessTokenProvider = async () =>
                        {
                            return await _localStorage.GetItemAsync<string>("authToken");
                        };
                    }
                )
                .Build();

            hubConnection.On<FullPostNotification>("refreshPost", async (notifcation) =>
            {


                if (notifcation.action == "INSERT")
                {

                    RecentPost = notifcation.data;
                    Posts.Add(RecentPost);
                    Posts.OrderByDescending(p => p.CreatedOn);

                }

                else if (notifcation.action == "UPDATE")
                {

                        ModifiedPost = notifcation.data;
                        Posts.Remove(Posts.FirstOrDefault(p => p.Id == ModifiedPost.Id));
                        Posts.Add(ModifiedPost);
                        Posts.OrderByDescending(p => p.CreatedOn);
             
                        InvokeAsync(StateHasChanged);
                        ShouldRender();

                }
                InvokeAsync(StateHasChanged);
            });
            await hubConnection.StartAsync();
            Console.WriteLine(hubConnection.State);
        }

        protected override async Task OnParametersSetAsync()
        {
            await StartHubConnection();
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

