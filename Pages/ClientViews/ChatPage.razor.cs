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
        private string? userInput;
        private string? messageInput;
        private IJSObjectReference jsModule { get; set; }
        protected override async Task OnInitializedAsync()
        {
            jsModule =await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/ClientViews/ChatPage.razor.js");
            hubConnection = new HubConnectionBuilder()
                .WithUrl(Navigation.ToAbsoluteUri("/chathub")
                , options =>
                {
                    options.AccessTokenProvider = async () =>
                    {
                        return await _localStorage.GetItemAsync<string>("authToken");
                    };
                }
                )
                .Build();

            hubConnection.On<string>("refreshUsername", (un) =>
            {
                userInput = un;
                InvokeAsync(StateHasChanged);
            });
            hubConnection.On<string, string, ChatMessage>("ReceiveMessage", async (user, room,message) =>
            {
                var encodedMsg = message;
                messages.Add(encodedMsg);
                InvokeAsync(StateHasChanged);

                await jsModule.InvokeVoidAsync("scrollToElement", "eleScroll");
            });

            await hubConnection.StartAsync();
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
    }
}

