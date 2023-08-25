using System;
using Blazored.LocalStorage;
using Google.Apis.Drive.v3.Data;
using judo_univ_rennes.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

namespace judo_univ_rennes.Pages.ClientViews
{
	public partial class ChatPage
	{
        [Inject] ILocalStorageService _localStorage { get; set; }
        [Inject] IJSRuntime JsRuntime { get; set; }
        private HubConnection? hubConnection;
        private List<ChatMessage> messages = new List<ChatMessage>();
        private List<ApiUser> onlineUsers = new List<ApiUser>();
        private string? userInput;
        private string? messageInput;
        private bool isVisible;
        private bool _started = false;


        private IJSObjectReference jsModule { get; set; }
        protected override async Task OnInitializedAsync()
        {
            jsModule =await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/ClientViews/ChatPage.razor.js");

        }
        private async Task Connect()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(Navigation.ToAbsoluteUri("/chathub"), options =>
                {
                    options.AccessTokenProvider = async () =>
                    {
                        return await _localStorage.GetItemAsync<string>("authToken");
                    };
                }
                )
                .Build();
            if (hubConnection.State == HubConnectionState.Connected)
            {
                Console.WriteLine("connection started");
                await JsRuntime.InvokeVoidAsync("console.log", "connected");

            }
            hubConnection.On<string>("refreshUsername", (un) =>
            {
                userInput = un;
                InvokeAsync(StateHasChanged);
            });

            hubConnection.On<List<ApiUser>>("refreshUserlist", (list) =>
            {

                onlineUsers = list;
                InvokeAsync(StateHasChanged);

            });
            hubConnection.On<string, string, ChatMessage>("ReceiveMessage", async (user, room, message) =>
            {
                var encodedMsg = message;
                messages.Add(encodedMsg);
                InvokeAsync(StateHasChanged);

                await jsModule.InvokeVoidAsync("scrollToElement", "eleScroll");
            });
            _started = true;
            await hubConnection.StartAsync();
        }
        private async Task Disconnect()
        {
            await hubConnection.StopAsync();
            _started = false;
            await hubConnection.DisposeAsync();
            hubConnection = null;
        }

        private async Task Send()
        {
            if (hubConnection is not null)
            {
                await hubConnection.SendAsync("SendMessage", userInput, messageInput);
            }
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


        public void ToggleOverlay(bool value)
        {
            isVisible = value;
        }
    }
}

