using System;
using PhysicsEngine.Physics.Shapes;
using Microsoft.Xna.Framework;

namespace PhysicsEngine.Physics.Calculations
{
    class CalculatePhysics
    {
        double _Radian { get; set; }
        Vector2 _Gravity { get; set; }
        Vector2 _WindForce { get; set; }

        internal CalculatePhysics()
        {
            _Radian = Math.PI / 180;
            _Gravity = new Vector2(0, 20.8f); //gravity is usually pushing down at 9.8m/s
            _WindForce = new Vector2(0, 0);
        }

        internal void ApplyGravity(Body body)
        {
            if (body._Mass == 0) return;
            body._Velocity += _Gravity * DungeonEscape._FrameTime;
        }

        internal void ApplyWind(Body obj)
        {
            
        }

        internal void ApplyFriction(Body obj1, Body obj2)
        {
            float frictA = obj1._Friction;
            float frictB = obj2._Friction;

            if (frictA == 0 && frictB == 0)
                return; //do nothing
            
            if (obj2._Velocity.X != 0)
            {
                if (obj2._Velocity.X < 0)
                {
                    obj2._Velocity = new Vector2(obj2._Velocity.X + frictA, obj2._Velocity.Y);
                    if (obj2._Velocity.X > 0)
                        obj2._Velocity = new Vector2(0, obj2._Velocity.Y);
                }
                else
                {
                    obj2._Velocity = new Vector2(obj2._Velocity.X - frictA, obj2._Velocity.Y);
                    if (obj2._Velocity.X < 0)
                        obj2._Velocity = new Vector2(0, obj2._Velocity.Y);
                }
            }

            if (obj1._Velocity.X == 0) return;

            if (obj1._Velocity.X < 0)
            {
                obj1._Velocity = new Vector2(obj1._Velocity.X + frictA, obj1._Velocity.Y);
                if (obj1._Velocity.X > 0)
                    obj1._Velocity = new Vector2(0, obj1._Velocity.Y);
            }
            else
            {
                obj1._Velocity = new Vector2(obj1._Velocity.X - frictA, obj1._Velocity.Y);
                if (obj1._Velocity.X < 0)
                    obj1._Velocity = new Vector2(0, obj1._Velocity.Y);
            }
        }

        internal void FireBody(Body body, int angle, int speed)
        {
            double radianAngle = angle * _Radian; //get the angle given in radians
            double xVel = speed * Math.Cos(radianAngle); //work out the side velocity
            double yVel = -(speed * Math.Sin(radianAngle)); //work out the upwards velocity

            body._Velocity = new Vector2((float)xVel, (float)yVel); //set the velocity
            body._Acceleration = new Vector2(0, 9.8f); //set gravity on the object
        }
    }
}
