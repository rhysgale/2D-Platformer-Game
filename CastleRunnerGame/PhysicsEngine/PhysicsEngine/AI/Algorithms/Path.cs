using PhysicsEngine.Artificial_Intelligence;

namespace PhysicsEngine.AI.Algorithms
{
    class Path
    {
        internal Node _PointA { get; set; }
        internal Node _PointB { get; set; }
        internal bool _JumpPath { get; set; } //whether the ai will need to jump for this path. if so need more calculations and stuffs
        internal int _Weight { get; set; }

        internal Path(Node a, Node b, bool jump)
        {
            _PointA = a;
            _PointB = b;
            _JumpPath = jump;
        }
    }
}
