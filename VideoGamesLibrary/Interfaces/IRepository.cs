namespace VideoGamesLibrary.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetAsync(int id);

        Task<bool> AddAsync(T item);

        Task<bool> Update(T item);

        Task<bool> Delete(int id);
 
    }
}
