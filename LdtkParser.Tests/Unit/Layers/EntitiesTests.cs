using LdtkParser.Exceptions;
using LdtkParser.Json;
using LdtkParser.Layers;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

namespace LdtkParser.Tests.Unit.Layers
{
    internal class FakeEntity : ILdtkEntity
    {
        public void FromLdtk(EntityModel entityModel)
        {

        }
    }

    internal class FakeEntityTwo : ILdtkEntity
    {
        public void FromLdtk(EntityModel entityModel)
        {

        }
    }

    class EntitiesTests
    {
        [TestCaseSource(typeof(EntityInstanceData), nameof(EntityInstanceData.GetEntityParams))]
        public void GetEntity_Exist_ReturnsEntity(Entities entities)
        {
            var myEntity = entities.GetEntity<FakeEntity>();

            Assert.IsInstanceOf(typeof(FakeEntity), myEntity);
        }

        [TestCaseSource(typeof(EntityInstanceData), nameof(EntityInstanceData.GetEntityParams))]
        public void GetEntity_NonExistant_ThrowsException(Entities entities)
        {
            Assert.Throws<EntityNotFoundException>(() => entities.GetEntity<FakeEntityTwo>());
        }

        [TestCaseSource(typeof(EntityInstanceData), nameof(EntityInstanceData.GetEntitiesParams))]
        public void GetEntities_All(Entities entities, int expectedCount)
        {
            var found = entities.GetEntities<FakeEntity>();

            Assert.AreEqual(expectedCount, found.Count);
        }

        [TestCaseSource(typeof(EntityInstanceData), nameof(EntityInstanceData.GetEntitiesParams))]
        public void GetEntities_NonExistant_ReturnsNothing(Entities entities, int expectedCount)
        {
            var found = entities.GetEntities<FakeEntityTwo>();

            Assert.Zero(found.Count);
        }
    }

    class EntityInstanceData
    {
        public static IEnumerable GetEntitiesParams
        {
            get
            {
                yield return new object[] {
                    new Entities("TestLayer", GetInstances(GetEntityNames("FakeEntity"))),
                    1
                };
                yield return new object[] {
                    new Entities("TestLayer", GetInstances(GetEntityNames("FakeEntity","FakeEntity"))),
                    2
                };
                yield return new object[] {
                    new Entities("TestLayer", GetInstances(GetEntityNames("DoesNotMatch", "FakeFakeEntity"))),
                    0
                };
            }
        }

        public static IEnumerable GetEntityParams
        {
            get
            {
                yield return new Entities("TestLayer", GetInstances(GetEntityNames("FakeEntity")));
                yield return new Entities("TestLayer", GetInstances(GetEntityNames("FakeEntity", "FakeEntity")));
            }
        }

        private static Queue<string> GetEntityNames(params string[] list) => new Queue<string>(list);

        private static List<EntityInstance> GetInstances(Queue<string> identifiers)
        {
            var instances = new List<EntityInstance>();
            while (identifiers.TryDequeue(out string instanceName))
            {
                instances.Add(GetEntityInstance(instanceName));
            }
            return instances;
        }

        private static EntityInstance GetEntityInstance(string instanceIdentifier)
        {
            return new EntityInstance
            {
                Identifier = instanceIdentifier,
                Px = new List<long>() { 0, 0 },
                Grid = new List<long>() { 0, 0 },
                Pivot = new List<double>() { 0, 0 },
                Tile = null,
                FieldInstances = new List<FieldInstance>()
            };
        }
    }
}
