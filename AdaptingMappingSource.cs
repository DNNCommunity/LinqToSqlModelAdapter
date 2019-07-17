using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;

namespace Dnn.LinqToSqlModelAdapter
{
    public class AdaptingMappingSource : MappingSource
    {
        public IModelAdapter ModelAdapter
        {
            get; set;
        }
        protected static IDictionary<Type, MappingSource> AdaptedMappingSources
        {
            get; private set;
        }
        protected MappingSource UnderlyingMappingSource
        {
            get; set;
        }

        static AdaptingMappingSource()
        {
            AdaptedMappingSources = new Dictionary<Type, MappingSource>();
        }

        public AdaptingMappingSource(MappingSource mappingSource, IModelAdapter modelAdapter)
            : base()
        {
            if (mappingSource == null)
                throw new ArgumentNullException("mappingSource");
            if (modelAdapter == null)
                throw new ArgumentNullException("modelAdapter");

            UnderlyingMappingSource = mappingSource;
            ModelAdapter = modelAdapter;
        }

        protected override MetaModel CreateModel(Type dataContextType)
        {
            return AdaptMappingSource(dataContextType).GetModel(dataContextType);
        }

        protected virtual MappingSource AdaptMappingSource(Type dataContextType)
        {
            MappingSource adaptedMappingSource;

            if (dataContextType == null)
                throw new ArgumentNullException("dataContextType");

            if (!AdaptedMappingSources.TryGetValue(dataContextType, out adaptedMappingSource))
            {
                adaptedMappingSource = XmlMappingSource
                    .FromXml(UnderlyingMappingSource
                        .GetModel(dataContextType)
                        .Adapt(ModelAdapter)
                        .ToString());

                AdaptedMappingSources.Add(dataContextType, adaptedMappingSource);
            }

            return adaptedMappingSource;
        }
    }
}
