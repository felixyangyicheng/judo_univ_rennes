

namespace judo_univ_rennes.Contracts
{
    public interface ICommentRepo:IBaseCRUD<Comment>
    {
        Task CallUpdate();
    }
}
