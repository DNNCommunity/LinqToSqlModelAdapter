using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Dnn.LinqToSqlModelAdapter
{
	public class DotNetNukeDataContext : AdaptingDataContext
		{
		public DotNetNukeDataContext(MappingSource mapping)
			: this(DotNetNuke.Common.Utilities.Config.GetConnectionString(), mapping)
			{ }

		public DotNetNukeDataContext(MappingSource mapping, IModelAdapter adapter)
			: this(DotNetNuke.Common.Utilities.Config.GetConnectionString(), mapping, adapter)
			{ }

		public DotNetNukeDataContext(string fileOrServerConnection, MappingSource mapping)
			: base(fileOrServerConnection, mapping, new DotNetNukeAdapter())
			{ }

		public DotNetNukeDataContext(string fileOrServerConnection, MappingSource mapping, IModelAdapter adapter)
			: base(fileOrServerConnection, mapping, adapter)
			{ }

		public DotNetNukeDataContext(IDbConnection connection, MappingSource mapping)
			: base(connection, mapping, new DotNetNukeAdapter())
			{ }

		public DotNetNukeDataContext(IDbConnection connection, MappingSource mapping, IModelAdapter adapter)
			: base(connection, mapping, adapter)
			{ }
		}
	}
