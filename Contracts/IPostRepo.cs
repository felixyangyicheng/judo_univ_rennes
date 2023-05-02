using judo_univ_rennes.Data;

namespace judo_univ_rennes.Contracts
{
    public interface IPostRepo:IBaseCRUD<Post>
    {
        Task CallUpdate();
    }
}
