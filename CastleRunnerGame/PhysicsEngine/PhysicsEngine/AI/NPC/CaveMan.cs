using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.Artificial_Intelligence;
using PhysicsEngine.GameDrawing;
using PhysicsEngine.Physics.Shapes;

namespace PhysicsEngine.AI.NPC
{
    internal class CaveMan
    {
        internal MapObject _CaveManBody { get; set; }
        internal Body _Caveman { get; set; }
        internal List<Node> _Path { get; set; }
        internal bool _IsGrounded { get; set; } = true;
        internal bool _FacingRight { get; set; }
        internal int _Health { get; set; }

        internal CaveMan(MapObject objBody)
        {
            _CaveManBody = objBody;
            _Caveman = objBody._BoundingBox;
            _Health = 100;
        }

        internal void Jump()
        {
            if (!_IsGrounded) return;

            _IsGrounded = false;
            _CaveManBody._BoundingBox._Velocity = new Vector2(_CaveManBody._BoundingBox._Velocity.X, -80);
        }

        internal void Update(Vector2 velo) //Pass in what we need to add to velocity. 
        {
            _CaveManBody._BoundingBox._Velocity = new Vector2(velo.X, _CaveManBody._BoundingBox._Velocity.Y);
            _FacingRight = !(velo.X < 0);
        }

        internal void Draw()
        {
            if (_Health <= 0)
                return;

            Rectangle rect = new Rectangle((int)_CaveManBody._BoundingBox._Shape.GetPos().X, (int)_CaveManBody._BoundingBox._Shape.GetPos().Y, 50, 50);
            Rectangle redRect = new Rectangle(rect.Left, rect.Top - 5, rect.Width, 5);
            Rectangle greenRect = new Rectangle(rect.Left, rect.Top - 5, rect.Width - (50 - DungeonEscape._NpcCaveman._Health/2), 5);

            DungeonEscape._Renderer.Draw(DungeonEscape._Textures.GetTexture(TextureType.RedBar), redRect, Color.White);
            DungeonEscape._Renderer.Draw(DungeonEscape._Textures.GetTexture(TextureType.GreenBar), greenRect, Color.White);
            Texture2D tex =
                DungeonEscape._Textures.GetTexture(_FacingRight
                    ? TextureType.CavemanRunRight
                    : TextureType.CavemanRunLeft);
        
            DungeonEscape._Renderer.Draw(tex, rect, Color.White);
            if (DungeonEscape._ShowBoundingBoxes)
            {
                _CaveManBody._BoundingBox._Shape.DebugDraw();
            }
        }
    }
}
