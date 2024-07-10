using AscendedZ.effects;
using Godot;
using System;

public partial class ShakeCamera : Camera2D
{
    private ShakeParameters _shakeParameters;
    private RandomNumberGenerator _randomNumberGenerator;
    private float _x;
    private Vector2 _originalPosition;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _shakeParameters = new ShakeParameters();
        _randomNumberGenerator = new RandomNumberGenerator();
        _randomNumberGenerator.Randomize();
        _originalPosition = new Vector2(this.Position.X, 0);
        _x = -1;
    }

    /// <summary>
    /// We're going to use Process to reset the strength of our Screen Shake back to 0.
    /// </summary>
    /// <param name="delta"></param>
    public override void _Process(double delta)
    {
        if (_shakeParameters.ShakeValue > 0)
        {
            _shakeParameters.ShakeValue = (float)Mathf.Lerp((double)_shakeParameters.ShakeValue, 0, (double)_shakeParameters.ShakeDecay * delta);
            float x = _randomNumberGenerator.RandfRange(-_shakeParameters.ShakeValue, _shakeParameters.ShakeValue);
            float y = _randomNumberGenerator.RandfRange(-_shakeParameters.ShakeValue, _shakeParameters.ShakeValue);
            this.Position = new Vector2(_x + x, y);
        }
        else
        {
            _x = this.Position.X;
            this.Position = new Vector2(_x, _originalPosition.Y);
        }
    }

    public void Shake()
    {
        _shakeParameters.ShakeValue = _shakeParameters.ShakeStrength;
    }
}
