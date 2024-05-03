using System.ComponentModel;
using System.Data.SqlClient;
using ScheduSquad.Models;
using ScheduSquad.DataAccess;

namespace ScheduSquad.DataAccess
{
    public class SquadRepository : IRepository<Squad>, ISquadMemberRepository
    {
        private readonly IDbConfiguration _dbConfiguration;

        private readonly IRepository<Member> _memberRep;

        public SquadRepository(IDbConfiguration dbConfiguration, IRepository<Member> memberRepo)
        {
            _dbConfiguration = dbConfiguration;
            _memberRep = memberRepo;
        }

        public string Test()
        {
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

        public void RemoveMemberFromSquad(Guid memberId, Guid squadId)
        {
            // using connection
            using (SqlConnection connection = (SqlConnection)_dbConfiguration.GetDbConnection())
            {
                // Open connection
                connection.Open();
                // Create & using command
                using (SqlCommand command = new SqlCommand("Delete_Member_from_Squad", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    // Set a parameter for memberId
                    command.Parameters.Add("@squadId", System.Data.SqlDbType.UniqueIdentifier).Value = squadId;
                    command.Parameters.Add("@userId", System.Data.SqlDbType.UniqueIdentifier).Value = memberId;
                    // Query returns a single value (top 1) single field; executeScalar instead of reader
                    command.ExecuteNonQuery();
                }
            } // Connection should close on dispose

        }

         public void AddMemberToSquad(Guid memberId, Guid squadId, bool isSquadMaster)
        {
            SqlCommand cmd = new SqlCommand("Add_Member_to_Squad");
            cmd.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
            cmd.Parameters.Add("@SquadId", System.Data.SqlDbType.UniqueIdentifier).Value = squadId;
            cmd.Parameters.Add("@UserId", System.Data.SqlDbType.UniqueIdentifier).Value = memberId;
            cmd.Parameters.Add("@IsSquadMaster", System.Data.SqlDbType.Bit).Value = isSquadMaster;

            ExecuteLogic(cmd);

        }



        public void ExecuteLogic(SqlCommand cmd)
        {
            using (SqlConnection con = new SqlConnection(_dbConfiguration.GetConnectionString()))
            {

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();
            }
        }

        public Squad ExecuteGetSquad(SqlCommand cmd)
        {

            var squad = new Squad();

            using (SqlConnection con = new SqlConnection(_dbConfiguration.GetConnectionString()))
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

            using (SqlConnection con = new SqlConnection(_dbConfiguration.GetConnectionString()))
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
            Member sqdmaster = _memberRep.GetById(new Guid(rdr["SquadMasterId"].ToString() ?? string.Empty));

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

    public interface ISquadMemberRepository
    {
        public void AddMemberToSquad(Guid memberId, Guid squadId, bool isSquadMaster);
        public void RemoveMemberFromSquad(Guid memberId, Guid squadId);
    }
}