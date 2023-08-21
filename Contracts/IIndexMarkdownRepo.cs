using System;
using judo_univ_rennes.Data;

namespace judo_univ_rennes.Contracts
{
	public interface IIndexMarkdownRepo : IBaseCRUD<IndexMarkdown>
    {
        Task CallUpdate();
    }
}

