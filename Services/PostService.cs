using AutoMapper;
using Blazored.LocalStorage;
using judo_univ_rennes.Contracts;
using judo_univ_rennes.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace judo_univ_rennes.Services
{
    public class PostService:IPostRepo
    {
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

        public Task CallUpdate()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Create(Post entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Post entity)
        {
            throw new NotImplementedException();
        }

        public Task<Post> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> isExists(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Save()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Post entity)
        {
            throw new NotImplementedException();
        }
    }
}
