using BaseLib.Abstracts;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Extensions;
using Godot;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Character;

public class PlagueBringerPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => PlagueBringer.Color;
    

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}