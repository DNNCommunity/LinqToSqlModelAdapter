using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using DotNetNuke.Common.Utilities;

namespace Dnn.LinqToSqlModelAdapter
{
    public class DotNetNukeAdapter : IModelAdapter
    {
        private const string dboPattern = @"^(?:[^\.]+\.)?";

        #region IModelAdapter Members

        public string AdaptDatabase(string databaseName)
        {
            return databaseName;
        }

        public string AdaptTable(string tableName)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}",
                Config.GetDataBaseOwner(),
                Config.GetObjectQualifer(),
                Regex.Replace(tableName, dboPattern, string.Empty));
        }

        public string AdaptFunction(string functionName)
        {
            return AdaptTable(functionName);
        }

        public string AdaptConnection(string fileOrServerConnection)
        {
            return Config.GetConnectionString();
        }

        public IDbConnection AdaptConnection(IDbConnection connection)
        {
            return connection;
        }

        #endregion
    }
}
