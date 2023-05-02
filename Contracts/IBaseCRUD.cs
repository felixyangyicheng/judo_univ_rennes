namespace judo_univ_rennes.Contracts
{
    public interface IBaseCRUD<T> where T : class
    {
        Task<T> FindById( int id);
        Task<bool> isExists( int id);
        Task<bool> Create( T entity);
        Task<bool> Update( T entity);
        Task<bool> Delete(T entity);
        Task<bool> Save();
    }
}
