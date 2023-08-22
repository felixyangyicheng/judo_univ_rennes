
namespace judo_univ_rennes.Contracts
{
    public interface ICommandRepo:IBaseCRUD<Command>
    {
        Task CallUpdate();
    }
}
