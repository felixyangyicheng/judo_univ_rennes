using System;
using Google.Apis.Drive.v3.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.SignalR;
using judo_univ_rennes.Data;

using judo_univ_rennes.Contracts;
using MongoDB.Driver.Core.Connections;
using Microsoft.AspNetCore.Identity;

namespace judo_univ_rennes.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatRoomService _chatRoomService;
        private readonly UserManager<ApiUser> _userManager;
        public ChatHub(IChatRoomService chatRoomService, UserManager<ApiUser> userManager)
        {
            _chatRoomService = chatRoomService;
            _userManager = userManager;
        }

        public override async Task OnConnectedAsync()
        {
            var roomId = await _chatRoomService.CreateRoom(Context.ConnectionId);
            var isAuthenticated = Context.User.Identity.IsAuthenticated;
            var username = Context.User.Claims.FirstOrDefault(s=>s.Type== "sub");
            var email = Context.User.Claims.FirstOrDefault(s=>s.Type== "email");
            var uid = Context.User.Claims.FirstOrDefault(s=>s.Type== "uid");
            var user = await _userManager.FindByIdAsync(uid.Value);

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());

            Console.WriteLine("room Id: "+roomId);
            Console.WriteLine("isAuthenticated: " + isAuthenticated);
            Console.WriteLine("username: " + user.UserName);
            Console.WriteLine("email: " + email);
            Console.WriteLine("uid: " + uid);
            Console.WriteLine("User ID: "+ Context.ConnectionId);

            //foreach (var item in Context.User.Claims)
            //{
            //    Console.WriteLine(item.Type + " " + item.Value);
            //}

            await SendMessage(user.UserName, $"Bienvenu {user.UserName}");

            await Clients.User(Context.ConnectionId).SendAsync("ReceiveId", Context.ConnectionId);


            await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId}");
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
        public async Task SendId()
        {
            var userId = Context.ConnectionId;
            await Clients.All.SendAsync(
                "ReceiveId",
                userId);

        }

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

