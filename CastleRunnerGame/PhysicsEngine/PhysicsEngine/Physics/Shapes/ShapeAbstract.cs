using Microsoft.Xna.Framework;
using System;

namespace PhysicsEngine.Physics.Shapes
{
    class ShapeAbstract
    {
        internal virtual Vector2 GetCentreOfMass() { throw new NotImplementedException(); }

        internal virtual void TransformShape(Vector2 pos) { throw new NotImplementedException(); }

        internal virtual void DebugDraw() { throw new NotImplementedException(); }

        internal virtual Vector2 GetPos() { throw new NotImplementedException(); }
    }
}
