

namespace judo_univ_rennes.Contracts
{
	public interface INewsRepo : IBaseCRUD<News>
    {
        Task CallUpdate();
    }
}

