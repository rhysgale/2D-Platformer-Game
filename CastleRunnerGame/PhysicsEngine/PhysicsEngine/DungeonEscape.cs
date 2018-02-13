using Microsoft.Xna.Framework.Graphics;
using PhysicsEngine.AI.NPC;
using PhysicsEngine.GameDrawing;
using PhysicsEngine.User;

namespace PhysicsEngine
{
    internal static class DungeonEscape
    {
        internal static GameController _GameController;

        internal static CharacterController _Player;
        internal static CaveMan _NpcCaveman;
        internal static SpriteBatch _Renderer;
        internal static TextureDictionary _Textures;
        internal static float _FrameTime = 0.1f;
        internal static GameMode _CurrentGameMode = GameMode.MainMenu;

        //For the main menu
        internal static Selection _CurrentlySelected = Selection.PlayGame;
        internal static Settings _SelectedSetting = Settings.ShowPaths;

        //Game Settings
        internal static bool _ShowPathfinderPaths = false;
        internal static bool _ShowBoundingBoxes = false;
    }
}
