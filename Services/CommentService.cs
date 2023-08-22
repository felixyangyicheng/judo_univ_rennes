
namespace judo_univ_rennes.Services
{
    public class CommentService : ICommentRepo
    {
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
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(Comment entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Comment> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> isExists(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Save()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(Comment entity)
        {
            throw new NotImplementedException();
        }
    }
}
