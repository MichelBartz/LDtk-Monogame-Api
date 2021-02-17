
using LdtkParser.Layers;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LdtkParser.Tests.Unit
{
    class LevelTests
    {
        [TestCaseSource(typeof(LayerData), nameof(LayerData.GetLayersParams))]
        public void GetLayers_OfValidType_ReturnsThem(Level level, int expectedNumLayers)
        {
            var intGrids = level.GetLayers<IntGrid>().ToList();
            Assert.AreEqual(expectedNumLayers, intGrids.Count);
        }

        [TestCaseSource(typeof(LayerData), nameof(LayerData.GetLayersParams))]
        public void GetLayerByName_OfValidType_ReturnsIt(Level level, int expectedNumLayers)
        {
            var layer = level.GetLayerByName<IntGrid>("Layer_0");

            Assert.IsInstanceOf(typeof(IntGrid), layer);
        }

        [TestCaseSource(typeof(LayerData), nameof(LayerData.GetLayersParams))]
        public void GetLayerByName_NotAvailable_ReturnsNone(Level level, int expectedNumLayers)
        {
            var layer = level.GetLayerByName<Tiles>("Layer_0");

            Assert.IsNull(layer);
        }

        [TestCaseSource(typeof(NeighbourData), nameof(NeighbourData.GetNeighbourParams))]
        public void GetNeighbourByUid_ValidDirection_ReturnsNeighbour(Level level, Direction dir, int expectedUid)
        {
            var neighbourUid = level.GetNeighbourUid(dir);

            Assert.AreEqual(expectedUid, neighbourUid);
        }

        [TestCaseSource(typeof(NeighbourData), nameof(NeighbourData.GetNeighbourParams))]
        public void GetNeighbourByUid_InvalidDirection_WillSee(Level level, Direction dir, int expectedUid)
        {
            var neighbourUid = level.GetNeighbourUid(Direction.East);

            Assert.AreEqual(-1, neighbourUid);
        }
    }

    internal class NeighbourData
    {
        public static IEnumerable GetNeighbourParams
        {
            get
            {
                yield return new object[] {
                    GetLevelWithNeighbour("Level_1", new (int, string)[]{ (2, "n"), (4, "s"), (5, "w") }),
                    Direction.West,
                    5
                };
                yield return new object[] {
                    GetLevelWithNeighbour("Level_1", new (int, string)[] { (2, "n"), (4, "s"), (5, "w") }),
                    Direction.South,
                    4
                };
            }
        }

        private static Level GetLevelWithNeighbour(string name, (int,string)[] neighbours)
        {
            var level = new Level(1, name, 0, 0);
            foreach((int uid, string dir) neighbour in neighbours)
            {
                level.AddNeighbour(neighbour.uid, neighbour.dir);
            }
            return level;
        }
    }


    internal class LayerData
    {
        public static IEnumerable GetLayersParams
        {
            get
            {
                yield return new object[] {
                    GetLevelWithLayers("Level_1", new Type[1] { typeof(IntGrid) }),
                    1
                };
                yield return new object[] {
                    GetLevelWithLayers("Level_1", new Type[2] { typeof(IntGrid), typeof(IntGrid) }),
                    2
                };
                yield return new object[] {
                    GetLevelWithLayers("Level_1", new Type[3] { typeof(IntGrid), typeof(IntGrid), typeof(Entities)}),
                    2
                };
            }
        }

        private static Level GetLevelWithLayers(string name, Type[] layerTypes)
        {
            var level = new Level(1, name, 0, 0);

            int layerNum = 0;
            foreach (Type t in layerTypes)
            {
                if (t.Name == typeof(IntGrid).Name)
                {
                    level.AddLayer(new IntGrid($"Layer_{layerNum}"));
                }
                else if(t.Name == typeof(Entities).Name)
                {
                    level.AddLayer(new Entities($"Layer_{layerNum}", new List<Json.EntityInstance>()));
                }
                layerNum++;
            }
            return level;
        }
    }
}
