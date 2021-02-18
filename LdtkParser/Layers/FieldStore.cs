using LdtkParser.Exceptions;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace LdtkParser.Layers
{
    /// <summary>
    /// This is to offer some type safety with the fieldInstances key,values on entities.
    /// </summary>
    public class FieldStore
    {
        private readonly Dictionary<string, int> intStore;
        private readonly Dictionary<string, float> floatStore;
        private readonly Dictionary<string, bool> boolStore;
        private readonly Dictionary<string, string> stringStore;
        private readonly Dictionary<string, Point> pointStore;
        private readonly Dictionary<string, Color> colorStore;
        private readonly Dictionary<string, EnumValue> enumStore;

        public FieldStore()
        {
            intStore = new Dictionary<string, int>();
            floatStore = new Dictionary<string, float>();
            boolStore = new Dictionary<string, bool>();
            stringStore = new Dictionary<string, string>();
            pointStore = new Dictionary<string, Point>();
            colorStore = new Dictionary<string, Color>();
            enumStore = new Dictionary<string, EnumValue>();
        }

        /// <summary>
        /// Stores a fieldInstance <k,v> pair
        /// </summary>
        /// <typeparam name="T">One of int, float, bool, string, Point, Color, EnumValue</typeparam>
        /// <param name="key">The field name</param>
        /// <param name="value">It's value</param>
        public void Add<T>(string key, T value)
        {
            switch (value)
            {
                case int intValue:
                    intStore.Add(key, intValue);
                    break;
                case float floatValue:
                    floatStore.Add(key, floatValue);
                    break;
                case bool boolValue:
                    boolStore.Add(key, boolValue);
                    break;
                case string stringValue:
                    stringStore.Add(key, stringValue);
                    break;
                case Point pointValue:
                    pointStore.Add(key, pointValue);
                    break;
                case Color colorValue:
                    colorStore.Add(key, colorValue);
                    break;
                case EnumValue enumValue:
                    enumStore.Add(key, enumValue);
                    break;
            }
        }

        private T Get<T>(string key, Dictionary<string, T> store)
        {
           if(store.TryGetValue(key, out T value))
           {
               return value;
           }
           throw new FieldStoreException($"Key '{key}' does not exists."); ;
        }

        /// <summary>
        /// Returns an int value from the store
        /// </summary>
        /// <param name="key">The field name</param>
        /// <returns></returns>
        /// <exception cref="LdtkParser.Exceptions.FieldStoreException">Thrown if the key is not found in the given store</exception>
        public int GetInt(string key) => Get(key, intStore);

        /// <summary>
        /// Returns a float value from the store
        /// </summary>
        /// <param name="key">The field name</param>
        /// <returns></returns>
        /// <exception cref="LdtkParser.Exceptions.FieldStoreException">Thrown if the key is not found in the given store</exception>
        public float GetFloat(string key) => Get(key, floatStore);
        /// <summary>
        /// Returns a bool value from the store
        /// </summary>
        /// <param name="key">The field name</param>
        /// <returns></returns>
        /// <exception cref="LdtkParser.Exceptions.FieldStoreException">Thrown if the key is not found in the given store</exception>
        public bool GetBool(string key) => Get(key, boolStore);
        /// <summary>
        /// Returns a string value from the store
        /// </summary>
        /// <param name="key">The field name</param>
        /// <returns></returns>
        /// <exception cref="LdtkParser.Exceptions.FieldStoreException">Thrown if the key is not found in the given store</exception>
        public string GetString(string key) => Get(key, stringStore);
        /// <summary>
        /// Returns a Point value from the store
        /// </summary>
        /// <param name="key">The field name</param>
        /// <returns></returns>
        /// <exception cref="LdtkParser.Exceptions.FieldStoreException">Thrown if the key is not found in the given store</exception>
        public Point GetPoint(string key) => Get(key, pointStore);
        /// <summary>
        /// Returns a Color value from the store
        /// </summary>
        /// <param name="key">The field name</param>
        /// <returns></returns>
        /// <exception cref="LdtkParser.Exceptions.FieldStoreException">Thrown if the key is not found in the given store</exception>
        public Color GetColor(string key) => Get(key, colorStore);
        /// <summary>
        /// Returns an EnumValue value from the store
        /// </summary>
        /// <param name="key">The field name</param>
        /// <returns></returns>
        /// <exception cref="LdtkParser.Exceptions.FieldStoreException">Thrown if the key is not found in the given store</exception>
        public EnumValue GetEnumValue(string key) => Get(key, enumStore);
    }
}
