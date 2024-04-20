using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

public class SqlExpressDbConfiguration : IDbConfiguration
{
    private readonly string _connectionString;
    private readonly IDbConnection _connection;

    public SqlExpressDbConfiguration(IConfiguration configuration)
    {
     
        _connectionString = configuration.GetConnectionString("DbConnectionString") ?? throw new InvalidOperationException("Connection string 'DbConnectionString' not found in appsettings.json.");
        _connection = new SqlConnection(_connectionString);
    
    }

    public IDbConnection Connection => _connection;

    public string GetConnectionString()
    {
        return _connectionString;
    }

    public IDbConnection GetDbConnection() {
        return new SqlConnection(GetConnectionString());
    }

    public void OpenConnection()
    {
        if (_connection.State != ConnectionState.Open)
        {
            _connection.Open();
        }
    }

    public void CloseConnection()
    {
        if (_connection.State != ConnectionState.Closed)
        {
            _connection.Close();
        }
    }

    public IDbCommand CreateCommand()
    {
        return _connection.CreateCommand();
    }

    public IDbCommand CreateStoredProcedureCommand(string procedureName)
    {
        var command = CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = procedureName;
        return command;
    }

    public void AddParameter(IDbCommand command, string parameterName, object value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = parameterName;
        parameter.Value = value ?? DBNull.Value;
        command.Parameters.Add(parameter);
    }

    public IDataReader ExecuteReader(IDbCommand command)
    {
        OpenConnection();
        return command.ExecuteReader(CommandBehavior.CloseConnection);
    }

    public int ExecuteNonQuery(IDbCommand command)
    {
        OpenConnection();
        return command.ExecuteNonQuery();
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}