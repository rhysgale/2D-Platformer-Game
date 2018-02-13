namespace PhysicsEngine.Artificial_Intelligence
{
    class Storage
    {
        internal Node _ParentNode { get; set; }
        internal float _CurrentDistance { get; set; }

        internal Storage(Node parent, float dist)
        {
            _CurrentDistance = dist;
            _ParentNode = parent;
        }
    }
}
