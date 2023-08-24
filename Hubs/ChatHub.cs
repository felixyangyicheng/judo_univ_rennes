

using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.SignalR;


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
            var user =  await _userManager.FindByIdAsync(uid.Value);
          

            await Groups.AddToGroupAsync(Context.ConnectionId, "Online");
            await Groups.AddToGroupAsync(Context.ConnectionId, Context.ConnectionId);

        
            await SendMessage("Chatroom System", $"Bienvenu {user.UserName}"); // Send to all members in chat
            await SendUserName(); // Send Username to current connected user
            await UpdateOnlineUserList();
            await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId}");
            await base.OnConnectedAsync();
        }
        public  async Task OnDisconnectedAsync()
        {
            var userId = Context.ConnectionId;
            var uid = Context.User.Claims.FirstOrDefault(s => s.Type == "uid");
            var user = await _userManager.FindByIdAsync(uid.Value);
         
            ConnectedUsers.onlineUsers.Remove(user);
            await Clients.All.SendAsync(
                "refreshUserlist",
                ConnectedUsers.onlineUsers
                );
            
        }
        private async Task<byte[]?> FindImage()
        {
            var uid = Context.User.Claims.FirstOrDefault(s => s.Type == "uid");
            var user = await _userManager.FindByIdAsync(uid.Value);
            var MyImage = user.ImageData;
            return MyImage;
        }

        public async Task SendMessage(string name, string text)
        {
            var roomId = await _chatRoomService.GetRoomForConnectionId(Context.ConnectionId);
            var userId = Context.ConnectionId;

            var uid = Context.User.Claims.FirstOrDefault(s => s.Type == "uid");
            var user = await _userManager.FindByIdAsync(uid.Value);

            var MyImage =new byte[512000];
    
            var message = new ChatMessage
            {
                SenderName = name,
                Text = text,
                SentAt = DateTimeOffset.UtcNow,
                ImageData=  MyImage
            };

            // Broadcast to all clients
            //await Clients.Group(roomId.ToString()).SendAsync(
            //    "ReceiveMessage",
            //    userId,
            //    roomId,
            //    message
            //    );
            await Clients.All.SendAsync(
                "ReceiveMessage",
                userId,
                roomId,
                message
                );
        }
        public async Task UpdateOnlineUserList()
        {
            var userId = Context.ConnectionId;
            var uid = Context.User.Claims.FirstOrDefault(s => s.Type == "uid");
            var user = await _userManager.FindByIdAsync(uid.Value);
            ConnectedUsers.onlineUsers.Add(user);
            foreach (var u in ConnectedUsers.onlineUsers)
            {
                Console.WriteLine(u.UserName);
            }
            await Clients.All.SendAsync(
                "refreshUserlist",
                ConnectedUsers.onlineUsers
                );

        }
        public async Task SendUserName()
        {
            var userId = Context.ConnectionId;
            var uid = Context.User.Claims.FirstOrDefault(s => s.Type == "uid");
            var user = await _userManager.FindByIdAsync(uid.Value);
            await Clients.Group(userId).SendAsync(
                "refreshUsername",
                user.UserName
                );

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

