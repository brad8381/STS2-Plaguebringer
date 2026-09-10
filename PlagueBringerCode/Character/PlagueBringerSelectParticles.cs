using Godot;

namespace PB.Character;

/// <summary>
/// Lightweight green spore motes for the character-select screen.
/// Drawn procedurally so no extra particle texture is required.
/// </summary>
public partial class PlagueBringerSelectParticles : Node2D
{
    private const int ParticleCount = 34;

    private readonly Vector2[] _positions = new Vector2[ParticleCount];
    private readonly float[] _speeds = new float[ParticleCount];
    private readonly float[] _radii = new float[ParticleCount];
    private readonly float[] _phases = new float[ParticleCount];
    private double _time;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;

        for (var i = 0; i < ParticleCount; i++)
        {
            _positions[i] = new Vector2(
                980f + ((i * 173) % 900),
                80f + ((i * 109) % 900));

            _speeds[i] = 10f + (i % 7) * 2.5f;
            _radii[i] = 1.2f + (i % 5) * 0.45f;
            _phases[i] = i * 0.73f;
        }

        SetProcess(true);
        QueueRedraw();
    }

    public override void _Process(double delta)
    {
        _time += delta;
        var dt = (float)delta;

        for (var i = 0; i < ParticleCount; i++)
        {
            var p = _positions[i];
            p.Y -= _speeds[i] * dt;
            p.X += Mathf.Sin((float)_time * 0.75f + _phases[i]) * 5.5f * dt;

            if (p.Y < 45f)
            {
                p.Y = 1030f;
                p.X = 980f + ((i * 211 + (int)(_time * 13)) % 900);
            }

            _positions[i] = p;
        }

        QueueRedraw();
    }

    public override void _Draw()
    {
        for (var i = 0; i < ParticleCount; i++)
        {
            var pulse = 0.5f + 0.5f * Mathf.Sin((float)_time * 1.1f + _phases[i]);
            var alpha = 0.10f + pulse * 0.24f;
            DrawCircle(_positions[i], _radii[i], new Color(0.48f, 0.88f, 0.18f, alpha));
        }
    }
}
