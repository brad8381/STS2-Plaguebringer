using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;

public sealed class SpecimenPower : PlagueBringerPower
{
    public const int MaxAmount = 6;

    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Specimen",
            "A combat resource gained from sacrifice and Exhaust effects. Persists between turns. Maximum 6.",
            "A combat resource gained from sacrifice and Exhaust effects. Persists between turns. Maximum 6."
        );

    public override string CustomPackedIconPath =>
        "res://Brad8381PlagueBringer/images/powers/specimen_power.svg";
    public override string CustomBigIconPath =>
        "res://Brad8381PlagueBringer/images/powers/big/specimen_power.svg";

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override bool IsVisibleInternal => false;
}
