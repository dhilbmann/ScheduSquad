using System.Data.SqlClient;
using System.Security.Claims;
using ScheduSquad.Models;

namespace ScheduSquad.DataAccess
{
    public class MemberRepository : IRepository<Member>, IMembersForSquadRepository
    {
        private IDbConfiguration _dbConfiguration;

        public MemberRepository(IDbConfiguration dbConfiguration)
        {
            _dbConfiguration = dbConfiguration;
        }

        public string Test()
        {
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
            //SqlCommand cmd = new SqlCommand("Get_AllMembers");
            SqlCommand cmd = new SqlCommand(@"SELECT UserPk As Id, FirstName, LastName, Email FROM Users WHERE IsDeleted = 0");
            cmd.CommandType = System.Data.CommandType.Text;
            return ExecuteGetAllMembers(cmd);
        }

        public IEnumerable<Member> GetAllByParentId(Guid id) //Get all members in a Squad
        {
            SqlCommand cmd = new SqlCommand("Get_SquadMembers");
            cmd.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier).Value = id;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            return ExecuteGetAllMembers(cmd);
        }


        public IEnumerable<Member> GetMembersNotInSquad(Guid squadId)
        {
            SqlCommand cmd = new SqlCommand("Get_MembersNotInSquad");
            cmd.Parameters.Add("@SquadId", System.Data.SqlDbType.UniqueIdentifier).Value = squadId;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            return ExecuteGetAllMembers(cmd);
        }

        public Member GetById(Guid id)
        {
            // Running the Get_Membesr proc with a specified Id will return a single member
            SqlCommand cmd = new SqlCommand("Select UserPk As Id, FirstName, LastName, Email From Users WHERE UserPK = @Id");
            cmd.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier).Value = id;
            cmd.CommandType = System.Data.CommandType.Text;
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

        public DateTime GetJoinedDateForSquadMember(Guid memberId, Guid squadId)
        {

            DateTime joinedDate = DateTime.MinValue;

            // using connection
            using (SqlConnection connection = (SqlConnection)_dbConfiguration.GetDbConnection())
            {
                // Open connection
                connection.Open();
                // Create & using command
                using (SqlCommand command = new SqlCommand("Get_JoinedDateForSquadMember", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@MemberId", System.Data.SqlDbType.UniqueIdentifier).Value = memberId;
                    command.Parameters.Add("@SquadId", System.Data.SqlDbType.UniqueIdentifier).Value = squadId;
                    // Query returns a single value (top 1) single field; executeScalar instead of reader
                    try
                    {
                        joinedDate = (DateTime)command.ExecuteScalar();
                    }
                    catch (FormatException ex)
                    {
                        throw new Exception(ex.Message);
                    }

                }
            } // Connection should close on dispose
              // Returns the password, whether it was found in the db or not.
            return joinedDate;
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

        private Member ExecuteGetMember(SqlCommand cmd)
        {

            var member = new Member();

            using (SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=ScheduSquad;Integrated Security=true"))
            {

                cmd.Connection = con;

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
                new Guid(rdr["Id"].ToString()), // Id
                rdr["FirstName"].ToString() ?? string.Empty, //name
                rdr["LastName"].ToString() ?? string.Empty,        //description  
                rdr["Email"].ToString() ?? string.Empty          //location
            );
            return member;
        }
    }

    public interface IMembersForSquadRepository
    {
        public IEnumerable<Member> GetMembersNotInSquad(Guid squadId);

        public DateTime GetJoinedDateForSquadMember(Guid memberId, Guid squadId);
    }
}