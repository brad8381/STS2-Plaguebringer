using Godot;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Character;

/// <summary>
/// Gives the character-select artwork a subtle breathing/sway loop.
/// </summary>
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

        var sway = Mathf.Sin((float)_time * 0.95f);
        var breathe = Mathf.Sin((float)_time * 1.45f + 0.8f);

        Position = _basePosition + new Vector2(sway * 4f, breathe * 6f);
        Rotation = _baseRotation + Mathf.DegToRad(sway * 0.75f);

        var scaleFactor = 1f + breathe * 0.006f;
        Scale = _baseScale * scaleFactor;
    }
}
