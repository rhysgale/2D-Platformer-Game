using Microsoft.Xna.Framework;

namespace PhysicsEngine.Physics.Shapes
{
    //Class used to store variables for each shape on the map. 
    internal class Body
    {
        Vector2 _Velo;
        internal Vector2 _VelocityCap { get; set; } //Positive x for highest cap, Negative x for lowest cap. Same for Y. 
        internal ShapeAbstract _Shape { get; set; }
        internal float _Mass { get; set; }
        internal float _Friction { get; set; } //amount velocity decreases for object touching the object.
        internal float _Elasticity { get; set; } //1 for perfect elasticity, 0 for not perfect elasticity. 
        internal Vector2 _Acceleration { get; set; }
        internal bool _StaticBody { get; set; }

        internal Vector2 _Velocity //sets velocity, as long as its in the cap. 
        {
            get
            {
                return _Velo;
            }
            set
            {
                if (value.X < _VelocityCap.X && value.X > -_VelocityCap.X && value.Y < _VelocityCap.Y && value.Y > -_VelocityCap.Y)
                {
                    _Velo = value;
                }
            }
        }


        //Constructors
        //First constructor for non static Body
        internal Body(ShapeAbstract shape, float mass, float elasticity)
        {
            _Shape = shape;
            _Velocity = new Vector2();
            _VelocityCap = new Vector2(400);
            _Friction = 1f;
            _Mass = mass;
            _StaticBody = false;
            _Elasticity = elasticity; //1 for bouce, 0 for no bounce.
        }

        //Second constructor for static Body
        internal Body(ShapeAbstract shape, bool staticBody, float elasticity)
        {
            _Shape = shape;
            _Velocity = new Vector2();
            _VelocityCap = new Vector2(400);
            _Friction = 1f;
            _StaticBody = staticBody;
            _Elasticity = elasticity; //1 for bouce, 0 for no bounce.

            _Mass = staticBody ? 0 : 1;
        }

        internal Body(ShapeAbstract shape)
        {
            _Shape = shape;
            _StaticBody = true;
            _Friction = 0;
            _Elasticity = 0;
        }
    }
}
