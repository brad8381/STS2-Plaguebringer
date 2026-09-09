using BaseLib.Abstracts;
using Godot;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Character;

public class PlagueBringerCardPool : CustomCardPoolModel
{
    public override string Title => PlagueBringer.CharacterId;

    public override string BigEnergyIconPath =>
        "res://Brad8381PlagueBringer/images/charui/energy_big.png";

    public override string TextEnergyIconPath =>
        "res://Brad8381PlagueBringer/images/charui/energy_text.png";

    public override float H => 1f;
    public override float S => 0.08f;
    public override float V => 0.3f;

    public override Color DeckEntryCardColor => new("55545a");

    public override bool IsColorless => false;
}