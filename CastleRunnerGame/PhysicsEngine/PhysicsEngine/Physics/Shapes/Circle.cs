using Microsoft.Xna.Framework;
namespace PhysicsEngine.Physics.Shapes
{
    class Circle : ShapeAbstract
    {
        internal Vector2 _Centre { get; set; }
        internal int _Radius { get; set; }

        internal override Vector2 GetCentreOfMass()
        {
            return _Centre;
        }

        internal override void TransformShape(Vector2 pos)
        {
            _Centre += pos;
        }

        internal override void DebugDraw()
        {
            Rectangle rect = new Rectangle((int)_Centre.X - _Radius, (int)_Centre.Y - _Radius, _Radius * 2, _Radius * 2);
            DungeonEscape._Renderer.Draw(DungeonEscape._Textures.GetTexture(TextureType.DebugCircle), rect, Color.Red);
        }

        internal override Vector2 GetPos()
        {
            return _Centre;
        }
    }
}
