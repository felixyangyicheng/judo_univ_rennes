
namespace judo_univ_rennes.Services
{
    public class PostService:IPostRepo
    {
        private readonly JudoDbContext _db;
        private readonly IDbContextFactory<JudoDbContext> _contextFactory;
        private readonly ILogger<PostService> logger;
        private readonly IMapper mapper;
        private readonly UserManager<ApiUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IEmailSender emailSender;
        private readonly ITokenRepo tokenRepo;
        private readonly IMemoryCache memoryCache;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public PostService(
        JudoDbContext db,
            ILogger<PostService> logger,
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

   

        public async Task<bool> Create(Post entity)
        {
            await _db.Posts.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Post entity)
        {
            _db.Posts.Remove(entity);
            return await Save();
        }

        public async Task<Post> FindById(int id)
        {
            return await _db.Posts
                .Include(u => u.ApiUser)
                .Include(u => u.Comments)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Post>> GetAll()
        {


            //using var context = _contextFactory.CreateDbContext();
            //var posts = await context.Posts

            //             .Include(u => u.ApiUser)
            //            .Include(u => u.Comments)
            //            .ThenInclude(c => c.ApiUser)
            //            .ToListAsync();
            //return posts;
            return await _db.Posts
                .AsNoTracking()
                .Include(u => u.ApiUser)
                .Include(u => u.Comments)
                .ThenInclude(c => c.ApiUser)
                .ToListAsync();
        }

        public async Task<PagedList<PostDto>> GetAllPaged(BaseItemParameters param)
        {
            var result = await _db.Posts
                .Include(u => u.ApiUser)
                .Include(u => u.Comments)
                .ThenInclude(c=>c.ApiUser)
                .ToListAsync();

          

            List<PostDto> posts = mapper.Map<List<PostDto>>(result);
            return PagedList<PostDto>.ToPagedList(posts, param.PageNumber, param.PageSize);
        }

        public async Task<bool> isExists(int id)
        {
            return await _db.Posts.AnyAsync(c => c.Id == id);

        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Post entity)
        {
            _db.Posts.Update(entity);
            return await Save();
        }
    }
}
