using System.Data.SqlClient;

public interface IDbConfiguration {
    SqlConnection GetConnection();
    SqlCommand GetCommand();

}






