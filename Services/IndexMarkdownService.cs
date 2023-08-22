
namespace judo_univ_rennes.Services
{
	public class IndexMarkdownService:IIndexMarkdownRepo
	{
        private readonly JudoDbContext _db;
        private readonly ILogger<IndexMarkdownService> logger;
        private readonly IMapper mapper;
        private readonly UserManager<ApiUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IEmailSender emailSender;
        private readonly ITokenRepo tokenRepo;
        private readonly IMemoryCache memoryCache;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public IndexMarkdownService(
        JudoDbContext db,
            ILogger<IndexMarkdownService> logger,
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
            _db = db;
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

        public async Task<bool> Create(IndexMarkdown entity)
        {
            await _db.IndexMarkdowns.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(IndexMarkdown entity)
        {
            _db.IndexMarkdowns.Remove(entity);
            return await Save();
        }

        public async Task<IndexMarkdown> FindById(int id)
        {
            return await _db.IndexMarkdowns
            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> isExists(int id)
        {
            return await _db.IndexMarkdowns.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(IndexMarkdown entity)
        {
            _db.IndexMarkdowns.Update(entity);
            return await Save();
        }
    }
}


