using System.Data.SqlClient;
using ScheduSquad.Models;

namespace ScheduSquad.DataAccess;
public class SquadRepository : IRepository<Squad>
{
    private IDbConfiguration _dbConfiguration;

    public SquadRepository(IDbConfiguration dbConfiguration) {
        _dbConfiguration = dbConfiguration;
    }

    public string Test() {
        return "SquadRepository.Test Return String";
    }

    public void Add(Squad entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(Squad entity)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Squad> GetAll()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Squad> GetAllByParentId(Guid id)
    {
        throw new NotImplementedException();
    }

    public Squad GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public void Update(Squad entity)
    {
        throw new NotImplementedException();
    }


}
