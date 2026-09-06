using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;

public sealed class PlaguePower : PlagueBringerPower
{
    public override string CustomPackedIconPath => "res://Brad8381PlagueBringer/images/powers/plague_power.svg";
    public override string CustomBigIconPath => "res://Brad8381PlagueBringer/images/powers/big/plague_power.svg";

    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext,
        CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        // The participant check prevents ticks for creatures that are not taking this turn.
        if (side != CombatSide.Enemy || Owner.Side != side || !participants.Contains(Owner)
            || !Owner.IsAlive || Amount <= 0) return;

        var target = Owner;
        var stacks = Amount;
        MainFile.Logger.Debug($"Plague tick: target={Owner}, stacks={stacks}");
        Flash();
        await CreatureCmd.Damage(choiceContext, target, stacks,
            ValueProp.Unpowered | ValueProp.Unblockable, Applier ?? target);

        // Damage may kill the creature or cause another effect to remove this power.
        if (target.IsAlive && target.GetPower<PlaguePower>() == this)
            await PowerCmd.ModifyAmount(this, 1, null, null);
    }
}
