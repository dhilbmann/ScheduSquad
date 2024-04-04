public interface IRepository<T> {
    T GetById(Guid id);
    IEnumerable<T> GetAll();
    IEnumerable<T> GetAllByParentId(Guid id);
    void Add (T entity);
    void Update (T entity);
    void Delete (T entity);
}