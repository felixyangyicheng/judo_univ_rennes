using System;
using Google.Apis.Drive.v3.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.SignalR;
using judo_univ_rennes.Data;

using judo_univ_rennes.Contracts;
using MongoDB.Driver.Core.Connections;

namespace judo_univ_rennes.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatRoomService _chatRoomService;

        public ChatHub(IChatRoomService chatRoomService)
        {
            _chatRoomService = chatRoomService;
        }

        public override async Task OnConnectedAsync()
        {
            var roomId = await _chatRoomService.CreateRoom(Context.ConnectionId);

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());

            Console.WriteLine("room Id: "+roomId);
            Console.WriteLine("User ID: "+ Context.ConnectionId);

            await Clients.Caller.SendAsync(
                "ReceiveMessage",
                "Foo",
                DateTimeOffset.UtcNow,
                "bar");

            await base.OnConnectedAsync();
        }

        public async Task SendMessage(string name, string text)
        {
            var roomId = await _chatRoomService.GetRoomForConnectionId(Context.ConnectionId);
            var userId = Context.ConnectionId;
            var message = new ChatMessage
            {
                SenderName = name,
                Text = text,
                SentAt = DateTimeOffset.UtcNow
            };

            // Broadcast to all clients
            await Clients.Group(roomId.ToString()).SendAsync(
                "ReceiveMessage",
                userId,
                roomId,
                message
                );

            //await Clients.All.SendAsync("ReceiveMessage", message.SenderName,
            //    roomId,
            //    message.SentAt,
            //    message.Text);
        }

        //public async Task SendMessage(string user, string message)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", user, message);
        //}

        public  Task SendPrivateMessage(string user, string message)
        {
            return  Clients.User(user).SendAsync("ReceiveMessage", message);
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
        }
    }
}

