using Microsoft.Xna.Framework;

namespace PhysicsEngine.User
{
    class InterfaceController
    {
        internal void Draw()
        {
            switch (DungeonEscape._CurrentGameMode)
            {
                case GameMode.MainMenu:
                    DungeonEscape._Renderer.DrawString(DungeonEscape._Textures.GetFont(FontType.TwentyFive), "Play Game", new Vector2(750, 200), DungeonEscape._CurrentlySelected == Selection.PlayGame ? Color.Yellow : Color.White);
                    DungeonEscape._Renderer.DrawString(DungeonEscape._Textures.GetFont(FontType.TwentyFive), "Settings", new Vector2(750, 250), DungeonEscape._CurrentlySelected == Selection.Settings ? Color.Yellow : Color.White);
                    DungeonEscape._Renderer.DrawString(DungeonEscape._Textures.GetFont(FontType.TwentyFive), "Exit Game", new Vector2(750, 300), DungeonEscape._CurrentlySelected == Selection.Exit ? Color.Yellow : Color.White);
                    break;
                case GameMode.Settings:
                    DungeonEscape._Renderer.DrawString(DungeonEscape._Textures.GetFont(FontType.TwentyFive), "Show Pathfinder Paths: ", new Vector2(750, 200), DungeonEscape._SelectedSetting == Settings.ShowPaths ? Color.Yellow : Color.White);
                    DungeonEscape._Renderer.DrawString(DungeonEscape._Textures.GetFont(FontType.TwentyFive), DungeonEscape._ShowPathfinderPaths ? "True" : "False", new Vector2(1100, 200), Color.Green);

                    DungeonEscape._Renderer.DrawString(DungeonEscape._Textures.GetFont(FontType.TwentyFive), "Show Bounding Boxes: ", new Vector2(750, 250), DungeonEscape._SelectedSetting == Settings.ShowBoundingBoxes ? Color.Yellow : Color.White);
                    DungeonEscape._Renderer.DrawString(DungeonEscape._Textures.GetFont(FontType.TwentyFive), DungeonEscape._ShowBoundingBoxes ? "True" : "False", new Vector2(1100, 250), Color.Green);

                    DungeonEscape._Renderer.DrawString(DungeonEscape._Textures.GetFont(FontType.TwentyFive), "Back To Menu", new Vector2(750, 300), DungeonEscape._SelectedSetting == Settings.Back ? Color.Yellow : Color.White);
                    break;
                case GameMode.InGame:
                    Rectangle topBar = new Rectangle(0, 0, 1200, 60);
                    Rectangle healthBar = new Rectangle(306, 12, DungeonEscape._Player._Health, 10); //Health 160, makes it easy to sort out drawing the bar
                    DungeonEscape._Renderer.Draw(DungeonEscape._Textures.GetTexture(TextureType.TopBar), topBar, Color.White);
                    DungeonEscape._Renderer.DrawString(DungeonEscape._Textures.GetFont(FontType.Fifteen), DungeonEscape._Player._Score.ToString(), new Vector2(670, 10), Color.White);
                    DungeonEscape._Renderer.Draw(DungeonEscape._Textures.GetTexture(TextureType.DebugLine), healthBar, Color.LightGreen);
                    break;
                case GameMode.Paused:
                    topBar = new Rectangle(0, 0, 1200, 60);
                    healthBar = new Rectangle(306, 12, DungeonEscape._Player._Health, 10); //Health 160, makes it easy to sort out drawing the bar
                    DungeonEscape._Renderer.Draw(DungeonEscape._Textures.GetTexture(TextureType.TopBar), topBar, Color.White);
                    DungeonEscape._Renderer.DrawString(DungeonEscape._Textures.GetFont(FontType.Fifteen), DungeonEscape._Player._Score.ToString(), new Vector2(670, 10), Color.White);
                    DungeonEscape._Renderer.Draw(DungeonEscape._Textures.GetTexture(TextureType.DebugLine), healthBar, Color.LightGreen);
                    DungeonEscape._Renderer.DrawString(DungeonEscape._Textures.GetFont(FontType.Fourty), "Game Paused. Press P to Continue.", new Vector2(100, 200), Color.White);
                    break;
            }
        }
    }
}
