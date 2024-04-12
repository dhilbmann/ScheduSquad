using System.Data.SqlClient;

public interface IDbConfiguration {
    SqlConnection getConnectionFactory();

}