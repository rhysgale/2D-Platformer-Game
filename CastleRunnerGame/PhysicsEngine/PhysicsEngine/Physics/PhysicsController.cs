using Microsoft.Xna.Framework;
using PhysicsEngine.Assets;
using PhysicsEngine.Physics.Calculations;
using PhysicsEngine.Physics.Shapes;
using System;
using System.Collections.Generic;
using PhysicsEngine.GameDrawing;
using static PhysicsEngine.DungeonEscape;

namespace PhysicsEngine.Physics
{
    class PhysicsController
    {
        CollisionDetection _CollisionCalculations;
        CalculatePhysics _PhysicsCalculations;

        internal PhysicsController()
        {
            _CollisionCalculations = new CollisionDetection();
            _PhysicsCalculations = new CalculatePhysics();
        }

        internal void ResolveCollisions(List<MapObject> objects)
        {
            _Player._IsGrounded = false;
            _NpcCaveman._IsGrounded = false;

            foreach (var body in objects)
            {
                foreach (var body2 in objects)
                {
                    if (body == body2) continue;

                    CollisionResult result = _CollisionCalculations.DetectCollision(body._BoundingBox, body2._BoundingBox);

                    if (!result._CollisionDetected) continue;

                    if (body._Type == TextureType.Projectile || body2._Type == TextureType.Projectile)
                    {
                        if (body == _NpcCaveman._CaveManBody || body2 == _NpcCaveman._CaveManBody)
                        {
                            _NpcCaveman._Health -= 1;
                        }
                        continue; //skip, don't want to resolve projectile collisions.
                    }

                    if (!body._HitEvent && !body2._HitEvent) //If collisions need to be resolved
                    {
                        body.UpdatePos(result._MinimumTranslationVectorA);
                        body2.UpdatePos(result._MinimumTranslationVectorB);
                    }
                    else //If collision doesn't need to be resolved (E.g collectable item)
                    {
                        if ((body._BoundingBox == _Player._CharacterBody._BoundingBox && body2._HitEvent) ||
                            (body2._BoundingBox == _Player._CharacterBody._BoundingBox && body._HitEvent))//if body is collectable
                        {
                            body2._ObjectTouched = body2._HitEvent;
                            body._ObjectTouched = body._HitEvent;
                            continue;
                        }
                    }

                    if (body._BoundingBox == _NpcCaveman._CaveManBody._BoundingBox || body2._BoundingBox == _NpcCaveman._CaveManBody._BoundingBox)
                    {
                        if (result._MinimumTranslationVectorA.Y > 0 && body._BoundingBox == _NpcCaveman._CaveManBody._BoundingBox)
                        {
                            _NpcCaveman._CaveManBody._BoundingBox._Velocity = new Vector2(_NpcCaveman._CaveManBody._BoundingBox._Velocity.X, 15);
                        }
                        if (result._MinimumTranslationVectorB.Y > 0 && body2._BoundingBox == _NpcCaveman._CaveManBody._BoundingBox)
                        {
                            _NpcCaveman._CaveManBody._BoundingBox._Velocity = new Vector2(_NpcCaveman._CaveManBody._BoundingBox._Velocity.X, 15);
                        }

                        if (!(result._MinimumTranslationVectorA.Y < 0) && body._BoundingBox == _NpcCaveman._CaveManBody._BoundingBox) continue;
                        if (!(result._MinimumTranslationVectorB.Y < 0) && body2._BoundingBox == _NpcCaveman._CaveManBody._BoundingBox) continue;

                        if (body._HitEvent || body2._HitEvent) continue;
                        _NpcCaveman._IsGrounded = true;
                        _NpcCaveman._CaveManBody._BoundingBox._Velocity = new Vector2(_NpcCaveman._CaveManBody._BoundingBox._Velocity.X, 9.8f);
                    }
                    else if (body._BoundingBox == _Player._CharacterBody._BoundingBox || body2._BoundingBox == _Player._CharacterBody._BoundingBox)
                    {
                        if (result._MinimumTranslationVectorA.Y != 0 && body._BoundingBox == _Player._CharacterBody._BoundingBox)
                        {
                            if (result._MinimumTranslationVectorA.Y < 0)
                            {
                                _Player._IsGrounded = true;
                                _Player._CharacterBody._BoundingBox._Velocity =
                                    new Vector2(_Player._CharacterBody._BoundingBox._Velocity.X, 9.8f);
                            }
                            else
                            {
                                _Player._CharacterBody._BoundingBox._Velocity = new Vector2(_Player._CharacterBody._BoundingBox._Velocity.X, 20);
                            }
                        }
                        if (result._MinimumTranslationVectorB.Y != 0 &&body2._BoundingBox == _Player._CharacterBody._BoundingBox)
                        {
                            if (result._MinimumTranslationVectorB.Y < 0)
                            {
                                _Player._IsGrounded = true;
                                _Player._CharacterBody._BoundingBox._Velocity =
                                    new Vector2(_Player._CharacterBody._BoundingBox._Velocity.X, 9.8f);
                            }
                            else
                            {
                                _Player._CharacterBody._BoundingBox._Velocity = new Vector2(_Player._CharacterBody._BoundingBox._Velocity.X, 20);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Apply world forces to a list of objects. 
        /// Applys to any objects that have a mass, as these will be effected by gravity.
        /// This effects each objects velocity.
        /// </summary>
        /// <param name="objList">List of objects to apply forces too.</param>
        internal void ApplyForces(List<MapObject> objList)
        {
            foreach (MapObject obj in objList)
            {
                if (obj._BoundingBox._Mass != 0)
                {
                    _PhysicsCalculations.ApplyGravity(obj._BoundingBox);
                    //_PhysicsCalculations.ApplyWind(obj);
                }
            }
        }

        /// <summary>
        /// Fires a given MapObject at a given angle with a given speed.
        /// </summary>
        /// <param name="projectile">The MapObject to fire.</param>
        /// <param name="angle">The angle to fire at.</param>
        /// <param name="speed">The speed to fire the object.</param>
        internal void FireObject(MapObject projectile, int angle, int speed)
        {
            _PhysicsCalculations.FireBody(projectile._BoundingBox, angle, speed);
        }

        /// <summary>
        /// Transforms each shape by its velocity.
        /// </summary>
        /// <param name="objList">The list of objects to update by velocity.</param>
        internal void ApplyVelocity(List<MapObject> objList)
        {
            foreach (MapObject obj in objList)
            {
                if (obj._BoundingBox._Mass != 0)
                {
                    obj.UpdatePos(obj._BoundingBox._Velocity * _FrameTime);
                    obj._BoundingBox._Velocity += obj._BoundingBox._Acceleration * _FrameTime;
                }
            }
        }
    }
}
