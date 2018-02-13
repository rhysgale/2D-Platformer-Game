using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace PhysicsEngine.GameDrawing
{
    class TextureDictionary
    {
        //Credit given to GameArt2D.com for the spritesheets for the tileset and characters used within the game

        readonly Dictionary<TextureType, Animation> _TextureDictionary;
        readonly Dictionary<FontType, SpriteFont> _FontDictionary;

        internal TextureDictionary(ContentManager manager)
        {
            _TextureDictionary = new Dictionary<TextureType, Animation>
            {
                { TextureType.DebugLine, new Animation(manager.Load<Texture2D>("Debug/DebugLine")) },
                { TextureType.GreenBar, new Animation(manager.Load<Texture2D>("Characters/Healthbar/greenbar")) },
                { TextureType.RedBar, new Animation(manager.Load<Texture2D>("Characters/Healthbar/redbar")) },
                { TextureType.DebugCircle, new Animation(manager.Load<Texture2D>("Debug/DebugCircle")) },
                { TextureType.TopBar, new Animation(manager.Load<Texture2D>("UserInterface/UserInterface1")) },
                { TextureType.Background, new Animation(manager.Load<Texture2D>("UserInterface/Background")) },
                { TextureType.Floor, new Animation(manager.Load<Texture2D>("CastleTiles/Floor")) },
                { TextureType.RedGem, new Animation(manager.Load<Texture2D>("CastleDecoration/RedGem")) },
                { TextureType.BlueGem, new Animation(manager.Load<Texture2D>("CastleDecoration/BlueGem")) },
                { TextureType.MainMenuBackground, new Animation(manager.Load<Texture2D>("UserInterface/MainMenuBackground")) },
                { TextureType.Projectile, new Animation(manager.Load<Texture2D>("CastleDecoration/Pot")) },
                { TextureType.Crate, new Animation(manager.Load<Texture2D>("CastleTiles/Crate")) } //CAN THROW POTS
            };

            LoadCavemanRightRun(manager);
            LoadCavemanLeftRun(manager);
            LoadLeftIdleAnimation(manager);
            LoadRightIdleAnimation(manager);
            LoadRunLeftAnimation(manager);
            LoadRunRightAnimation(manager);
            LoadCastleLever(manager);
            LoadCastleDoor(manager);
            LoadSaw(manager);

            _FontDictionary = new Dictionary<FontType, SpriteFont>
            {
                { FontType.Fifty, manager.Load<SpriteFont>("Fonts/50Pt") },
                { FontType.Fourty, manager.Load<SpriteFont>("Fonts/40Pt") },
                { FontType.Thirty, manager.Load<SpriteFont>("Fonts/30Pt") },
                { FontType.TwentyFive, manager.Load<SpriteFont>("Fonts/25Pt") },
                { FontType.Twenty, manager.Load<SpriteFont>("Fonts/20Pt") },
                { FontType.Fifteen, manager.Load<SpriteFont>("Fonts/15Pt") },
                { FontType.Ten, manager.Load<SpriteFont>("Fonts/10Pt") }
            };
        }


        private void LoadSaw(ContentManager manager)
        {
            List<Texture2D> saw = new List<Texture2D>
            {
                manager.Load<Texture2D>("CastleDecoration/Saw"),
                manager.Load<Texture2D>("CastleDecoration/Saw2"),
                manager.Load<Texture2D>("CastleDecoration/Saw3")
            };
            _TextureDictionary.Add(TextureType.MovingSaw, new Animation(saw, 5, true, false));
        }

        private void LoadCastleLever(ContentManager manager)
        {
            List<Texture2D> lever = new List<Texture2D>
            {
                manager.Load<Texture2D>("CastleDecoration/Lever1"),
                manager.Load<Texture2D>("CastleDecoration/Lever2"),
                manager.Load<Texture2D>("CastleDecoration/Lever3"),
                manager.Load<Texture2D>("CastleDecoration/Lever4")
            };
            _TextureDictionary.Add(TextureType.Lever, new Animation(lever, 5, false, true));
        }

        private void LoadCastleDoor(ContentManager manager)
        {
            List<Texture2D> castleDoor = new List<Texture2D>
            {
                manager.Load<Texture2D>("CastleDecoration/Door"),
                manager.Load<Texture2D>("CastleDecoration/Door2"),
                manager.Load<Texture2D>("CastleDecoration/Door3"),
                manager.Load<Texture2D>("CastleDecoration/Door4"),
                manager.Load<Texture2D>("CastleDecoration/Door5")
            };
            _TextureDictionary.Add(TextureType.CastleDoor, new Animation(castleDoor, 5, false, true));
        }

        private void LoadCavemanRightRun(ContentManager manager)
        {
            List<Texture2D> caveman = new List<Texture2D>
            {
                manager.Load<Texture2D>("Characters/Caveman/RunRight/CaveWalk1"),
                manager.Load<Texture2D>("Characters/Caveman/RunRight/CaveWalk2"),
                manager.Load<Texture2D>("Characters/Caveman/RunRight/CaveWalk3"),
                manager.Load<Texture2D>("Characters/Caveman/RunRight/CaveWalk4"),
                manager.Load<Texture2D>("Characters/Caveman/RunRight/CaveWalk5"),
                manager.Load<Texture2D>("Characters/Caveman/RunRight/CaveWalk6"),
                manager.Load<Texture2D>("Characters/Caveman/RunRight/CaveWalk7"),
                manager.Load<Texture2D>("Characters/Caveman/RunRight/CaveWalk8"),
                manager.Load<Texture2D>("Characters/Caveman/RunRight/CaveWalk9"),
                manager.Load<Texture2D>("Characters/Caveman/RunRight/CaveWalk10")
            };
            _TextureDictionary.Add(TextureType.CavemanRunRight, new Animation(caveman, 5, true, false));
        }

        private void LoadCavemanLeftRun(ContentManager manager)
        {
            List<Texture2D> caveman = new List<Texture2D>
            {
                manager.Load<Texture2D>("Characters/Caveman/RunLeft/CaveWalk1"),
                manager.Load<Texture2D>("Characters/Caveman/RunLeft/CaveWalk2"),
                manager.Load<Texture2D>("Characters/Caveman/RunLeft/CaveWalk3"),
                manager.Load<Texture2D>("Characters/Caveman/RunLeft/CaveWalk4"),
                manager.Load<Texture2D>("Characters/Caveman/RunLeft/CaveWalk5"),
                manager.Load<Texture2D>("Characters/Caveman/RunLeft/CaveWalk6"),
                manager.Load<Texture2D>("Characters/Caveman/RunLeft/CaveWalk7"),
                manager.Load<Texture2D>("Characters/Caveman/RunLeft/CaveWalk8"),
                manager.Load<Texture2D>("Characters/Caveman/RunLeft/CaveWalk9"),
                manager.Load<Texture2D>("Characters/Caveman/RunLeft/CaveWalk10")
            };
            _TextureDictionary.Add(TextureType.CavemanRunLeft, new Animation(caveman, 5, true, false));
        }

        private void LoadRunRightAnimation(ContentManager manager)
        {
            List<Texture2D> characterWalkAnim = new List<Texture2D>
            {
                manager.Load<Texture2D>("Characters/Knight/RightRun/Run1"),
                manager.Load<Texture2D>("Characters/Knight/RightRun/Run2"),
                manager.Load<Texture2D>("Characters/Knight/RightRun/Run3"),
                manager.Load<Texture2D>("Characters/Knight/RightRun/Run4"),
                manager.Load<Texture2D>("Characters/Knight/RightRun/Run5"),
                manager.Load<Texture2D>("Characters/Knight/RightRun/Run6"),
                manager.Load<Texture2D>("Characters/Knight/RightRun/Run7"),
                manager.Load<Texture2D>("Characters/Knight/RightRun/Run8"),
                manager.Load<Texture2D>("Characters/Knight/RightRun/Run9"),
                manager.Load<Texture2D>("Characters/Knight/RightRun/Run10")
            };

            _TextureDictionary.Add(TextureType.RunRight, new Animation(characterWalkAnim, 5, true, false));
        }

        private void LoadRunLeftAnimation(ContentManager manager)
        {
            List<Texture2D> characterWalkAnim = new List<Texture2D>
            {
                manager.Load<Texture2D>("Characters/Knight/LeftRun/Run1"),
                manager.Load<Texture2D>("Characters/Knight/LeftRun/Run2"),
                manager.Load<Texture2D>("Characters/Knight/LeftRun/Run3"),
                manager.Load<Texture2D>("Characters/Knight/LeftRun/Run4"),
                manager.Load<Texture2D>("Characters/Knight/LeftRun/Run5"),
                manager.Load<Texture2D>("Characters/Knight/LeftRun/Run6"),
                manager.Load<Texture2D>("Characters/Knight/LeftRun/Run7"),
                manager.Load<Texture2D>("Characters/Knight/LeftRun/Run8"),
                manager.Load<Texture2D>("Characters/Knight/LeftRun/Run9"),
                manager.Load<Texture2D>("Characters/Knight/LeftRun/Run10")
            };

            _TextureDictionary.Add(TextureType.RunLeft, new Animation(characterWalkAnim, 5, true, false));
        }

        private void LoadRightIdleAnimation(ContentManager manager)
        {
            List<Texture2D> characterIdleAnim = new List<Texture2D>
            {
                manager.Load<Texture2D>("Characters/Knight/RightIdle/Idle1"),
                manager.Load<Texture2D>("Characters/Knight/RightIdle/Idle2"),
                manager.Load<Texture2D>("Characters/Knight/RightIdle/Idle3"),
                manager.Load<Texture2D>("Characters/Knight/RightIdle/Idle4"),
                manager.Load<Texture2D>("Characters/Knight/RightIdle/Idle5"),
                manager.Load<Texture2D>("Characters/Knight/RightIdle/Idle6"),
                manager.Load<Texture2D>("Characters/Knight/RightIdle/Idle7"),
                manager.Load<Texture2D>("Characters/Knight/RightIdle/Idle8"),
                manager.Load<Texture2D>("Characters/Knight/RightIdle/Idle9"),
                manager.Load<Texture2D>("Characters/Knight/RightIdle/Idle10")
            };

            _TextureDictionary.Add(TextureType.IdleRight, new Animation(characterIdleAnim, 5, true, false));
        }

        private void LoadLeftIdleAnimation(ContentManager manager)
        {
            List<Texture2D> characterIdleAnim = new List<Texture2D>
            {
                manager.Load<Texture2D>("Characters/Knight/LeftIdle/Idle1"),
                manager.Load<Texture2D>("Characters/Knight/LeftIdle/Idle2"),
                manager.Load<Texture2D>("Characters/Knight/LeftIdle/Idle3"),
                manager.Load<Texture2D>("Characters/Knight/LeftIdle/Idle4"),
                manager.Load<Texture2D>("Characters/Knight/LeftIdle/Idle5"),
                manager.Load<Texture2D>("Characters/Knight/LeftIdle/Idle6"),
                manager.Load<Texture2D>("Characters/Knight/LeftIdle/Idle7"),
                manager.Load<Texture2D>("Characters/Knight/LeftIdle/Idle8"),
                manager.Load<Texture2D>("Characters/Knight/LeftIdle/Idle9"),
                manager.Load<Texture2D>("Characters/Knight/LeftIdle/Idle10")
            };

            _TextureDictionary.Add(TextureType.IdleLeft, new Animation(characterIdleAnim, 5, true, false));
        }


        internal void UnpauseAnimation(TextureType type)
        {
            _TextureDictionary[type]._Paused = false;
        }

        internal void ResetAnimation(TextureType type)
        {
            _TextureDictionary[type].ResetAnimation();
        }

        internal SpriteFont GetFont(FontType type)
        {
            return _FontDictionary[type];
        }

        internal bool IsPaused(TextureType type)
        {
            return _TextureDictionary[type]._Paused;
        }

        internal Texture2D GetTexture(TextureType type)
        {
            if (!_TextureDictionary.ContainsKey(type))
                throw new NotImplementedException();

            return _TextureDictionary[type].GetCurrentTexture();
        }
    }
}
