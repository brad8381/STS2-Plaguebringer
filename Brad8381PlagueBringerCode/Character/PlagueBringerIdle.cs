using Godot;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Character;

/// <summary>
/// Small idle motion for the temporary static combat sprite.
/// Keeps the character from looking frozen until a proper animated rig exists.
/// </summary>
public partial class PlagueBringerIdle : Sprite2D
{
    private Vector2 _basePosition;
    private float _baseRotation;
    private double _time;

    public override void _Ready()
    {
        _basePosition = Position;
        _baseRotation = Rotation;
    }

    public override void _Process(double delta)
    {
        _time += delta;

        var wave = Mathf.Sin((float)_time * 1.65f);
        Position = _basePosition + new Vector2(0f, wave * 2.5f);
        Rotation = _baseRotation + Mathf.DegToRad(wave * 0.65f);
    }
}
