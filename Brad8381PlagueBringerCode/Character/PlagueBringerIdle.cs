using Godot;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Character;

/// <summary>
/// Subtle idle movement for the static Plaguebringer artwork.
/// This is attached to the Visuals parent so combat animations can independently
/// move/fade the Sprite child without fighting this script every frame.
/// </summary>
public partial class PlagueBringerIdle : Node2D
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
        SetProcess(true);
    }

    public override void _Process(double delta)
    {
        _time += delta;

        var slow = Mathf.Sin((float)_time * 1.25f);
        var fast = Mathf.Sin((float)_time * 2.05f + 0.7f);

        Position = _basePosition + new Vector2(slow * 2.5f, fast * 7.0f);
        Rotation = _baseRotation + Mathf.DegToRad(slow * 1.15f);

        var breathe = 1f + fast * 0.009f;
        Scale = _baseScale * breathe;
    }
}
