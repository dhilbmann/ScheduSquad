using System.Data.SqlClient;
using ScheduSquad.Models;

namespace ScheduSquad.DataAccess;
public class AvailabilityRepository : IRepository<Availability>
{
    private IDbConfiguration _dbConfiguration;
    
    public AvailabilityRepository(IDbConfiguration dbConfiguration) {
        _dbConfiguration = dbConfiguration;
    }

    public string Test() {
        return "AvailabilityRepository.Test Return String";
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
