namespace NEWS.Infrastructure.Data.Repo
{
    public interface IRepository<T>
       where T : class
    {
        IQueryable<T> All();

        Task AddAsync(T entity);

        Task<T> GetByIdAsync(int id);

        Task AddRangeAsync(IEnumerable<T> entities);

        void Update(T entity);

        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entities);

        Task<int> SaveChangesAsync();
    }
}
