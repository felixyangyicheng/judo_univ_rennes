
namespace judo_univ_rennes.Services
{
    public class CommentService : ICommentRepo
    {
        private readonly JudoDbContext _db;
        private readonly ILogger<CommentService> logger;
        private readonly IMapper mapper;
        private readonly UserManager<ApiUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IEmailSender emailSender;
        private readonly ITokenRepo tokenRepo;
        private readonly IMemoryCache memoryCache;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public CommentService(
                  JudoDbContext db,
            ILogger<CommentService> logger,
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

        public async Task<bool> Create(Comment entity)
        {
            await _db.Comments.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Comment entity)
        {
            _db.Comments.Remove(entity);
            return await Save();
        }

        public async Task<Comment> FindById(int id)
        {
            return await _db.Comments
                 .Include(u => u.ApiUser)
                 .Include(u => u.Post)
                 .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> isExists(int id)
        {
            return await _db.Comments.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Comment entity)
        {
            _db.Comments.Update(entity);
            return await Save();
        }
    }
}
