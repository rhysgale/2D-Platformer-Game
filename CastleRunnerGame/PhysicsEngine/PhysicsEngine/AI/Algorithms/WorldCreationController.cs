using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PhysicsEngine.AI.Algorithms;
using PhysicsEngine.Artificial_Intelligence;
using PhysicsEngine.Assets;
using PhysicsEngine.GameDrawing;
using PhysicsEngine.Physics.Shapes;

namespace PhysicsEngine
{
    internal class WorldCreationController
    {
        private readonly PathCreation _PathCreationControl;
        internal List<Node> _NodeList;
        internal List<Path> _PathList;

        internal List<Node> GetNodeList()
        {
            return _NodeList;
        }

        internal List<Path> GetPathList()
        {
            return _PathList;
        }

        internal WorldCreationController()
        {
            _NodeList = new List<Node>();
            _PathList = new List<Path>();
            _PathCreationControl = new PathCreation();
        }

        internal MapObject CreateLever(Vector2 pos)
        {
            List<Vector2> leverBoundingBox = new List<Vector2>
            {
                pos,
                new Vector2(pos.X + 100, pos.Y),
                new Vector2(pos.X + 100, pos.Y + 50),
                new Vector2(pos.X, pos.Y + 50)
            };

            return new MapObject(TextureType.Lever, new Body(new Polygon(leverBoundingBox))) { _HitEvent = true };
        }

        internal MapObject CreateDoor(Vector2 pos)
        {
            List<Vector2> doorBoundingBox = new List<Vector2>
            {
                pos,
                new Vector2(pos.X + 50, pos.Y),
                new Vector2(pos.X + 50, pos.Y + 100),
                new Vector2(pos.X, pos.Y + 100)
            };

            return new MapObject(TextureType.CastleDoor, new Body(new Polygon(doorBoundingBox))) { _HitEvent = true };
        }

        internal MapObject CreateStaticSaw(Vector2 pos)
        {
            List<Vector2> sawBounding = new List<Vector2>
            {
                pos,
                new Vector2(pos.X + 50, pos.Y),
                new Vector2(pos.X + 50, pos.Y + 50),
                new Vector2(pos.X, pos.Y + 50)
            };

            return new MapObject(TextureType.MovingSaw, new Body(new Polygon(sawBounding))) { _HitEvent = true };
        }

        /// <summary>
        /// Create a red gem at a given position. Worth 50 score.
        /// </summary>
        /// <param name="pos"></param>
        internal MapObject CreateRedGem(Vector2 pos)
        {
            List<Vector2> gemBoundingBox = new List<Vector2>
            {
                pos,
                new Vector2(pos.X + 30, pos.Y),
                new Vector2(pos.X + 30, pos.Y + 30),
                new Vector2(pos.X, pos.Y + 30)
            };

            return new MapObject(TextureType.RedGem, new Body(new Polygon(gemBoundingBox))) {_HitEvent = true};
        }

        /// <summary>
        /// Create a blue gem at a given position. Worth 100 score.
        /// </summary>
        /// <param name="pos"></param>
        internal MapObject CreateBlueGem(Vector2 pos)
        {
            List<Vector2> gemBoundingBox = new List<Vector2>
            {
                pos,
                new Vector2(pos.X + 30, pos.Y),
                new Vector2(pos.X + 30, pos.Y + 30),
                new Vector2(pos.X, pos.Y + 30)
            };

            return new MapObject(TextureType.BlueGem, new Body(new Polygon(gemBoundingBox))) { _HitEvent = true };
        }


        /// <summary>
        /// Create a moving saw object that is passed into the gameworld.
        /// </summary>
        internal MapObject CreateMovingSaw(Vector2 pos, int min, int max, bool sideways)
        {
            List<Vector2> sawBoundingBox = new List<Vector2>
            {
                pos,
                new Vector2(pos.X + 50, pos.Y),
                new Vector2(pos.X + 50, pos.Y + 50),
                new Vector2(pos.X, pos.Y + 50)
            };

            var obj = new MapObject(TextureType.MovingSaw, new Body(new Polygon(sawBoundingBox))) {_HitEvent = true};
            MovingSaw saw = new MovingSaw(obj, min, max, sideways);
            DungeonEscape._GameController.AddMapSaw(saw);

            return obj;
        }

