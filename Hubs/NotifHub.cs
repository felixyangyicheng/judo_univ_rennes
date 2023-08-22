



namespace judo_univ_rennes.Hubs
{
	public class NotifHub : Hub
    {

        private readonly UserManager<ApiUser> _userManager;
        private readonly JudoDbContext _db;
        
        string connectionString = "";

        public NotifHub(JudoDbContext db, IConfiguration configuration, UserManager<ApiUser> userManager)
        {

            _userManager = userManager;
            _db = db;
            connectionString = configuration.GetConnectionString("Account");

        }
        public override async Task OnConnectedAsync()
        {

            await CommandCallUpdate();

            await base.OnConnectedAsync();
        }
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
                // Attendre les évènements
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
                data = new CommandDto
                {

                }
            };
            // var claims = _httpcontext.HttpContext.User.Claims;

            Clients.All.SendAsync("refreshCommand", notification);

            //Envoyer la notification au Client avec SignalR
        }

    }
}

