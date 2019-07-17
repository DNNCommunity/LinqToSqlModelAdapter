using System.Data;

namespace Dnn.LinqToSqlModelAdapter
{
    public class NonTransformingAdapter : IModelAdapter
    {
        #region IModelAdapter Members

        public string AdaptDatabase(string databaseName)
        {
            return databaseName;
        }

        public string AdaptTable(string tableName)
        {
            return tableName;
        }

        public string AdaptFunction(string functionName)
        {
            return functionName;
        }

        public string AdaptConnection(string fileOrServerConnection)
        {
            return fileOrServerConnection;
        }

        public IDbConnection AdaptConnection(IDbConnection connection)
        {
            return connection;
        }

        #endregion
    }
}
