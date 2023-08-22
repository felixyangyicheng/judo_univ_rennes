
namespace judo_univ_rennes.Services
{
    public class CommandService : ICommandRepo
    {
        private readonly JudoDbContext _db;
        private readonly ILogger<CommandService> logger;
        private readonly IMapper mapper;
        private readonly UserManager<ApiUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IEmailSender emailSender;
        private readonly ITokenRepo tokenRepo;
        private readonly IMemoryCache memoryCache;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public CommandService(
        JudoDbContext db,
        ILogger<CommandService> logger,
            UserManager<ApiUser> userManager,
            IMapper mapper,
            IConfiguration configuration,
            IMemoryCache memoryCache,
            IEmailSender emailSender,
            ITokenRepo tokenRepo,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider
        )
        {
            this._db = db;
            this.logger = logger;
            this.mapper = mapper;
            this.userManager = userManager;
            this.configuration = configuration;
            this.emailSender = emailSender;
            this.tokenRepo = tokenRepo;
            this.memoryCache = memoryCache;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
        }
        public async Task CallUpdate()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Create(Command entity)
        {
            await _db.Commands.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Command entity)
        {
             _db.Commands.Remove(entity);
            return await Save();
        }

        public async Task<Command> FindById(int id)
        {
            return await _db.Commands
                .Include(u=>u.ApiUser)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> isExists(int id)
        {
            return  await _db.Commands.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Command entity)
        {
            _db.Commands.Update(entity);
            return await Save();
        }
    }
}
