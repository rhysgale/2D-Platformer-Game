using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine.Physics.Shapes
{
    internal class Polygon : ShapeAbstract
    {
        internal List<Vector2> _Vertices { get; set; }
        internal List<Vector2> _Edges { get; set; }

        internal Polygon(List<Vector2> vertices)
        {
            _Vertices = vertices;
            _Edges = new List<Vector2>();
            CreateEdges();
        }

        private void CreateEdges() //populate the list of edges, these are used to create the axies later
        {
            _Edges.Clear();

            for (int i = 0; i < _Vertices.Count; i++)
            {
                var v1 = _Vertices[i];
                var v2 = (i + 1 == _Vertices.Count ? _Vertices[0] : _Vertices[i + 1]);
                _Edges.Add(v2 - v1);
            }
        }

        internal override Vector2 GetCentreOfMass()
        {
            Vector2 total = new Vector2(0);
            foreach (Vector2 vertex in _Vertices)
            {
                total += vertex;
            }
            return total / _Vertices.Count;
        }

        internal override void TransformShape(Vector2 pos) //change all the verticies positions
        {
            Vector2 temp = new Vector2((int)pos.X, (int)pos.Y); //need to round to keep in line with bounding box
            for (int i = 0; i < _Vertices.Count; i++)
            {
                _Vertices[i] += temp;
            }
        }

        //draw the polygon on screen
        internal override void DebugDraw()
        {
            for (int i = 0; i < _Vertices.Count; i++)
            {
                Vector2 vertex1 = _Vertices[i];
                Vector2 edge = _Edges[i];

                float lineAngle = (float)Math.Atan2(edge.Y, edge.X);
                Texture2D lineTexture = DungeonEscape._Textures.GetTexture(TextureType.DebugLine);

                DungeonEscape._Renderer.Draw(lineTexture,
                                        new Rectangle
                                        (
                                        (int)vertex1.X,
                                        (int)vertex1.Y,
                                        (int)edge.Length(),
                                        3
                                        ),
                                        null,
                                        Color.Yellow,
                                        lineAngle,
                                        new Vector2(0, 0),
                                        SpriteEffects.None,
                                        0);
            }
        }

        internal override Vector2 GetPos()
        {
            return _Vertices[0];
        }
    }
}
