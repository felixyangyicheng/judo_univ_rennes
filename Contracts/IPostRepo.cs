
namespace judo_univ_rennes.Contracts
{
    public interface IPostRepo:IBaseCRUD<Post>
    {
        Task<List<Post>> GetAll();
        Task<PagedList<PostDto>> GetAllPaged(BaseItemParameters param);
    }
}
