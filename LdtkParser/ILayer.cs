namespace LdtkParser
{
    public interface ILayer
    {
        public string Name { get; }
        public LayerType GetLayerType();
    }
}
