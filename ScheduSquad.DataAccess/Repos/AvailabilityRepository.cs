using System.Data.SqlClient;
using ScheduSquad.Models;

namespace ScheduSquad.DataAccess;
public class AvailabilityRepository : IRepository<Availability>
{
    private IDbConfiguration _dbConfiguration;
    

    public AvailabilityRepository(IDbConfiguration dbConfiguration)
    {
       _dbConfiguration = dbConfiguration;
    }

    public void Add(Availability entity, Guid userId)
    {
        SqlCommand cmd = new SqlCommand("Add_Availability");
        cmd.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier).Value = entity.Id;
        cmd.Parameters.Add("@UserId", System.Data.SqlDbType.VarChar).Value = userId;
        cmd.Parameters.Add("@DayEnum", System.Data.SqlDbType.VarChar).Value = entity.DayOfWeek;
        cmd.Parameters.Add("@StartTime", System.Data.SqlDbType.VarChar).Value = entity.StartTime;
        cmd.Parameters.Add("@EndTime", System.Data.SqlDbType.VarChar).Value = entity.EndTime;
        ExecuteLogic(cmd);
    }

    public void Delete(Availability entity)
    {
        SqlCommand cmd = new SqlCommand("Delete_Availability");
        cmd.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier).Value = entity.Id;
        ExecuteLogic(cmd);
    }

    public IEnumerable<Availability> GetAll()
    {
        SqlCommand cmd = new SqlCommand("Get_All_Availability");
        cmd.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier).Value = null;
        return ExecuteGetAllAvailability(cmd);
    }

    public IEnumerable<Availability> GetAllByParentId(Guid id) //ParentId by User
    {
        SqlCommand cmd = new SqlCommand("Get_All_Availability");
        cmd.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier).Value = id;
        return ExecuteGetAllAvailability(cmd);
    }

    public Availability GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public void Update(Availability entity)
    {
        throw new NotImplementedException();
    }

    public void ExecuteLogic(SqlCommand cmd) 
    {
        using (SqlConnection con = new SqlConnection(_dbConfiguration.ToString()))
        {  
            
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            
            con.Open();

            cmd.ExecuteNonQuery();

            con.Close();
        }
    }

    private IEnumerable<Availability> ExecuteGetAllAvailability(SqlCommand cmd)
    {
        IEnumerable<Availability> availabilityList = new List<Availability>();
        
        using (SqlConnection con = new SqlConnection(cs))
        {  
            
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            
            con.Open();

            SqlDataReader rdr = cmd.ExecuteReader();  
            while (rdr.Read())  
            {
                var availability = getAvailabilityData(rdr);
                
               availabilityList.Append(availability);  
            }  

            con.Close();
        }

        return availabilityList;
    }
    public Availability getAvailabilityData(SqlDataReader rdr)
    {
        DayOfWeek dayOfWeek= (DayOfWeek)Convert.ToInt32(rdr["DayEnum"]);

        var availability = new Availability(
            new Guid((rdr["Id"]).ToString() ?? string.Empty),     //Id                                         //SquadMaster
            dayOfWeek,                                           //dayofweek
            (TimeSpan)rdr["StartTime"],                         //starttime  
            (TimeSpan)rdr["EndTime"]                           //endtime
        );

        return availability;
    }
}
