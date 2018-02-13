using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace PhysicsEngine.GameDrawing
{
    //class that animates objects
    internal class Animation
    {
        List<Texture2D> _Animations;
        int _CurrentTextureIndex = 0;
        int _FramesPerChange;
        int _CurrentGameFrame;
        bool _Looped;
        internal bool _Paused { get; set; }

        internal Animation(List<Texture2D> frames, int framePerChange, bool looped, bool paused)
        {
            _Looped = looped;
            _Paused = paused;
            _FramesPerChange = framePerChange;
            _Animations = frames;
        }

        internal void ResetAnimation()
        {
            _Paused = true;
            _CurrentTextureIndex = 0;
        }

        internal Animation(Texture2D staticTexture)
        {
            _FramesPerChange = 0;
            _Looped = false;
            _Animations = new List<Texture2D>
            {
                staticTexture
            };
        }

        internal Texture2D GetCurrentTexture()
        {
            Texture2D type;

            if (!_Paused)
                _CurrentGameFrame++;

            type = _Animations[_CurrentTextureIndex];

            if (_CurrentGameFrame == _FramesPerChange)
            {
                _CurrentTextureIndex++;

                if (!_Looped && _CurrentTextureIndex == _Animations.Count)
                    _CurrentTextureIndex--;
                else if (_CurrentTextureIndex == _Animations.Count)
                    _CurrentTextureIndex = 0;

                _CurrentGameFrame = 0;
            }

            return type;
        }
    }
}
