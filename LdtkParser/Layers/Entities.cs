using LdtkParser.Exceptions;
using LdtkParser.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LdtkParser.Layers
{
    public class Entities: ILayer
    {
        public string Name { get; }
        private readonly List<EntityInstance> entityInstances;

        public Entities(string identifier, List<EntityInstance> ei)
        {
            Name = identifier;
            entityInstances = ei;
        }
        public LayerType GetLayerType() => LayerType.Entities;

        public T GetEntity<T>() where T: ILdtkEntity, new()
        {
            try
            {
                return Convert<T>(
                    entityInstances.Where(ei => ei.Identifier.Equals(typeof(T).Name))
                        .First()
                );
            } catch(InvalidOperationException e)
            {
                throw new EntityNotFoundException($"Entity of type '{typeof(T)}' was not found in layer '{Name}'", e);
            }
        }
        // Do I worry about the performance of this?
        // Would I want to do a preloading indexing of EntityInstance by Identifier for easy yes/no lookup. (Seems like an obvious yes but is performance that bad as is?)
        public List<T> GetEntities<T>() where T: ILdtkEntity, new()
        {
           return entityInstances.Where(ei => ei.Identifier.Equals(typeof(T).Name))
                .Select(ei => Convert<T>(ei))
                .ToList();
        }

        private T Convert<T>(EntityInstance ei) where T: ILdtkEntity, new()
        {
            var entity = new T();
            entity.FromLdtk(new EntityModel(ei));
            return entity;
        }
    }
}
