using BaseLib.Abstracts;
using Godot;

namespace PB.Character;

public class PlagueBringerPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => PlagueBringer.Color;

    public override string BigEnergyIconPath =>
        "res://images/atlases/ui_atlas.sprites/card/energy_ironclad.tres";
    public override string TextEnergyIconPath =>
        "res://images/atlases/ui_atlas.sprites/card/energy_ironclad.tres";
}
