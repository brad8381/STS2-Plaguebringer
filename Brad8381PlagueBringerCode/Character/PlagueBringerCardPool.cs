using BaseLib.Abstracts;
using Godot;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Character;

public class PlagueBringerCardPool : CustomCardPoolModel
{
    public override string Title => PlagueBringer.CharacterId; //This is not a display name.

    // Temporary vanilla energy icon until the Plaguebringer energy art is final.
    // Keeping this on a known-good game resource also prevents Neow reward generation
    // from failing when it builds energy-related hover tips.
    public override string BigEnergyIconPath =>
        "res://images/atlases/ui_atlas.sprites/card/energy_ironclad.tres";
    public override string TextEnergyIconPath =>
        "res://images/atlases/ui_atlas.sprites/card/energy_ironclad.tres";

    /* These HSV values will determine the color of your card back.
    They are applied as a shader onto an already colored image,
    so it may take some experimentation to find a color you like.
    Generally they should be values between 0 and 1. */
    public override float H => 1f; //Hue; changes the color.
    public override float S => 0.08f; //Saturation
    public override float V => 0.3f; //Brightness

    //Color of small card icons
    public override Color DeckEntryCardColor => new("55545a");

    public override bool IsColorless => false;
}
