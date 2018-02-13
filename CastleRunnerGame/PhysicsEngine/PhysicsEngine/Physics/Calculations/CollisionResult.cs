using Microsoft.Xna.Framework;

namespace PhysicsEngine.Physics.Calculations
{
    class CollisionResult
    {
        internal bool _CollisionDetected = true;
        internal Vector2 _MinimumTranslationVectorA;
        internal Vector2 _MinimumTranslationVectorB;
    }
}
