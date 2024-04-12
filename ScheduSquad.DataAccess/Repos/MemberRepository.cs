using System.Data.SqlClient;
using ScheduSquad.Models;

namespace ScheduSquad.DataAccess;
public class MemberRepository : IRepository<Member>
{
    public void Add(Member entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(Member entity)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Member> GetAll()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Member> GetAllByParentId(Guid id)
    {
        throw new NotImplementedException();
    }

    public Member GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public void Update(Member entity)
    {
        throw new NotImplementedException();
    }
}
   

