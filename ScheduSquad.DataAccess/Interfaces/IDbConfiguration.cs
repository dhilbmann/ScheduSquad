using System.Data;
using System.Data.SqlClient;

public interface IDbConfiguration : IDisposable {

    IDbConnection Connection { get; }
    
    void OpenConnection();
    void CloseConnection();
    string GetConnectionString();
    IDbConnection GetDbConnection();

    IDbCommand CreateCommand();
    IDbCommand CreateStoredProcedureCommand(string procedureName);
    
    void AddParameter(IDbCommand command, string parameterName, object value);
    
    IDataReader ExecuteReader(IDbCommand command);
    int ExecuteNonQuery(IDbCommand command);

}






