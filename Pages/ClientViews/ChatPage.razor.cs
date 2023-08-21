using System;
using judo_univ_rennes.Data;
using Microsoft.AspNetCore.SignalR.Client;

namespace judo_univ_rennes.Pages.ClientViews
{
	public partial class ChatPage
	{
        private HubConnection? hubConnection;
        private List<string> messages = new List<string>();
        private string? userInput;
        private string? messageInput;

        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(Navigation.ToAbsoluteUri("/chathub"))
                .Build();

            hubConnection.On<string, string, ChatMessage>("ReceiveMessage", (user, room,message) =>
            {
                var encodedMsg = $"{user}/{room}/{message.SentAt.ToString("f")}: {message.Text}";
                messages.Add(encodedMsg);
                InvokeAsync(StateHasChanged);
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

