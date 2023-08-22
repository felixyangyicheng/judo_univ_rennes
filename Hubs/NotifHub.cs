



namespace judo_univ_rennes.Hubs
{
	public class NotifHub : Hub
    {

        private readonly UserManager<ApiUser> _userManager;
        private readonly JudoDbContext _db;
        private readonly IMapper _mapper;
        string connectionString = "";

        public NotifHub(JudoDbContext db, IConfiguration configuration, IMapper mapper, UserManager<ApiUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _db = db;
            connectionString = configuration.GetConnectionString("Account");

        }
        public override async Task OnConnectedAsync()
        {

            await CommandCallUpdate();
            await CommentCallUpdate();
            await PostCallUpdate();
            await NewsCallUpdate();
            await IndexMarkdownCallUpdate();

            await base.OnConnectedAsync();
        }

        #region Command

        public async Task CommandCallUpdate()
        {
            await using var con = new NpgsqlConnection(connectionString);
            await con.OpenAsync();
            con.Notification += CommandNotificationHelper;
            await using (var cmd = new NpgsqlCommand())
            {
                cmd.CommandText = "LISTEN commandchange;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
            }
            while (true)
            {
                con.Wait();
            }
        }
        private void CommandNotificationHelper(object sender, NpgsqlNotificationEventArgs e)
        {
            //Desérializser les données reçues de PostgreSQL
            var dataPayload = JsonConvert.DeserializeObject<TableCommandInfo>(e.Payload);
            Console.WriteLine("{0}", dataPayload.table + " :: " + dataPayload.action + " :: " + dataPayload.data.ApiUser.Id + " :: " + dataPayload.data.Content);
            FullCommandNotification notification = new FullCommandNotification
            {
                table = dataPayload.table,
                action = dataPayload.action,
                data = _mapper.Map<CommandDto>(dataPayload.data)
            };
            Clients.All.SendAsync("refreshCommand", notification);
            //Envoyer la notification au Client avec SignalR
        }
        #endregion

        #region Comment
        public async Task CommentCallUpdate()
        {
            await using var con = new NpgsqlConnection(connectionString);
            await con.OpenAsync();
            con.Notification += CommentNotificationHelper;
            await using (var cmd = new NpgsqlCommand())
            {
                cmd.CommandText = "LISTEN commentchange;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
            }
            while (true)
            {
                con.Wait();
            }
        }
        private void CommentNotificationHelper(object sender, NpgsqlNotificationEventArgs e)
        {
            //Desérializser les données reçues de PostgreSQL
            var dataPayload = JsonConvert.DeserializeObject<TableCommentInfo>(e.Payload);
            Console.WriteLine("{0}", dataPayload.table + " :: " + dataPayload.action + " :: " + dataPayload.data.ApiUser.Id + " :: " + dataPayload.data.Content);
            FullCommentNotification notification = new FullCommentNotification
            {
                table = dataPayload.table,
                action = dataPayload.action,
                data = _mapper.Map<CommentDto>(dataPayload.data)
            };
            Clients.All.SendAsync("refreshComment", notification);
            //Envoyer la notification au Client avec SignalR
        }
        #endregion

        #region Post
        public async Task PostCallUpdate()
        {
            await using var con = new NpgsqlConnection(connectionString);
            await con.OpenAsync();
            con.Notification += PostNotificationHelper;
            await using (var cmd = new NpgsqlCommand())
            {
                cmd.CommandText = "LISTEN postchange;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
            }
            while (true)
            {
                con.Wait();
            }
        }
        private void PostNotificationHelper(object sender, NpgsqlNotificationEventArgs e)
        {
            //Desérializser les données reçues de PostgreSQL
            var dataPayload = JsonConvert.DeserializeObject<TablePostInfo>(e.Payload);
            Console.WriteLine("{0}", dataPayload.table + " :: " + dataPayload.action + " :: " + dataPayload.data.ApiUser.Id + " :: " + dataPayload.data.Content);
            FullPostNotification notification = new FullPostNotification
            {
                table = dataPayload.table,
                action = dataPayload.action,
                data = _mapper.Map<PostDto>(dataPayload.data)
            };
            Clients.All.SendAsync("refreshPost", notification);
            //Envoyer la notification au Client avec SignalR
        }
        #endregion

        #region News
        public async Task NewsCallUpdate()
        {
            await using var con = new NpgsqlConnection(connectionString);
            await con.OpenAsync();
            con.Notification += NewsNotificationHelper;
            await using (var cmd = new NpgsqlCommand())
            {
                cmd.CommandText = "LISTEN newschange;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
            }
            while (true)
            {
                con.Wait();
            }
        }
        private void NewsNotificationHelper(object sender, NpgsqlNotificationEventArgs e)
        {
            //Desérializser les données reçues de PostgreSQL
            var dataPayload = JsonConvert.DeserializeObject<TableNewsInfo>(e.Payload);
            Console.WriteLine("{0}", dataPayload.table + " :: " + dataPayload.action + " :: " + dataPayload.data.ApiUser.Id + " :: " + dataPayload.data.Content);
            FullNewsNotification notification = new FullNewsNotification
            {
                table = dataPayload.table,
                action = dataPayload.action,
                data = _mapper.Map<NewsDto>(dataPayload.data)
            };
            Clients.All.SendAsync("refreshNews", notification);
            //Envoyer la notification au Client avec SignalR
        }
        #endregion

        #region IndexMardown
        public async Task IndexMarkdownCallUpdate()
        {
            await using var con = new NpgsqlConnection(connectionString);
            await con.OpenAsync();
            con.Notification += IndexMarkdownNotificationHelper;
            await using (var cmd = new NpgsqlCommand())
            {
                cmd.CommandText = "LISTEN indexmarkdownchange;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
            }
            while (true)
            {
                con.Wait();
            }
        }
        private void IndexMarkdownNotificationHelper(object sender, NpgsqlNotificationEventArgs e)
        {
            //Desérializser les données reçues de PostgreSQL
            var dataPayload = JsonConvert.DeserializeObject<TableIndexMarkdownInfo>(e.Payload);
            Console.WriteLine("{0}", dataPayload.table + " :: " + dataPayload.action + " :: " + dataPayload.data.ApiUser.Id + " :: " + dataPayload.data.Content);
            FullIndexMarkdownNotification notification = new FullIndexMarkdownNotification
            {
                table = dataPayload.table,
                action = dataPayload.action,
                data = _mapper.Map<IndexMarkdownDto>(dataPayload.data)
            };
            Clients.All.SendAsync("refreshIndexMarkdown", notification);
            //Envoyer la notification au Client avec SignalR
        }
        #endregion
    }
}

