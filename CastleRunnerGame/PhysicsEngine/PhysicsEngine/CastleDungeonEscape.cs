using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PhysicsEngine.GameDrawing;
using static PhysicsEngine.DungeonEscape;

namespace PhysicsEngine
{
    public class CastleDungeonEscape : Game
    {
        public CastleDungeonEscape()
        {
            new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 700,
                PreferredBackBufferWidth = 1200
            };

            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            _Renderer = new SpriteBatch(GraphicsDevice);
            _Textures = new TextureDictionary(Content);
            _GameController = new GameController(this);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            _GameController.Update(Keyboard.GetState());

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _Renderer.Begin();

            Rectangle rect = new Rectangle(0, 0, 1200, 700);

            _Renderer.Draw(
                _CurrentGameMode == GameMode.MainMenu || _CurrentGameMode == GameMode.Settings
                    ? _Textures.GetTexture(TextureType.MainMenuBackground)
                    : _Textures.GetTexture(TextureType.Background), rect, Color.White);

            _GameController.Draw();

            _Renderer.End();

            base.Draw(gameTime);
        }
    }
}
