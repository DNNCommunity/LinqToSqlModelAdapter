using System;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Xml.Linq;

namespace Dnn.LinqToSqlModelAdapter
{
    static class ModelExtensions
    {
        private static XNamespace Namespace = "http://schemas.microsoft.com/linqtosql/mapping/2007";

        public static XElement Adapt(this MetaModel model, IModelAdapter adapter)
        {
            return new XElement(Namespace + "Database",
                new XAttribute("Name", adapter.AdaptDatabase(model.DatabaseName)),
                new XAttribute("Provider", model.ProviderType.AssemblyQualifiedName),
                model.GetTables().Select(table => table.Adapt(adapter)),
                model.GetFunctions().Select(function => function.Adapt(adapter))
                );
        }

        #region Table Adapters
        public static XElement Adapt(this MetaTable table, IModelAdapter adapter)
        {
            return new XElement(Namespace + "Table",
                new XAttribute("Name", adapter.AdaptTable(table.TableName)),
                new XElement(Namespace + "Type",
                    new XAttribute("Name", table.RowType.Name),
                    table.RowType.DataMembers
                        .Where(member => member.StorageMember != null)
                        .Select(member => member.Adapt())
                    )
                );
        }

        public static XElement Adapt(this MetaDataMember dataMember)
        {
            if (dataMember.IsAssociation)
                return dataMember.AdaptAsAssociation();
            else
                return dataMember.AdaptAsColumn();
        }

        private static XElement AdaptAsColumn(this MetaDataMember column)
        {
            return new XElement(Namespace + "Column",
                new XAttribute("Name", column.Name),
                new XAttribute("Member", column.Member.Name),
                new XAttribute("Storage", column.StorageMember.Name),
                new XAttribute("DbType", column.DbType),
                new XAttribute("IsPrimaryKey", column.IsPrimaryKey.ToString()),
                new XAttribute("IsDbGenerated", column.IsDbGenerated.ToString()),
                new XAttribute("CanBeNull", column.CanBeNull.ToString()),
                new XAttribute("UpdateCheck", column.UpdateCheck.ToString()),
                new XAttribute("IsDiscriminator", column.IsDiscriminator.ToString()),
                new XAttribute("Expression", column.Expression ?? string.Empty),
                new XAttribute("IsVersion", column.IsVersion.ToString()),
                new XAttribute("AutoSync", column.AutoSync.ToString())
                );
        }

        private static XElement AdaptAsAssociation(this MetaDataMember associationMember)
        {
            return new XElement(Namespace + "Association",
                new XAttribute("Name", associationMember.Name),
                new XAttribute("Member", associationMember.Member.Name),
                new XAttribute("Storage", associationMember.StorageMember.Name),
                new XAttribute("ThisKey", string.Join(" ", associationMember.Association.ThisKey.Select(key => key.Name).ToArray())),
                new XAttribute("OtherKey", string.Join(" ", associationMember.Association.OtherKey.Select(key => key.Name).ToArray())),
                new XAttribute("IsForeignKey", associationMember.Association.IsForeignKey.ToString()),
                new XAttribute("IsUnique", associationMember.Association.IsUnique.ToString()),
                new XAttribute("DeleteRule", associationMember.Association.DeleteRule ?? string.Empty),
                new XAttribute("DeleteOnNull", associationMember.Association.DeleteOnNull.ToString())
                );
        }
        #endregion

        #region Function Adapters
        public static XElement Adapt(this MetaFunction function, IModelAdapter adapter)
        {
            return new XElement(Namespace + "Function",
                new XAttribute("Name", function.Name),
                new XAttribute("Method", adapter.AdaptFunction(function.Method.Name)),
                new XAttribute("IsComposable", function.IsComposable.ToString()),
                function.Parameters.Select(parameter => parameter.Adapt()),
                (function.ReturnParameter == null ?
                    (object)function.ResultRowTypes.Select(resultType => resultType.Adapt()) :
                    function.ReturnParameter.AdaptAsReturnType())
                );
        }

        public static XElement Adapt(this MetaParameter parameter)
        {
            return new XElement(Namespace + "Parameter",
                new XAttribute("Name", parameter.Name),
                new XAttribute("Direction", (parameter.ParameterType.IsByRef ? "InOut" : "In")),
                new XAttribute("Parameter", parameter.ParameterType.NonNullableType()),
                new XAttribute("DbType", parameter.DbType)
                );
        }

        public static XElement AdaptAsReturnType(this MetaParameter parameter)
        {
            return new XElement(Namespace + "Return",
                new XAttribute("DbType", Type.GetTypeCode(parameter.ParameterType.NonNullableType()).ToString())
                );
        }

        public static XElement Adapt(this MetaType resultType)
        {
            return new XElement(Namespace + "ElementType",
                new XAttribute("Name", resultType.Name),
                resultType.DataMembers
                    .Where(member => !member.IsAssociation)
                    .Select(member => member.Adapt())
                );
        }
        #endregion

        #region Support Methods
        private static Type NonNullableType(this Type type)
        {
            if (type.IsByRef)
                return NonNullableType(type.GetElementType());
            else
                return type.IsNullable() ? type.GetGenericArguments().First() : type;
        }

        private static bool IsNullable(this Type type)
        {
            return (((type != null) && type.IsGenericType) && (type.GetGenericTypeDefinition() == typeof(Nullable<>)));
        }
        #endregion
    }
}
