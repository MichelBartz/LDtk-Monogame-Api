namespace LdtkParser
{
    internal class Neighbour
    {
        public int Uid { get; }
        public Direction Dir { get; }

        public Neighbour(int uid, string direction)
        {
            Uid = uid;
            Dir = StrDirToDirection(direction);
        }

        private Direction StrDirToDirection(string direction)
        {
            switch(direction)
            {
                case "n": return Direction.North;
                case "e": return Direction.East;
                case "s": return Direction.South;
                case "w": return Direction.West;
            }
            return Direction.Invalid;
        }
    }
}
