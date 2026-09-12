using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Rooms;

namespace PB.Powers;

public sealed class SpecimenPower : PlagueBringerPower
{
    public const int MaxAmount = 6;

    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Specimen",
            "A persistent combat resource. Persists between turns. Maximum +6. It can become negative. At the end of combat, lose HP equal to negative Specimen.",
            "Persists between turns. Max [gold]+6[/gold]. Can become negative. At the end of combat, lose [red]HP equal to negative Specimen[/red]."
        );

    public override string CustomPackedIconPath =>
        "res://PlagueBringer/images/powers/specimen_power.png";

    public override string CustomBigIconPath =>
        "res://PlagueBringer/images/powers/big/specimen_power.png";

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool AllowNegative => true;

    protected override bool IsVisibleInternal => false;

    public override async Task AfterCombatEnd(CombatRoom room)
    {
        if (Amount >= 0 || !Owner.IsPlayer)
            return;

        var debt = -Amount;
        var newHp = Math.Max(1, Owner.CurrentHp - debt);

        await CreatureCmd.SetCurrentHp(Owner, newHp);
    }
}
