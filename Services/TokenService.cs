using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using judo_univ_rennes.Contracts;
using judo_univ_rennes.Data;
using judo_univ_rennes.Statics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace judo_univ_rennes.Services
{
    public class TokenService : ITokenRepo
    {

        private readonly ILogger<TokenService> logger;
        private readonly IMapper mapper;
        private readonly UserManager<ApiUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IEmailSender emailSender;
        private readonly IMemoryCache memoryCache;

        public TokenService(
            ILogger<TokenService> logger,
            UserManager<ApiUser> userManager,
            IMapper mapper,
            IConfiguration configuration,
            IMemoryCache memoryCache,
            IEmailSender emailSender)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.userManager = userManager;
            this.configuration = configuration;
            this.emailSender = emailSender;
            this.memoryCache = memoryCache;
        }
        public async Task<string> GenerateToken(ApiUser user, bool thirdParty, string? imageLink)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var roles = await userManager.GetRolesAsync(user);

            var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();
            var userClaims = await userManager.GetClaimsAsync(user);
            //var isClient = roles.Any(o => o.Contains("Client"));
            var isAdmin = roles.Any(o => o.Contains("Admin"));
            var isSA = roles.Any(o => o.Contains("SuperAdmin"));
            var isThirdParty = thirdParty;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(CustomClaimTypes.IsAdmin, isAdmin.ToString()),
                new Claim(CustomClaimTypes.ThirdParty, isThirdParty.ToString()),
                new Claim(CustomClaimTypes.Uid, user.Id),
                new Claim(CustomClaimTypes.ImageLink, imageLink)
            }.Union(userClaims).Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToInt32(configuration["JwtSettings:Duration"])),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
