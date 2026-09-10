using Godot;

namespace PB.Character;

public partial class PlagueBringerSelectSway : TextureRect
{
    private Vector2 _basePosition;
    private Vector2 _baseScale;
    private float _baseRotation;
    private double _time;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;

        _basePosition = Position;
        _baseScale = Scale;
        _baseRotation = Rotation;

        PivotOffset = Size * 0.5f;

        SetProcess(true);
    }

    public override void _Process(double delta)
    {
        _time += delta;

        var sway =
            Mathf.Sin((float)_time * 0.80f);

        var breathe =
            Mathf.Sin((float)_time * 1.20f + 0.8f);

        Position =
            _basePosition +
            new Vector2(sway * 6f, 0f);

        Rotation =
            _baseRotation +
            Mathf.DegToRad(sway * 0.35f);

        var scaleFactor =
            1f + breathe * 0.003f;

        Scale =
            _baseScale * scaleFactor;
    }
}