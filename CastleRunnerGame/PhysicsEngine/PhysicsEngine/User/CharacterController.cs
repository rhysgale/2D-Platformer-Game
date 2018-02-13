using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PhysicsEngine.GameDrawing;
using PhysicsEngine.Physics.Shapes;

namespace PhysicsEngine.User
{
    class CharacterController
    {
        internal int _Health { get; set; }
        internal MapObject _CharacterBody { get; set; }
        internal bool _IsGrounded { get; set; }
        internal bool _FacingRight { get; set; }
        internal int _Score { get; set; }

        private bool _FKeyPressed = false;

        internal CharacterController(MapObject body)
        {
            _CharacterBody = body;
            _Health = 170;
            _IsGrounded = true;
        }

        internal void Update()
        {

        }

        internal void Draw()
        {
            int posX = (int)_CharacterBody._BoundingBox._Shape.GetPos().X;
            int posY = (int)_CharacterBody._BoundingBox._Shape.GetPos().Y;

            Rectangle rect = new Rectangle(posX, posY, 50, 50);

            if (_FacingRight)
            {
                DungeonEscape._Renderer.Draw(
                    _CharacterBody._BoundingBox._Velocity.X < 5
                        ? DungeonEscape._Textures.GetTexture(TextureType.IdleRight)
                        : DungeonEscape._Textures.GetTexture(TextureType.RunRight), rect, Color.White);
            }
            else
            {
                DungeonEscape._Renderer.Draw(
                    _CharacterBody._BoundingBox._Velocity.X > -5
                        ? DungeonEscape._Textures.GetTexture(TextureType.IdleLeft)
                        : DungeonEscape._Textures.GetTexture(TextureType.RunLeft), rect, Color.White);
            }

            if (DungeonEscape._ShowBoundingBoxes)
            {
                _CharacterBody._BoundingBox._Shape.DebugDraw();
            }
        }

        internal void HandleCharacterInput(KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.Space))
            {
                if (_IsGrounded)
                {
                    _CharacterBody._BoundingBox._Velocity = new Vector2(_CharacterBody._BoundingBox._Velocity.X, -90);
                    _IsGrounded = false;
                }
            }

            if (keyboard.IsKeyDown(Keys.F) && !_FKeyPressed)
            {
                _FKeyPressed = true;
                Vector2 pos = _CharacterBody._BoundingBox._Shape.GetPos();
                List<Vector2> vertices = new List<Vector2>
                {
                    pos, 
                    new Vector2(pos.X + 25, pos.Y),
                    new Vector2(pos.X + 25, pos.Y + 25),
                    new Vector2(pos.X, pos.Y + 25)
                };

                MapObject projectile = new MapObject(TextureType.Projectile, new Body(new Polygon(vertices), 1, 0)); //need to add this in game 
                projectile.UpdatePos(_FacingRight ? new Vector2(50, 0) : new Vector2(-30, 0));
                DungeonEscape._GameController.FireObject(projectile);
                DungeonEscape._GameController.AddMapObject(projectile);
            }
            if (!keyboard.IsKeyDown(Keys.F))
            {
                _FKeyPressed = false;
            }

            if (keyboard.IsKeyDown(Keys.D))
            {
                _CharacterBody._BoundingBox._Velocity = new Vector2(20, _CharacterBody._BoundingBox._Velocity.Y);
                _FacingRight = true;
            }
            else
            {
                if (keyboard.IsKeyDown(Keys.A))
                {
                    _CharacterBody._BoundingBox._Velocity = new Vector2(-20, _CharacterBody._BoundingBox._Velocity.Y);
                    _FacingRight = false;
                }
                else
                {
                    _CharacterBody._BoundingBox._Velocity = new Vector2(0, _CharacterBody._BoundingBox._Velocity.Y);
                }
            }
        }
    }
}
