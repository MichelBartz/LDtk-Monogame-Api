
using LdtkParser.Layers;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LdtkParser.Tests
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
            var level = new Level(name, 0, 0);
            int layerNum = 0;
            foreach(Type t in layerTypes)
            {
                if (t.Name == typeof(IntGrid).Name)
                {
                    level.AddLayer(new IntGrid($"Layer_{layerNum}"));
                }
                else if(t.Name == typeof(Entities).Name)
                {
                    level.AddLayer(new Entities($"Layer_{layerNum}", new List<Json.EntityInstance>()));
                }
                else if(t.Name == typeof(Tiles).Name)
                {
                    //Not supported until I can figure out how to get this whole graphics device thingy figured out for unit tests
                }
            }
            return level;
        }
    }
}
