
namespace judo_univ_rennes.Contracts
{
	public interface IIndexMarkdownRepo : IBaseCRUD<IndexMarkdown>
    {
        Task CallUpdate();
    }
}

