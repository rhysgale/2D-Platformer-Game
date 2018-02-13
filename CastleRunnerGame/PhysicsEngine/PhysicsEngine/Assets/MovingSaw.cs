using System;
using Microsoft.Xna.Framework;
using PhysicsEngine.GameDrawing;

namespace PhysicsEngine.Assets
{
    class MovingSaw
    {
        internal MapObject _SawObject { get; set; }
        private readonly int _Min;
        private readonly int _Max;
        private bool _GoToMin;
        private readonly bool _Sideways;

        internal MovingSaw(MapObject obj, int min, int max, bool sideways)
        {
            _SawObject = obj;

            _Min = min;
            _Max = max;
            _GoToMin = false;
            _Sideways = sideways;
        }

        internal void Update()
        {
            if (_Sideways)
            {
                _SawObject.UpdatePos(_GoToMin ? new Vector2(-1, 0) : new Vector2(1, 0));

                if (_GoToMin && _SawObject._BoundingBox._Shape.GetPos().X < _Min)
                    _GoToMin = !_GoToMin;
                else if (!_GoToMin && _SawObject._BoundingBox._Shape.GetPos().X > _Max)
                    _GoToMin = !_GoToMin;
            }
            else
            {
                _SawObject.UpdatePos(_GoToMin ? new Vector2(0, -1) : new Vector2(0, 1));

                if (_GoToMin && _SawObject._BoundingBox._Shape.GetPos().Y < _Min)
                    _GoToMin = !_GoToMin;
                else if (!_GoToMin && _SawObject._BoundingBox._Shape.GetPos().Y > _Max)
                    _GoToMin = !_GoToMin;
            }
        }
    }
}
