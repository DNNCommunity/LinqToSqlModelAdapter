using System.Data;

namespace Dnn.LinqToSqlModelAdapter
{
    public interface IModelAdapter
    {
        string AdaptDatabase(string databaseName);
        string AdaptTable(string tableName);
        string AdaptFunction(string functionName);
        string AdaptConnection(string fileOrServerConnection);
        IDbConnection AdaptConnection(IDbConnection connection);
    }
}
