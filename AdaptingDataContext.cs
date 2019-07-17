using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Dnn.LinqToSqlModelAdapter
{
    public class AdaptingDataContext : DataContext
    {
        public AdaptingDataContext(string fileOrServerConnection, MappingSource mapping, IModelAdapter adapter)
            : base(adapter.AdaptConnection(fileOrServerConnection), new AdaptingMappingSource(mapping, adapter))
        { }

        public AdaptingDataContext(IDbConnection connection, MappingSource mapping, IModelAdapter adapter)
            : base(adapter.AdaptConnection(connection), new AdaptingMappingSource(mapping, adapter))
        { }
    }
}
