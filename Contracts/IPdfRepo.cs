

namespace judo_univ_rennes.Contracts
{
    public interface IPdfRepo:IMongoReposBase<PdfModel>
    {
        public Task<PdfModel?> GetByNameAsync(string name);
        //public Task<PdfModel?> GetByProjectNameAndFileNameAsync(string projectname, string filename);
        //public Task<IList<PdfModel?>> GetByProjectNameAndCategoryAsync(string projectname, string category);
        //public Task<IList<PdfModel?>> GetByProjectNameAsync(string projectname);
        //public Task<IList<PdfModel?>> SearchInProjectNameByKeywordAsync(string projectname, string keyword);


    }
}
