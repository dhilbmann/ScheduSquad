using System.Data.SqlClient;
using ScheduSquad.Models;

namespace ScheduSquad.DataAccess;
public class MemberRepository : IRepository<Member>
{
    private IDbConfiguration _dbConfiguration;

    public MemberRepository(IDbConfiguration dbConfiguration) {
        _dbConfiguration = dbConfiguration;
    }

   public string Test() {
        return "MemberRepository.Test Return String";
    }
    
    public void Add(Member entity)
    {
        SqlCommand cmd = new SqlCommand("Add_Member");
        cmd.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier).Value = entity.Id;
        cmd.Parameters.Add("@FirstName", System.Data.SqlDbType.VarChar).Value = entity.FirstName;
        cmd.Parameters.Add("@LastName", System.Data.SqlDbType.VarChar).Value = entity.LastName;
        cmd.Parameters.Add("@Email", System.Data.SqlDbType.VarChar).Value = entity.Email;
        ExecuteLogic(cmd);
    }

    public void Delete(Member entity)
    {
        SqlCommand cmd = new SqlCommand("Delete_Member");
        cmd.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier).Value = entity.Id;
        ExecuteLogic(cmd);
    }

    public IEnumerable<Member> GetAll()
    {
        SqlCommand cmd = new SqlCommand("Get_AllMembers");
        return ExecuteGetAllMembers(cmd);
    }

    public IEnumerable<Member> GetAllByParentId(Guid id) //Get all members in a Squad
    {
        SqlCommand cmd = new SqlCommand("Get_SquadMembers");
        cmd.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier).Value = id;
        return ExecuteGetAllMembers(cmd);
    }

    public Member GetById(Guid id)
    {
        // Running the Get_Membesr proc with a specified Id will return a single member
        SqlCommand cmd = new SqlCommand("Get_Member");
        cmd.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier).Value = id;
        return ExecuteGetMember(cmd);
    }

    public void Update(Member entity)
    {
        SqlCommand cmd = new SqlCommand("Update_Member");
        cmd.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier).Value = entity.Id;
        cmd.Parameters.Add("@FirstName", System.Data.SqlDbType.VarChar).Value = entity.FirstName;
        cmd.Parameters.Add("@LastName", System.Data.SqlDbType.VarChar).Value = entity.LastName;
        cmd.Parameters.Add("@Email", System.Data.SqlDbType.VarChar).Value = entity.Email;
        ExecuteLogic(cmd);
    }

    private void ExecuteLogic(SqlCommand cmd) 
    {
        using (SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=ScheduSquad;Integrated Security=true"))
        {  
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            
            con.Open();

            cmd.ExecuteNonQuery();

            con.Close();
        }
    }

    private Member ExecuteGetMember(SqlCommand cmd){

        var member = new Member();

        using (SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=ScheduSquad;Integrated Security=true"))
        {  
            
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            
            con.Open();

            SqlDataReader rdr = cmd.ExecuteReader();  
            while (rdr.Read())  
            {
                member = getMemberData(rdr);
            }  

            con.Close();
        }

        return member;
    }

    public IEnumerable<Member> ExecuteGetAllMembers(SqlCommand cmd) 
    {
        List<Member> memberList = new List<Member>();
        
        using (SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=ScheduSquad;Integrated Security=true"))
        {  
            
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            
            con.Open();

            SqlDataReader rdr = cmd.ExecuteReader();  
            while (rdr.Read())  
            {
                var member = getMemberData(rdr);
                
               memberList.Add(member);  
            }  

            con.Close();
        }

        return memberList;
    }

    public Member getMemberData(SqlDataReader rdr)
    {
        Guid g;
        var member = new Member(
            rdr["Id"].ToString(),   //Id
            rdr["FirstName"].ToString() ?? string.Empty,        //name
            rdr["LastName"].ToString() ?? string.Empty,        //description  
            rdr["Email"].ToString() ?? string.Empty          //location

        );

        return member;
    }
 
}
   

