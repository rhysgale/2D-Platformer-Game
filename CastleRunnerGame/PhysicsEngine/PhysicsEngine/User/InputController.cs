using Microsoft.Xna.Framework.Input;
using PhysicsEngine.Physics;

namespace PhysicsEngine.User
{
    internal class InputController
    {
        GameController _MainGame;
        bool _KeyDown;

        internal InputController(GameController main)
        {
            _MainGame = main;
        }

        internal void HandleInput(KeyboardState keyboardState)
        {
            switch (DungeonEscape._CurrentGameMode)
            {
                case GameMode.MainMenu:
                    {
                        if (keyboardState.IsKeyDown(Keys.Enter) && _KeyDown == false)
                        {
                            if (DungeonEscape._CurrentlySelected == Selection.PlayGame) //0 for game, 1 for settings... etc
                            {
                                DungeonEscape._CurrentGameMode = GameMode.InGame;
                                _MainGame.BeginGame();
                            }
                            if (DungeonEscape._CurrentlySelected == Selection.Exit)
                                _MainGame.ExitGame();
                            if (DungeonEscape._CurrentlySelected == Selection.Settings)
                                DungeonEscape._CurrentGameMode = GameMode.Settings;
                            _KeyDown = true;
                        }
                        if (keyboardState.IsKeyDown(Keys.W) && !_KeyDown)
                        {
                            switch (DungeonEscape._CurrentlySelected)
                            {
                                case Selection.PlayGame:
                                    DungeonEscape._CurrentlySelected = Selection.Exit;
                                    break;
                                case Selection.Exit:
                                    DungeonEscape._CurrentlySelected = Selection.Settings;
                                    break;
                                default:
                                    DungeonEscape._CurrentlySelected = Selection.PlayGame;
                                    break;
                            }

                            _KeyDown = true;
                        }
                        if (keyboardState.IsKeyDown(Keys.S) && !_KeyDown)
                        {
                            switch (DungeonEscape._CurrentlySelected)
                            {
                                case Selection.PlayGame:
                                    DungeonEscape._CurrentlySelected = Selection.Settings;
                                    break;
                                case Selection.Exit:
                                    DungeonEscape._CurrentlySelected = Selection.PlayGame;
                                    break;
                                default:
                                    DungeonEscape._CurrentlySelected = Selection.Exit;
                                    break;
                            }

                            _KeyDown = true;
                        }

                        if (!keyboardState.IsKeyDown(Keys.S) && !keyboardState.IsKeyDown(Keys.W) && !keyboardState.IsKeyDown(Keys.Enter))
                            _KeyDown = false;

                        break;
                    }
                case GameMode.Paused:
                    {
                        if (!_KeyDown && keyboardState.IsKeyDown(Keys.P))
                        {
                            _KeyDown = true;
                            DungeonEscape._CurrentGameMode = GameMode.InGame;
                        }
                        if (!keyboardState.IsKeyDown(Keys.P))
                        {
                            _KeyDown = false;
                        }

                        break;
                    }
                case GameMode.InGame:
                    {
                        if (keyboardState.IsKeyDown(Keys.P) && !_KeyDown)
                        {
                            _KeyDown = true;
                            DungeonEscape._CurrentGameMode = GameMode.Paused;
                        }
                        if (!keyboardState.IsKeyDown(Keys.P))
                            _KeyDown = false;

                        DungeonEscape._Player.HandleCharacterInput(keyboardState);
                        break;
                    }
                case GameMode.Settings:
                    {
                        if (keyboardState.IsKeyDown(Keys.Enter) && _KeyDown == false)
                        {
                            if (DungeonEscape._SelectedSetting == Settings.ShowPaths)
                            {
                                DungeonEscape._ShowPathfinderPaths = !DungeonEscape._ShowPathfinderPaths;
                            }
                            if (DungeonEscape._SelectedSetting == Settings.ShowBoundingBoxes)
                            {
                                DungeonEscape._ShowBoundingBoxes = !DungeonEscape._ShowBoundingBoxes;
                            }
                            if (DungeonEscape._SelectedSetting == Settings.Back)
                            {
                                DungeonEscape._CurrentGameMode = GameMode.MainMenu;
                            }
                            _KeyDown = true;
                        }
                        if (keyboardState.IsKeyDown(Keys.S) && !_KeyDown)
                        {
                            switch (DungeonEscape._SelectedSetting)
                            {
                                case Settings.ShowPaths:
                                    DungeonEscape._SelectedSetting = Settings.ShowBoundingBoxes;
                                    break;
                                case Settings.ShowBoundingBoxes:
                                    DungeonEscape._SelectedSetting = Settings.Back;
                                    break;
                                default:
                                    DungeonEscape._SelectedSetting = Settings.ShowPaths;
                                    break;
                            }

                            _KeyDown = true;
                        }
                        if (keyboardState.IsKeyDown(Keys.W) && !_KeyDown)
                        {
                            switch (DungeonEscape._SelectedSetting)
                            {
                                case Settings.ShowPaths:
                                    DungeonEscape._SelectedSetting = Settings.Back;
                                    break;
                                case Settings.ShowBoundingBoxes:
                                    DungeonEscape._SelectedSetting = Settings.ShowPaths;
                                    break;
                                default:
                                    DungeonEscape._SelectedSetting = Settings.ShowBoundingBoxes;
                                    break;
                            }

                            _KeyDown = true;
                        }

                        if (!keyboardState.IsKeyDown(Keys.S) && !keyboardState.IsKeyDown(Keys.W) && !keyboardState.IsKeyDown(Keys.Enter))
                            _KeyDown = false;

                        break;
                    }
            }
        }
    }
}
