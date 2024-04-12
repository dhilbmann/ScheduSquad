using System.Data.SqlClient;
using ScheduSquad.Models;

namespace ScheduSquad.DataAccess;
public class AvailabilityRepository : IRepository<Availability>
{
    

    public AvailabilityRepository()
    {
      
    }

    public void Add(Availability entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(Availability entity)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Availability> GetAll()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Availability> GetAllByParentId(Guid id)
    {
        throw new NotImplementedException();
    }

    public Availability GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public void Update(Availability entity)
    {
        throw new NotImplementedException();
    }
}
