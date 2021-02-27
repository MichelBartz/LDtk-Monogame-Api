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

        /// <summary>
        /// Returns an entity of type T
        /// </summary>
        /// <typeparam name="T">An entity that implements ILdtkEntity</typeparam>
        /// <returns>The found entity</returns>
        /// <exception cref="LdtkParser.Exceptions.EntityNotFoundException">Thrown if no entity of type T was found</exception>
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
                throw new EntityNotFoundException($"Entity of type '{typeof(T).Name}' was not found in layer '{Name}'", e);
            }
        }

        /// <summary>
        /// Returns all entities of type T
        /// </summary>
        /// <typeparam name="T">An entity that implements ILdtkEntity</typeparam>
        /// <returns>The entities found (can be empty)</returns>
        public List<T> GetEntities<T>() where T: ILdtkEntity, new()
        {
           return entityInstances.Where(ei => ei.Identifier.Equals(typeof(T).Name))
                .Select(ei => Convert<T>(ei))
                .ToList();
        }

        public List<EntityModel> GetEntities() => entityInstances.Select(ei => new EntityModel(ei)).ToList();

        private T Convert<T>(EntityInstance ei) where T: ILdtkEntity, new()
        {
            var entity = new T();
            entity.FromLdtk(new EntityModel(ei));
            return entity;
        }
    }
}
