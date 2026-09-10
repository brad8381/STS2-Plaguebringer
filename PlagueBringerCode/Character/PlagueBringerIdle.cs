using Godot;

namespace PB.Character;

public partial class PlagueBringerIdle : Node2D
{
    private Vector2 _basePosition;
    private Vector2 _baseScale;
    private float _baseRotation;
    private double _time;

    private Sprite2D? _sprite;
    private AnimationPlayer? _animationPlayer;

    private Texture2D? _idleTexture;
    private Texture2D? _attackTexture;
    private Texture2D? _castTexture;
    private Texture2D? _deathTexture;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;

        _basePosition = Position;
        _baseScale = Scale;
        _baseRotation = Rotation;

        _sprite = GetNodeOrNull<Sprite2D>("Sprite");
        _animationPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");

        _idleTexture = GD.Load<Texture2D>(
            "res://PlagueBringer/images/character/idle.png");

        _attackTexture = GD.Load<Texture2D>(
            "res://PlagueBringer/images/character/attack.png");

        _castTexture = GD.Load<Texture2D>(
            "res://PlagueBringer/images/character/cast.png");

        _deathTexture = GD.Load<Texture2D>(
            "res://PlagueBringer/images/character/death.png");

        if (_sprite != null && _idleTexture != null)
            _sprite.Texture = _idleTexture;

        SetProcess(true);
    }

    public override void _Process(double delta)
    {
        _time += delta;

        var sway = Mathf.Sin((float)_time * 1.05f);

        Position = _basePosition + new Vector2(sway * 3.5f, 0f);
        Rotation = _baseRotation + Mathf.DegToRad(sway * 0.35f);
        Scale = _baseScale;

        if (_sprite == null)
            return;

        var animation =
            _animationPlayer?.CurrentAnimation.ToString() ?? "";

        Texture2D? wanted = animation switch
        {
            "attack" => _attackTexture,
            "cast" => _castTexture,
            "die" => _deathTexture,
            _ => _idleTexture
        };

        if (wanted != null && _sprite.Texture != wanted)
            _sprite.Texture = wanted;
    }
}