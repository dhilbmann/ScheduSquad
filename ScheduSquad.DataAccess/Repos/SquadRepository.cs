using System.ComponentModel;
using System.Data.SqlClient;
using ScheduSquad.Models;

namespace ScheduSquad.DataAccess;
public class SquadRepository : IRepository<Squad>
{
    private IDbConfiguration _dbConfiguration;

    private MemberRepository memberRep;

    public SquadRepository(IDbConfiguration dbConfiguration) {
        _dbConfiguration = dbConfiguration;
        memberRep = new MemberRepository(dbConfiguration);
    }

   public string Test() {
        return "SquadRepository.Test Return String";
    }

    public void Add(Squad entity)
    {
        SqlCommand cmd = new SqlCommand("Add_Squad");
        cmd.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier).Value = entity.Id;
        cmd.Parameters.Add("@SquadName", System.Data.SqlDbType.VarChar).Value = entity.Name;
        cmd.Parameters.Add("@SquadDesc", System.Data.SqlDbType.VarChar).Value = entity.Description;
        cmd.Parameters.Add("@SquadLocation", System.Data.SqlDbType.VarChar).Value = entity.Location;
        ExecuteLogic(cmd);
    }

    public void Delete(Squad entity)
    {
        SqlCommand cmd = new SqlCommand("Delete_Squad");
        cmd.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier).Value = entity.Id;
        ExecuteLogic(cmd);
    }

    public IEnumerable<Squad> GetAll()
    {
        SqlCommand cmd = new SqlCommand("Get_Squads");
        return ExecuteGetAllSquads(cmd);
    }

    public IEnumerable<Squad> GetAllByParentId(Guid id)
    {
        SqlCommand cmd = new SqlCommand("Get_SquadsByMember");
        cmd.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier).Value = id;
        return ExecuteGetAllSquads(cmd);
    }

    public Squad GetById(Guid id)
    {
        SqlCommand cmd = new SqlCommand("Get_Squad");
        cmd.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier).Value = id;
        return ExecuteGetSquad(cmd);
    }

    public void Update(Squad entity)
    {
        SqlCommand cmd = new SqlCommand("Update_Squad");
        cmd.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier).Value = entity.Id;
        cmd.Parameters.Add("@SquadName", System.Data.SqlDbType.VarChar).Value = entity.Name;
        cmd.Parameters.Add("@SquadDesc", System.Data.SqlDbType.VarChar).Value = entity.Description;
        cmd.Parameters.Add("@SquadLocation", System.Data.SqlDbType.VarChar).Value = entity.Location;
        ExecuteLogic(cmd);
    }

    public void ExecuteLogic(SqlCommand cmd) 
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

    public Squad ExecuteGetSquad(SqlCommand cmd){

        var squad = new Squad();

        using (SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=ScheduSquad;Integrated Security=true"))
        {  
            
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            
            con.Open();

            SqlDataReader rdr = cmd.ExecuteReader();  
            while (rdr.Read())  
            {
                squad = getSquadData(rdr);
            }  

            con.Close();
        }

        return squad;
    }

    public IEnumerable<Squad> ExecuteGetAllSquads(SqlCommand cmd) 
    {
        List<Squad> squadList = new List<Squad>();
        
        using (SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=ScheduSquad;Integrated Security=true"))
        {  
            
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            
            con.Open();

            SqlDataReader rdr = cmd.ExecuteReader();  
            while (rdr.Read())  
            {
                var squad = getSquadData(rdr);
                
               squadList.Add(squad);  
            }  

            con.Close();
        }

        return squadList;
    }

    public Squad getSquadData(SqlDataReader rdr)
    {
        Member sqdmaster = memberRep.GetById(new Guid(rdr["SquadMasterId"].ToString() ?? string.Empty));

        var squad = new Squad(
            new Guid((rdr["Id"]).ToString() ?? string.Empty),   //Id
            sqdmaster,                                          //SquadMaster
            rdr["SquadName"].ToString() ?? string.Empty,        //name
            rdr["SquadDesc"].ToString() ?? string.Empty,        //description  
            rdr["SquadLocation"].ToString() ?? string.Empty          //location
        );

        return squad;
    }
}
