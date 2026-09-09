using Godot;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Character;

public partial class PlagueBringerSilentSelectVfx : Control
{
    private const string SilentScene =
        "res://scenes/screens/char_select/char_select_bg_silent.tscn";

    private const string PlagueBackground =
        "res://Brad8381PlagueBringer/images/character/plaguebringer_select_background.png";

    public override void _Ready()
    {
        MouseFilter = MouseFilterEnum.Ignore;

        var packed = GD.Load<PackedScene>(SilentScene);

        if (packed == null)
        {
            GD.PushWarning(
                $"Could not load Silent character-select VFX: {SilentScene}");
            return;
        }

        var silentBg = packed.Instantiate<Control>();

        AddChild(silentBg);

        // Remove the actual Silent character.
        var silentCharacter =
            silentBg.GetNodeOrNull<CanvasItem>("SpineSprite");

        if (silentCharacter != null)
            silentCharacter.Visible = false;

        // Keep Silent's animated/distorted background system,
        // but show our Plaguebringer background instead.
        var background =
            silentBg.GetNodeOrNull<TextureRect>("TextureRect");

        if (background != null)
        {
            var plagueTexture =
                GD.Load<Texture2D>(PlagueBackground);

            if (plagueTexture != null)
                background.Texture = plagueTexture;
        }

        // Darker / dirtier than Silent.
        // This multiplies Silent's existing green VFX.
        silentBg.Modulate =
            new Color(
                0.42f,
                0.55f,
                0.34f,
                0.72f);
    }
}