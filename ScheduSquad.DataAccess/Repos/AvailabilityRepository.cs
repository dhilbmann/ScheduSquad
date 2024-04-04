
namespace ScheduSquad.DataAccess;
public class AvailabilityRepository<T> : IRepository<T> where T : class
{
    public void Add(T entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(T entity)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<T> GetAll()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<T> GetAllByParentId(Guid id)
    {
        throw new NotImplementedException();
    }

    public T GetById(Guid id)
    {
        // Set paramater
        // Build command object = query
        // Execute Command object = 
        // Get results
        // Put results on Availability Object
        // Return object

        throw new NotImplementedException();
    }

    public void Update(T entity)
    {
        throw new NotImplementedException();
    }
}
