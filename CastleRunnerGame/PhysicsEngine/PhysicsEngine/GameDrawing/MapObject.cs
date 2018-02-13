using Microsoft.Xna.Framework;
using System.Collections.Generic;
using PhysicsEngine.Physics.Shapes;

namespace PhysicsEngine.GameDrawing
{
    //Class for combining the textures with physics objects.
    internal class MapObject
    {
        internal Body _BoundingBox { get; set; }
        internal List<Rectangle> _DrawRectList { get; set; } //Mainly for floor.
        internal TextureType _Type { get; set; }
        internal bool _ObjectTouched { get; set; } //used for objects that can be collected. Need to be removed from the game.
        internal bool _HitEvent { get; set; } //Whether the map object can be picked up or not

        internal MapObject(TextureType type, Body boundingBox)
        {
            _DrawRectList = new List<Rectangle>();
            _Type = type;
            _BoundingBox = boundingBox;
       
            switch (boundingBox._Shape)
            {
                case Polygon poly:
                    if (poly._Vertices.Count == 4)
                    {
                        if (_Type == TextureType.Floor)
                        {
                            var numTiles = (int) (poly._Vertices[1].X - poly._Vertices[0].X) / 50;
                            var rectanglePosition = new Point((int) poly._Vertices[0].X, (int) poly._Vertices[0].Y);

                            for (; numTiles > 0; numTiles--)
                            {
                                _DrawRectList.Add(new Rectangle(rectanglePosition, new Point(50, 50)));
                                rectanglePosition.X += 50;
                            }
                        }
                        else
                        {
                            var sizeX = (int)(poly._Vertices[1].X - poly._Vertices[0].X);
                            var sizeY = (int)(poly._Vertices[3].Y - poly._Vertices[0].Y);
                            var rectanglePosition = new Point((int)poly._Vertices[0].X, (int)poly._Vertices[0].Y);
                            _DrawRectList.Add(new Rectangle(rectanglePosition, new Point(sizeX, sizeY)));
                        }
                    }
                    else //Different kinda polygon
                    {
                        //TODO: Handle different kind of polygon
                    }
                    break;
                case Circle circ:
                    int size = circ._Radius * 2;
                    Point pos = new Point((int)circ._Centre.X - circ._Radius, (int)circ._Centre.Y - circ._Radius);
                    _DrawRectList.Add(new Rectangle(pos, new Point(size)));
                    break;
            }
        }

        internal void UpdatePos(Vector2 pos)
        {
            for (int i = 0; i < _DrawRectList.Count; i++)
            {
                _DrawRectList[i] = new Rectangle((int)(_DrawRectList[i].X + pos.X), (int)(_DrawRectList[i].Y + pos.Y),
                    _DrawRectList[i].Width, _DrawRectList[i].Height);
            }
            _BoundingBox._Shape.TransformShape(pos);
        }

        internal void DrawMapObject()
        {
            //Where we draw the object to the screen. 
            if (DungeonEscape._ShowBoundingBoxes == false)
            {
                foreach (var rectangle in _DrawRectList)
                {
                    DungeonEscape._Renderer.Draw(DungeonEscape._Textures.GetTexture(_Type), rectangle, Color.White);
                }
            }
            else
            {
                _BoundingBox._Shape.DebugDraw();
            }
        }
    }
}
