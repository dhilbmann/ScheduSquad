using System.Data.SqlClient;

public class SqlExpressDbConfiguration : IDbConfiguration
{
    public SqlExpressDbConfiguration()
    {
    }

    public SqlCommand GetCommand()
    {
        throw new NotImplementedException();
    }

    public SqlConnection GetConnection()
    {
        throw new NotImplementedException();
    }


}