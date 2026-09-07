using Godot;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Character;

/// <summary>
/// Slow, dark-green light sweep for the character-select background.
/// It is intentionally subtle so it feels closer to the Silent's ambient lighting
/// than a bright spell effect.
/// </summary>
public partial class PlagueBringerSelectLight : TextureRect
{
    private Vector2 _basePosition;
    private double _time;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
        _basePosition = Position;
        PivotOffset = Size * 0.5f;
        SetProcess(true);
    }

    public override void _Process(double delta)
    {
        _time += delta;

        var sweep = Mathf.Sin((float)_time * 0.48f);
        var drift = Mathf.Sin((float)_time * 0.31f + 1.2f);
        var pulse = Mathf.Sin((float)_time * 0.72f + 0.4f);

        Position = _basePosition + new Vector2(sweep * 185f, drift * 34f);
        Rotation = Mathf.DegToRad(sweep * 1.8f);
        SelfModulate = new Color(1f, 1f, 1f, 0.62f + pulse * 0.10f);
    }
}
