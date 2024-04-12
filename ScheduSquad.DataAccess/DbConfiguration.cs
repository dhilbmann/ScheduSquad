using System.Data.SqlClient;

public class DbConfiguration : IDbConfiguration
{

    private SqlCommand _command;
    private SqlConnection _connection;
    //private SqlTransaction _transaction;
    //private SqlParameter[] _parameters;

    public DbConfiguration()
    {
        _command = new SqlCommand();
        _connection = new SqlConnection();
        //_transaction = new SqlTransaction();
        // _SqlParameter = new SqlParameter[];
    }

    public SqlConnection getConnectionFactory()
    {
        throw new NotImplementedException();
    }
}