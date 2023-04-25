using judo_univ_rennes.Data;

namespace judo_univ_rennes.Contracts
{
    public interface ITokenRepo
    {
       Task<string> GenerateToken(ApiUser user,bool thirdParty, string? imageLink);
    }
}
