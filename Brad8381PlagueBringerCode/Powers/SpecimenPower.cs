using MegaCrit.Sts2.Core.Entities.Powers;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;

public sealed class SpecimenPower : PlagueBringerPower
{
    public const int MaxAmount = 6;

    public override string CustomPackedIconPath => "res://Brad8381PlagueBringer/images/powers/specimen_power.svg";
    public override string CustomBigIconPath => "res://Brad8381PlagueBringer/images/powers/big/specimen_power.svg";

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    // Specimens have their own HUD counter beside Energy, so do not duplicate them in the power row.
    protected override bool IsVisibleInternal => false;
}
