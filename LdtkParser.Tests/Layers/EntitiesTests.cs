using LdtkParser.Json;
using LdtkParser.Layers;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

namespace LdtkParser.Tests.Layers
{
    internal class FakeEntity : ILdtkEntity
    {
        public void FromLdtk(EntityModel entityModel)
        {

        }
    }

    [TestFixtureSource(typeof(EntityInstanceData), nameof(EntityInstanceData.Fixtureparams))]
    class EntitiesTests
    {
        private Entities entities;

        public EntitiesTests(List<EntityInstance> entityInstances)
        {
            entities = new Entities("TestLayer", entityInstances);
        }

        [Test]
        public void GetEntity_WithOne_ReturnsEntity()
        {
            var myEntity = entities.GetEntity<FakeEntity>();

            Assert.IsInstanceOf(typeof(FakeEntity), myEntity);
        }
    }

    class EntityInstanceData
    {
        public static IEnumerable Fixtureparams
        {
            get
            {
                yield return new TestFixtureData(GetInstances(GetEntityNames("FakeEntity")));
            }
        }

        private static Queue<string> GetEntityNames(params string[] list) => new Queue<string>(list);

        private static List<EntityInstance> GetInstances(Queue<string> identifiers)
        {
            var instances = new List<EntityInstance>();
            string instanceName;
            while (identifiers.TryDequeue(out instanceName))
            {
                instances.Add(GetEntityInstance(instanceName));
            }
            return instances;
        }

        private static EntityInstance GetEntityInstance(string instanceIdentifier)
        {
            var ei = new EntityInstance();
            ei.Identifier = instanceIdentifier;
            ei.Px = new List<long>() { 0, 0 };
            ei.Grid = new List<long>() { 0, 0 };
            ei.Pivot = new List<double>() { 0, 0 };
            ei.Tile = null;
            ei.FieldInstances = new List<FieldInstance>();
            return ei;
        }
    }
}
