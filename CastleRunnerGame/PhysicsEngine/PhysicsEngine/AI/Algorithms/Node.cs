using Microsoft.Xna.Framework;

namespace PhysicsEngine.Artificial_Intelligence
{
    internal class Node
    {
        internal Vector2 _Position { get; set; }
        internal bool _Closed { get; set; } = false;
        internal bool _StartNode { get; set; }
        internal bool _EndNode { get; set; }

        internal Node(Vector2 pos, bool start, bool end)
        {
            _Position = pos;
            _StartNode = start;
            _EndNode = end;
        }
    }
}
