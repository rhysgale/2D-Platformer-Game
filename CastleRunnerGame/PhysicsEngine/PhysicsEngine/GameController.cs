using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PhysicsEngine.AI.Algorithms;
using PhysicsEngine.AI.NPC;
using PhysicsEngine.Assets;
using PhysicsEngine.GameDrawing;
using PhysicsEngine.Physics;
using PhysicsEngine.Physics.Shapes;
using PhysicsEngine.User;
using static PhysicsEngine.DungeonEscape;

namespace PhysicsEngine
{
    internal class GameController
    {
        private CastleDungeonEscape _Main;

        //Where all game stuff is handled. 
        private PhysicsController _PhysicsController; //Where all physics stuff is done
        private NpcController _NpcController; //Where all AI pathfinding is handled 
        private WorldCreationController _WorldCreationController; //Where objects for the world are created

        private List<MapObject> _GameWorld;
        private List<MovingSaw> _Saws;

        private readonly InterfaceController _UserInterface;
        private readonly InputController _Input;

        internal GameController(CastleDungeonEscape main)
        {
            _GameWorld = new List<MapObject>();
            _PhysicsController = new PhysicsController();
            _UserInterface = new InterfaceController();
            _Input = new InputController(this);
            _WorldCreationController = new WorldCreationController();
            _Saws = new List<MovingSaw>(); 
            _Main = main;
        }

        internal void AddMapObject(MapObject obj)
        {
            _GameWorld.Add(obj);
        }

        internal void AddMapSaw(MovingSaw saw)
        {
            _Saws.Add(saw);
        }

        internal void ExitGame()
        {
            _Main.Exit();
        }

        internal void FireObject(MapObject obj)
        {
            int angle = _Player._FacingRight ? 60 : 120;
            _PhysicsController.FireObject(obj, angle, 80);
        }

        internal void Update(KeyboardState keyboard)
        {
            _Input.HandleInput(keyboard);

            if (_CurrentGameMode != GameMode.Paused)
            {
                if (_CurrentGameMode == GameMode.InGame)
                {
                    if (_NpcCaveman._Health <= 0)
                    {
                        _GameWorld.Remove(_NpcCaveman._CaveManBody);
                    }

                    foreach (var movingSaw in _Saws)
                    {
                        movingSaw.Update();
                    }

                    _PhysicsController.ApplyForces(_GameWorld);
                    _PhysicsController.ApplyVelocity(_GameWorld);
                    _PhysicsController.ResolveCollisions(_GameWorld);
                    _NpcController.Update();
                }
            }
        }

        internal void Draw()
        {
            if (_CurrentGameMode == GameMode.InGame || _CurrentGameMode == GameMode.Paused)
            {
                List<MapObject> touchedObject = new List<MapObject>();
                foreach (var mapObject in _GameWorld)
                {
                    if (mapObject._Type == TextureType.NotDrawn) continue; //For character and Caveman, as these have their own draw handlers.

                    if (!mapObject._ObjectTouched && mapObject._Type != TextureType.Projectile)
                        mapObject.DrawMapObject();
                    else
                    {
                        if (mapObject._Type != TextureType.Projectile)
                            touchedObject.Add(mapObject);
                        else
                        {
                            if (mapObject._BoundingBox._Shape.GetPos().Y > 800) //Projectile Cleanup.
                                touchedObject.Add(mapObject);
                            else
                                mapObject.DrawMapObject();
                        }
                    }
                }

                //This handles map collisions, such as gems, doors, levers, etc.
                foreach (MapObject obj in touchedObject)
                {
                    switch (obj._Type)
                    {
                        case TextureType.BlueGem:
                            _GameWorld.Remove(obj);
                            _Player._Score += 100;
                            break;
                        case TextureType.RedGem:
                            _GameWorld.Remove(obj);
                            _Player._Score += 50;
                            break;
                        case TextureType.Lever:
                            obj.DrawMapObject();
                            _Textures.UnpauseAnimation(TextureType.Lever);
                            _Textures.UnpauseAnimation(TextureType.CastleDoor);
                            break;
                        case TextureType.CastleDoor:
                            obj.DrawMapObject();
                            if (!_Textures.IsPaused(TextureType.CastleDoor)) //if lever has been touched.
                            {
                                //TODO: Next level progression. 
                            }
                            break;
                        case TextureType.Projectile:
                            _GameWorld.Remove(obj);
                            break;
                        case TextureType.MovingSaw:
                            obj.DrawMapObject();
                            _Player._Health -= 1;
                            obj._ObjectTouched = false;
                            break;
                    }
                }

                _Player.Draw();
                _NpcCaveman.Draw();
                if (_ShowPathfinderPaths)
                {
                    DrawAStarPathAndNodes();
                }
            }
            _UserInterface.Draw();
        }

        /// <summary>
        /// For debugging. Draw the paths for the nodes. 
        /// </summary>
        private void DrawAStarPathAndNodes()
        {
            foreach (Path path in _WorldCreationController._PathList)
            {
                Vector2 vertex1 = path._PointA._Position;

                Vector2 vertex2 = path._PointB._Position;
                Vector2 edge = vertex2 - vertex1;
                vertex1.Y -= 10;
                vertex2.Y -= 10;

                float lineAngle = (float)Math.Atan2(edge.Y, edge.X);
                Texture2D lineTexture = DungeonEscape._Textures.GetTexture(TextureType.DebugLine);

                _Renderer.Draw(lineTexture,
                    new Rectangle
                    (
                        (int)vertex1.X,
                        (int)vertex1.Y,
                        (int)edge.Length(),
                        1
                    ),
                    null,
                    Color.Yellow,
                    lineAngle,
                    new Vector2(0, 0),
                    SpriteEffects.None,
                    0);

                Rectangle recty = new Rectangle((int)path._PointA._Position.X, (int)path._PointA._Position.Y - 15, 10, 10);
                _Renderer.Draw(_Textures.GetTexture(TextureType.DebugLine), recty, Color.Pink);
                recty = new Rectangle((int)path._PointB._Position.X, (int)path._PointB._Position.Y - 15, 10, 10);
                _Renderer.Draw(_Textures.GetTexture(TextureType.DebugLine), recty, Color.Pink);
            }
        }

        internal void BeginGame()
        {
            _GameWorld = _WorldCreationController.LevelOneLoad();
            var vertices = new List<Vector2>
            {
                new Vector2(0, 500),
                new Vector2(50, 500),
                new Vector2(50, 550),
                new Vector2(0, 550)
            };
            Body body = new Body(new Polygon(vertices), 1, 0);
            _Player = new CharacterController(new MapObject(TextureType.NotDrawn, body));
            vertices = new List<Vector2>
            {
                new Vector2(750, 500),
                new Vector2(800, 500),
                new Vector2(800, 550),
                new Vector2(750, 550)
            };
            body = new Body(new Polygon(vertices), 1, 0);

            _NpcCaveman = new CaveMan(new MapObject(TextureType.NotDrawn, body));
            _NpcController = new NpcController(_WorldCreationController._NodeList, _WorldCreationController._PathList, _NpcCaveman);
            _GameWorld.Add(_Player._CharacterBody);
            _GameWorld.Add(_NpcCaveman._CaveManBody);
        }
    }
}
