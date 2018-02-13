using PhysicsEngine.Assets;
using PhysicsEngine.GameDrawing;
using PhysicsEngine.Physics.Shapes;

namespace PhysicsEngine
{
    //Can be a gem, lever, door, etc.
    class Object : DecorationAbstract
    {
        //The bounding box linked to the object.
        internal Body _BoundingBox { get; set; }

        //The link to the texture. 
        internal MapObject _TextureLink { get; set; }
    }
}