        /// <summary>
        /// Create a map object of floor.
        /// </summary>
        /// <param name="pos">Top Left X and Y coodinate vector.</param>
        /// <param name="amntTiles">Amount of tiles to go across.</param>
        internal MapObject CreateFloor(Vector2 pos, int amntTiles)
        {
            List<Vector2> boundingCorners = new List<Vector2>
            {
                pos,
                new Vector2(pos.X + (50 * amntTiles), pos.Y),
                new Vector2(pos.X + (50 * amntTiles), pos.Y + 50),
                new Vector2(pos.X, pos.Y + 50)
            };

            var obj = new MapObject(TextureType.Floor, new Body(new Polygon(boundingCorners)));

            CreateNodesAndPaths(obj);

            return obj;
        }

        /// <summary>
        /// Create a crate that can be pushed around the map
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        internal MapObject CreateCrate(Vector2 pos)
        {
            List<Vector2> crateBB = new List<Vector2>
            {
                pos,
                new Vector2(pos.X + 50, pos.Y),
                new Vector2(pos.X + 50, pos.Y + 50),
                new Vector2(pos.X, pos.Y + 50)
            };

            return new MapObject(TextureType.Crate, new Body(new Polygon(crateBB), 0.8f, 0));
        }

        /// <summary>
        /// Creates the nodes and paths for a given map object. 
        /// This doesn't however, create paths where you can jump.
        /// </summary>
        /// <param name="obj"></param>
        private void CreateNodesAndPaths(MapObject obj)
        {
            var nodes = _PathCreationControl.GetNodesFromObject(obj);
            var paths = _PathCreationControl.CreatePathList(obj);

            foreach (var path in paths)
            {
                _PathList.Add(path);
            }
            foreach (var node in nodes)
            {
                _NodeList.Add(node);
            }          
        }

        //Levels. Here is where the levels are created and defined. 

        /// <summary>
        /// Load the first level
        /// </summary>
        /// <returns></returns>
        internal List<MapObject> LevelOneLoad()
        {
            _NodeList.Clear();
            _PathList.Clear();

            List<MapObject> objectList = new List<MapObject>
            {
                CreateFloor(new Vector2(0, 650), 4),
                CreateFloor(new Vector2(600, 650), 10),
                CreateFloor(new Vector2(450, 520), 4),
                CreateFloor(new Vector2(850, 520), 5),
                CreateFloor(new Vector2(100, 520), 5),
                CreateFloor(new Vector2(650, 400), 4),
                CreateFloor(new Vector2(300, 400), 4),
                CreateFloor(new Vector2(50, 280), 5),
                CreateFloor(new Vector2(400, 200), 7),
                CreateFloor(new Vector2(850, 130), 4),

                CreateLever(new Vector2(1000, 600)),
                CreateDoor(new Vector2(600, 100)),
                CreateRedGem(new Vector2(150, 470)),
                CreateBlueGem(new Vector2(200, 470)),
                CreateRedGem(new Vector2(250, 470)),
                CreateRedGem(new Vector2(800, 600)),
                CreateBlueGem(new Vector2(850, 600)),
                CreateRedGem(new Vector2(900, 600)),
                CreateStaticSaw(new Vector2(900, 75)),
                CreateMovingSaw(new Vector2(400, 150), 400, 700, true),
                CreateCrate(new Vector2(200, 0)),
                CreateCrate(new Vector2(1000, 0)),
                CreateCrate(new Vector2(500, 0))
            };

            var paths = _PathCreationControl.CreatePath(_NodeList);

            foreach (var path in paths)
            {
                _PathList.Add(path);
            }

            return objectList;
        }
    }
}
